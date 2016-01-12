using DogSE.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DogSE.Server.Database.MySQL
{
    /// <summary>
    /// 带分区的mysql数据库访问器
    /// </summary>
    public class MySqlZoneService : IDataService
    {

        /// <summary>
        /// 增加一个id和分区的映射关系
        /// </summary>
        /// <param name="id"></param>
        /// <param name="zoneId"></param>
        public void AddIdZoneMap(int id, int zoneId)
        {
            IdMap[id] = zoneId;
        }

        /// <summary>
        /// 增加一个数据访问分区和对应数据库的映射关系
        /// </summary>
        /// <param name="zoneId"></param>
        /// <param name="connectString"></param>
        public void AddMysqlService(int zoneId, string connectString)
        {
            //zoneMap[zoneId] = new MySqlService(connectString);
            zoneMap[zoneId] = new MySqlServiceNoPools(connectString);
        }

        //private Dictionary<int, MySqlService> zoneMap = new Dictionary<int, MySqlService>();
        private Dictionary<int, MySqlServiceNoPools> zoneMap = new Dictionary<int, MySqlServiceNoPools>();
        /// <summary>
        /// id和zoneId的关系表
        /// </summary>
        private Dictionary<int, int> IdMap = new Dictionary<int, int>();

        public T LoadEntity<T>(int serial) where T : class, IDataEntity, new()
        {
            int zoneId;
            if (!IdMap.TryGetValue(serial, out zoneId))
            {
                throw new Exception(string.Format("Load {0} id {1} not find zoneId", typeof (T).Name, serial));
            }

            var db = zoneMap[zoneId];
            return db.LoadEntity<T>(serial);
        }

        /// <summary>
        /// 加载全部数据库里的数据
        /// 这里会逐个访问数据库 zoneMap 里对应的数据库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] LoadEntitys<T>() where T : class, Common.IDataEntity, new()
        {
            List<T> rets = new List<T>();

            foreach (var item in zoneMap.Keys.ToArray())
            {
                rets.AddRange(LoadEntitys<T>(item));
            }

            return rets.ToArray();
        }

        /// <summary>
        /// 加载某个数据库里的所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public T[] LoadEntitys<T>(int zoneId) where T : class, Common.IDataEntity, new()
        {
            var db = zoneMap[zoneId];
            return db.LoadEntitys<T>();
        }

        /// <summary>
        /// 更新实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateEntity<T>(T entity) where T : class, Common.IDataEntity, new()
        {
            var serial = entity.Id;

            int zoneId;
            if (!IdMap.TryGetValue(serial, out zoneId))
            {
                throw new Exception(string.Format("Update {0} id {1} not find zoneId", typeof(T).Name, serial));
            }

            var db = zoneMap[zoneId];
            return db.UpdateEntity(entity);
        }

        public int InsertEntity<T>(T entity) where T : class, Common.IDataEntity, new()
        {
            var serial = entity.Id;

            int zoneId;
            if (!IdMap.TryGetValue(serial, out zoneId))
            {
                throw new Exception(string.Format("Insert {0} id {1} not find zoneId", typeof(T).Name, serial));
            }

            var db = zoneMap[zoneId];
            return db.InsertEntity(entity);
        }

        public int DeleteEntity<T>(T entity) where T : class, Common.IDataEntity, new()
        {
            var serial = entity.Id;

            int zoneId;
            if (!IdMap.TryGetValue(serial, out zoneId))
            {
                throw new Exception(string.Format("Delete {0} id {1} not find zoneId", typeof(T).Name, serial));
            }

            var db = zoneMap[zoneId];
            return db.DeleteEntity(entity);
        }

        public int ExecuteSql(string sql)
        {
            throw new NotImplementedException("这里需要带分区才能访问");
        }

        public DataSet ExecuteDataSet(string sql)
        {
            throw new NotImplementedException("这里需要带分区才能访问");
        }
    }
}
