using System.Collections.Generic;
using System.Diagnostics;
using DogSE.Common;

namespace DogSE.Server.Database
{
    /// <summary>
    /// 抽象的性能监控类
    /// </summary>
    public abstract class AbstractDBEntityProfile
    {
        /// <summary>
        /// 统计的实体的名字
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 加载
        /// </summary>
        public AccessData Load;

        /// <summary>
        /// 插入
        /// </summary>
        public AccessData Insert;

        /// <summary>
        /// 更新
        /// </summary>
        public AccessData Update;

        /// <summary>
        /// 更新
        /// </summary>
        public AccessData Delete;

        /// <summary>
        /// 查询
        /// </summary>
        public AccessData Query;

        /// <summary>
        /// 搜索
        /// </summary>
        public AccessData Seach;

        /// <summary>
        /// 模糊搜索
        /// </summary>
        public AccessData MatchesSeach;
    }

    /// <summary>
    /// 性能统计工具
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DBEntityProfile<T> : AbstractDBEntityProfile
    {

        private static DBEntityProfile<T> s_instance;

        /// <summary>
        /// 单例模式
        /// </summary>
        public static AbstractDBEntityProfile Instance
        {
            get { return s_instance ?? (s_instance = new DBEntityProfile<T>()); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DBEntityProfile()
        {
            Load.Watch = new Stopwatch();
            Insert.Watch = new Stopwatch();
            Update.Watch = new Stopwatch();
            Delete.Watch = new Stopwatch();
            Seach.Watch = new Stopwatch();
            MatchesSeach.Watch = new Stopwatch();
            Query.Watch = new Stopwatch();

            Name = typeof (T).Name;

            EntityProfileManager.AddEntityProfile(this);
        }
    }

    /// <summary>
    /// 访问统计
    /// </summary>
    public struct AccessData
    {
        /// <summary>
        /// 加载访问次数
        /// </summary>
        public int TotalCount;

        /// <summary>
        /// 访问耗时，单位Ticks
        /// </summary>
        public long TotalTime;

        /// <summary>
        /// 发生错误次数
        /// </summary>
        public int ErrorCount;

        /// <summary>
        /// 监视器
        /// </summary>
        public Stopwatch Watch;
    }

    /// <summary>
    /// 性能监控管理类
    /// </summary>
    public static class EntityProfileManager
    {
        private static readonly List<AbstractDBEntityProfile> list = new List<AbstractDBEntityProfile>();

        /// <summary>
        /// 添加一个监控对象
        /// </summary>
        /// <param name="profile"></param>
        public static void AddEntityProfile(AbstractDBEntityProfile profile)
        {
            list.Add(profile);
            isNew = true;
        }

        private static bool isNew = true;

        private static AbstractDBEntityProfile[] cache;

        /// <summary>
        /// 获得数据库的监控数据
        /// </summary>
        /// <returns></returns>
        public static AbstractDBEntityProfile[] GetDBProfiles()
        {
            if (isNew || cache == null)
            {
                cache = list.ToArray();
                isNew = false;
            }

            return cache;
        }
    }
}
