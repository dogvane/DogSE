using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Ship;
using Assets.Scripts.UI.Form;
using DogSE.Library.Time;
using TradeAge.Client.Entity.Template;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// 游戏的主控制器
    /// </summary>
    class Game: MonoBehaviour
    {
        /// <summary>
        /// 现阶段是模拟开发移动功能，所以将模拟的代码
        /// 放到一个demo的类里
        /// </summary>
        private SimulationMoveDemo demo = new SimulationMoveDemo();

        public static PlayerShipController PlayerShip
        {
            get { return SimulationMoveDemo.PlayerShip; }
        }

        /// <summary>
        /// 系统退出后清理数据
        /// </summary>
        private void OnDisable()
        {
            GameCenter.Release();
        }

        /// <summary>
        /// 游戏启动的时候哦
        /// </summary>
        public void Start()
        {
            GameCenter.Init();
            demo.Start();
            
        }

        public void FixedUpdate()
        {
            demo.FixedUpdate();
            ShipManager.Instatnce.Update();
        }
    }
}
