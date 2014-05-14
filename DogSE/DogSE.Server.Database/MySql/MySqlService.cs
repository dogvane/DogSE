using System.Data;
using DogSE.Common;
using IvyOrm;
using MySql.Data.MySqlClient;

namespace DogSE.Server.Database.MySQL
{
    /// <summary>
    ///     Mysql对数据访问接口的实现
    /// </summary>
    public class MySqlService : IDataService
    {
        /// <summary>
        /// </summary>
        /// <param name="connectStr"></param>
        public MySqlService(string connectStr)
        {
            ConPool = new MySqlConnectPool();
            ConPool.ConnectString = connectStr;
        }

        protected readonly MySqlConnectPool ConPool;

        #region IDataService Members

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serial"></param>
        /// <returns></returns>
        public T LoadEntity<T>(int serial) where T : class, IDataEntity, new()
        {
            MySqlConnection con = ConPool.GetConnection();
            string sql = string.Format("select * from {0} where id = {1}", typeof (T).Name, serial);
            var ret = con.RecordSingleOrDefault<T>(sql);
            ConPool.ReleaseContent(con);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public T QueryEntity<T>(string where) where T : class, new()
        {
            MySqlConnection con = ConPool.GetConnection();
            string sql = string.Format("select * from {0} where {1}", typeof (T).Name, where);
            var ret = con.RecordSingleOrDefault<T>(sql);
            ConPool.ReleaseContent(con);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T QueryValueEntitySQL<T>(string sql) where T : new()
        {
            MySqlConnection con = ConPool.GetConnection();
            var ret = con.ValueSingleOrDefault<T>(sql);
            ConPool.ReleaseContent(con);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public T QueryValueEntity<T>(string where) where T : new()
        {
            MySqlConnection con = ConPool.GetConnection();
            string sql = string.Format("select * from {0} where {1}", typeof (T).Name, where);
            var ret = con.ValueSingleOrDefault<T>(sql);
            ConPool.ReleaseContent(con);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T QueryEntitySQL<T>(string sql) where T : class, new()
        {
            MySqlConnection con = ConPool.GetConnection();
            var ret = con.RecordSingleOrDefault<T>(sql);
            ConPool.ReleaseContent(con);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereFormatter"></param>
        /// <param name="param0"></param>
        /// <returns></returns>
        public T QueryEntity<T>(string whereFormatter, string param0) where T : class, new()
        {
            return QueryEntity<T>(string.Format(whereFormatter, param0));
        }


        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereFormatter"></param>
        /// <param name="param0"></param>
        /// <returns></returns>
        public T QueryEntity<T>(string whereFormatter, int param0) where T : class, new()
        {
            return QueryEntity<T>(string.Format(whereFormatter, param0));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereFormatter"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <returns></returns>
        public T QueryEntity<T>(string whereFormatter, string param0, string param1) where T : class, new()
        {
            return QueryEntity<T>(string.Format(whereFormatter, param0, param1));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public T[] QueryEntitys<T>(string where) where T : class, new()
        {
            MySqlConnection con = ConPool.GetConnection();
            string sql = string.Format("select * from {0} where {1}", typeof (T).Name, where);
            T[] ret = con.RecordQuery<T>(sql);
            ConPool.ReleaseContent(con);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T[] QueryEntitysSQL<T>(string sql) where T : class, new()
        {
            MySqlConnection con = ConPool.GetConnection();
            T[] ret = con.RecordQuery<T>(sql);
            ConPool.ReleaseContent(con);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereFormatter"></param>
        /// <param name="param0"></param>
        /// <returns></returns>
        public T[] QueryEntitys<T>(string whereFormatter, string param0) where T : class, new()
        {
            return QueryEntitys<T>(string.Format(whereFormatter, param0));
        }


        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereFormatter"></param>
        /// <param name="param0"></param>
        /// <returns></returns>
        public T[] QueryEntitys<T>(string whereFormatter, int param0) where T : class, new()
        {
            return QueryEntitys<T>(string.Format(whereFormatter, param0));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereFormatter"></param>
        /// <param name="param0"></param>
        /// <returns></returns>
        public T[] QueryEntitysSQL<T>(string whereFormatter, int param0) where T : class, new()
        {
            return QueryEntitysSQL<T>(string.Format(whereFormatter, param0));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereFormatter"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
        /// <returns></returns>
        public T[] QueryEntitys<T>(string whereFormatter, string param0, string param1) where T : class, new()
        {
            return QueryEntitys<T>(string.Format(whereFormatter, param0, param1));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] LoadEntitys<T>() where T : class, IDataEntity, new()
        {
            MySqlConnection con = ConPool.GetConnection();
            string sql = string.Format("select * from {0}", typeof (T).Name);
            T[] ret = con.RecordQuery<T>(sql);
            ConPool.ReleaseContent(con);
            return ret;
        }


        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateEntity<T>(T entity) where T : class, IDataEntity
        {
            MySqlConnection con = ConPool.GetConnection();
            con.RecordUpdate(entity);
            ConPool.ReleaseContent(con);

            return 1;
        }


        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity<T>(T entity) where T : class, IDataEntity
        {
            MySqlConnection con = ConPool.GetConnection();
            con.RecordInsert(entity);
            ConPool.ReleaseContent(con);
            return entity.Id;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int DeleteEntity<T>(T entity) where T : class, IDataEntity
        {
            MySqlConnection con = ConPool.GetConnection();
            con.RecordDelete(entity);
            ConPool.ReleaseContent(con);

            return 1;
        }

        /// <summary>
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            MySqlConnection con = ConPool.GetConnection();
            int ret = con.ExecuteNonQuery(sql);
            ConPool.ReleaseContent(con);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql)
        {
            MySqlConnection con = ConPool.GetConnection();
            DataSet ret = MySqlHelper.ExecuteDataset(con, sql);
            ConPool.ReleaseContent(con);
            return ret;
        }

        #endregion
    }
}