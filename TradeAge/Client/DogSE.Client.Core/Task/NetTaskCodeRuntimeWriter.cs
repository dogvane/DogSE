using System;
using System.IO;
using System.Text;

namespace DogSE.Client.Core.Task
{
    /// <summary>
    /// 网络任务执行耗时记录
    /// </summary>
    static class NetTaskCodeRuntimeWriter
    {
        private static DateTime _nextDay;
        private static StreamWriter _writer;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            var now = DateTime.Now;

            OpenFile(now);
            _nextDay = now.Date.AddDays(1);
        }

        /// <summary>
        /// 按照某个时间打开一个新的时间文件
        /// </summary>
        /// <param name="now"></param>
        private static void OpenFile(DateTime now)
        {
            var fileName = string.Format(@"data\{0}\NetTaskTime {1}.csv", now.ToString("yyyy-MM"), now.ToString("yyyy-MM-dd"));

            if (_writer != null)
                _writer.Close();

            if (!File.Exists(fileName)) //  文件不存在则新建一个文件
            {
                new FileInfo(fileName).Directory.Create(); //  创建日志文件目录

                //  纯数字和英文的数据记录，因此开ascii的编码写入
                //  开一个2M的缓冲区，应该够日志数据的收集了。
                _writer = new StreamWriter(fileName, true, Encoding.UTF8, 1024*1024*2);
                _writer.WriteLine("TimeTick,Code,PlayerId,RunTime,WaitTime,IsException");
                //  不用自动刷新，我们有一个定时器每隔1s进行一次刷新，正常些日志时，数据会先些在写入流的缓冲区里的
                _writer.AutoFlush = false;
            }
            else
            {
                _writer = new StreamWriter(fileName, true, Encoding.UTF8, 1024*1024*2); //  开一个2M的缓冲区，应该够日志数据的收集了。
                _writer.AutoFlush = false;
            }
        }

        /// <summary>
        /// 记录数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="playerId">网络接口id（玩家id）</param>
        /// <param name="runTime"></param>
        /// <param name="waitTime"></param>
        /// <param name="isException">是否抛出异常</param>
        public static void Write(ushort code, long playerId, long runTime, long waitTime, bool isException)
        {
            try
            {
                var now = DateTime.Now;
                if (now > _nextDay)
                {
                    //  如果过了一天，则新建一个文件
                    OpenFile(now);
                    _nextDay = now.Date.AddDays(1);
                }
                var tick = now.Ticks;
                _writer.Write(tick);
                _writer.Write(',');
                _writer.Write(code);
                _writer.Write(',');
                _writer.Write(playerId);
                _writer.Write(',');
                _writer.Write(runTime);
                _writer.Write(',');
                _writer.Write(waitTime);
                _writer.Write(',');
                _writer.Write(isException ? 1 : 0);
                _writer.Write("\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// 刷新日志文件
        /// </summary>
        public static void Flush()
        {
            try
            {
                if (_writer != null)
                    _writer.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        /// <summary>
        /// 关闭文件
        /// </summary>
        public static void Close()
        {
            try
            {
                if (_writer != null)
                {
                    _writer.Flush();
                    _writer.Close();
                    _writer = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
