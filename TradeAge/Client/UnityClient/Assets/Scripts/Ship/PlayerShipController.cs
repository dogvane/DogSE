using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeAge.Client.Entity.Ship;
using UnityEngine;

namespace Assets.Scripts.Ship
{
    /// <summary>
    /// 玩家的船只控制器
    /// </summary>
    class PlayerShipController: ShipController
    {
        /// <summary>
        /// 更新船只的信息
        /// </summary>
        public override void UpdateShip()
        {
            var updateTime = Time.fixedDeltaTime;

            if (Input.GetKey(KeyCode.A))
            {
                //Debug.Log("key:A" + RotationRate);
                //  向左旋转
                //  左转最大值是-1
                if (RotationRate > -1)
                {
                    //  还在向左转向中
                    RotationRate -= updateTime / Template.AngleUpSec;
                }
            }
            else if (Input.GetKey(KeyCode.D))
            {
                //Debug.Log("key:D" + RotationRate);
                //  向右旋转
                //  右转最大值是1
                if (RotationRate < 1)
                {
                    RotationRate += updateTime / Template.AngleUpSec;
                }
            }
            else
            {
                if (RotationRate != 0f)
                {
                    var change = updateTime / Template.AngleUpSec;
                    //  不向右也不向左，则自然回舵
                    if (RotationRate > 0f)
                    {
                        RotationRate -= change;
                        if (RotationRate < 0f)
                            RotationRate = 0f;
                    }
                    else
                    {
                        RotationRate += change;
                        if (RotationRate > 0)
                            RotationRate = 0f;
                    }
                }
            }


            if (Input.GetKeyDown(KeyCode.W))
            {

                //Debug.Log("key:W " + SpeedUpType);
                SpeedUpType += 1;
                if (SpeedUpType > SpeedUpTypes.Full)
                    SpeedUpType = SpeedUpTypes.Full;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                //Debug.Log("key:S " + SpeedUpType);

                SpeedUpType -= 1;
                if (SpeedUpType < SpeedUpTypes.Stop)
                    SpeedUpType = SpeedUpTypes.Stop;
            }

            base.UpdateShip();

            //Debug.Log("Player ship speed:" + Speed);
        }

        void UpdateInput()
        {
            
        }
    }
}
