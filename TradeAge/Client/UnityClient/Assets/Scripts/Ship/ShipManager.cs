using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Ship
{
    /// <summary>
    /// 船只的控制器
    /// </summary>
    class ShipManager
    {
        static ShipManager instatnce = new ShipManager();

        static public ShipManager Instatnce {
            get { return instatnce; }
        }

        private List<ShipController> ships = new List<ShipController>();

        public void AddShip(ShipController ship)
        {
            ships.Add(ship);
        }

        public void RemoveShip(ShipController ship)
        {
            ships.Remove(ship);
        }

        public void Update()
        {
            ships.ForEach(o => o.UpdateShip());
        }
    }
}
