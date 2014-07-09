using System.Data;
using DogSE.Common;

namespace DogSE.Server.Database
{
    /// <summary>
    /// 游戏里的数据访问接口
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// 通过实体id，加载某个具体的实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serial"></param>
        /// <returns></returns>
        T LoadEntity<T>(int serial) where T : class, IDataEntity, new();

        /// <summary>
        /// 加载某个类型的所有实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T[] LoadEntitys<T>() where T : class, IDataEntity, new();


        /// <summary>
        /// 更新（新增）某个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns>
        /// </returns>
        int UpdateEntity<T>(T entity) where T : class, IDataEntity, new();


        /// <summary>
        /// 更新（新增）某个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns>
        /// </returns>
        int InsertEntity<T>(T entity) where T : class, IDataEntity, new();

        /// <summary>
        /// 删除某个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int DeleteEntity<T>(T entity) where T : class, IDataEntity, new();

        /// <summary>
        /// 执行一组sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteSql(string sql);
        
        /// <summary>
        /// 执行并返回一个结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(string sql);
    }
}
