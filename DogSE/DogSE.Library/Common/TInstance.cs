namespace DogSE.Library.Common
{
    /// <summary>
    /// 单例模板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TInstance<T> where T: new()
    {
        private static readonly T s_instance = new T();

        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance { get { return s_instance; } }
    }
}
