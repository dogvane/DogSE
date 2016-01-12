using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeAge.Server.Entity.Character;

namespace TradeAge.Server.Logic.Scene
{
    /// <summary>
    /// 
    /// </summary>
    static class SecneUtils
    {
        /// <summary>
        /// 将玩家对象转换为场景精灵对象
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static SceneSprite GetSceneSprite(this Player player)
        {
            var ss= player.GetComponent<SceneSprite>();

            if (ss == null)
            {
                ss = new SceneSprite
                {
                    Id = player.Id,
                    Name = player.Name,
                    SpriteType = SpriteType.Player,
                };
                player.RegisterComponent(ss);
            }

            ss.Rotation = player.Rotation;
            ss.Postion = player.Postion;

            return ss;
        }

        /// <summary>
        /// 获得玩家的简单信息
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static SimplePlayer GetSimplePlayer(this Player player)
        {
            var ss = player.GetComponent<SimplePlayer>();
            
            if (ss == null)
            {
                ss = new SimplePlayer
                {
                    Id = player.Id,
                    Name = player.Name,
                    Sex = player.Sex,
                };
                player.RegisterComponent(ss);
            }

            ss.Rotation = player.Rotation;
            ss.Postion = player.Postion;

            return ss;

        }
    }
}
