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
        /// 获得一个类型的连接器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        MongoCollection GetCollection<T>()
        {
            var type = typeof(T);
            string connectionString = "mongodb://" + MangoDBConfig.Host;

            var client = new MongoClient(connectionString);

            var db = client.GetServer().GetDatabase(MangoDBConfig.Database);
            return db.GetCollection(type.Name);
        }


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
                return ret.Skip(page*pageSize).Take(pageSize).ToArray();

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
                return ret.Skip(page*pageSize).Take(pageSize).ToArray();
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
                Logs.Error("update entity {0} is null", typeof (T).Name);
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
            catch
            {
                profile.Update.ErrorCount++;
                proMangoDB.Update.ErrorCount++;

                throw;
            }
            finally
            {
                profile.Update.Watch.Stop();
                profile.Update.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMangoDB.Update.TotalTime  += profile.Load.Watch.ElapsedTicks;
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
                var ret = ThreadQueue.AppendIOCache(entity.GetHashCode(),obj.Update);
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
            catch
            {
                profile.Insert.ErrorCount++;
                proMangoDB.Insert.ErrorCount++;

                throw;
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
        public int DeleteEntity<T>(T entity) where T : class, IDataEntity,new()
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

                var db = GetCollection<T>();
                db.Remove(Query.EQ("_id", entity.Id));
                return 0;
            }
            catch
            {
                profile.Delete.ErrorCount++;
                proMangoDB.Delete.ErrorCount++;

                throw;
            }
            finally
            {
                profile.Delete.Watch.Stop();
                profile.Delete.TotalTime += profile.Load.Watch.ElapsedTicks;
                proMangoDB.Delete.TotalTime+= profile.Load.Watch.ElapsedTicks;
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

        class MangoDB : DBEntityProfile<MangoDB>
        {

        }
    }
}
