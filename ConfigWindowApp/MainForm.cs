using Common;
using CommonWinForm;
using System.Drawing.Imaging;

namespace ConfigWindowApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void DetectionBtn_Click(object sender, EventArgs e)
        {
            // ��С�������壬ȷ����ͼʱ�����岻��������Ļ��
            this.WindowState = FormWindowState.Minimized;
            Application.DoEvents();
            System.Threading.Thread.Sleep(200);
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
                            string currentDirectory = Directory.GetCurrentDirectory();
                            string projectPath = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
                            string targetFolder = Path.Combine(projectPath, "DetectionImage");

                            // ����ļ��в����ڣ��򴴽�
                            if (!Directory.Exists(targetFolder))
                            {
                                Directory.CreateDirectory(targetFolder);
                            }
                            string randomFileName = Utils.GenerateRandomFileName(8);
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
            this.WindowState = FormWindowState.Normal;
        }
    }
}
