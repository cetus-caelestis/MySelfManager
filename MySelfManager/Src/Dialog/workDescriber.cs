using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySelfManager
{
    public partial class WorkDescriber : Form
    {
        private static string[] m_resultStrings = null;
        public static string[] ResultStrings { get { return m_resultStrings; } }

        //public static event Action<string> OnCommitedResultStr;

        public WorkDescriber()
        {
            // 初期化しておく
            m_resultStrings = null;
            InitializeComponent();
        }
        // 確定
        private void button1_Click(object sender, EventArgs e)
        {
            //OnCommitedResultStr(textBox1.Text);
        }

        private void WorkDescriber_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK) return;

            var namelines = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (var name in namelines)
            {
                if (!MyUtility.Utility.IsValidXmlName(name))
                {
                    MessageBox.Show("無効な文字が含まれています：\n", "注意");
                    e.Cancel = true;
                    return;
                }
            }
            m_resultStrings = namelines;
        }
    }
}
