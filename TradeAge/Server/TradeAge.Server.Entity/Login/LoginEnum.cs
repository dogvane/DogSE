using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeAge.Server.Entity.Login
{
    /// <summary>
    /// 登陆返回结果
    /// </summary>
    public enum LoginResult
    {
        /// <summary>
        /// 登陆成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 登陆失败
        /// </summary>
        Fail = 1,

        /// <summary>
        /// 密码或者账号错误
        /// </summary>
        PassOrAccountError = 2,
    }
}
