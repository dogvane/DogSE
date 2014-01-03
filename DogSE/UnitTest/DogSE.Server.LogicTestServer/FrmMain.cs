using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DogSE.Library.Log;
using DogSE.Server.Core;

namespace DogSE.Server.LogicTestServer
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            Logs.ConfigLogFile("error.log", LogMessageType.MSG_DEBUG);
        }

        private WorldBase gameWorld = new WorldBase();

        private void FrmMain_Load(object sender, EventArgs e)
        {
            gameWorld.StartWorld();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            gameWorld.StopWorld();
        }
    }
}
