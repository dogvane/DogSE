using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DogSE.Library.Log;
using DogSE.Server.Core;
using DogSE.Server.Core.Config;

namespace TradeAge.Server.Game
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            ServerConfig.Tcp = new []
            {
                new TcpConfig
                {
                    Host = "127.0.0.1",
                    Port = 4530,
                }
            };

            Logs.ConfigLogFile("tradeage.log");

            GameServerService.StartGame(new WorldBase());
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameServerService.StopGame();
        }
    }
}
