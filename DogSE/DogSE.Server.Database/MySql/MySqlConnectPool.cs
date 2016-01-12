using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DogSE.Library.Common;
using DogSE.Library.Log;
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
        /// 清理所有连接，释放对应的资源
        /// </summary>
        public void ClearConnection()
        {
            timeMap.Clear();

            while (!m_FreePool.IsEmpty)
            {
                MySqlConnection con;
                if (m_FreePool.TryDequeue(out con))
                {
                    con.Dispose();
                }
            }

            GC.Collect();
        }

        private Dictionary<MySqlConnection, DateTime> timeMap = new Dictionary<MySqlConnection, DateTime>();

        /// <summary>
        /// 获得一个连接对象，注意，使用完后要返回连接池
        /// 方法内部会初始化数据库连接
        /// </summary>
        /// <returns></returns>
        public MySqlConnection GetConnection()
        {
            var con = AcquireContent();

            DateTime dropTime;
            if (timeMap.TryGetValue(con, out dropTime))
            {
                if (DateTime.Now > dropTime)
                {
                    //  超过一天了，这个sql连接需要抛弃
                    try
                    {
                        Logs.Info("drop mysql connect. {0}", con.GetHashCode());
                        con.Close();
                        con.Dispose();
                        timeMap.Remove(con);
                    }
                    catch
                    {
                    }

                    con = new MySqlConnection();
                    timeMap[con] = DateTime.Now.AddDays(1);
                }
            }
            else
            {
                timeMap[con] = DateTime.Now.AddDays(1);
            }

            if (con.State == ConnectionState.Open)
            {
                if (con.Ping())
                    return con;

                con = new MySqlConnection();
            }
            Logs.Info("set connestring {0}", m_connectStr);
            con.ConnectionString = m_connectStr;
            con.Open();

            return con;
        }

        /// <summary>
        /// 移除一个错误的对象
        /// </summary>
        /// <param name="con"></param>
        public void RemoveContent(MySqlConnection con)
        {
            timeMap.Remove(con);
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
