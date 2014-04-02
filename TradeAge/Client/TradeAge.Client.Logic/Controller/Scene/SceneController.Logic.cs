using System.Collections.Generic;
using System.Linq;
using DogSE.Common;
using TradeAge.Client.Entity.Character;
using System;
using DogSE.Library.Log;

namespace TradeAge.Client.Logic.Controller.Scene
{
    public partial class SceneController : BaseSceneController
    {

        internal override void OnEnterSceneInfo(Vector3 postion, Vector3 direction)
        {
            if (PlayerPostionChange != null)
            {
                PlayerPostionChange(this, new PlayerPostionChangeEventArgs
                {
                    Postion = postion,
                    Direction = direction,
                });
            }
        }

        /// <summary>
        /// 玩家自己的位置变化
        /// </summary>
        public class PlayerPostionChangeEventArgs : EventArgs
        {
            public Vector3 Postion { get; internal set; }

            public Vector3 Direction { get; internal set; }
        }

        public event EventHandler<PlayerPostionChangeEventArgs> PlayerPostionChange;

        //记录场景内的玩家
        private List<SimplePlayer> scenePlayerLists = new List<SimplePlayer>();


        internal override void OnSpriteEnter(SimplePlayer simplePlayer)
        {
            //  这里需要把SimplePlayer进行包装
            //  目前代码简化就不进行处理，直接抛给逻辑层

            var exitsts = scenePlayerLists.FirstOrDefault(o => o.Id == simplePlayer.Id);
            if (exitsts != null)
            {

            }
            else
            {
                scenePlayerLists.Add(simplePlayer);
                if (SpriteEnter != null)
                {
                    SpriteEnter(this, new SpriteEnterEventArgs { Player = simplePlayer});
                }
            }
        }

        public class SpriteEnterEventArgs : EventArgs
        {
            public SimplePlayer Player { get; internal set; }
        }

        /// <summary>
        /// 有精灵（玩家）进入
        /// </summary>
        public event EventHandler<SpriteEnterEventArgs> SpriteEnter;


        internal override void OnSpriteMove(int playerId, Vector3 postion, Vector3 direction)
        {
            var player = scenePlayerLists.FirstOrDefault(o => o.Id == playerId);
            if (player == null)
            {
                Logs.Warn("玩家 {0} 的移动数据，没有再本地找到对应的对象", playerId);
                return;
            }

            player.Direction = direction;
            player.Postion = postion;

            if (SpriteMove != null)
            {
                SpriteMove(this, new SpriteMoveEventArgs
                {
                    Player = player,
                    Direction = direction,
                    Postion = postion,
                });
            }
        }

        public class SpriteMoveEventArgs : EventArgs
        {
            public SimplePlayer Player { get; internal set; }

            public Vector3 Postion { get; internal set; }

            public Vector3 Direction { get; internal set; }
        }

        public event EventHandler<SpriteMoveEventArgs> SpriteMove;

        internal override void OnSpriteLeave(int playerId)
        {

            var exitsts = scenePlayerLists.FirstOrDefault(o => o.Id == playerId);
            if (exitsts != null)
            {
                scenePlayerLists.Remove(exitsts);
                if (SpriteLeave != null)
                {
                    SpriteLeave(this, new SpriteLeaveEventArgs {Player = exitsts});
                }
            }
            else
            {
                Logs.Info("玩家 {0} 离开，本地没有找到缓存对象", playerId);
            }
        }


        public class SpriteLeaveEventArgs : EventArgs
        {
            public SimplePlayer Player { get; internal set; }
        }

        /// <summary>
        /// 有精灵（玩家）离开
        /// </summary>
        public event EventHandler<SpriteLeaveEventArgs> SpriteLeave;
    }
}