using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Library.Common;
using DogSE.Library.Time;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;

namespace DogSE.Server.LogicTestServer.LogicData
{
    /// <summary>
    /// 
    /// </summary>
    public class GetPingTimeLogic:ILogicModule
    {
        #region ILogicModule 成员

        public string ModuleId
        {
            get { return "GetPingTimeLogic"; }
        }

        public void Initializationing()
        {
            CreateClientProxyCode.Register(typeof (ClientProxy));
        }

        public void Initializationed()
        {
        }

        public void ReLoadTemplate()
        {
        }

        public void Release()
        {
        }

        #endregion

        /// <summary>
        /// 只是获得当前的时间
        /// </summary>
        [NetMethod(0, NetMethodType.SimpleMethod, false)]
        public void GetTime(NetState netstate, long clientTime)
        {
            var now = OneServer.NowTime;

            ClientProxy.GetPingTime.GetTimeResult(netstate, now.Ticks);

            //var package = TimeBack.GetPackage(OneServer.NowTime);
            
            //netstate.Send(package);
        }
    }

    class TimeBack : Packet
    {
        public TimeBack() : base(1)
        {
        }

        public static TimeBack GetPackage(DateTime time)
        {
            var package = new TimeBack();

            package.WriterStream.Write(time.Ticks);

            return package;
        }
    }

    /// <summary>
    /// 客户端代理，所有给客户端的数据都从这里发送
    /// </summary>
    public static class ClientProxy
    {
        /// <summary>
        /// 客户端时间
        /// </summary>
        public static IGetPingTimeLogicBack GetPingTime { get; set; }
    }

    /// <summary>
    /// 客户端接口
    /// </summary>
    [ClientInterface]
    public interface IGetPingTimeLogicBack
    {
        /// <summary>
        /// 获取时间返回
        /// </summary>
        /// <param name="net"></param>
        /// <param name="serverTime"></param>
        [NetMethod(2, NetMethodType.SimpleMethod)]
        void GetTimeResult(NetState net, long serverTime);
    }
}
