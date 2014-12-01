using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Common;
using TradeAge.Server.Entity.Common;

#if Server 
namespace TradeAge.Server.Entity.Character
#else
namespace TradeAge.Client.Entity.Character
#endif
{
    /// <summary>
    /// 简单玩家信息
    /// </summary>
    public class SimplePlayer
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 玩家当前的位置
        /// </summary>
        public Vector3 Postion { get; set; }

        /// <summary>
        /// 方向已经速度
        /// </summary>
        public Vector3 Direction { get; set; }

        /// <summary>
        /// 玩家的id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 玩家对应的账号id
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }
    }
}
