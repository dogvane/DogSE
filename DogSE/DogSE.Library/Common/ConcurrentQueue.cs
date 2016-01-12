using System.Collections.Generic;

namespace DogSE.Library.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentQueue<T>
    {
        private Queue<T> queue = new Queue<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public void Enqueue(T p)
        {
#if !UNITY_IPHONE
            lock (queue)
#endif
            {
                queue.Enqueue(p);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnT"></param>
        /// <returns></returns>
        public bool TryDequeue(out T returnT)
        {
            returnT = default(T);

#if !UNITY_IPHONE
            lock (queue)
#endif
            {
                if (queue.Count == 0)
                    return false;
                returnT = queue.Dequeue();

                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public long Count
        {
            get
            {
                return queue.Count;
            }

        }
    }
}
