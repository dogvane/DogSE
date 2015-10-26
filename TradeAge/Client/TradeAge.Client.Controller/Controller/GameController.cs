using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using TradeAge.Client.Controller.Login;

namespace TradeAge.Client.Controller
{
    /// <summary>
    /// 游戏控制器
    /// </summary>
    public class GameController
    {
        /// <summary>
        /// 游戏控制器
        /// </summary>
        public GameController()
        {
            Net = new NetController();
            Net.Tag = this;

            Login = new LoginController(Net);
        }

        /// <summary>
        ///     游戏控制器的网络控制器部分
        /// </summary>
        public NetController Net { get; set; }


        /// <summary>
        /// 登陆相关的接口在这里
        /// </summary>
        public LoginController Login { get; private set; }
    }
}
