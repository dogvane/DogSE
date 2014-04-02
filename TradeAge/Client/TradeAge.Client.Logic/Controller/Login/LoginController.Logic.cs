using System;
using TradeAge.Client.Entity.Login;

namespace TradeAge.Client.Logic.Controller.Login
{
    public partial class LoginController : BaseLoginController
    {
        #region ILogin 成员

         internal override void OnLoginServerResult(LoginServerResult result, bool isCraetePlayer)
        {
            if (LoginServerRet != null)
            {
                var arg = new LoginServerResultEventArgs()
                {
                    Result = result,
                    IsCreatePlayered = isCraetePlayer
                };
                LoginServerRet(this, arg);
            }
        }

        public class LoginServerResultEventArgs : EventArgs
        {
            public LoginServerResult Result { get; set; }

            public bool IsCreatePlayered { get; set; }
        }

        /// <summary>
        /// 登陆服务器返回事件
        /// </summary>
        public event EventHandler<LoginServerResultEventArgs> LoginServerRet;


        internal override  void OnCreatePlayerResult(CraetePlayerResult result)
        {
            if (CreatePlayerRet != null)
            {
                CreatePlayerRet(this, new CreatePlayerResultEventArgs {Result = result});
            }
        }

        public class CreatePlayerResultEventArgs : EventArgs
        {
            public CraetePlayerResult Result { get; internal set; }
        }

        public event EventHandler<CreatePlayerResultEventArgs> CreatePlayerRet;

        #endregion
    }
}
