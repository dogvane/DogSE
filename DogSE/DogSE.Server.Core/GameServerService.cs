using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using DogSE.Library.Log;
using DogSE.Library.Thread;
using DogSE.Library.Util;
using DogSE.Server.Core.Config;
using System.ComponentModel;

namespace DogSE.Server.Core
{
    /// <summary>
    ///  服务器状态类型
    /// </summary>
    public enum ServerStateType
    {
        /// <summary>
        /// 启动中
        /// </summary>
        [Description("启动中")]
        Starting = 0,

        /// <summary>
        /// 运行中
        /// </summary>
        [Description("运行中")]
        Runing = 1,

        /// <summary>
        /// 关闭中
        /// </summary>
        [Description("关闭中")]
        Closing = 2,
    }

    /// <summary>
    /// 服务器维护状态
    /// </summary>
    public enum ServerMaintainStatus
    {
        /// <summary>
        /// 开启中
        /// </summary>
        [Description("开放")]
        Opening = 0,

        /// <summary>
        /// 维护中
        /// </summary>
        [Description("维护")]
        Maintain = 1,
    }

    /// <summary>
    ///     游戏服务器服务
    ///     这个是整个游戏的入口事件以及基本操作处理流程
    /// </summary>
    public static class GameServerService
    {
        /// <summary>
        ///     最初的配置（一般用于初始化配置文件）
        /// </summary>
        public static Func<bool> FirstInit;

        /// <summary>
        ///     初始化目标文件
        /// </summary>
        public static Func<bool> InitTemplate;

        /// <summary>
        ///     加载基础数据
        /// </summary>
        public static Func<bool> LoadDefaultGameData;

        /// <summary>
        ///     模块初始化前
        /// </summary>
        public static Func<bool> BeforeModuleInit;

        /// <summary>
        ///     模块初始化后
        /// </summary>
        /// <remarks>
        ///     这个时候socket已经打开了
        /// </remarks>
        public static Func<bool> AfterModuleInit;

        /// <summary>
        ///     停止监听(开始停止游戏服务器进程)
        /// </summary>
        public static Func<bool> BeforeStopListen;

        /// <summary>
        ///     停止游戏（服务器完全停止前做最后的清理工作）
        /// 例如等待某些需要退出的线程完成最后的清理工作
        /// </summary>
        public static Func<bool> AfterStopListen;

        private static WorldBase s_world;

        /// <summary>
        /// 获得当前游戏世界实例
        /// </summary>
        /// <returns></returns>
        public static WorldBase GetWorldInstatnce()
        {
            return s_world;
        }

        /// <summary>
        ///     是否是控制台运行（默认是否）
        ///     如果是控制台运行
        ///     则会StartGame执行后不会退出
        /// </summary>
        public static bool IsConsoleRun { get; set; }

        /// <summary>
        ///     开启游戏
        /// </summary>
        /// <remarks>
        ///     游戏启动执行顺序
        ///     1.FirstInit
        ///     2.InitTemplate
        ///     3.LoadDefaultGameData
        ///     4.BeforeModuleInit
        ///     5.world.Start()
        ///     6.AfterModuleInit
        /// </remarks>
        /// <param name="world"></param>
        public static void StartGame(WorldBase world)
        {
            if (world == null)
                throw new NullReferenceException("world");

            s_world = world;

            RunType = ServerStateType.Starting;

            //  先加载最基础的配置文件
            StaticConfigFileManager.LoadData();

            if (FirstInit != null)
                IsErrorToExitProgram(FirstInit());

            if (InitTemplate != null)
                IsErrorToExitProgram(InitTemplate());

            if (LoadDefaultGameData != null)
                IsErrorToExitProgram(LoadDefaultGameData());

            if (BeforeModuleInit != null)
                IsErrorToExitProgram(BeforeModuleInit());

            world.StartWorld();

            if (AfterModuleInit != null)
                IsErrorToExitProgram(AfterModuleInit());

            RunType = ServerStateType.Runing;

            if (IsConsoleRun)
            {
                StartCommandlinesDisposal();
            }
        }

