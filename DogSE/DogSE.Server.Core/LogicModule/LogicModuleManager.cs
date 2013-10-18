using System;
using System.Collections.Generic;
using System.Linq;
using DogSE.Library.Log;
using DogSE.Server.Core.ServerState;
using DogSE.Server.Core.Util;

namespace DogSE.Server.Core.LogicModule
{
    /// <summary>
    /// 逻辑模块管理
    /// </summary>
    /// <remarks>
    /// 功能模块的初始化需要在静态模板加载完成后才进行
    /// 因为功能的加载依赖于已经初始化好的模板数据
    /// 
    /// ReLoadTemplate() 
    /// 方法主要用于服务器动态更新数据后使用
    /// </remarks>
    public class LogicModuleManager
    {
        /// <summary>
        /// 当前系统加载的逻辑模块
        /// </summary>
        private readonly List<ILogicModule> modules = new List<ILogicModule>();

        /// <summary>
        /// 初始化Logic modules
        /// </summary>
        /// <remarks>
        /// 方法在 InitOnceServer_Step1() 里被调用
        /// </remarks>
        public void Initializationing()
        {
            var dependenceModules = new List<KeyValuePair<InitDependenceAttribute, ILogicModule>>();
            foreach (var type in AssemblyUtil.GetTypesByInterface(typeof(ILogicModule)))
            {
                string moduleId = string.Empty;

                try
                {
                    var obj = Activator.CreateInstance(type);
                    var module = (ILogicModule) obj;
                    moduleId = module.ModuleId;

                    //  获得模块的初始化依赖
                    var dependances = type.GetCustomAttributes(typeof (InitDependenceAttribute), false);
                    if (dependances.Length > 0)
                    {
                        var dependance = dependances[0] as InitDependenceAttribute;
                        if (dependance != null)
                        {
                            dependenceModules.Add(
                                new KeyValuePair<InitDependenceAttribute, ILogicModule>(dependance, module));
                        }
                    }

                    //  忽略某些模块的初始化
                    var ignore = type.GetCustomAttributes(typeof (IgnoreInitializationAttribute), true);
                    if (ignore.Length > 0)
                    {
                        Logs.Info("Ignore {0} init.", moduleId);
                        continue;
                    }

                    //  一般逻辑模块会要求实现一个模块状态输出功能
                    if (type.GetInterface(typeof (IServerState).Name) != null)
                    {
                        ServerStateManager.Register((IServerState) obj);
                    }

                    Logs.Info("Craete module {0}", moduleId);

                    modules.Add(module);
                }
                catch (Exception ex)
                {
                    Logs.Error("LogicModule.CreateModule fail. ModuleName:{0} Type:{1}    ",
                               moduleId, type.Name, ex);
                }
            }

            foreach(var dp in dependenceModules)
            {
                modules.Remove(dp.Value);
            }

            while (dependenceModules.Count > 0)
            {
                bool isChange = false;
                foreach (var dp in dependenceModules.ToArray())
                {
                    int content = 0;
                    foreach (var moduleName in dp.Key.Dependences)
                    {
                        content += modules.Count(o => o.ModuleId == moduleName);
                    }

                    if (content == dp.Key.Dependences.Length)
                    {
                        //  满足依赖条件
                        modules.Add(dp.Value);
                        isChange = true;
                        dependenceModules.Remove(dp);
                    }
                }

                if (!isChange)
                {
                    string moduleName = string.Empty;
                    dependenceModules.ForEach(o => { moduleName += o.Value.ModuleId + ","; });
                    Logs.Error("模块依赖可能存在循环依赖问题，请检查以下模块：{0}", moduleName.TrimEnd(','));

                    modules.AddRange(dependenceModules.Select(o => o.Value).ToArray());
                    break;
                }
            }

            foreach (var module in modules)
            {
                try
                {
                    module.Initializationing();
                }
                catch (Exception ex)
                {
                    Logs.Error("LogicModule.Initializationing fail. moduleId:{0}", module.ModuleId, ex);
                }
            }
        }

        /// <summary>
        /// 初始化所有的逻辑模块
        /// </summary>
        /// <remarks>
        /// 注意方法会在 InitOnceServer_Step2 里被调用
        /// </remarks>
        public void Initializationed()
        {
            foreach (var module in modules)
            {
                try
                {
                    module.Initializationed();
                }
                catch (Exception ex)
                {
                    Logs.Error("LogicModule.Initializationed fail. moduleId:{0}", module.ModuleId, ex);
                }
            }
        }

        /// <summary>
        /// 重新加载模板文件后的逻辑处理
        /// </summary>
        public void ReLoadTemplate()
        {
            foreach (var module in modules)
            {
                try
                {
                    module.ReLoadTemplate();
                }
                catch (Exception ex)
                {
                    Logs.Error(string.Format("LogicModule.ReLoadTemplate fail. moduleId:{0}", module.ModuleId),
                               ex);
                }
            }
        }

        /// <summary>
        /// 推出时释放逻辑模块
        /// </summary>
        public void Release()
        {
            foreach (var module in modules)
            {
                try
                {
                    module.Release();
                }
                catch (Exception ex)
                {
                    Logs.Error(string.Format("LogicModule.Release fail. moduleId:{0}", module.ModuleId),
                               ex);
                }
            }
        }

        /// <summary>
        /// 通过模块ID来获得模块实例
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public ILogicModule GetModule(string moduleId)
        {
            return modules.FirstOrDefault(o => o.ModuleId == moduleId);
        }

        /// <summary>
        /// 获得当前管理器里所有的模块
        /// </summary>
        internal ILogicModule[] GetModules()
        {
            return modules.ToArray();
        }
    }

    /// <summary>
    /// 逻辑模块的依赖（初始化依赖）
    /// </summary>
    public class InitDependenceAttribute : Attribute
    {
        /// <summary>
        /// 模块的依赖
        /// </summary>
        /// <param name="moduleNames"></param>
        public InitDependenceAttribute(params string[] moduleNames)
        {
            if (moduleNames == null)
                Dependences = new string[0];
            else
                Dependences = moduleNames;
        }

        /// <summary>
        /// 依赖模块的模块名
        /// </summary>
        public string[] Dependences { get; private set; }
    }

    /// <summary>
    /// 模块不进行初始化，在正式发布项目里，如果要屏蔽某个模块，可以用给模块打这个标签
    /// </summary>
    public class IgnoreInitializationAttribute : Attribute
    {
        
    }
}
