using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core.Net;

namespace TradeAge.Client.Logic.Controller
{
    /// <summary>
    /// 控制器的基类
    /// </summary>
    public class BaseController
    {
        /// <summary>
        /// 网络连接对象
        /// </summary>
        public NetState NetState { get; set; }


    }
}
