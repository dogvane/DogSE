using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Common;

namespace TradeAge.Server.Entity.Character
{
    /// <summary>
    /// 账号信息，账号和玩家对象不是同一个对象，虽然他们的id可能是一样的
    /// 但是账号信息只用于登录时用于登录验证时用
    /// </summary>
    public class Account:IDataEntity
    {
        /// <summary>
        /// 账号id，和玩家id里路上一一对应
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 账号名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账号密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 账号创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 服务器id
        /// </summary>
        public int ServerId { get; set; }

        /// <summary>
        /// 玩家id
        /// </summary>
        /// <remarks>
        /// 因为玩家对象与账号对象是一对一对应的
        /// 所以这里只用一个 playerId对应
        /// 如果是一对多则可以改成数组
        /// </remarks>
        public int PlayerId { get; set; }
    }
}
