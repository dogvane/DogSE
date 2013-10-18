using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Common.Entity.NetCode;
using TradeAge.Server.Interface.ServerLogic;

namespace TradeAge.Server.Logic.Login
{
    /// <summary>
    /// 登陆模块
    /// </summary>
    class LogicModule : ILogin
    {
        #region ILogicModule 成员

        /// <summary>
        /// 模块id
        /// </summary>
        public string ModuleId
        {
            get { return "LogicModule"; }
        }

        public void Initializationing()
        {
            
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
        /// 客户端登陆服务器，采用简单模式，就只有2个参数
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="accountName"></param>
        /// <param name="password"></param>
        [NetMethod((ushort)OpCode.LoginServer, NetMethodType.SimpleMethod)]
        public void OnLoginServer(NetState netstate, string accountName, string password)
        {
            
        }
    }
}
