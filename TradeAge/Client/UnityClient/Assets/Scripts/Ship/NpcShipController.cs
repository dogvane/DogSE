using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Library.Time;
using TradeAge.Client.Entity.Character;
using TradeAge.Client.Entity.Ship;
using UnityEngine;

namespace Assets.Scripts.Ship
{
    /// <summary>
    /// Npc的船只控制器
    /// </summary>
    class NpcShipController:ShipController
    {

        /// <summary>
        /// 和Model关联的场景精灵对象
        /// </summary>
        public SceneSprite Sprite { get; set; }


        /// <summary>
        /// 同步时间，是对方客户端到自己这里的时间
        /// 主要的目的为了推测新的位置用
        /// </summary>
        private float syncTime;

        public void UpdateNetPostion(CheckData cd)
        {
            var trans = GameObject.transform;

            if (lastCheckData == null)
            {
                trans.position = cd.Postion;
                trans.rotation = cd.Rotation;
            }

            lastCheckData = cd;

            syncTime = (float) (OneServer.NowTime - cd.Time).TotalSeconds;

            //  获得预测放的位置
            var forecastPostion = lastCheckData.Postion + lastCheckData.Rotation * Vector3.forward * (lastCheckData.Speed * syncTime);

            //  修正一下自己的朝向和速度
            lastCheckData.Rotation = Quaternion.FromToRotation(trans.position, forecastPostion);


            var length = Vector3.Distance(trans.position, forecastPostion);
            var speed = length/syncTime;
            Speed = speed;
            RotationRate = lastCheckData.RotationRate;

            //Debug.Log("Length=" + length);
            //Debug.Log(string.Format("speed  {0} {1}" , lastCheckData.Speed, speed));
        }

        /// <summary>
        /// 更新船只信息
        /// </summary>
        public override void UpdateShip()
        {
            if (lastCheckData == null)
                return;

            //  预测当前可能在的位置
            var updateTime = Time.deltaTime;

            var cms = GetCurrentMaxSpeed(lastCheckData.SpeedUpTypes);
            var a = 0f; //当前的加速度

            if (SpeedUpType == SpeedUpTypes.Stop)
            {
                //  船只应该是停船状态，就啥也不干了。
                if (Speed <= 0f)
                    return;

                //  船只还有速度，说明在减速慢行
                a = -GetAcceleration(SpeedUpTypes.Half);
            }
            else
            {
                a = GetAcceleration(SpeedUpType);
                if (Speed > cms)
                {
                    //  当前速度速度大于当前船帆允许的最大速度，说明船只要减速
                    a = -a;
                }
            }

            //  当前的速度等于上次的速度*加速度
            var nextSpeed = Speed + (a * updateTime);

            //  获得上一帧到现在的理论移动距离
            var s = nextSpeed * updateTime;
            
            Quaternion trans = GameObject.transform.rotation;

            //  转向控制
            if (RotationRate != 0f)
            {
                Quaternion nextQuaternion = Quaternion.Euler(trans.eulerAngles.x, trans.eulerAngles.y, EulerAngle.z);

                var roationSpeed = MaxMoveSpeed * 0.8f;

                //  转向的角度和船只的转向度，时间，方向舵（RotationRate），船只速度有关

                var angle = Template.AnglePreSec * updateTime * RotationRate * (Speed / roationSpeed);

                nextQuaternion = nextQuaternion * Quaternion.AngleAxis(angle, Vector3.up);

                var yAngle = RotationRate * -5 + -10 * (Speed / MaxMoveSpeed * RotationRate);
                trans = nextQuaternion * Quaternion.Euler(0, 0,yAngle);

                //  船只在转向的时候，会有一定的失速
                //if (nextSpeed > roationSpeed)
                //{
                //    nextSpeed -= (a + GetAcceleration(SpeedUpType.Full)) * RotationRate * updateTime;
                //    if (nextSpeed < roationSpeed)
                //        nextSpeed = roationSpeed;
                //}

                GameObject.transform.rotation = trans;
            }

            //  预测的位置

            Postion = Postion + trans * Vector3.forward * (s * updateTime);

        }

        private CheckData lastCheckData;


    }
}
