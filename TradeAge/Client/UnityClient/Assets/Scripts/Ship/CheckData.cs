using System;
using TradeAge.Client.Entity.Ship;
using UnityEngine;

namespace Assets.Scripts.Ship
{
    class CheckData
    {
        public DateTime Time;

        public Vector3 Postion;

        public Quaternion Rotation;

        public float Speed;

        public float RotationRate;

        public SpeedUpTypes SpeedUpTypes;
    }
}