using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeAge.Client.Entity.Character;

namespace TradeAge.Client.Controller
{
    /// <summary>
    /// 和玩家自身相关的模型数据
    /// </summary>
    public class PlayerModel
    {
        public PlayerModel()
        {
            Player = new SimplePlayer();
        }

        /// <summary>
        /// 玩家自己
        /// </summary>
        public SimplePlayer Player { get; set; }
    }
}
