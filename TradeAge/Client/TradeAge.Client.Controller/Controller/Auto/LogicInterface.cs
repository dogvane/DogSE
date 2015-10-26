
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradeAge.Client.Controller
{


    /// <summary>
    /// Login
    /// </summary>
    
    public abstract class BaseLoginController
    {
internal abstract void OnLoginServerResult(TradeAge.Client.Entity.Login.LoginServerResult result,bool isCreatePlayer);
internal abstract void OnCreatePlayerResult(TradeAge.Client.Entity.Login.CraetePlayerResult result);
        
    }


    /// <summary>
    /// Scene
    /// </summary>
    
    public abstract class BaseSceneController
    {
internal abstract void OnEnterSceneInfo(TradeAge.Client.Entity.Common.Vector3 postion,TradeAge.Client.Entity.Common.Vector3 direction);
internal abstract void OnSpriteEnter(TradeAge.Client.Entity.Character.SimplePlayer player);
internal abstract void OnSpriteMove(int playerId,TradeAge.Client.Entity.Common.Vector3 postion,TradeAge.Client.Entity.Common.Vector3 direction);
internal abstract void OnSpriteLeave(int playerId);
        
    }

}

