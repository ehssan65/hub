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


namespace server1
{
    public partial class Form1 : Form
    {
        
        Socket serversoc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Socket acceptedsocket;
        static Socket a1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Socket serversoc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serversoc.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
                serversoc.Listen(1);
                acceptedsocket = serversoc.Accept();
                a1 = acceptedsocket;
                button1.Enabled = false;

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
                
                byte[] recieve = new byte[1024];
                int bytecnt = a1.Receive(recieve);
                string text = Encoding.UTF8.GetString(recieve, 0, bytecnt);
                //Console.WriteLine("Received from client: " + text);
                this.Invoke(new MethodInvoker(delegate ()
                {
                    listBox1.Items.Add("client:" + text);
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
                string response = message.Text; //Console.ReadLine();
                byte[] respBuffer = Encoding.UTF8.GetBytes(response);
                listBox1.Items.Add("server:"+response);
                listBox1.Refresh();
                acceptedsocket.Send(respBuffer);              
                //acceptedsocket.Close();
                //socnum = 0;
            //}
        }

        private void message_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                string response = message.Text; //Console.ReadLine();
                byte[] respBuffer = Encoding.UTF8.GetBytes(response);
                listBox1.Items.Add("server:" + response);
                listBox1.Refresh();
                acceptedsocket.Send(respBuffer);
                message.Text = "";
            }
        }
    }
}
