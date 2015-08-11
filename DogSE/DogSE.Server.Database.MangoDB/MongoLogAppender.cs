using System;
using DogSE.Library.Log;
using DogSE.Library.Time;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace DogSE.Server.Database.MangoDB
{
    /// <summary>
    /// Mogodb的日志写入器
    /// </summary>
    public static class MongoLogAppender
    {
        /// <summary>
        ///  创建一个日志写入器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbHost"></param>
        /// <param name="dbName"></param>
        /// <param name="createBaseLogEntity"></param>
        /// <returns></returns>
        public static ILogAppender CreateAppender<T>(string dbHost, string dbName, Func<T> createBaseLogEntity)
            where T : MongodbLogEntity, new()
        {
            if (createBaseLogEntity == null)
                throw new ArgumentNullException("createBaseLogEntity");

            //  先测试这个创建日志的方法是否正确
            var obj = createBaseLogEntity();
            if (obj == null)
                throw new NullReferenceException("createBaseLogEntity return null object.");

            var ret = new MongodbWriter<T>();
            ret.Host = dbHost;
            ret.Database = dbName;
            ret.CreateLogEntity = createBaseLogEntity;

            return ret;
        }

        private class MongodbWriter<T> : ILogAppender where T : MongodbLogEntity, new()
        {
            internal string Host;
            internal string Database;

            internal Func<T> CreateLogEntity;

            private MongoCollection GetCollection()
            {
                var type = typeof (T);
                string connectionString = "mongodb://" + Host;


                if (db == null)
                {
                    var client = new MongoClient(connectionString);
                    db = client.GetServer().GetDatabase(Database);
                }

                string typeName;
                if (type.IsGenericType)
                    typeName = type.Name.Substring(0, type.Name.IndexOf('`'));
                else
                    typeName = type.Name;

                return db.GetCollection(typeName);
            }

            private MongoDatabase db;

            public void WriterToDb(T entity)
            {
                if (entity == null)
                {
                    Logs.Error("insert entity {0} is null", typeof (T).Name);
                    return;
                }

                var profile = DBEntityProfile<T>.Instance;
                profile.Insert.Watch.Restart();
                var proMangoDB = MangoDBService.MangoDB.Instance;

                try
                {
                    profile.Insert.TotalCount++;
                    proMangoDB.Insert.TotalCount++;

                    var collection = GetCollection();
                    collection.Insert(entity);
                }
                catch (Exception ex)
                {
                    profile.Insert.ErrorCount++;
                    proMangoDB.Insert.ErrorCount++;

                    //throw new Exception(string.Format("Entity {0} insert fail.", typeof(T).Name), ex);
                    //  这里自己把错误日志吞掉
                }
                finally
                {
                    profile.Insert.Watch.Stop();
                    profile.Insert.TotalTime += profile.Load.Watch.ElapsedTicks;
                    proMangoDB.Insert.TotalTime += profile.Load.Watch.ElapsedTicks;
                }
            }

            public void Write(LogInfo info)
            {
                if (info.MessageFlag >= Level)
                {
                    var entity = CreateLogEntity();
                    entity.Time = OneServer.NowTime;
                    entity.LogLevel = info.MessageFlag;
                    if (info.Parameter == null)
                        entity.Message = info.Format;
                    else
                        entity.Message = string.Format(info.Format, info.Parameter);

                    WriterToDb(entity);
                }
            }

            public LogMessageType Level { get; set; }
        }
    }


    public class MongodbLogEntity
    {

        /// <summary>
        /// 日志的时间
        /// </summary>
        [BsonDateTimeOptions(Representation = BsonType.DateTime, Kind = DateTimeKind.Local)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 日志消息的内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogMessageType LogLevel { get; set; }
    }
}
