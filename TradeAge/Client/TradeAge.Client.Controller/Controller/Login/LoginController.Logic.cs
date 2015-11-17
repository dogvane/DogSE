using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DogSE.Client.Core;
using DogSE.Library.Log;
using TradeAge.Client.Entity.Login;

namespace TradeAge.Client.Controller.Login
{
    /// <summary>
    /// 登陆控制器
    /// </summary>
    public partial class LoginController: BaseLoginController
    {
        private readonly GameController controller;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gc"></param>
        /// <param name="nc"></param>
        public LoginController(GameController gc, NetController nc)
            : this(nc)
        {
            controller = gc;
        }

        internal override void OnLoginServerResult(LoginServerResult result, bool isCreatePlayer)
        {
            if (result == LoginServerResult.Success)
            {
                if (isCreatePlayer)
                    controller.Game.SyncTime();
            }

            if (LoginServerRet != null)
                LoginServerRet(this, new LoginServerResultEventArgs
                {
                    Result = result,
                    IsCreatePlayer = isCreatePlayer
                });
        }



        internal override void OnCreatePlayerResult(CraetePlayerResult result)
        {
            if (result == CraetePlayerResult.Success)
                controller.Game.SyncTime();

            if (CreatePlayerRet != null)
                CreatePlayerRet(result);
        }

        internal override void OnSyncInitDataFinish()
        {
            if (SyncDataFinish != null)
                SyncDataFinish();
        }

        /*
        这里用了2种事件的返回方法，大家可以根据实际需要自己选择
        第一种，需要声明一个 xxEventArgs 的事件对象
                好处是今后通知的数据变化的时候，不会导致接口的变化，
                坏处是需要多一个类对象定义，以及每次调用的时候，会创建一个新对象

        第二种，使用Action来包括返回参数
                好处是返回值可以直接定义在接口里，不用增加类，触发时，也不会创建新对象
                坏处是如果参数发生变化，监听这个事件的代码都得修改

        */

        /// <summary>
        /// 登陆服务器返回事件
        /// </summary>
        public event EventHandler<LoginServerResultEventArgs> LoginServerRet;

        /// <summary>
        /// 创建玩家返回
        /// </summary>
        public event Action<CraetePlayerResult> CreatePlayerRet;

        /// <summary>
        /// 同步数据结束
        /// </summary>
        public event Action SyncDataFinish;
    }

    /// <summary>
    /// 登陆服务器返回事件
    /// </summary>
    public class LoginServerResultEventArgs : EventArgs
    {
        /// <summary>
        /// 返回值
        /// </summary>
        public LoginServerResult Result { get; internal set; }

        /// <summary>
        /// 是否创建了玩家
        /// </summary>
        public bool IsCreatePlayer { get; internal set; }
    }
}
