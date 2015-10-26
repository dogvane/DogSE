using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using TradeAge.Client.Entity.Character;
using TradeAge.Client.Entity.Common;

namespace TradeAge.Client.Controller.Scene
{
    public partial class SceneController: BaseSceneController
    {
        private readonly GameController controller;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gc"></param>
        /// <param name="nc"></param>
        public SceneController(GameController gc, NetController nc)
            : this(nc)
        {
            controller = gc;
        }

        internal override void OnEnterSceneInfo(Vector3 postion, Vector3 direction)
        {
        }

        internal override void OnSpriteEnter(SimplePlayer simplePlayer)
        {
        }

        internal override void OnSpriteMove(int playerId, Vector3 postion, Vector3 direction)
        {
        }

        internal override void OnSpriteLeave(int playerId)
        {
        }
    }
}