        private static List<string> s_Commandlines = new List<string>(1024);
        private static int s_CommandlinesIndex;

        /// <summary>
        /// 命令行命令回调. 当控制台收到命令后回调该函数.
        /// </summary>
        /// <param name="cmdStr">命令字符串. 必须的.</param>
        /// <returns>如果要求控制台继续执行则返回true; 否则返回false.</returns>
        public delegate bool CommandCallbackDelegate(String cmdStr);
        private static readonly Dictionary<String, CommandCallbackDelegate> s_cmdProcList =
            new Dictionary<string, CommandCallbackDelegate>(64);

        /// <summary>
        /// 向控制台注册一个控制台命令
        /// </summary>
        /// <param name="commandName">控制台名称</param>
        /// <param name="fun">回调方法</param>
        public static void RegisterConsoleCommand(string commandName, CommandCallbackDelegate fun)
        {
            if (string.IsNullOrEmpty(commandName))
                throw new ArgumentNullException("commandName");

            if (fun == null)
                throw new ArgumentNullException("fun");

            var name = commandName.ToLower();
            if (s_cmdProcList.ContainsKey(name))
            {
                Logs.Warn("控制台命令行已经注册过 {0} 的命令。", commandName);
            }

            s_cmdProcList[name] = fun;
        }

        /// <summary>
        /// 服务器是否处于关闭状态
        /// </summary>
        public static ServerStateType RunType { get; set; }

        /// <summary>
        /// 维护状态
        /// 一般来说，进入维护状态后
        /// 普通玩家就不运行进入游戏
        /// 但是GM账号的玩家还是可以进入的
        /// 这里只记录状态，相关禁止进入由逻辑模块进行
        /// </summary>
        public static ServerMaintainStatus MaintainStatus { get; set; }
        
