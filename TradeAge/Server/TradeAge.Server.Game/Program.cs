using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TradeAge.Server.Game
{
    /// <summary>
    /// 游戏的启动项目，同时也是一个服务器状态的监视窗口
    /// </summary>
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
