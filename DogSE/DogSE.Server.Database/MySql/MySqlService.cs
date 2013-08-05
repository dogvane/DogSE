using System;
using System.Data;
using DogSE.Server.Common;

namespace DogSE.Server.Database.MySql
{
    /// <summary>
    /// Mysql对数据访问接口的实现
    /// </summary>
    internal class MySqlService : IDataService
    {
        #region IDataService Members

        public T LoadEntity<T>(Serial serial) where T : IDataEntity
        {
            throw new NotImplementedException();
        }

        public T[] LoadEntitys<T>() where T : IDataEntity
        {
            throw new NotImplementedException();
        }

        public int UpdateEntity<T>(T entity) where T : IDataEntity
        {
            throw new NotImplementedException();
        }

        public int UpdateEntitys<T>(T[] entitys) where T : IDataEntity
        {
            throw new NotImplementedException();
        }

        public int DeleteEntity<T>(T entity) where T : IDataEntity
        {
            throw new NotImplementedException();
        }

        public int DeleteEntitys<T>(T[] entitys) where T : IDataEntity
        {
            throw new NotImplementedException();
        }

        public int ExecuteSql(string sql)
        {
            throw new NotImplementedException();
        }

        public DataSet ExecuteDataSet(string sql)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}