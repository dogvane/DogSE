using System.Collections.Concurrent;
using System.Linq;
using DogSE.Library.Log;
using DogSE.Server.Core.Entity;
using TradeAge.Server.Entity.Character;

namespace TradeAge.Server.Entity
{
    /// <summary>
    /// 整个游戏世界的数据管理类，
    /// 整个服务器的全局对象数据都来这里访问
    /// </summary>
    public static class WorldEntityManager
    {
         static WorldEntityManager()
         {
             Accounts = new EntityMap<int, Account>();
             OnlinePlayers = new EntityMap<int, Player>();
             AccountNames = new EntityMap<string, Account>();

             Players = new EntityMap<int, SimplePlayer>();
             PlayerNames = new EntityMap<string, SimplePlayer>();

             PlayerCache = new HotDataManager<Player>();
         }

        /// <summary>
        /// 账号的全局数据
        /// </summary>
        public static EntityMap<int, Account> Accounts { get; private set; }

        /// <summary>
        /// 通过账号名对应账号数据
        /// </summary>
        public static EntityMap<string, Account> AccountNames { get; private set; }

        /// <summary>
        /// 玩家的在线数据
        /// </summary>
        public static EntityMap<int, Player> OnlinePlayers { get; private set; }

        /// <summary>
        /// 玩家数据的缓存
        /// </summary>
        public static HotDataManager<Player> PlayerCache { get; private set; }


        /// <summary>
        /// 全局的玩家列表
        /// </summary>
        public static EntityMap<int, SimplePlayer> Players { get; private set; }

        /// <summary>
        /// 全局的玩家名字对应的列表
        /// </summary>
        public static EntityMap<string, SimplePlayer> PlayerNames { get; private set; }

        #region EntityMap

        /// <summary>
        /// 实体类值的访问器，线程安全
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public class EntityMap<TKey, TValue>
        {
            private readonly ConcurrentDictionary<TKey, TValue> map = new ConcurrentDictionary<TKey, TValue>();

            /// <summary>
            /// 数量
            /// </summary>
            public int Count {
                get { return map.Count; }
            }

            /// <summary>
            /// 获得一个值
            /// </summary>
            /// <typeparam name="TKey"></typeparam>
            /// <param name="key"></param>
            /// <returns>如果key找不到对应值，返回null</returns>
            public TValue GetValue(TKey key)
            {
                TValue value;
                if (map.TryGetValue(key, out value))
                    return value;

                return default(TValue);
            }

            /// <summary>
            /// 设置一个值
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetValue(TKey key, TValue value)
            {
                if (arrayCache != null)
                {
                    TValue mapValue;
                    if (map.TryGetValue(key, out mapValue))
                    {
                        if (!mapValue.Equals(value))
                            arrayCache = null;
                    }
                    else
                    {
                        arrayCache = null;
                    }
                }
                map[key] = value;
            }

            private TValue[] arrayCache;

            /// <summary>
            /// 获得所有的数据（内部有缓存，如果数据没改变过，则返回同一个数组）
            /// </summary>
            /// <returns></returns>
            public TValue[] ToArray()
            {
                if (arrayCache == null)
                    arrayCache = map.Values.ToArray();

                return arrayCache;
            }

            /// <summary>
            /// 删除一个数据
            /// </summary>
            /// <param name="key"></param>
            public void Remove(TKey key)
            {
                TValue value;
                if (map.TryRemove(key, out value))
                    arrayCache = null;
            }

            public void Clear()
            {
                map.Clear();
                arrayCache = null;
            }
        } 

        #endregion
    }
}
