using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradeAge.Server.Entity.Common
{
    /// <summary>
    /// 游戏世界的唯一序列生成器
    /// </summary>
    public static class WorldSeqGen
    {
        static WorldSeqGen()
        {
            AccountSeq = new IntSequenceGenerator(1);
        }

        /// <summary>
        /// 账号的序列生成器
        /// </summary>
        public static IntSequenceGenerator AccountSeq { get; set; }
    }
}
