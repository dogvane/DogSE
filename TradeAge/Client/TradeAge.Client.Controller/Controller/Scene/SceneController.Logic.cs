using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using DogSE.Library.Log;
using DogSE.Library.Maths;
using TradeAge.Client.Entity.Character;
using TradeAge.Client.Entity.Ship;

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

        internal override void OnEnterSceneInfo(SimplePlayer player)
        {
            controller.Model.Player = player;
        }

        internal override void OnSpriteEnter(SceneSprite[] sprites)
        {
            List<SceneSprite> add = new List<SceneSprite>();

            foreach (var item in sprites)
            {
                if (m_sprites.All(o => o.Id != item.Id))
                {
                    add.Add(item);
                    m_sprites.Add(item);
                }
            }

            if (add.Count > 0 && SpriteEnter != null)
                SpriteEnter(add.ToArray());
        }


        internal override void OnSpriteLeave(int[] spriteId)
        {
            var remove = m_sprites.Where(o => spriteId.Contains(o.Id)).ToArray();
            if (remove.Length > 0)
            {
                m_sprites.RemoveAll(o => spriteId.Contains(o.Id));

                if (SpriteLeave != null)
                    SpriteLeave(remove);
            }
        }

        internal override void OnSpriteMove(int spriteId, DateTime time, Vector3 postion, Quaternion rotation,
            float speed, float rotationRate, SpeedUpTypes speedUpType)
        {
            var sprite = m_sprites.FirstOrDefault(o => o.Id == spriteId);
            if (sprite == null)
            {
                Logs.Error("本地没发现精灵id{0}", spriteId);
                return;
            }

            sprite.Postion = postion;
            sprite.Rotation = rotation;
            sprite.Speed = speed;
            sprite.RotationRate = rotationRate;
            sprite.SpeedUpTypes = speedUpType;

            if (SpriteMove != null)
                SpriteMove(time, sprite);
        }

        private List<SceneSprite> m_sprites = new List<SceneSprite>();

        /// <summary>
        /// 当前场景里的精灵
        /// </summary>
        public SceneSprite[] Sprites
        {
            get { return m_sprites.ToArray(); }
        }

        /// <summary>
        /// 精灵进入事件
        /// </summary>
        public event Action<SceneSprite[]> SpriteEnter;

        /// <summary>
        /// 精灵离开事件
        /// </summary>
        public event Action<SceneSprite[]> SpriteLeave;

        /// <summary>
        /// 精灵位置发生移动
        /// </summary>
        /// <remarks>
        /// 这个参数在u3d做移动模拟的时候，再考虑应该传出哪些参数
        /// </remarks>
        public event Action<DateTime, SceneSprite> SpriteMove;
    }
}
