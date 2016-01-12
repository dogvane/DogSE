using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Form
{
    class FrmPlayerShip: MonoBehaviour
    {
        public void Start()
        {
            Init();
        }


        void Init()
        {
            labSpeed = FrmUtils.GetText("LabSpeed");
            labRotationRate = FrmUtils.GetText("LabRotationRate");
        }

        private Text labSpeed;
        private Text labRotationRate;

        /// <summary>
        /// 更新数据
        /// </summary>
        void Update()
        {
            if (labSpeed == null)
                Init();

            var ship = Game.PlayerShip;

            labSpeed.text = ship.SpeedUpType  + ship.GetCurrentSpeed().ToString("f2");

            if (ship.RotationRate != 0f)
                labRotationRate.text = ship.RotationRate.ToString("f2");
            else
                labRotationRate.text = "";
        }
    }
}
