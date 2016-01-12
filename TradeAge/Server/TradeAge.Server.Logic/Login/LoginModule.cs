using System;
using System.Collections.Generic;
using System.Text;
using DogSE.Library.Log;
using DogSE.Library.Time;
using DogSE.Server.Core.LogicModule;
using DogSE.Server.Core.Net;
using DogSE.Server.Core.Protocol;
using TradeAge.Server.Entity;
using TradeAge.Server.Entity.Character;
using TradeAge.Server.Entity.Common;
using TradeAge.Server.Entity.Login;
using TradeAge.Server.Interface.Client;
using System.Linq;
using DogSE.Library.Maths;
using DogSE.Server.Core;
using TradeAge.Server.Entity.GameEvent;
using TradeAge.Server.Logic.Scene;

namespace TradeAge.Server.Logic.Login
{
    /// <summary>
    /// 登陆模块
    /// </summary>
    class LogicModule : Interface.Server.ILogin
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
            LoadAccountFromDB();
            GameServerService.GetWorldInstatnce().NetStateDisconnect += OnNetStateDisconnect;
            GameServerService.GetWorldInstatnce().NetStateConnect += OnNetStateConnect;
        }


        public void Initializationed()
        {
            
        }

        public void ReLoadTemplate()
        {
            
        }

        public void Release()
        {
            GameServerService.GetWorldInstatnce().NetStateDisconnect -= OnNetStateDisconnect;
        }

        #endregion

        /// <summary>
        /// 服务器启动时，需要加载所有账户数据到内存里，否则无法通过账户名查找到玩家账户信息
        /// 当然，正式环境下，可以在登陆的时候，再向db做查询
        /// </summary>
        void LoadAccountFromDB()
        {
            WorldEntityManager.AccountNames.Clear();

            var accounts = DB.GameDB.LoadEntitys<Account>();
            foreach (var account in accounts)
            {
                WorldEntityManager.AccountNames.SetValue(account.Name, account);
            }

            if (accounts.Length > 0)
                WorldSeqGen.AccountSeq = new IntSequenceGenerator(accounts.Max(o => o.Id) + 1);

            Logs.Info("加载账号数据 {0} 条", accounts.Length);
        }

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

                //  保存到数据，demo阶段，已xml的方式写入文件里，减少对第三方系统的依赖
                DB.GameDB.InsertEntity(account);
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

            //  先瞅瞅内存里玩家数据有没有被缓存着
            var player = WorldEntityManager.PlayerCache.GetEntity(account.Id);

            if (player == null)
            {
                player = DB.GameDB.LoadEntity<Player>(account.Id);
                if(player != null)
                    WorldEntityManager.PlayerCache.AddOrReplace(player);
            }

            var isCreatePlayer = player != null;
            ClientProxy.Login.LoginServerResult(netstate, LoginServerResult.Success, isCreatePlayer);
            if (isCreatePlayer)
            {
                if (player.NetState != null)
                {
                    //  玩家重复登陆，需要将之前的玩家连接关闭
                    var oldCon = player.NetState;
                    oldCon.IsVerifyLogin = false;
                    oldCon.Player = null;
                    oldCon.BizId = 0;
                    oldCon.NetSocket.CloseSocket();
                }

                //  给玩家绑定网络连接账号
                player.NetState = netstate;
                netstate.Player = player;
                netstate.IsVerifyLogin = true;

                //  创建过角色，这里就发送玩家的数据到客户端
                PlayerEnterGame(player);

            }
        }

        #region ILogin 成员

        void PlayerEnterGame(Player player)
        {
            WorldEntityManager.OnlinePlayers.SetValue(player.Id, player);

            ClientProxy.Scene.EnterSceneInfo(player.NetState, player.GetSimplePlayer());

            PlayerEvents.OnEnterGame(player);

            ClientProxy.Login.SyncInitDataFinish(player.NetState);
        }

        /// <summary>
        /// 创建玩家
        /// </summary>
        /// <param name="netstate"></param>
        /// <param name="playerName"></param>
        /// <param name="sex"></param>
        public void OnCreatePlayer(NetState netstate, string playerName, Sex sex)
        {
            //  如果已经创建过（例如玩家连续点了两次创建角色）
            var player = WorldEntityManager.PlayerCache.GetEntity(netstate.BizId);
            if (player != null)
            {
                ClientProxy.Login.CreatePlayerResult(netstate, CraetePlayerResult.Fail);
                return;
            }

            player = new Player
            {
                Id = netstate.BizId,
                AccountId = netstate.BizId,
                Name = playerName,
                Sex = sex,
                NetState = netstate
            };

            player.Postion = new Vector3();
            player.Rotation = new Quaternion();

            DB.GameDB.InsertEntity(player);

            WorldEntityManager.PlayerCache.AddOrReplace(player);

            Logs.Info("角色 {0}({1}) 创建成功", playerName, player.Id);

            netstate.Player = player;
            ClientProxy.Login.CreatePlayerResult(netstate, CraetePlayerResult.Success);

            PlayerEnterGame(player);
        }

        #endregion


        private void OnNetStateConnect(object sender, NetStateConnectEventArgs e)
        {
            Logs.Info("有人连上服务器拉~ {0}", e.NetState.NetAddress);
            if (WorldEntityManager.OnlinePlayers.Count > 9999)
            {
                //  在这里可以对人数连接上线做限制
                //  e.AllowConnect = false;
            }
        }

        private void OnNetStateDisconnect(object sender, NetStateDisconnectEventArgs e)
        {
            var netState = e.NetState;
            if (netState.IsVerifyLogin && netState.BizId > 0)
            {

                //  标示玩家已经登陆过游戏，离线时会触发一系列的操作
                var player = (Player)netState.Player;
                Logs.Info("{0} net disconnect.", player.Name);

                player.LastLogoffTime = OneServer.NowTime;
                var onlineTime = player.LastLogoffTime - player.LastLoginTime;
                player.OnlineTime += (int)onlineTime.TotalSeconds;

                WorldEntityManager.OnlinePlayers.Remove(player.Id);
                
                PlayerEvents.OnExitGame(player);

                //  这里暂时是同步写文件了
                DB.GameDB.UpdateEntity(player);
            }
            else
            {
                Logs.Info("not login account net disconnect.");
            }
        }
    }
}
