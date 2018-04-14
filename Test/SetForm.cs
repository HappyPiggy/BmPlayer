using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class SetForm : Form
    {
        public static MoneyPlayer player;
        private Settings settings=new Settings();
        //public string groupKeyword="";
        //public int maxBeishu=0;
        //public int m=3;//钱的大小  分 角 元
        //public int mode = 1;
        //public int startBeishu = 1;
        
        public SetForm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SetForm_Load(object sender, EventArgs e)
        {
            player = new MoneyPlayer();
            UpdatePic();

            InitCombox();

        }



        private void InitCombox()
        {
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 3;
            comboBox3.SelectedIndex=1;
            comboBox4.SelectedIndex = 1;
            comboBox5.SelectedIndex = 3;
            comboBox6.SelectedIndex = 2;
            comboBox7.SelectedIndex = 2;
            comboBox8.SelectedIndex = 2;
            comboBox9.SelectedIndex = 1;
            comboBox10.SelectedIndex = 1;
            comboBox11.SelectedIndex = 3;
            comboBox12.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var username = textBox1.Text;
            var password = textBox2.Text;
            var validCode = textBox3.Text;
            var res = player.LoginAccount(validCode, username, password);
            MessageBox.Show(res);

            if (res.Contains("成功"))
            {
                player.UpdatePlayerInfo();
                player.SetStartMoney();
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                groupBox4.Enabled = true;

            }
            else
            {
                UpdatePic();
            }
            
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            UpdatePic();
        }


        private void UpdatePic()
        {
            player.DownLoadValidCodePic();
           // pictureBox1.Image = Image.FromFile(@"E:\test\1.png");
            pictureBox1.Image = Image.FromStream(ByteToStream(SetImageToByteArray(@"E:\ValidCode\1.png")));
        }


        //图片变成流再到picbox里
        public byte[] SetImageToByteArray(string fileName)
        {
            byte[] image = null;
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                FileInfo fileInfo = new FileInfo(fileName);
                int streamLength = (int)fs.Length;
                image = new byte[streamLength];
                fs.Read(image, 0, streamLength);
                fs.Close();
                return image;

            }
            catch
            {
                return image;
            }
        }

        public MemoryStream ByteToStream(byte[] mybyte)
        {
            MemoryStream mymemorystream = new MemoryStream(mybyte, 0, mybyte.Length);
            return mymemorystream;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Settings.mode = 1;
            //Settings.groupKeyword = textBox4.Text;
           
            //bool res = int.TryParse(textBox5.Text, out Settings.maxBeishu);

            //if (radioButton1.Checked)
            //    Settings.m = 1;
            //else if (radioButton2.Checked)
            //    Settings.m = 2;
            //else if (radioButton3.Checked)
            //    Settings.m = 3;
            //else if (radioButton4.Checked)
            //    Settings.m = 4;

            //if (res == false || Settings.groupKeyword == "" || Settings.maxBeishu < 1)
            //    MessageBox.Show("请检查:\n1.群关键字是否为空\n2.倍数是否大于等于一倍");
            //else
            //    MessageBox.Show("模式一设置成功！");

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

                Settings.groupKeyword = textBox4.Text;
                Settings.gwjj.startBeishu = int.Parse(comboBox1.SelectedItem.ToString());
                Settings.gwjj.maxBeishu = int.Parse(comboBox2.SelectedItem.ToString());
                Settings.gwjj.m = comboBox3.SelectedIndex + 1;

                if (Settings.gwjj.startBeishu == 1)
                {
                    Settings.gwjj.flag = 1;
                }
                else
                {
                    Settings.gwjj.flag = 2;
                }

                if (Settings.groupKeyword == "")
                    MessageBox.Show("请检查:\n1.群关键字是否为空");
                else
                    MessageBox.Show("个位掘金设置成功！");

            }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Settings.groupKeyword = textBox4.Text;
            Settings.dd.startBeishu = int.Parse(comboBox6.SelectedItem.ToString());
            Settings.dd.maxBeishu = int.Parse(comboBox5.SelectedItem.ToString());
            Settings.dd.m = comboBox4.SelectedIndex + 1;

            if (Settings.dd.startBeishu == 1)
            {
                Settings.dd.flag = 1;
            }
            else
            {
                Settings.dd.flag = 2;
            }

            if (Settings.groupKeyword == "")
                MessageBox.Show("请检查:\n1.群关键字是否为空");
            else
                MessageBox.Show("独胆设置成功！");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.listenerList[1] = checkBox2.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.listenerList[0] = checkBox1.Checked;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Settings.UpdateConfig();
            MessageBox.Show("已重载正则表达式");
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Settings.listenerList[2] = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Settings.listenerList[3] = checkBox4.Checked;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Settings.groupKeyword = textBox4.Text;
            Settings.hszl.startBeishu = int.Parse(comboBox9.SelectedItem.ToString());
            Settings.hszl.maxBeishu = int.Parse(comboBox8.SelectedItem.ToString());
            Settings.hszl.m = comboBox7.SelectedIndex + 1;

            if (Settings.hszl.startBeishu == 1)
            {
                Settings.hszl.flag = 1;
            }
            else
            {
                Settings.hszl.flag = 2;
            }

            if (Settings.groupKeyword == "")
                MessageBox.Show("请检查:\n1.群关键字是否为空");
            else
                MessageBox.Show("后三组六设置成功！");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Settings.groupKeyword = textBox4.Text;
            Settings.bjsc.startBeishu = int.Parse(comboBox12.SelectedItem.ToString());
            Settings.bjsc.maxBeishu = int.Parse(comboBox11.SelectedItem.ToString());
            Settings.bjsc.m = comboBox10.SelectedIndex + 1;

            if (Settings.bjsc.startBeishu == 1)
            {
                Settings.bjsc.flag = 1;
            }
            else
            {
                Settings.bjsc.flag = 2;
            }

            if (Settings.groupKeyword == "")
                MessageBox.Show("请检查:\n1.群关键字是否为空");
            else
                MessageBox.Show("北京赛车设置成功！");
        }

         //   MessageBox.Show(Settings.url.PeriodURL);
        
    }
}
