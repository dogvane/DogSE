using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DogSE.Server.Common;

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
        T LoadEntity<T>(Serial serial) where T : IDataEntity;

        /// <summary>
        /// 加载某个类型的所有实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T[] LoadEntitys<T>() where T : IDataEntity;


        /// <summary>
        /// 更新（新增）某个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns>
        /// </returns>
        int UpdateEntity<T>(T entity) where T : IDataEntity;


        /// <summary>
        /// 更新（新增）一组实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        int UpdateEntitys<T>(T[] entitys) where T : IDataEntity;

        /// <summary>
        /// 删除某个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int DeleteEntity<T>(T entity) where T : IDataEntity;

        /// <summary>
        /// 删除一组实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        int DeleteEntitys<T>(T[] entitys) where T : IDataEntity;

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
