using System;
using System.Collections.Generic;
using DogSE.Library.Maths;
using DogSE.Server.Core.Net;
using TradeAge.Server.Entity;
using TradeAge.Server.Entity.Character;
using TradeAge.Server.Entity.Common;
using TradeAge.Server.Entity.GameEvent;
using TradeAge.Server.Entity.Ship;
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

        /// <summary>
        /// 玩家通报一次移动
        /// </summary>
        /// <remarks>
        /// 我也不想要这么多参数
        /// 但是unity3d的对象的参数，不是以属性的方式存在
        /// 不能被协议代码生成工具识别，只好把参数拆开
        /// </remarks>
        /// <param name="netstate">网络客户端</param>
        /// <param name="time">当前客户端时间</param>
        /// <param name="postion"></param>
        /// <param name="rotation"></param>
        /// <param name="speed"></param>
        /// <param name="rotationRate"></param>
        /// <param name="speedUpType"></param>
        public void OnMove(NetState netstate, DateTime time, Vector3 postion, Quaternion rotation, float speed, float rotationRate,
            SpeedUpTypes speedUpType)
        {
            var player = (Player)netstate.Player;

            if (player == null)
                return;

            //Console.WriteLine(postion.ToString() + direction.ToString());
            //  理论上这里需要对玩家的位置做验证，这里先忽略，全部信任
            //player.Postion = postion;
            //player.Direction = direction;

            //  广播给场景里的其它玩家
            var onlinePlayer = WorldEntityManager.OnlinePlayers.ToArray();
            foreach (var p in onlinePlayer)
            {
                if (p.Id != player.Id)
                {
                    ClientProxy.Scene.SpriteMove(p.NetState, player.Id, time, postion, rotation
                        , speed, rotationRate, speedUpType);
                }
            }
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

    }
}