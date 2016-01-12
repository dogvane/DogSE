using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if Server 
namespace TradeAge.Server.Entity.Template
#else
namespace TradeAge.Client.Entity.Template
#endif
{
    /// <summary>
    /// 船只的配置数据
    /// </summary>
    public class ShipTemplate
    {
        /// <summary>
        /// 船只的id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 船只使用的预设
        /// </summary>
        public string Pref { get; set; }

        /// <summary>
        /// 船只速度(单位：节）
        /// </summary>
        public float Speed { get; set; }


        /// <summary>
        /// 转向度（每秒可以转多少度）
        /// </summary>
        public float AnglePreSec { get; set; }

        /// <summary>
        /// 转向到最大转向度需要多少时间
        /// </summary>
        public float AngleUpSec { get; set; }

        /// <summary>
        /// 船只的生命值
        /// </summary>
        public int Hp { get; set; }

        /// <summary>
        /// 船只加速到极速（最大速度）用的时间（满帆的时候）
        /// 用来决定船只的加速度
        /// </summary>
        public float SpeededUpSec { get; set; }
    }
}
