using System;
using System.Linq;
using DogSE.Library.Log;
using DogSE.Library.Thread;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DogSE.Server.Database.MangoDB
{
    /// <summary>
    /// mongodb日志用的服务
    /// </summary>
    /// <remarks>
    ///  这个的作用主要是做mongodb的数据插入，不限定插入的数据的格式
    /// </remarks>
    public class MongoDBLogService
    {
          /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="database"></param>
        public MongoDBLogService(string host, string database)
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
        public MongoDBLogService()
        {
            _host = MangoDBConfig.Host;
            _database = MangoDBConfig.Database;
        }

        /// <summary>
        /// 获得一个类型的连接器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        MongoCollection GetCollection<T>()
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
        /// 插入一个属性
        /// （如果可能，尽量使用异步插入）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity<T>(T entity) where T : class, new()
        {
            if (entity == null)
            {
                Logs.Error("insert entity {0} is null", typeof(T).Name);
                return 0;
            }

            var profile = DBEntityProfile<T>.Instance;
            profile.Insert.Watch.Restart();
            var proMangoDB = MangoDBService.MangoDB.Instance;

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
        public void SyncInsertEntity<T>(T entity) where T : class,  new()
        {
            ThreadQueue.AppendIO(() => InsertEntity(entity));
        }

        /// <summary>
        /// 通过Json查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonWhere"></param>
        /// <param name="num">返回的数量</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        public T[] QueryEntitys<T>(string jsonWhere, int num = 50, IMongoSortBy orderby = null)
        {
            var c = GetCollection<T>();
            
            var q = c.FindAs<T>(new QueryDocument(BsonDocument.Parse(jsonWhere)));
            if (orderby != null)
                return q.SetSortOrder(orderby).Take(num).ToArray();

            return q.Take(num).ToArray();
        }
    }
}

