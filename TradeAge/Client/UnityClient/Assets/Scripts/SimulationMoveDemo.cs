using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Ship;
using Assets.Scripts.Ship.UIUtils;
using Assets.Scripts.UI.Form;
using DogSE.Library.Time;
using TradeAge.Client.Entity.Template;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    class SimulationMoveDemo : MonoBehaviour
    {
        public void Start()
        {
            //var ship = Resources.Load<GameObject>("ship1.prefab");
            //var ship = Resources.Load<GameObject>("ShipGameStarterKit/Models/Ship of the Line/ship1.prefab");
            var ship = Resources.Load<GameObject>("ship_1");

            if (ship == null)
            {
                Debug.Log("没找到船只的预设");
            }
            else
            {
                //ship.transform.position = new Vector3(0, 0, 0);
                //ship.transform.localScale = new Vector3(1, 1, 1);
                var ship1 = Object.Instantiate(ship) as GameObject;

                var pc = Instantiate(Resources.Load<GameObject>("PlayerCamera")) as GameObject;
                pc.transform.parent = ship1.transform;
                var so = pc.GetComponentsInChildren<ShipOrbit>()[0];

                var shipController = new PlayerShipController();
                so.control = shipController;

                shipController.BindU3DModel(ship1);

                ShipTemplate st = new ShipTemplate
                {
                    Id = 1,
                    Pref = "ship_1",
                    Speed = 4,
                    SpeededUpSec = 15,
                    AnglePreSec = 10.3f,
                    AngleUpSec = 5.0f,
                    Hp = 1000
                };

                shipController.SetShipTemplate(st);
                PlayerShip = shipController;

                //  这里先模拟船只向前移动（z轴增加的移动）
                //  在Z轴上移动
                PlayerShip.Direction = new Vector3(0, 0, 1);

                ShipManager.Instatnce.AddShip(PlayerShip);

                var ship2 = Object.Instantiate(Resources.Load<GameObject>("ship_2")) as GameObject;
                //ship2.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                ship2.transform.position = new Vector3(40, 0, 0);
                npcShip1 = new NpcShipController();
                npcShip1.BindU3DModel(ship2);
                npcShip1.SetShipTemplate(st);
                ShipManager.Instatnce.AddShip(npcShip1);

                var ship3 = Object.Instantiate(Resources.Load<GameObject>("ship_3")) as GameObject;
                ship3.transform.position = new Vector3(-40, 0, 0);

                var camera = GameObject.Find("Main Camera");
                camera.AddComponent<FrmPlayerShip>();

            }

            lastUpdateTime = OneServer.NowTime;

        }



        public static PlayerShipController PlayerShip { get; private set; }

        private static NpcShipController npcShip1;

        private static DateTime lastUpdateTime;

        /// <summary>
        /// 本次的时间延迟
        /// </summary>
        public float DeltaTime { get; private set; }


        private int updateIndex = 0;

        private Queue<CheckData> notifyData = new Queue<CheckData>();



        public void FixedUpdate()
        {
            var now = OneServer.NowTime;
            DeltaTime = (float)(now - lastUpdateTime).TotalSeconds;


            if (updateIndex++ % 10 == 0)
            {
                //  间隔 30/10 *1000 ms的间隔向服务器通知一次当前的位置
                var data = new CheckData
                {
                    Postion = new Vector3(PlayerShip.Postion.x + 200, PlayerShip.Postion.y, PlayerShip.Postion.z + 200),
                    Rotation = PlayerShip.GameObject.transform.rotation,
                    Speed = PlayerShip.Speed,
                    SpeedUpTypes = PlayerShip.SpeedUpType,
                    Time = now,
                    RotationRate = PlayerShip.RotationRate,
                };

                notifyData.Enqueue(data);
            }
            else
            {
                //  每隔3帧向模拟的npc船只传递一次数据
                //  模拟网络延迟
                if (notifyData.Count > 1 && updateIndex % 3 == 0)
                {
                    var data = notifyData.Dequeue();
                    npcShip1.UpdateNetPostion(data);
                }
            }
            //npcShip1.UpdateShip();
        }
    }
}
