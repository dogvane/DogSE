using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeAge.Client.Entity.Ship;
using TradeAge.Client.Entity.Template;
using UnityEngine;

namespace Assets.Scripts.Ship
{
    public abstract class ShipController 
    {
        protected Vector3 EulerAngle;

        /// <summary>
        /// 和U3d绑定的模型
        /// </summary>
        public GameObject GameObject
        {
            get;
            private set;
        }

        /// <summary>
        /// 给船只绑定一个GameObject
        /// </summary>
        /// <param name="obj"></param>
        public void BindU3DModel(GameObject obj)
        {
            GameObject = obj;
            trans = obj.transform;
            EulerAngle = trans.eulerAngles;
            SpeedUpType = SpeedUpTypes.Half;
        }


        /// <summary>
        /// 更新船只的信息
        /// </summary>
        public virtual void UpdateShip()
        {
            //  本次更新的时间间隔
            var updateTime = Time.deltaTime;
            var cms = GetCurrentMaxSpeed();
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
            var s = Speed * updateTime;

            //  转向控制
            if (RotationRate != 0f)
            {
                Quaternion nextQuaternion = Quaternion.Euler(trans.eulerAngles.x, trans.eulerAngles.y, EulerAngle.z);

                var roationSpeed = MaxMoveSpeed*0.8f;

                //  转向的角度和船只的转向度，时间，方向舵（RotationRate），船只速度有关
                var angle = Template.AnglePreSec * updateTime * RotationRate * (Speed / roationSpeed);

                nextQuaternion = nextQuaternion * Quaternion.AngleAxis(angle, Vector3.up);

                trans.rotation = nextQuaternion * Quaternion.Euler(0, 0, RotationRate * -5 + -10 *(Speed/MaxMoveSpeed* RotationRate));

                //  船只在转向的时候，会有一定的失速
                if (nextSpeed > roationSpeed)
                {
                    nextSpeed -= ( a + GetAcceleration(SpeedUpTypes.Full)) * RotationRate*updateTime;
                    if (nextSpeed < roationSpeed)
                        nextSpeed = roationSpeed;
                }
            }

            //  修改船只的位置
            Postion = Postion + trans.rotation * Vector3.forward * (s * updateTime);

            //  本次移动结束，按照新的速度和朝向来调整船只的数据
            Speed = nextSpeed;
        }

        /// <summary>
        /// 转向的转速
        /// 负数表示左转
        /// 正数表示右转
        /// </summary>
        public float RotationRate { get; protected set; }

        /// <summary>
        /// 船只的最高移动速度
        /// 这里的移动速度根据船只的配置值
        /// 换算为每秒移动的距离（米/秒）
        /// </summary>
        public float MaxMoveSpeed { get; set; }


        /// <summary>
        /// 获得当前可以移动的最大速度
        /// </summary>
        /// <returns></returns>
        float GetCurrentMaxSpeed()
        {
            return GetCurrentMaxSpeed(SpeedUpType);
        }

        /// <summary>
        /// 获得当前可以移动的最大速度
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        protected float GetCurrentMaxSpeed(SpeedUpTypes types)
        {
            float[] speedRate = { 0f, 0.25f, 0.5f, 0.75f, 1f };

            return speedRate[(int)types] * MaxMoveSpeed;
        }

        /// <summary>
        /// 获得不同帆张力下的船只加速度
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        protected float GetAcceleration(SpeedUpTypes types)
        {
            float[] speedRate = { 0f, 0.25f, 0.5f, 0.75f, 1f };

            return speedRate[(int)types] * Acceleration;
        }

        /// <summary>
        /// 船只的模板值
        /// </summary>
        public ShipTemplate Template { get; private set; }


        public void SetShipTemplate(ShipTemplate template)
        {
            Template = template;

            //  节*1807米/ 3600 * SpeedZoom
            MaxMoveSpeed = template.Speed * 1807 / 60 / 60 * SpeedZoom;

            //  加速度等于 最大速度/加速时间
            Acceleration = MaxMoveSpeed / Template.SpeededUpSec;
        }

        /// <summary>
        /// 获得当前速度，节单位
        /// </summary>
        public float GetCurrentSpeed()
        {
            return Speed / SpeedZoom * Template.Speed * 1807 / 60 / 60;
        }

        private const float SpeedZoom = 500;

        private Transform trans;

        /// <summary>
        /// 船只的位置
        /// </summary>
        public Vector3 Postion
        {
            get { return trans.position; }
            set { trans.position = value; }
        }

        public Vector3 direction;

        /// <summary>
        /// 船只的朝向
        /// </summary>
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// 船只当前的移动数据
        /// 会根据当前风帆的高度决定
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// 船只的加速度（每秒）
        /// </summary>
        public float Acceleration { get; set; }

        /// <summary>
        /// 船只速度类型
        /// </summary>
        public SpeedUpTypes SpeedUpType { get; protected set; }

        /// <summary>
        /// 设置船只速度
        /// </summary>
        /// <param name="types"></param>
        public void SetSpeedUpType(SpeedUpTypes types)
        {
            SpeedUpType = types;
        }
    }


}
