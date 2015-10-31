using System.Collections.Generic;
using System.Linq;

namespace TradeAge.Client.Core.Net
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ConcurrentDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> dir = new Dictionary<TKey, TValue>();

        /// <summary>
        /// </summary>
        /// <param name="i"></param>
        /// <param name="handlerCapacitySize"></param>
        public ConcurrentDictionary(int i, int handlerCapacitySize)
        {
            dir = new Dictionary<TKey, TValue>(handlerCapacitySize);
        }

        /// <summary>
        /// </summary>
        public ConcurrentDictionary()
        {
        }

        /// <summary>
        /// </summary>
        public int Count
        {
            get { return dir.Count; }
        }

        /// <summary>
        /// </summary>
        public TValue[] Values
        {
            get
            {
#if !UNITY_IPHONE
                lock (dir)
#endif
                {
                    return dir.Values.ToArray();
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal TValue GetOrAdd(TKey key, TValue value)
        {
#if !UNITY_IPHONE
            lock (dir)
#endif
            {
                if (dir.ContainsKey(key))
                    return dir[key];

                dir.Add(key, value);
            }

            return value;
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryRemove(TKey key, out TValue value)
        {
            value = default(TValue);
#if !UNITY_IPHONE
            lock (dir)
#endif
            {
                if (dir.ContainsKey(key) == false)
                    return false;
                dir.Remove(key);
                return true;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
#if !UNITY_IPHONE
            lock (dir)
#endif
            {
                if (dir.ContainsKey(key) == false)
                    return false;
                value = dir[key];

                return true;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryAdd(TKey key, TValue value)
        {
#if !UNITY_IPHONE
            lock (dir)
#endif
            {
                if (dir.ContainsKey(key))
                    return true;

                dir.Add(key, value);
            }

            return true;
        }
    }
}