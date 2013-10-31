using System;
using DogSE.Server.Database;
using DogSE.Server.Database.MySQL;

namespace TradeAge.Server.Database
{
    /// <summary>
    /// 游戏的数据库访问器
    /// </summary>
    public class GameDB
    {
        private static MySqlService s_service;

        /// <summary>
        /// 游戏db的访问接口
        /// </summary>
        public static IDataService DB
        {
            get
            {
                if (s_service == null)
                {
                    if (string.IsNullOrEmpty(DatabaseConfig.GameDbConnectString))
                        throw new NullReferenceException("GameDbConnectString is null");

                    s_service = new MySqlService(DatabaseConfig.GameDbConnectString);
                }
                return s_service;
            }
        }
    }
}
