using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Common;
using DogSE.Library.Maths;
#if Server 
using TradeAge.Server.Entity.Ship;
using TradeAge.Server.Entity.Common;
namespace TradeAge.Server.Entity.Character
#else
using TradeAge.Client.Entity.Ship;
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
        /// 当前的位置
        /// </summary>
        public Vector3 Postion { get; set; }

        /// <summary>
        /// 方向已经速度
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// 当前速度
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// 转弯速度
        /// </summary>
        public float RotationRate;

        /// <summary>
        /// 风帆等级
        /// </summary>
        public SpeedUpTypes SpeedUpTypes;

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
