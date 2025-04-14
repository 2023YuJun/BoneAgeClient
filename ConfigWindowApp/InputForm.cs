using Common;
using Common.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConfigWindowApp
{
    public partial class InputForm : Form
    {
        public string InputText { get; private set; }
        public InputForm(string Formtext, string Labeltext)
        {
            InitializeComponent();
            this.Text = Formtext;
            label.Text = Labeltext;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            InputText = textBox1.Text;
            DialogResult = DialogResult.OK;
            Close();
        }


        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OKBtn.PerformClick();
            }
        }

        private void InputForm_Load(object sender, EventArgs e)
        {
            var settings = ConfigProvider.Settings.GetConfig();
            if(label.Text == "修改服务器IP")
            {
                string IP = settings.ServiceIP;
                textBox1.Text = IP;
            }
            else if(label.Text == "修改识别方式")
            {
                string IFZ = settings.IFZ;
                textBox1.Text = IFZ;
            }
            else if (label.Text == "修改正则表达式")
            {
                string RE = settings.RE;
                textBox1.Text = RE;
            }
        }
    }
}
