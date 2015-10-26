using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using TradeAge.Client.Entity.Login;

namespace TradeAge.Client.Controller.Login
{
    /// <summary>
    /// 登陆控制器
    /// </summary>
    public partial class LoginController: BaseLoginController
    {
        private readonly GameController controller;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gc"></param>
        /// <param name="nc"></param>
        public LoginController(GameController gc, NetController nc)
            : this(nc)
        {
            controller = gc;
        }

        internal override void OnLoginServerResult(LoginServerResult result, bool isCreatePlayer)
        {
        }

        internal override void OnCreatePlayerResult(CraetePlayerResult result)
        {
        }
    }
}
