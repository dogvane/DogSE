namespace TradeAge.Server.Entity.NetCode
{
    /// <summary>
    /// 游戏的消息码
    /// </summary>
    public enum OpCode:ushort
    {
        #region 0 - 1000 游戏系统保留消息码
        
        /// <summary>
        /// 心跳包
        /// </summary>
        Heart = 1,

        /// <summary>
        /// 服务器时间
        /// </summary>
        ServerTime = 2,

        #endregion

        #region 1000  - 1099 登陆相关消息码
        
        /// <summary>
        /// 客户端登陆服务器
        /// </summary>
        LoginServer = 1000,

        /// <summary>
        /// 登陆服务器返回结果
        /// </summary>
        LoginServerResult = 1001,

        /// <summary>
        /// 创建玩家
        /// </summary>
        CreatePlayer = 1002,

        /// <summary>
        /// 创建玩家结果
        /// </summary>
        CreatePlayerResult = 1003,


        /// <summary>
        /// 数据同步完成
        /// </summary>
        SyncInitDataFinish = 1004,

        #endregion

        #region 1100  - 1199 移动相关的代码

        /// <summary>
        /// 玩家发起的移动请求
        /// </summary>
        OnMove = 1100,

        /// <summary>
        /// 玩家进入场景时返回的基本信息
        /// </summary>
        EnterSceneInfo = 1101,


        /// <summary>
        /// 场景里有其他精灵（玩家）进入
        /// </summary>
        SpriteEnter = 1102,

        /// <summary>
        /// 场景里有其他精灵（玩家）进行移动
        /// </summary>
        SpriteMove = 1103,


        /// <summary>
        /// 场景里有其他精灵（玩家）进行离开
        /// </summary>
        SpriteLeave = 1104,

        #endregion
    }
}
