using System;
using System.Linq;
using DogSE.Common;
using DogSE.Library.Common;
using DogSE.Library.Log;
using DogSE.Library.Thread;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace DogSE.Server.Database.MangoDB
{
    /// <summary>
    /// MangoDB的数据库操作器
    /// </summary>
    public class MangoDBService : IDataService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="database"></param>
        public MangoDBService(string host, string database)
        {
            _host = host;
            _database = database;
        }

        private string _host;
        private string _database;

        /// <summary>
        /// Mongodb数据库访问器
        /// 服务器信息从 MangoDBConfig 静态对象里获得
        /// 
        /// </summary>
        public MangoDBService()
        {
            _host = MangoDBConfig.Host;
            _database = MangoDBConfig.Database;
        }

        /// <summary>
        /// 获得一个类型的连接器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public MongoCollection GetCollection<T>()
        {
            var type = typeof(T);
            string connectionString = "mongodb://" + _host;


            if (db == null)
            {
                var client = new MongoClient(connectionString);
                db = client.GetServer().GetDatabase(_database);
            }

            string typeName;
            if (type.IsGenericType)
                typeName = type.Name.Substring(0, type.Name.IndexOf('`'));
            else
                typeName = type.Name;

            return db.GetCollection(typeName);
        }

        private MongoDatabase db;


        /// <summary>
        /// 根据id加载一个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serial"></param>
        /// <returns></returns>
        public T LoadEntity<T>(int serial) where T : class, IDataEntity, new()
        {
            var profile = DBEntityProfile<T>.Instance;
            profile.Load.Watch.Restart();
            var proMangoDB = MangoDB.Instance;

            try
            {
                profile.Load.TotalCount++;
                proMangoDB.Load.TotalCount++;

                var collection = GetCollection<T>();
                return collection.FindOneByIdAs<T>(serial);
            }
            catch
            {
                profile.Load.ErrorCount++;
                proMangoDB.Load.ErrorCount++;
                throw;
            }
            finally
            {
                profile.Load.Watch.Stop();
                profile.Load.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMangoDB.Load.TotalTime += profile.Load.Watch.ElapsedTicks;
            }
        }

        /// <summary>
        /// 根据名字做搜索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public T[] SeachEntityByName<T>(string where, int page = 0, int pageSize = 10) where T : class, IDataEntity, new()
        {
            var profile = DBEntityProfile<T>.Instance;
            profile.Seach.Watch.Restart();
            var proMangoDB = MangoDB.Instance;

            try
            {
                profile.Seach.TotalCount++;
                proMangoDB.Seach.TotalCount++;

                var collection = GetCollection<T>();
                var q = Query.EQ("Name", where);
                var ret = collection.FindAs<T>(q);
                return ret.Skip(page * pageSize).Take(pageSize).ToArray();

            }
            catch
            {
                profile.Seach.ErrorCount++;
                proMangoDB.Seach.ErrorCount++;
                throw;
            }
            finally
            {
                profile.Seach.Watch.Stop();
                profile.Seach.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMangoDB.Seach.TotalTime += profile.Load.Watch.ElapsedTicks;
            }
        }

        /// <summary>
        /// 对名字做模糊搜索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public T[] MatchesSeachEntityByName<T>(string where, int page = 0, int pageSize = 10) where T : class, IDataEntity, new()
        {
            var profile = DBEntityProfile<T>.Instance;
            profile.MatchesSeach.Watch.Restart();
            var proMangoDB = MangoDB.Instance;

            try
            {
                profile.MatchesSeach.TotalCount++;
                proMangoDB.MatchesSeach.TotalCount++;

                var collection = GetCollection<T>();
                var q = Query.Matches("Name", where);
                var ret = collection.FindAs<T>(q);
                return ret.Skip(page * pageSize).Take(pageSize).ToArray();
            }
            catch
            {
                profile.MatchesSeach.ErrorCount++;
                proMangoDB.MatchesSeach.ErrorCount++;

                throw;
            }
            finally
            {
                profile.MatchesSeach.Watch.Stop();
                profile.MatchesSeach.TotalTime += profile.MatchesSeach.Watch.ElapsedTicks;
                proMangoDB.MatchesSeach.TotalTime += profile.MatchesSeach.Watch.ElapsedTicks;
            }
        }

        /// <summary>
        /// 获得某个实例当前的最大id
        /// 如果没有数据，或者异常则返回1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int MaxId<T>() where T : class, IDataEntity, new()
        {
            try
            {
                var collection = GetCollection<T>();
                var ret = collection.FindAs<T>(null).SetSortOrder(new SortByDocument { { "_id", -1 } }).SetLimit(1);
                var arr = ret.ToArray();
                if (arr.Length > 0)
                    return arr[0].Id;
            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("Get {0} max id fail.", typeof(T).Name), ex);
            }

            return 1;
        }

        /// <summary>
        /// 加载所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] LoadEntitys<T>() where T : class, IDataEntity, new()
        {
            var collection = GetCollection<T>();
            return collection.FindAllAs<T>().ToArray();
        }

        /// <summary>
        /// 更像一个属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            if (entity == null)
            {
                Logs.Error("update entity {0} is null", typeof(T).Name);
                return 0;
            }

            var profile = DBEntityProfile<T>.Instance;
            profile.Update.Watch.Restart();
            var proMangoDB = MangoDB.Instance;

            try
            {
                profile.Update.TotalCount++;
                proMangoDB.Update.TotalCount++;

                var collection = GetCollection<T>();
                collection.Save(entity);
                return 1;
            }
            catch (Exception ex)
            {
                profile.Update.ErrorCount++;
                proMangoDB.Update.ErrorCount++;

                throw new Exception(string.Format("Entity {0} update fail.", typeof(T).Name), ex);
            }
            finally
            {
                profile.Update.Watch.Stop();
                profile.Update.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMangoDB.Update.TotalTime += profile.Load.Watch.ElapsedTicks;
            }
        }

        /// <summary>
        /// 异步保存一个实体到db
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SyncUpdateEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            if (MangoDBConfig.IOCache)
            {
                var obj = UpdateEntityPool<T>.AcquireContent(this, entity);
                var ret = ThreadQueue.AppendIOCache(entity.GetHashCode(), obj.Update);
                if (ret == false)
                {
                    //  更新失败的话，则将自己的这个对象放回对象池
                    obj.Release();
                }
            }
            else
                ThreadQueue.AppendIO(UpdateEntityPool<T>.AcquireContent(this, entity).Update);
        }


        private class UpdateEntityPool<T> where T : class, IDataEntity, new()
        {
            private static readonly ObjectPool<UpdateEntityPool<T>> s_pool = new ObjectPool<UpdateEntityPool<T>>(128);

            private MangoDBService DB;

            public void Update()
            {
                try
                {
                    DB.UpdateEntity(Entity);
                }
                finally
                {
                    Release();
                }
            }

            T Entity;

            public static UpdateEntityPool<T> AcquireContent(MangoDBService db, T entity)
            {
                var ret = s_pool.AcquireContent();
                ret.DB = db;
                ret.Entity = entity;
                return ret;
            }

            public void Release()
            {
                Entity = null;
                DB = null;
                s_pool.ReleaseContent(this);
            }
        }

        /// <summary>
        /// 插入一个属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            if (entity == null)
            {
                Logs.Error("insert entity {0} is null", typeof(T).Name);
                return 0;
            }

            var profile = DBEntityProfile<T>.Instance;
            profile.Insert.Watch.Restart();
            var proMangoDB = MangoDB.Instance;

            try
            {
                profile.Insert.TotalCount++;
                proMangoDB.Insert.TotalCount++;

                var collection = GetCollection<T>();
                collection.Insert(entity);
                return 0;
            }
            catch (Exception ex)
            {
                profile.Insert.ErrorCount++;
                proMangoDB.Insert.ErrorCount++;

                throw new Exception(string.Format("Entity {0} insert fail.", typeof(T).Name), ex);
            }
            finally
            {
                profile.Insert.Watch.Stop();
                profile.Insert.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMangoDB.Insert.TotalTime += profile.Load.Watch.ElapsedTicks;
            }
        }

        /// <summary>
        /// 异步插入一个属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void SyncInsertEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            ThreadQueue.AppendIO(() => InsertEntity(entity));
        }

        /// <summary>
        /// 删除一个属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int DeleteEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            if (entity == null)
            {
                Logs.Error("delete entity {0} is null", typeof(T).Name);
                return 0;
            }

            var profile = DBEntityProfile<T>.Instance;
            profile.Delete.Watch.Restart();
            var proMangoDB = MangoDB.Instance;


            try
            {
                profile.Delete.TotalCount++;
                proMangoDB.Delete.TotalCount++;

                var collection = GetCollection<T>();
                collection.Remove(Query.EQ("_id", entity.Id));
                return 0;
            }
            catch (Exception ex)
            {
                profile.Delete.ErrorCount++;
                proMangoDB.Delete.ErrorCount++;

                throw new Exception(string.Format("Entity {0} delete fail.", typeof(T).Name), ex);
            }
            finally
            {
                profile.Delete.Watch.Stop();
                profile.Delete.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMangoDB.Delete.TotalTime += profile.Load.Watch.ElapsedTicks;
            }


        }

        /// <summary>
        /// 异步删除一个属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void SyncDeleteEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            ThreadQueue.AppendIO(() => DeleteEntity(entity));
        }

        public int ExecuteSql(string sql)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet ExecuteDataSet(string sql)
        {
            throw new NotImplementedException();
        }

        public class MangoDB : DBEntityProfile<MangoDB>
        {

        }
    }
}
