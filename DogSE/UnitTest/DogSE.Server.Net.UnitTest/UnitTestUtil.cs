using System;
using System.Threading;

namespace DogSE.Server.Net.UnitTest
{
    public static class UnitTestUtil
    {
        public static bool Wait(int waitTimeSec, Func<bool> fun)
        {
            var startTime = DateTime.Now;
            while((DateTime.Now - startTime).TotalMilliseconds < waitTimeSec)
            {
                Thread.Sleep(1);
                if (fun())
                    return true;
            }

            return false;
        }
    }
}