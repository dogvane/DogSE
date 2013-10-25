using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeAge.Server.Entity.Login
{
    /// <summary>
    /// 登陆服务器返回结果
    /// </summary>
    public enum LoginServerResult
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

    /// <summary>
    /// 创建玩家返回结果
    /// </summary>
    public enum CraetePlayerResult
    {
        /// <summary>
        /// 创建成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 创建失败
        /// </summary>
        Fail = 1,

        /// <summary>
        /// 名字已经存在
        /// </summary>
        NameExists = 2,


    }
}
