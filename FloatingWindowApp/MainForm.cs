using Common;
using CommonWinForm;
using System.Configuration;
using System.Drawing.Imaging;
using System.Linq;

namespace FloatingWindowApp
{
    public partial class MainForm : Form
    {
        private bool isDragging = false;
        private Point startPoint = new Point(0, 0);
        public MainForm()
        {
            InitializeComponent();
        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void SelectColor_Click(object sender, EventArgs e)
        {

        }
        private void AutoOpen_Click(object sender, EventArgs e)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings["AutoOpen"] == null)
                {
                    config.AppSettings.Settings.Add("AutoOpen", AutoOpen.Checked.ToString());
                }
                else
                {
                    config.AppSettings.Settings["AutoOpen"].Value = AutoOpen.Checked.ToString();
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
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
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings["ResultResident"] == null)
                {
                    config.AppSettings.Settings.Add("ResultResident", ResultResident.Checked.ToString());
                }
                else
                {
                    config.AppSettings.Settings["ResultResident"].Value = ResultResident.Checked.ToString();
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
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
                int x = int.Parse(ConfigurationManager.AppSettings["FormLocationX"]);
                int y = int.Parse(ConfigurationManager.AppSettings["FormLocationY"]);
                string AutoOpen = ConfigurationManager.AppSettings["AutoOpen"];
                string ResultResident = ConfigurationManager.AppSettings["ResultResident"];
                string bootup;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(x, y);
                if (AutoOpen != null) this.AutoOpen.Checked = bool.Parse(AutoOpen);
                if (ResultResident != null) this.ResultResident.Checked = bool.Parse(ResultResident);
                ConfigHelper.GetSetting("BootUp", out bootup);
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

    }
}