using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Library.Maths;
using TradeAge.Client.Entity.Character;

namespace TradeAge.Client.Simulator.Test
{
    class SimuPlayer:BaseLoginTest
    {
        public void Start2(string userName, string pw)
        {
            Start(userName, pw);

            Controller.Scene.SpriteEnter += OnSpriteEnter;
            Controller.Scene.SpriteLeave += OnSpriteLeave;
            Controller.Scene.SpriteMove += OnSpriteMove;
        }

        private void OnSpriteMove(DateTime arg1, SceneSprite sprite)
        {
            Console.WriteLine("看到玩家 {0} 在 {1} 移动中", sprite.Name, sprite.Postion);
        }

        private void OnSpriteLeave(SceneSprite[] sprites)
        {
            foreach (var s in sprites)
            {
                Console.WriteLine("看到玩家 {0} 离开游戏", s.Name);
            }
        }

        private void OnSpriteEnter(SceneSprite[] sprites)
        {
            foreach (var s in sprites)
            {
                Console.WriteLine("看到玩家 {0} 进入游戏", s.Name);
            }
        }

        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 TargetPostion { get; set; }

        /// <summary>
        /// 获得到某个目标点的角度
        /// </summary>
        public void UpdateToTargetAngle()
        {
            
        }

        private const float 警戒距离 = 100f;

        /// <summary>
        /// 找到离自己最近的精灵
        /// </summary>
        /// <returns>
        /// 如果没有找到，返回null
        /// </returns>
        public SceneSprite FindNearSprite()
        {
            var player = Controller.Model.Player;
            float minDistance = 警戒距离;
            SceneSprite retSprite = null;

            foreach (var sp in Controller.Scene.Sprites)
            {
                var distance = Vector3.Distance(sp.Postion, player.Postion);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    retSprite = sp;
                }
            }

            return retSprite;
        }
    }
}
