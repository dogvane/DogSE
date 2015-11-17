using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
