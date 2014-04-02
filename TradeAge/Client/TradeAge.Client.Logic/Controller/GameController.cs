using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using TradeAge.Client.Logic.Controller.Login;
using TradeAge.Client.Logic.Controller.Scene;

namespace TradeAge.Client.Logic.Controller
{
    /// <summary>
    /// 游戏控制器
    /// </summary>
    public class GameController
    {
        public GameController()
        {
            Net = new NetController();
            Login = new LoginController(Net);
            Scene = new SceneController(Net);
        }

        /// <summary>
        /// 游戏控制器的网络控制器部分
        /// </summary>
        public NetController Net { get; set; }

        /// <summary>
        /// 登陆控制器
        /// </summary>
        public LoginController Login { get; private set; }


        /// <summary>
        /// 场景控制器
        /// </summary>
        public SceneController Scene { get; private set; }
    }
}
