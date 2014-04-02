using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if Server 
namespace TradeAge.Server.Entity.Character
#else
namespace TradeAge.Client.Entity.Character
#endif
{
    /// <summary>
    /// 性别
    /// </summary>
    public enum Sex
    {
        /// <summary>
        /// 男性
        /// </summary>
        Male = 0,

        /// <summary>
        /// 女性
        /// </summary>
        Female = 1,
    }
}
