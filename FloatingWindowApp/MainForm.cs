using Common;
using Common.Config;
using Common.Helpers;
using Common.Services;
using CommonWinForm;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FloatingWindowApp
{
    public partial class MainForm : Form
    {
        public static string appName = Application.ProductName;
        public static string appPath = Application.ExecutablePath;

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

        private readonly HttpClientService _client;
        private ResultForm resultForm;
        private bool isResponse = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeHook();
            SubscribeToOcrEvents();
            ConfigProvider.Settings.ConfigChanged += OnConfigChanged;
            _client = new HttpClientService();
        }
        private void SubscribeToOcrEvents()
        {
            // 订阅 OCR 完成事件
            OCRService.OcrCompleted += OcrService_OcrCompleted;
        }
        private void OnConfigChanged()
        {
            // 确保在 UI 线程执行
            if (InvokeRequired)
            {
                Invoke(new Action(OnConfigChanged));
                return;
            }
            LoadConfigAndUpdateUI();
        }
        private void LoadConfigAndUpdateUI()
        {
            var settings = ConfigProvider.Settings.GetConfig();
            Location = new Point(settings.FormLocationX, settings.FormLocationY);
            BootUp.Checked = settings.BootUp;
        }
        private async void OcrService_OcrCompleted(object sender, OcrCompletedEventArgs e)
        {
            isResponse = false;
            // 更新 TextBox 的内容
            this.Invoke(new Action(() =>
            {
                if (!string.IsNullOrEmpty(e.Result))
                {
                    textBox.Text = e.Result;
                    resultForm.ResultLabelText = "检测成功，正在请求后端处理";
                }
                else if (!string.IsNullOrEmpty(e.Tip))
                {
                    resultForm.ResultLabelText = e.Tip;
                }
                else if (!string.IsNullOrEmpty(e.Error))
                {
                    resultForm.ResultLabelText = "出现错误";
                    ShowError(e.Error);
                }

                // 如果 ResultResident 为 false，则显示 resultForm 并启动计时器
                if (!ConfigProvider.Settings.GetConfig().ResultResident)
                {
                    resultForm.Show();
                    timer.Start();
                }
            }));
            if (AutoOpen.Checked)
            {
                var queryParams = new Dictionary<string, string>
                    {
                        { "patientID", e.Result }
                    };
                var response = await _client.GetAsync("search", queryParams);
                if (response.IsSuccessStatusCode)
                {
                    isResponse = true;
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonResponse);
                    resultForm.ResultLabelText = "预测成功";
                }
            }
        }
        private async void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isResponse)
            {
                MessageBox.Show("处理中，请稍候……");
                return;
            }
            isResponse = false;
            var text = textBox.Text.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                ConfigProvider.Settings.UpdateConfig(s => s.DetectData = text);
                var queryParams = new Dictionary<string, string>
                    {
                        { "patientID", text }
                    };
                var response = await _client.GetAsync("search", queryParams);
                if (response.IsSuccessStatusCode)
                {
                    isResponse = true;
                    resultForm.ResultLabelText = "预测成功";
                }

                if (!ConfigProvider.Settings.GetConfig().ResultResident)
                {
                    resultForm.Show();
                    timer.Start();
                }
            }
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
                throw;
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
                if (ResultResident.Checked)
                {
                    resultForm.Show();
                    timer.Stop();
                }
                else
                {
                    resultForm.Hide();
                }
            }
            catch (Exception ex)
            {
                ShowError($"保存结果常驻设置时发生错误: {ex.Message}");
                throw;
            }
        }

        private void BootUp_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigService.SettingBootUp(BootUp.Checked);
            }
            catch (Exception ex)
            {
                ShowError($"设置开机自启动失败: {ex.Message}");
                BootUp.Checked = false;
                throw;
            }

        }
        private void Detection_Click(object sender, EventArgs e)
        {
            using (var scf = new ScreenCaptureForm())
            {
                if (scf.ShowDialog() == DialogResult.OK && scf.IsConfirmed)
                {

                }
            }
        }

        private void UsageGuide_Click(object sender, EventArgs e)
        {
            MessageBox.Show("这是悬浮窗的使用指南。您可以拖动窗口到任意位置，右键点击可以访问功能菜单。");
        }

        private void Feedback_Click(object sender, EventArgs e)
        {
            string configAppName = "ConfigWindowApp";
            string configAppPath = Path.Combine(ConfigProvider.solutionRoot, "ConfigWindowApp", "bin", "Debug", "net8.0-windows", "ConfigWindowApp.exe");
            if (!System.IO.File.Exists(configAppPath))
            {
                MessageBox.Show("未找到配置程序！");
                return;
            }
            if (Process.GetProcessesByName(configAppName).Length <= 0)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = appPath,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
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
        private void UpdateResultFormPosition()
        {
            if (resultForm != null && !resultForm.IsDisposed)
            {
                int offsetY = this.Height + 5;

                resultForm.Location = new Point(
                    this.Left,
                    this.Top + offsetY
                );
            }
        }
        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            UpdateResultFormPosition();
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
                ConfigProvider.Settings.UpdateConfig(s =>
                {
                    s.FormLocationX = Location.X;
                    s.FormLocationY = Location.Y;
                });
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point p = PointToScreen(new Point(e.X, e.Y));
                Location = new Point(p.X - startPoint.X, p.Y - startPoint.Y);
                UpdateResultFormPosition();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                ConfigProvider.Settings.UpdateConfig(s =>
                {
                    s.AppName = appName;
                    s.AppPath = appPath;
                    s.StartStatus = true;
                });
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

                // 创建并定位ResultForm
                resultForm = new ResultForm();
                if (ResultResident)
                {
                    resultForm.Show();
                    UpdateResultFormPosition();
                }
                // 保持置顶状态同步
                resultForm.TopMost = this.TopMost;
            }
            catch (Exception ex)
            {
                this.StartPosition = FormStartPosition.Manual;
                Location = new Point(800, 50);
                ShowError($"恢复窗体位置时发生错误: {ex.Message}");
                throw;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ConfigProvider.Settings.UpdateConfig(s =>
                {
                    s.StartStatus = false;
                });
                ConfigProvider.Settings.ConfigChanged -= OnConfigChanged;
                UnhookWindowsHookEx(_hookID);
                resultForm?.Close();
            }
            catch (Exception ex)
            {
                ShowError($"保存窗体位置时发生错误: {ex.Message}");
                throw;
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
            if (_hookID == IntPtr.Zero)
            {
                _hookID = SetHook(_proc);
            }
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
            if (isTaskRunning) return;
            isTaskRunning = true;
            try
            {
                Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                (Mat screenshot, Rectangle tableRegion) = await Task.Run(() => CaptureScreenshot(bitmap, mousePosition));
                if (screenshot != null)
                {
                    await Task.Run(() => OCRService.OCRServiceProcessing(
                        screenshot,
                        mousePosition,
                        screenshot.Height,
                        tableRegion
                        )
                    );
                }
            }
            catch (Exception ex)
            {
                ShowError($"处理过程中发生错误: {ex.Message}");
                throw;
            }
            finally
            {
                isTaskRunning = false;
            }
        }

        private (Mat processedTable, Rectangle tableRegion) CaptureScreenshot(Bitmap bitmap, Point mousePosition)
        {
            try
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(Point.Empty, Point.Empty, bitmap.Size);
                }
                using (Mat colorImage = bitmap.ToMat())
                using (Mat grayImage = new Mat())
                {
                    // 使用 OpenCV 转换为灰度图像
                    CvInvoke.CvtColor(colorImage, grayImage, ColorConversion.Bgr2Gray);
                    // 传入灰度图像进行表格检测
                    Rectangle tableRegion = ImageProcessService.DetectTableRegion(grayImage);

                    if (!tableRegion.IsEmpty)
                    {
                        // 裁剪并处理表格区域
                        Rectangle leftHalfRegion = new Rectangle(
                            tableRegion.X,
                            tableRegion.Y,
                            tableRegion.Width / 2,
                            tableRegion.Height
                        );
                        Mat processedTable = ImageProcessService.ProcessTableRegion(grayImage, leftHalfRegion);
                        return (processedTable, tableRegion);
                    }
                }
                return (bitmap.ToMat(), Rectangle.Empty);
            }
            catch
            {
                return (null, Rectangle.Empty);
                throw;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            resultForm?.Hide();
        }
    }
}