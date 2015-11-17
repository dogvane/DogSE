using System;
using System.Collections.Generic;
using DogSE.Server.Core.Net;
using TradeAge.Server.Entity;
using TradeAge.Server.Entity.Character;
using TradeAge.Server.Entity.Common;
using TradeAge.Server.Entity.GameEvent;
using TradeAge.Server.Interface.Client;
using IScene = TradeAge.Server.Interface.Server.IScene;

namespace TradeAge.Server.Logic.Scene
{
    public class SecneModule : IScene
    {
        #region ILogicModule 成员

        public string ModuleId
        {
            get { return "SecneModule"; }
        }

        public void Initializationing()
        {
            PlayerEvents.EnterGame += OnPlayerEnterGame;
            PlayerEvents.ExitGame += OnPlayerExitGame;
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

        /// <summary>
        /// 玩家离开游戏
        /// </summary>
        /// <param name="player"></param>
        private void OnPlayerExitGame(Player player)
        {
            //  广播给场景里的其它玩家有玩家离开
            var onlinePlayer = WorldEntityManager.OnlinePlayers.ToArray();
            foreach (var p in onlinePlayer)
            {
                if (p.Id != player.Id)
                {
                    ClientProxy.Scene.SpriteLeave(p.NetState, player.Id);
                }
            }
        }


        /// <summary>
        /// 玩家进入游戏
        /// </summary>
        /// <param name="player"></param>
        private void OnPlayerEnterGame(Player player)
        {
            

            List<SceneSprite> existsPlayers = new List<SceneSprite>();

            //  广播给场景里的其它玩家有玩家进入
            var onlinePlayer = WorldEntityManager.OnlinePlayers.ToArray();
            foreach (var p in onlinePlayer)
            {
                if (p.Id != player.Id)
                {
                    existsPlayers.Add(p.GetSceneSprite());
                    ClientProxy.Scene.SpriteEnter(p.NetState, player.GetSceneSprite());
                }
            }

            //  给当前进入的玩家当前场景里有的玩家
            ClientProxy.Scene.SpriteEnter(player.NetState, existsPlayers.ToArray());
        }


        /// <summary>
        /// 玩家通报一次移动
        /// </summary>
        /// <param name="netstate">网络客户端</param>
        /// <param name="time">当前客户端时间</param>
        /// <param name="postion">当前位置</param>
        /// <param name="direction">朝向（方向以及速度）</param>
        public void OnMove(NetState netstate, DateTime time, Vector2 postion, Vector2 direction)
        {
            var player = (Player)netstate.Player;

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
                    ClientProxy.Scene.SpriteMove(p.NetState, player.Id, time, postion, direction);
                }
            }
        }

    }
}