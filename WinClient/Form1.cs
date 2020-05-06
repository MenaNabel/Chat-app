using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinClient
{
    public partial class Form1 : Form
    {
        private HubConnection _connection;
        private IHubProxy _chatHub;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            _connection = new HubConnection("https://localhost:44330/");
            _chatHub = _connection.CreateHubProxy("chat");
            _chatHub.On("receiveMessage", msg => OnReceive(msg));
            _connection.Start().Wait();
            btnConnect.Enabled = false;
        }

        private void OnReceive(string msg)
        {
            listBox1.BeginInvoke(new Action(()=> { listBox1.Items.Add(msg); }));//.Add(msg);
        }

        private void listConv_Click(object sender, EventArgs e)
        {
            _chatHub.Invoke("sendMessage", txtMesssage.Text);
        }
    }
}
