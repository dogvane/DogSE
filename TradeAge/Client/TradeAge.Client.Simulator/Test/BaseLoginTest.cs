using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using DogSE.Library.Log;
using TradeAge.Client.Controller;
using TradeAge.Client.Controller.Login;
using TradeAge.Client.Entity.Character;
using TradeAge.Client.Entity.Login;

namespace TradeAge.Client.Simulator.Test
{
    class BaseLoginTest
    {

        protected string _userName;
        protected string _pw;

        public void Start(string userName, string pw)
        {
            _userName = userName;
            _pw = pw;


            controller.Login.LoginServerRet += OnLoginServerRet;
            controller.Login.CreatePlayerRet += OnCreatePlayerRet;
            controller.Net.NetStateConnect += Net_NetStateConnect;
            controller.Login.SyncDataFinish += OnSyncDataFinish;

            controller.Net.StartWorld();
            controller.Net.ConnectServer("127.0.0.1", 4530);
        }

        public bool IsLoginSuccess { get; set; }

        private void OnSyncDataFinish()
        {
            IsLoginSuccess = true;
            Logs.Info("玩家 {0} 进入游戏", controller.Model.Player.Name);
        }

        private void OnCreatePlayerRet(CraetePlayerResult result)
        {
            if (result == CraetePlayerResult.Success)
            {
                Logs.Info("创建角色成功");
            }
            else
            {
                Logs.Error("创建角色失败 {0}", result.ToString());
            }
            
        }

        private void OnLoginServerRet(object sender, LoginServerResultEventArgs e)
        {
            if (e.Result == LoginServerResult.Success)
            {
                Logs.Info("登陆成功 {0}", e.IsCreatePlayer ? "创建过角色" : "未创建过角色");

                if (!e.IsCreatePlayer)
                {
                    Logs.Info("请求创建角色 {0}", _userName);
                    controller.Login.CreatePlayer(_userName, Sex.Male);
                }
            }
            else
            {
                Logs.Error("登陆失败 {0}", e.Result.ToString());
            }
        }

        readonly GameController controller = new GameController();
        public GameController Controller {
            get { return controller; }
        }

        private void Net_NetStateConnect(object sender, NetStateConnectEventArgs e)
        {
            if (e.IsConnected)
            {
                Logs.Info("服务器连接成功");

                controller.Login.LoginServer(_userName, _pw, 0);
            }
            else
            {
                Logs.Error("服务器连接失败。");
            }
        }
    }
}
