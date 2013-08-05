using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DogSE.Server.Core.ServerState
{
    /// <summary>
    /// 服务器状态
    /// </summary>
    public interface IServerState
    {
        /// <summary>
        /// 模块的ID（名字）
        /// </summary>
        string ModuleId { get; }

        /// <summary>
        /// 往一个StreamBuilder里写入当前模块的状态数据
        /// </summary>
        /// <param name="writer"></param>
        void AppendSimpleState(StreamWriter writer);

        /// <summary>
        /// 写完整的状态数据
        /// </summary>
        /// <param name="writer"></param>
        void AppendFullState(StreamWriter writer);
    }
}
