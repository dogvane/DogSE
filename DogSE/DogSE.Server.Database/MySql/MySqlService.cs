using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DogSE.Common;
using DogSE.Library.Log;
using DogSE.Library.Thread;
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

        /// <summary>
        /// mysql的连接池
        /// </summary>
        protected readonly MySqlConnectPool ConPool;

        /// <summary>
        /// 
        /// </summary>
        public void ClearConnectPool()
        {
            ConPool.ClearConnection();
        }

        #region IDataService Members

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serial"></param>
        /// <returns></returns>
        public T LoadEntity<T>(int serial) where T : class, IDataEntity, new()
        {
            var profile = DBEntityProfile<T>.Instance;
            profile.Load.Watch.Restart();
            var proMySql = MySQL.Instance;
            MySqlConnection con = null;
            try
            {
                profile.Load.TotalCount++;
                proMySql.Load.TotalCount++;

                con = ConPool.GetConnection();
                string sql = string.Format("select * from {0} where id = {1}", GetTableName(typeof(T)), serial);
                var ret = con.RecordSingleOrDefault<T>(sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch
            {
                profile.Load.ErrorCount++;
                proMySql.Load.ErrorCount++;
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }

                throw;
            }
            finally
            {
                profile.Load.Watch.Stop();
                profile.Load.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMySql.Load.TotalTime+= profile.Load.Watch.ElapsedTicks;
            }

        }

        #region GM用的一些查询工具

        /// <summary>
        /// 根据条件，和类型查询某个数据实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public T QueryEntity<T>(string where) where T : class, new()
        {
            MySqlConnection con = ConPool.GetConnection();
            try
            {
                string sql = string.Format("select * from {0} where {1} limit 1", GetTableName(typeof(T)), where);
                var ret = con.RecordSingleOrDefault<T>(sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch (Exception ex)
            {
                Logs.Error("Query<{0}> faild.", typeof(T).Name, ex);
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }

                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T QueryValueEntitySQL<T>(string sql) where T : new()
        {
            MySqlConnection con = ConPool.GetConnection();
            try
            {
                var ret = con.ValueSingleOrDefault<T>(sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("QueryValueEntitySQL<{0}> faild. sql={1}", typeof(T).Name,sql), ex);
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }

                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T[] QueryValueEntitysSQL<T>(string sql) 
        {
            MySqlConnection con = ConPool.GetConnection();
            try
            {
                var ret = con.ValueQuery<T>(sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("QueryValueEntitysSQL<{0}> faild. sql={1}", typeof (T).Name, sql), ex);
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }

                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public T QueryValueEntity<T>(string where) where T : new()
        {
            MySqlConnection con = ConPool.GetConnection();
            try
            {
                string sql = string.Format("select * from {0} where {1}", GetTableName(typeof (T)), where);
                var ret = con.ValueSingleOrDefault<T>(sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("QueryValueEntity<{0}> faild. sql={1}", typeof (T).Name, where), ex);
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }

                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T QueryEntitySQL<T>(string sql) where T : class, new()
        {
            MySqlConnection con = ConPool.GetConnection();
            try
            {
                var ret = con.RecordSingleOrDefault<T>(sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("QueryValueEntity<{0}> faild. sql={1}", typeof (T).Name, sql), ex);
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }

                throw;
            }
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
            try
            {
                string sql = string.Format("select * from {0} where {1}", GetTableName(typeof (T)), where);
                T[] ret = con.RecordQuery<T>(sql);
                ConPool.ReleaseContent(con);
                return ret;

            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("QueryValueEntity<{0}> faild. sql={1}", typeof (T).Name, where), ex);
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T[] QueryEntitysSQL<T>(string sql) where T : class, new()
        {
            MySqlConnection con = ConPool.GetConnection();

            try
            {
                T[] ret = con.RecordQuery<T>(sql);
                ConPool.ReleaseContent(con);
                return ret;

            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("QueryValueEntity<{0}> faild. sql={1}", typeof (T).Name, sql), ex);
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }
                throw;
            }
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

        #endregion

        private readonly Dictionary<Type, string> tableMap = new Dictionary<Type, string>();

        /// <summary>
        /// 获得某个类型所对应的数据库表名
        /// 这里通过 Type与string 的字典做缓存
        /// 如果定义过 TableAttribute 则使用里面的Table去访问数据库
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetTableName(Type type)
        {
            string tableName;
            if (tableMap.TryGetValue(type, out tableName))
                return tableName;

            var tableAtt = type.GetCustomAttributes(typeof (TableAttribute), true);
            if (tableAtt.Length > 0)
                tableName = ((TableAttribute) tableAtt[0]).TableName;
            else
                tableName = type.Name;
            tableMap[type] = tableName;
            return tableName;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] LoadEntitys<T>() where T : class, IDataEntity, new()
        {
            MySqlConnection con = ConPool.GetConnection();
            string sql = string.Format("select * from {0}", GetTableName(typeof(T)));
            T[] ret = con.RecordQuery<T>(sql);
            ConPool.ReleaseContent(con);
            return ret;
        }


        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            var profile = DBEntityProfile<T>.Instance;
            profile.Update.Watch.Restart();
            var proMySql = MySQL.Instance;
            MySqlConnection con = null;
            try
            {
                profile.Update.TotalCount++;
                proMySql.Update.TotalCount++;

                con = ConPool.GetConnection();
                con.RecordUpdate(entity);
                ConPool.ReleaseContent(con);

                return 1;
            }
            catch
            {
                profile.Update.ErrorCount++;
                proMySql.Update.ErrorCount++;
                if (con != null)
                { 
                    con.Dispose();
                    ConPool.RemoveContent(con);    
                }
                throw;
            }
            finally
            {
                profile.Update.Watch.Stop();
                profile.Update.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMySql.Update.TotalTime += profile.Load.Watch.ElapsedTicks;
            }
        }

        /// <summary>
        /// 异步更新一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void SyncUpdateEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            ThreadQueue.AppendIO(() => UpdateEntity(entity));
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity<T>(T entity) where T : class, IDataEntity,new()
        {
            var profile = DBEntityProfile<T>.Instance;
            profile.Insert.Watch.Restart();
            var proMySql = MySQL.Instance;
            MySqlConnection con = null;

            try
            {
                profile.Insert.TotalCount++;
                proMySql.Insert.TotalCount++;

                con = ConPool.GetConnection();
                con.RecordInsert(entity);
                ConPool.ReleaseContent(con);
                return entity.Id;
            }
            catch
            {
                profile.Insert.ErrorCount++;
                proMySql.Insert.ErrorCount++;
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }


                throw;
            }
            finally
            {
                profile.Insert.Watch.Stop();
                profile.Insert.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMySql.Insert.TotalTime += profile.Load.Watch.ElapsedTicks;
            }

        }

        /// <summary>
        /// 插入一个log数据的实体类，对于这个对象没有主键id的需求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void InsertLogEntity<T>(T entity) where T : class, new()
        {
            var profile = DBEntityProfile<T>.Instance;
            profile.Insert.Watch.Restart();
            var proMySql = MySQL.Instance;
            MySqlConnection con = null;

            try
            {
                profile.Insert.TotalCount++;
                proMySql.Insert.TotalCount++;

                con = ConPool.GetConnection();
                con.RecordInsert(entity);
                ConPool.ReleaseContent(con);
            }
            catch
            {
                profile.Insert.ErrorCount++;
                proMySql.Insert.ErrorCount++;

                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }


                throw;
            }
            finally
            {
                profile.Insert.Watch.Stop();
                profile.Insert.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMySql.Insert.TotalTime += profile.Load.Watch.ElapsedTicks;
            }

        }


        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int DeleteEntity<T>(T entity) where T : class, IDataEntity,new()
        {
            var profile = DBEntityProfile<T>.Instance;
            profile.Delete.Watch.Restart();
            var proMySql = MySQL.Instance;
            MySqlConnection con = null;


            try
            {
                profile.Delete.TotalCount++;
                proMySql.Delete.TotalCount++;

                con = ConPool.GetConnection();
                con.RecordDelete(entity);
                ConPool.ReleaseContent(con);

                return 1;
            }
            catch
            {
                profile.Delete.ErrorCount++;
                proMySql.Delete.ErrorCount++;
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }


                throw;
            }
            finally
            {
                profile.Delete.Watch.Stop();
                profile.Delete.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMySql.Delete.TotalTime  += profile.Load.Watch.ElapsedTicks;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            var profile = DBEntityProfile<MySqlService>.Instance;
            profile.Query.Watch.Restart();
            var proMySql = MySQL.Instance;
            MySqlConnection con = null;

            try
            {
                profile.Query.TotalCount++;
                proMySql.Query.TotalCount++;

                con = ConPool.GetConnection();
                int ret = con.ExecuteNonQuery(sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch
            {
                profile.Query.ErrorCount++;
                proMySql.Query.ErrorCount++;
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }

                
                throw;
            }
            finally
            {
                profile.Query.Watch.Stop();
                profile.Query.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMySql.Query.TotalTime += profile.Load.Watch.ElapsedTicks;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql)
        {
            var profile = DBEntityProfile<MySqlService>.Instance;
            profile.Query.Watch.Restart();
            var proMySql = MySQL.Instance;
            MySqlConnection con = null;

            try
            {
                profile.Query.TotalCount++;
                proMySql.Query.TotalCount++;

                con = ConPool.GetConnection();
                DataSet ret = MySqlHelper.ExecuteDataset(con, sql);
                ConPool.ReleaseContent(con);
                return ret;
            }
            catch
            {
                profile.Query.ErrorCount++;
                proMySql.Query.ErrorCount++;
                if (con != null)
                {
                    con.Dispose();
                    ConPool.RemoveContent(con);
                }


                throw;
            }
            finally
            {
                profile.Query.Watch.Stop();
                profile.Query.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMySql.Query.TotalTime += profile.Load.Watch.ElapsedTicks;
            }
        }

        #endregion

        /// <summary>
        /// 异步出入一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void SyncInsertEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            ThreadQueue.AppendIO(() => InsertEntity(entity));
        }

        /// <summary>
        /// 替换sql注入文档
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSqlInjection(string str)
        {
            StringBuilder sb = new StringBuilder(str);

            sb.Replace("'", "");
            sb.Replace("like", "");
            sb.Replace("or", "");
            sb.Replace("\"", "");
            sb.Replace("exec", "");
            sb.Replace("run", "");
            sb.Replace("insert", "");
            sb.Replace("input", "");
            sb.Replace("update", "");

            return sb.ToString();
        }

        /// <summary>
        /// 数据库对应的分区id，这个值不一定存在
        /// </summary>
        public int ZoneId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MySQL : DBEntityProfile<MySQL>
    {
        
    }
}