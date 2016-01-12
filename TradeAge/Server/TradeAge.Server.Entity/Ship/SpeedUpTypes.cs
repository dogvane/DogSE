using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if Server 
namespace TradeAge.Server.Entity.Ship
#else
namespace TradeAge.Client.Entity.Ship
#endif
{

    /// <summary>
    /// 船只速度类型
    /// </summary>
    public enum SpeedUpTypes
    {
        Stop = 0,

        OneFourth = 1,

        Half = 2,

        ThreeFourth = 3,

        Full = 4,
    }
}
