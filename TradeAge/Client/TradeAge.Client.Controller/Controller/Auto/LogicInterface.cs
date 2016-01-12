
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TradeAge.Client.Controller
{


    /// <summary>
    /// Game
    /// </summary>
    
    public abstract class BaseGameController
    {
internal abstract void OnSyncServerTime(DateTime serverTime,int id);
        
    }


    /// <summary>
    /// Login
    /// </summary>
    
    public abstract class BaseLoginController
    {
internal abstract void OnLoginServerResult(TradeAge.Client.Entity.Login.LoginServerResult result,bool isCreatePlayer);
internal abstract void OnCreatePlayerResult(TradeAge.Client.Entity.Login.CraetePlayerResult result);
internal abstract void OnSyncInitDataFinish();
        
    }


    /// <summary>
    /// Scene
    /// </summary>
    
    public abstract class BaseSceneController
    {
internal abstract void OnEnterSceneInfo(TradeAge.Client.Entity.Character.SimplePlayer player);
internal abstract void OnSpriteEnter(TradeAge.Client.Entity.Character.SceneSprite[] sprite);
internal abstract void OnSpriteLeave(System.Int32[] spriteId);
internal abstract void OnSpriteMove(int spriteId,DateTime time,DogSE.Library.Maths.Vector3 postion,DogSE.Library.Maths.Quaternion rotation,float speed,float rotationRate,TradeAge.Client.Entity.Ship.SpeedUpTypes speedUpType);
        
    }

}

