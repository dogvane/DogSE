using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeAge.Server.Entity.Character;

namespace TradeAge.Server.Entity.GameEvent
{
    /// <summary>
    /// 玩家事件
    /// </summary>
    public static class PlayerEvents
    {

        /// <summary>
        /// 触发玩家进入游戏事件
        /// 通常让各个模块进入加载模块
        /// </summary>
        /// <param name="player"></param>
        public static void OnEnterGame(Player player)
        {
            var temp = EnterGame;
            if (temp != null)
                temp(player);
        }


        /// <summary>
        /// 玩家进入游戏
        /// </summary>
        public static event Action<Player> EnterGame;

        /// <summary>
        /// 触发离开游戏事件
        /// </summary>
        /// <param name="player"></param>
        public static void OnExitGame(Player player)
        {
            var temp = ExitGame;
            if (temp != null)
                temp(player);
        }

        /// <summary>
        /// 玩家离开游戏
        /// </summary>
        public static event Action<Player> ExitGame;
    }
}
