using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeAge.Server.Interface.Client
{
    /// <summary>
    /// 客户端接口代理
    /// </summary>
    public static class ClientProxy
    {
        /// <summary>
        /// 登陆代理接口
        /// </summary>
        public static ILogin Login { get; set; }

        /// <summary>
        /// 场景的控制器接口
        /// </summary>
        public static IScene Scene { get; set; }

        /// <summary>
        /// 游戏相关的接口
        /// </summary>
        public static IGame Game { get; set; }
    }
}