        /// <summary>
        /// 8 - 2) 开始命令行(在主线程处理)
        /// </summary>
        private static void StartCommandlinesDisposal()
        {
            //  如果启动默认的控制台功能，则把控制台程序的关闭按钮给取消
            //  防止误关闭没有执行一些必要的退出流程
            ConsoleUtils.RemoveSystemCloseMenu();

            //--------------------------------------
            // XG: 后加的
            //RegisterConsoleCommand("restart", cmd_restart);
            RegisterConsoleCommand("exit", cmd_exit);

            //const string DOS_PROMPT = "DogSE:>";

            while (RunType == ServerStateType.Runing)
            {
                ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
                StringBuilder strStringBuilder = new StringBuilder();
                string strReadLine = string.Empty;

                do
                {
                    #region 等待键盘的输入

                    try
                    {
                        while (true)
                        {
                            //  这里通过100ms间隔的方式检测是否有键盘输入事件
                            //  如果是，则进行下面的逻辑处理，如果没有判断服务器是否要停止
                            //  如果需要停止，则不再走下面的流程，这样才能让主线程退出
                            //  添加这个的目的是为了让GM命令使服务器退出，而不卡在主线程的键盘输入等待里
                            if (Console.KeyAvailable)
                            {
                                keyInfo = Console.ReadKey(false);
                                break;
                            }

                            if (RunType == ServerStateType.Closing)
                                break;

                            Thread.Sleep(100);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logs.Error("Console read key fail.", ex);
                        if (RunType == ServerStateType.Closing)
                            break;
                        continue;
                    }

                    #endregion

                    // 再次检测程序已经运行
                    if (RunType == ServerStateType.Closing)
                        break;

                    // 像是串一个命令行, 如果收到"回车"则退出while
                    #region 对输入的按键（回车，空格，退格，方向键【用于快速切换上下命令】）做处理

                    // 回车
                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        strReadLine = strStringBuilder.ToString();
                        strStringBuilder.Clear();

                        // 如果输入的键值为空，继续等待输入
                        if (string.IsNullOrEmpty(strReadLine))
                            continue;
                        break;
                    }

                    // 退格
                    if (keyInfo.Key == ConsoleKey.Backspace)
                    {
                        if (strStringBuilder.Length > 0)
                            strStringBuilder.Remove(strStringBuilder.Length - 1, 1);
                        else
                            continue;
                    }
                    else if (keyInfo.Key == ConsoleKey.UpArrow)
                    {
                        s_CommandlinesIndex--;
                        int iCommandlinesIndex = s_CommandlinesIndex;

                        if (iCommandlinesIndex < 0)
                        {
                            s_CommandlinesIndex = 0;
                            continue;
                        }
                        else
                        {
                            strStringBuilder.Clear();
                            strStringBuilder.Append(s_Commandlines[iCommandlinesIndex]);
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                    {
                        s_CommandlinesIndex++;
                        int iCommandlinesIndex = s_CommandlinesIndex;

                        if (iCommandlinesIndex >= s_Commandlines.Count)
                        {
                            s_CommandlinesIndex = s_Commandlines.Count - 1;
                            continue;
                        }
                        else
                        {
                            strStringBuilder.Clear();
                            strStringBuilder.Append(s_Commandlines[iCommandlinesIndex]);
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.Spacebar)
                    {
                        if (strStringBuilder.Length < 255)
                            strStringBuilder.Append(keyInfo.KeyChar);
                        else
                            continue;
                    }
                    else
                    {
                        if (((int)keyInfo.Key >= 65 && (int)keyInfo.Key <= 90) || ((int)keyInfo.Key >= 97 && (int)keyInfo.Key <= 122)
                            || ((keyInfo.KeyChar >= '0') && keyInfo.KeyChar <= '9'))
                        {
                            // 最多 255 个有效的字符
                            if (strStringBuilder.Length < 255)
                                strStringBuilder.Append(keyInfo.KeyChar);
                            else
                                continue;
                        }
                    }

                    #endregion

                    strReadLine = strStringBuilder.ToString();
                    //Logs.WriteLine(true, LogMessageType.MSG_INPUT, strReadLine);

                } while (true);

                if (string.IsNullOrEmpty(strReadLine))
                {
                    continue;
                }

                s_Commandlines.Add(strReadLine);
                s_CommandlinesIndex++;

                string cmdStr = strReadLine.TrimStart();
                var first = cmdStr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var op = string.Empty;
                if (first.Length > 0)
                    op = first[0].ToLower();

                if (first.Length > 0 && s_cmdProcList.ContainsKey(op))
                {
                    try
                    {
                        if (!s_cmdProcList[op](cmdStr))
                            Logs.Error(string.Format("run gm command fail. {0}", op));
                    }
                    catch (Exception ex)
                    {
                        Logs.Error(string.Format("run gm exception. command={0}", op), ex);
                    }
                }
                else
                {
                    if (strReadLine != string.Empty)
                        Logs.Warn("命令({0}) : 未知的无效命令", cmdStr);
                }

            } // while

            StopGame();
        }

        private static bool cmd_exit(string cmdStr)
        {
            RunType = ServerStateType.Closing;
            
            return true;
        }


        private static void IsErrorToExitProgram(bool isNotError = true)
        {
            if (!isNotError)
            {
                RunType = ServerStateType.Closing;
                Process.GetCurrentProcess().Close();
            }
        }

        /// <summary>
        ///     停止游戏
        /// </summary>
        public static void StopGame()
        {
            RunType = ServerStateType.Closing;

            if (BeforeStopListen != null)
                BeforeStopListen();

            if (s_world != null)
                s_world.StopWorld();

            if (AfterStopListen != null)
                AfterStopListen();

            Logs.Info("wait thread queue exit.");
            //  等待所有的线程队列退出
            while (ThreadQueueEntity.HasActionInAllQueue)
            {
                Thread.Sleep(100);
            }
        }
    }
}