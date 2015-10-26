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
    /// 三维向量
    /// </summary>
    public class Vector3
    {
        /// <summary>
        /// 
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float Y { get; set; }

        public float Z { get; set; }
    }
}
