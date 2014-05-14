using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DogSE.Library.Common;
using MySql.Data.MySqlClient;

namespace DogSE.Server.Database.MySQL
{
    /// <summary>
    /// MySQl的连接池
    /// </summary>
    public class MySqlConnectPool : ObjectPool<MySqlConnection>
    {

        /// <summary>
        /// 只创建10个就OK了
        /// </summary>
         public MySqlConnectPool()
             : base("MySqlConnectPool", 10)
        {
            
        }

        /// <summary>
        /// 获得一个连接对象，注意，使用完后要返回连接池
        /// 方法内部会初始化数据库连接
        /// </summary>
        /// <returns></returns>
        public MySqlConnection GetConnection()
        {
            var con = AcquireContent();
            if (string.IsNullOrEmpty(con.ConnectionString))
                con.ConnectionString = m_connectStr;

            if (con.State == ConnectionState.Open)
                return con;

            con.Open();
            return con;
        }

        string m_connectStr = "server=localhost;User Id=root;Persist Security Info=True;database=tradeage";

        /// <summary>
        /// 数据库的连接字符串
        /// </summary>
        public string ConnectString
        {
            get { return m_connectStr; }
            set { m_connectStr = value; }
        }
    }
}
