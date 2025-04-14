using Common;
using Common.Config;
using Common.Helpers;
using Common.Services;
using CommonWinForm;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace FloatingWindowApp
{
    public partial class MainForm : Form
    {
        private bool isDragging = false;
        private Point startPoint = new Point(0, 0);

        private const int WH_MOUSE_LL = 14;
        private const int WM_LBUTTONDOWN = 0x0201;
        private LowLevelMouseProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private Stopwatch _stopwatch = new Stopwatch();
        private const int ThrottleInterval = 500; // 500毫秒内只处理一次

        private static bool isShowingMessageBox = false;
        private bool isTaskRunning = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeHook();
            SubscribeToOcrEvents();
        }
        private void SubscribeToOcrEvents()
        {
            // 订阅 OCR 完成事件
            OCRService.OcrCompleted += OcrService_OcrCompleted;
        }

        private void OcrService_OcrCompleted(object sender, OcrCompletedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Result) && Regex.IsMatch(e.Result, @"^\d+$"))
            {
                // 更新 TextBox 的内容
                this.Invoke(new Action(() =>
                {
                    textBox.Text = e.Result;
                }));
            }
            else if (!string.IsNullOrEmpty(e.Error))
            {
                ShowError(e.Error);
            }
        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ConfigProvider.Settings.UpdateConfig(s =>
            {
                s.DetectData = textBox.Text;
            });
        }
        private void AutoOpen_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigProvider.Settings.UpdateConfig(s =>
                {
                    s.AutoOpen = AutoOpen.Checked;
                });
            }
            catch (Exception ex)
            {
                ShowError($"保存自动打开设置时发生错误: {ex.Message}");
            }
        }
        private void ResultResident_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigProvider.Settings.UpdateConfig(s =>
                {
                    s.ResultResident = ResultResident.Checked;
                });
            }
            catch (Exception ex)
            {
                ShowError($"保存结果常驻设置时发生错误: {ex.Message}");
            }
        }

        private void BootUp_Click(object sender, EventArgs e)
        {
            string appPath = Application.ExecutablePath;
            try
            {
                ConfigService.SettingBootUp(appPath, BootUp.Checked);
            }
            catch (Exception ex)
            {
                ShowError($"设置开机自启动失败: {ex.Message}");
                BootUp.Checked = false;
            }

        }
        private void Detection_Click(object sender, EventArgs e)
        {
            using (ScreenCaptureForm scf = new ScreenCaptureForm())
            {
                // 显示覆盖窗体，让用户框选区域
                if (scf.ShowDialog() == DialogResult.OK && scf.IsConfirmed)
                {
                    Bitmap capturedImage = scf.CaptureSelectedRegion();
                    if (capturedImage != null)
                    {
                        // 保存截图到文件
                        try
                        {
                            // 获取解决方案文件夹的路径
                            string currentDirectory = Directory.GetCurrentDirectory();
                            string projectPath = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
                            string targetFolder = Path.Combine(projectPath, "DetectionImage");

                            // 如果文件夹不存在，则创建
                            if (!Directory.Exists(targetFolder))
                            {
                                Directory.CreateDirectory(targetFolder);
                            }
                            string randomFileName = Utils.GenerateRandomFileName(8);
                            // 定义文件路径
                            string filePath = Path.Combine(targetFolder, randomFileName + ".png");

                            capturedImage.Save(filePath, ImageFormat.Png);
                            MessageBox.Show("截图已保存：" + filePath);
                        }
                        catch (Exception ex)
                        {
                            ShowError($"保存内容时发生错误: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void UsageGuide_Click(object sender, EventArgs e)
        {
            MessageBox.Show("这是悬浮窗的使用指南。您可以拖动窗口到任意位置，右键点击可以访问功能菜单。");
        }

        private void Feedback_Click(object sender, EventArgs e)
        {
            MessageBox.Show("如果您遇到任何问题，请联系我们进行故障反馈。");
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowError(string errorMessage)
        {
            if (isShowingMessageBox)
            {
                return; // 如果已经有提示框在运行，直接返回
            }

            isShowingMessageBox = true;
            try
            {
                MessageBox.Show(errorMessage, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isShowingMessageBox = false; // 确保提示框关闭后重置标记
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                startPoint = new Point(e.X, e.Y);
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point p = PointToScreen(new Point(e.X, e.Y));
                Location = new Point(p.X - startPoint.X, p.Y - startPoint.Y);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                var settings = ConfigProvider.Settings.GetConfig();
                int x = settings.FormLocationX;
                int y = settings.FormLocationY;
                bool AutoOpen = settings.AutoOpen;
                bool ResultResident = settings.ResultResident;
                bool bootup = settings.BootUp;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(x, y);
                this.AutoOpen.Checked = AutoOpen;
                this.ResultResident.Checked = ResultResident;
                BootUp.Checked = bootup;
            }
            catch (Exception ex)
            {
                this.StartPosition = FormStartPosition.Manual;
                Location = new Point(800, 50);
                ShowError($"恢复窗体位置时发生错误: {ex.Message}");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ConfigProvider.Settings.UpdateConfig(s =>
                {
                    s.FormLocationX = this.Location.X;
                    s.FormLocationY = this.Location.Y;
                });
                UnhookWindowsHookEx(_hookID);
            }
            catch (Exception ex)
            {
                ShowError($"保存窗体位置时发生错误: {ex.Message}");
            }
        }



        // 全局鼠标事件监听钩子
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private void InitializeHook()
        {
            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }

        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process process = Process.GetCurrentProcess())
            using (ProcessModule module = process.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(module.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_LBUTTONDOWN)
            {
                if (_stopwatch.IsRunning && _stopwatch.ElapsedMilliseconds < ThrottleInterval)
                {
                    return IntPtr.Zero; // 忽略频繁的点击
                }

                _stopwatch.Restart();

                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                Point mousePosition = new Point(hookStruct.pt.x, hookStruct.pt.y);

                ProcessMouseClick(mousePosition);
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private async void ProcessMouseClick(Point mousePosition)
        {
            if (isTaskRunning)
            {
                return; // 如果任务已经在运行，直接返回
            }
            try
            {
                isTaskRunning = true;
                (string screenshotPath, Rectangle tableRegion) = await Task.Run(() => CaptureScreenshot(mousePosition));

                if (!string.IsNullOrEmpty(screenshotPath))
                {
                    int screenHeight = tableRegion.Height;
                    await Task.Run(() => OCRService.OCRSercive(screenshotPath, mousePosition, screenHeight, tableRegion));
                }
            }
            catch (Exception ex)
            {
                ShowError($"处理过程中发生错误: {ex.Message}");
            }
            finally
            {
                isTaskRunning = false;
            }
        }

        private (string screenshotPath, Rectangle tableRegion) CaptureScreenshot(Point mousePosition)
        {
            using (Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(Point.Empty, Point.Empty, bitmap.Size);
                }

                // 对图像进行预处理
                //Bitmap processedBitmap = ImageProcessService.ConvertToGrayscale(bitmap);
                //Bitmap processedBitmap = ImageProcessService.ApplyGammaCorrection(processedBitmap);
                string screenshotPath = Path.Combine(Path.GetTempPath(), "temp.png");
                bitmap.Save(screenshotPath, ImageFormat.Png);

                // 获取裁剪后的表格区域及其边界信息
                //Rectangle tableRegion = ImageProcessService.ProcessColorfulTableScreenshot(screenshotPath);
                Rectangle tableRegion = ImageProcessService.ExtractAndSaveTableRegion(screenshotPath);
                //ImageProcessService.InvertImage(screenshotPath);
                //ImageProcessService.BinarizeImageInPlace(screenshotPath, 140);
                return (screenshotPath, tableRegion);
            }
        }

    }
}