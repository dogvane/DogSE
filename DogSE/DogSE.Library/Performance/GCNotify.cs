using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DogSE.Library.Performance
{
    /// <summary>
    /// GC通知功能
    /// </summary>
    public static class GCNotify
    {
        static Action<int> s_GCNotify;

        /// <summary>
        /// GC通知事件
        /// </summary>
        public static event Action<int> Notify
        {
            add
            {
                if (s_GCNotify == null)
                {
                    new GCObject(0);
                    new GCObject(2);
                }
                s_GCNotify += value;
            }
            remove { if (value != null && s_GCNotify != null) s_GCNotify -= value; }
        }

        class GCObject
        {
            ~GCObject()
            {
                var currentG = GC.GetGeneration(this);
                if (currentG == G)
                {
                    var f = s_GCNotify;
                    if (f != null)
                    {
                        f(G);
                    }
                }

                if (s_GCNotify != null && !AppDomain.CurrentDomain.IsFinalizingForUnload())
                {
                    if (G == 0)
                        new GCObject(0);
                    else
                        GC.ReRegisterForFinalize(this);
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="g"></param>
            public GCObject(int g)
            {
                G = g;
            }

            /// <summary>
            /// 第几代
            /// </summary>
            private int G;

        }
    }


}
