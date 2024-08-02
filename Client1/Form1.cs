using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client1
{
    public partial class Form1 : Form
    {
        Socket socclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Socket socclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                socclient.Connect(endpoint);

                conect.Enabled = false;
                Thread th1 = new Thread(recmess);
                th1.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void recmess()
        {
            while (true)
            {

                byte[] buffer = new byte[1024];
                int recByte = socclient.Receive(buffer);
                string recMessage = Encoding.UTF8.GetString(buffer, 0, recByte);
                //Console.WriteLine("Received from server: " + recMessage);
                //socclient.Close();
                //socnum = 0;
                this.Invoke(new MethodInvoker(delegate ()
                {
                    listBox1.Items.Add("server:" + recMessage);
                    listBox1.Refresh();
                }));
            }
        }

        private void send_Click(object sender, EventArgs e)
        {
            //int socnum = 1;
            //while (socnum == 1)
            //{
                //Console.WriteLine("enter a message:");
                //string message = Console.ReadLine();
                byte[] text = Encoding.UTF8.GetBytes(message.Text);
                socclient.Send(text);
                listBox1.Items.Add("client:"+message.Text);
                listBox1.Refresh();
                
            //}
        }

        private void message_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                byte[] text = Encoding.UTF8.GetBytes(message.Text);
                socclient.Send(text);
                listBox1.Items.Add("client:" + message.Text);
                listBox1.Refresh();
                message.Clear();
            }
        }
    }
}
