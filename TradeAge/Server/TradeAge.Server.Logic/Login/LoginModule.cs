using System;
using System.Collections.Generic;
using System.Text;
using DogSE.Library.Time;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Server.Entity;
using TradeAge.Server.Entity.Character;
using TradeAge.Server.Entity.Common;
using TradeAge.Server.Entity.Login;
using TradeAge.Server.Interface.Client;
using ILogin = TradeAge.Server.Interface.ServerLogic.ILogin;

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
        /// <param name="serverId"></param>
        public void OnLoginServer(NetState netstate, string accountName, string password, int serverId = 0)
        {
            var account = WorldEntityManager.AccountNames.GetValue(accountName);
            if (account == null)
            {
                //  账号不存在，则创建账号
                account = new Account
                {
                    Name = accountName,
                    Password = password,
                    CreateTime = OneServer.NowTime,
                    ServerId = serverId,
                    Id = WorldSeqGen.AccountSeq.NextSeq(),
                };

                WorldEntityManager.AccountNames.SetValue(accountName, account);
                WorldEntityManager.Accounts.SetValue(account.Id, account);

                ClientProxy.Login.LoginServerResult(netstate, LoginServerResult.Success);

                //  保存到数据
            }
            else
            {
                if (account.Password != password)
                {
                    //  账户存在，但是密码验证失败，返回错误
                    ClientProxy.Login.LoginServerResult(netstate, LoginServerResult.PassOrAccountError);
                    return;
                }

           }

            netstate.BizId = account.Id;
            netstate.IsVerifyLogin = true;
        }

        #region ILogin 成员

        /// <summary>
        /// 创建玩家
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="playerName"></param>
        /// <param name="sex"></param>
        public void OnCreatePlayer(NetState netstate, string playerName, Sex sex)
        {
            
        }

        #endregion
    }
}
