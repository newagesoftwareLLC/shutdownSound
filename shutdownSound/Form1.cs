using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shutdownSound
{
    public partial class Form1 : Form
    {
        [DllImport("winmm.dll")]
        private static extern uint mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

        int dur1 = 0;
        int dur2 = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.exitFile;
            dur1 = GetSoundLength(textBox1.Text);
            textBox2.Text = Properties.Settings.Default.startFile;
            dur2 = GetSoundLength(textBox2.Text);

            if (textBox2.Text != "")
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(textBox2.Text);
                player.Play();
            }
        }

        public static int GetSoundLength(string fileName)
        {
            StringBuilder lengthBuf = new StringBuilder(32);

            mciSendString(string.Format("open \"{0}\" type waveaudio alias wave", fileName), null, 0, IntPtr.Zero);
            mciSendString("status wave length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
            mciSendString("close wave", null, 0, IntPtr.Zero);

            int length = 0;
            int.TryParse(lengthBuf.ToString(), out length);

            return length;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (textBox1.Text != "")
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(textBox1.Text);
                player.Play();
                System.Threading.Thread.Sleep(dur1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Shutdown Audio File";
            openFileDialog1.Filter = "WAV files (*.wav) | *.wav";
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBox1.Text = openFileDialog1.FileName;
            Properties.Settings.Default.exitFile = textBox1.Text;
            if (textBox1.Text != "")
                dur1 = GetSoundLength(textBox1.Text);
            Properties.Settings.Default.Save();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                this.Hide();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Place this program in your" + Environment.NewLine + "\"C:\\ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\StartUp\"" + Environment.NewLine + "folder.");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //if (textBox1.Text != "")
                this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog2.Title = "Startup Audio File";
            openFileDialog2.Filter = "WAV files (*.wav) | *.wav";
            openFileDialog2.FileName = "";
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            textBox2.Text = openFileDialog2.FileName;
            Properties.Settings.Default.startFile = textBox2.Text;
            if (textBox1.Text != "")
                dur2 = GetSoundLength(textBox2.Text);
            Properties.Settings.Default.Save();
        }
    }
}
