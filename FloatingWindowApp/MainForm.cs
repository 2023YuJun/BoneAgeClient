using Common;
using CommonWinForm;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

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
        private const int ThrottleInterval = 200; // 200������ֻ����һ��

        public MainForm()
        {
            InitializeComponent();
            InitializeHook();
            SubscribeToOcrEvents();
        }
        private void SubscribeToOcrEvents()
        {
            // ���� OCR ����¼�
            OCRService.OcrCompleted += OcrService_OcrCompleted;
        }

        private void OcrService_OcrCompleted(object sender, Common.OcrCompletedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Result))
            {
                // ���� TextBox ������
                this.Invoke(new Action(() =>
                {
                    textBox.Text = e.Result;
                }));
            }
            else if (!string.IsNullOrEmpty(e.Error))
            {
                MessageBox.Show(e.Error);
            }
        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void AutoOpen_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigHelper.SetSetting("AutoOpen", AutoOpen.Checked.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"�����Զ�������ʱ��������: {ex.Message}");
            }
        }
        private void ResultResident_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigHelper.SetSetting("ResultResident", ResultResident.Checked.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������פ����ʱ��������: {ex.Message}");
            }
        }

        private void BootUp_Click(object sender, EventArgs e)
        {
            string appPath = Application.ExecutablePath;
            try
            {
                Config.SettingBootUp(appPath, BootUp.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���ÿ���������ʧ��: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                BootUp.Checked = false;
            }

        }
        private void Detection_Click(object sender, EventArgs e)
        {
            using (ScreenCaptureForm scf = new ScreenCaptureForm())
            {
                // ��ʾ���Ǵ��壬���û���ѡ����
                if (scf.ShowDialog() == DialogResult.OK && scf.IsConfirmed)
                {
                    Bitmap capturedImage = scf.CaptureSelectedRegion();
                    if (capturedImage != null)
                    {
                        // �����ͼ���ļ�
                        try
                        {
                            // ��ȡ��������ļ��е�·��
                            string currentDirectory = Directory.GetCurrentDirectory();
                            string projectPath = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
                            string targetFolder = Path.Combine(projectPath, "DetectionImage");

                            // ����ļ��в����ڣ��򴴽�
                            if (!Directory.Exists(targetFolder))
                            {
                                Directory.CreateDirectory(targetFolder);
                            }
                            string randomFileName = Utils.GenerateRandomFileName(8);
                            // �����ļ�·��
                            string filePath = Path.Combine(targetFolder, randomFileName + ".jpeg");

                            capturedImage.Save(filePath, ImageFormat.Jpeg);
                            MessageBox.Show("��ͼ�ѱ��棺" + filePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"��������ʱ��������: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void UsageGuide_Click(object sender, EventArgs e)
        {
            MessageBox.Show("������������ʹ��ָ�ϡ��������϶����ڵ�����λ�ã��Ҽ�������Է��ʹ��ܲ˵���");
        }

        private void Feedback_Click(object sender, EventArgs e)
        {
            MessageBox.Show("����������κ����⣬����ϵ���ǽ��й��Ϸ�����");
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
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
                string x, y, AutoOpen, ResultResident, bootup;
                ConfigHelper.GetSetting("FormLocationX", out x);
                ConfigHelper.GetSetting("FormLocationY", out y);
                ConfigHelper.GetSetting("AutoOpen", out AutoOpen);
                ConfigHelper.GetSetting("ResultResident", out ResultResident);
                Common.ConfigHelper.GetSetting("BootUp", out bootup);
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(int.Parse(x), int.Parse(y));
                if (AutoOpen != null) this.AutoOpen.Checked = bool.Parse(AutoOpen);
                if (ResultResident != null) this.ResultResident.Checked = bool.Parse(ResultResident);
                if (bootup != null) BootUp.Checked = bool.Parse(bootup);
            }
            catch (Exception ex)
            {
                this.StartPosition = FormStartPosition.Manual;
                Location = new Point(800, 50);
                MessageBox.Show($"�ָ�����λ��ʱ��������: {ex.Message}");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["FormLocationX"].Value = this.Location.X.ToString();
                config.AppSettings.Settings["FormLocationY"].Value = this.Location.Y.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"���洰��λ��ʱ��������: {ex.Message}");
            }
        }



        // ȫ������¼���������
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
                    return IntPtr.Zero; // ����Ƶ���ĵ��
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
            try
            {
                string screenshotPath = await Task.Run(() => CaptureScreenshot(mousePosition));
                if (!string.IsNullOrEmpty(screenshotPath))
                {
                    int screenHeight = Screen.PrimaryScreen.Bounds.Height;
                    await Task.Run(() => OCRService.OCRSercive(screenshotPath, mousePosition, screenHeight));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"��������з�������: {ex.Message}");
            }
        }

        private string CaptureScreenshot(Point mousePosition)
        {
            using (Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(Point.Empty, Point.Empty, bitmap.Size);
                }

                //��ͼ�����Ԥ����
                Bitmap processedBitmap = ImageProcessService.ConvertToGrayscale(bitmap);
                processedBitmap = ImageProcessService.ApplyGammaCorrection(processedBitmap);
                string screenshotPath = Path.Combine(Path.GetTempPath(), "temp.png");
                processedBitmap.Save(screenshotPath, ImageFormat.Png);
                return screenshotPath;
            }   
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            UnhookWindowsHookEx(_hookID);
        }
    }
}