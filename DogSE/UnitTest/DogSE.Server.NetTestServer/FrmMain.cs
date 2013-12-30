using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DogSE.Library.Log;
using DogSE.Server.Net;

namespace DogSE.Server.NetTestServer
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            Logs.ConfigLogFile("error.log", LogMessageType.MSG_DEBUG);
        }


        private Listener<NetData> m_server = new Listener<NetData>();

        private void button1_Click(object sender, EventArgs e)
        {
            m_server.StartServer(8088);
            m_server.SocketConnect += m_server_SocketConnect;
            m_server.SocketRecv += m_server_SocketRecv;
            m_server.SocketDisconnect += m_server_SocketDisconnect;
        }

        void m_server_SocketDisconnect(object sender, SocketDisconnectEventArgs<NetData> e)
        {
            e.Session.Data = null;
            m_connectCount --;
        }

        private int m_recvCount = 0;
        private int m_recvBytes = 0;
        void m_server_SocketRecv(object sender, SocketRecvEventArgs<NetData> e)
        {
            //  数据原样送回
            e.Session.SendPackage(e.Buffer);
            m_recvCount++;
            m_recvBytes += e.Buffer.Length;
        }

        void m_server_SocketConnect(object sender, SocketConnectEventArgs<NetData> e)
        {
            e.Session.Data = new NetData();
            m_connectCount++;
        }

        private int m_connectCount;

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = string.Format("{0} / {1} ({2})", m_connectCount, m_recvCount, m_recvBytes);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            m_server.StartServer("192.168.2.44", 8088);
            m_server.SocketConnect += m_server_SocketConnect;
            m_server.SocketRecv += m_server_SocketRecv;
            m_server.SocketDisconnect += m_server_SocketDisconnect;
        }


    }

    public class NetData
    {
        public int Id { get; set; }
    }
}
