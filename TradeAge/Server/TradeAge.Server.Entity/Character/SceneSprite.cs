using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// 场景里的精灵
    /// </summary>
    public class SceneSprite
    {
        /// <summary>
        /// id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 场景精灵的类型
        /// </summary>
        public SpriteType SpriteType { get; set; }

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

    }

    /// <summary>
    /// 精灵的类型
    /// </summary>
    public enum SpriteType
    {
        /// <summary>
        /// 玩家
        /// </summary>
        Player = 1,

        /// <summary>
        /// Npc
        /// </summary>
        Npc = 2,

        /// <summary>
        /// 怪物
        /// </summary>
        Monster = 3,

        /// <summary>
        /// 游戏里的物品
        /// </summary>
        GameObject = 4,
    }
}
