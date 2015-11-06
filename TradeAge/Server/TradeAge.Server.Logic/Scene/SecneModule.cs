using System;
using DogSE.Common;
using DogSE.Server.Core.Net;
using TradeAge.Server.Entity;
using TradeAge.Server.Entity.Character;
using TradeAge.Server.Entity.Common;
using TradeAge.Server.Interface.Client;
using IScene = TradeAge.Server.Interface.Server.IScene;

namespace TradeAge.Server.Logic.Scene
{
    public class SecneModule : Interface.Server.IScene
    {
        #region ILogicModule 成员

        public string ModuleId
        {
            get { return "SecneModule"; }
        }

        public void Initializationing()
        {
        }

        public void Initializationed()
        {
        }

        public void ReLoadTemplate()
        {
        }

        public void Release()
        {
        }

        #endregion

        #region IScene 成员

        /// <summary>
        /// 触发移动广播
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="postion"></param>
        /// <param name="direction"></param>
        public void OnMove(NetState netstate, Vector3 postion, Vector3 direction)
        {
            var player = (Player) netstate.Player;

            if (player == null)
                return;

            //Console.WriteLine(postion.ToString() + direction.ToString());
            //  理论上这里需要对玩家的位置做验证，这里先忽略，全部信任
            player.Postion = postion;
            player.Direction = direction;
            
            //  广播给场景里的其它玩家
            var onlinePlayer = WorldEntityManager.OnlinePlayers.ToArray();
            foreach (var p in onlinePlayer)
            {
                if (p.Id != player.Id)
                {
                    ClientProxy.Scene.SpriteMove(p.NetState, player.Id, postion, direction);
                }
            }
        }

        #endregion
    }
}