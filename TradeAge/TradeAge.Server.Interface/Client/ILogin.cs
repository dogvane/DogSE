using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeAge.Server.Entity.Login;

namespace TradeAge.Server.Interface.Client
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogin
    {
        /// <summary>
        /// 登陆返回
        /// </summary>
        /// <param name="result"></param>
        void LoginResult(LoginResult result);
    }
}
