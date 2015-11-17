using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if Server 
using TradeAge.Server.Entity.Common;
namespace TradeAge.Server.Entity.Common
#else
namespace TradeAge.Client.Entity.Common
#endif
{
    /// <summary>
    /// 二维的向量，只有xy值
    /// </summary>
    public class Vector2
    {
        /// <summary>
        /// 
        /// </summary>
        public Vector2()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }


        /// <summary>
        /// 
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float Y { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(" x:{0} y:{1} ", X, Y);
        }
    }
}
