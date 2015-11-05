using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DogSE.Common;
using DogSE.Library.Log;
using DogSE.Server.Database;

namespace TradeAge.Server.Database.XmlFile
{
    /// <summary>
    /// Xml文件系统
    /// </summary>
    public class XmlFileService : IDataService
    {
        private static readonly string Rundata;

        static XmlFileService()
        {
            Rundata = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"RunData");
        }


        private string GetFolderName<T>()
        {
            var type = typeof (T);

            string typeName;
            if (type.IsGenericType)
                typeName = type.Name.Substring(0, type.Name.IndexOf('`'));
            else
                typeName = type.Name;

            return Path.Combine(Rundata, typeName);
        }


        /// <summary>
        /// 反序列化XML字符串为指定类型
        /// </summary>
        private static T Deserialize<T>(string xmlFile) where T : class, new()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
            T result;
            var xmlData = File.ReadAllText(xmlFile);
            using (StringReader stringReader = new StringReader(xmlData))
            {
                result = xmlSerializer.Deserialize(stringReader) as T;
            }

            return result;
        }

        /// <summary>
        /// 序列化object对象为XML字符串
        /// </summary>
        private static string Serialize<T>(T obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, new UTF8Encoding(false));
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlSerializer.Serialize(xmlTextWriter, obj);
                xmlTextWriter.Flush();
                xmlTextWriter.Close();

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        private static void WriterXmlFile<T>(string fileName, T obj)
        {
            var xml = Serialize<T>(obj);
            File.WriteAllText(fileName, xml);
        }


        /// <summary>
        /// 通过实体id，加载某个具体的实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serial"></param>
        /// <returns></returns>
        public T LoadEntity<T>(int serial) where T : class, IDataEntity, new()
        {
            var folder = GetFolderName<T>();
            var fileName = Path.Combine(folder, serial + ".xml");
            if (!File.Exists(fileName))
            {
                Logs.Error("LoadEntity<{0}> not find id:{1}", typeof (T).Name, serial.ToString());
                return default(T);
            }


            try
            {
                return Deserialize<T>(fileName);
            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("LoadEntity<{0}> fail", typeof (T).Name), ex);
                throw;
            }
        }


        /// <summary>
        /// 加载某个类型的所有实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T[] LoadEntitys<T>() where T : class, IDataEntity, new()
        {
            var folder = GetFolderName<T>();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                return new T[0];
            }

            List<T> rets = new List<T>();
            try
            {
                foreach (var fileName in Directory.GetFiles(folder, "*.xml"))
                {
                    rets.Add(Deserialize<T>(fileName));
                }


                return rets.ToArray();
            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("LoadEntitys<{0}> fail", typeof (T).Name), ex);
                throw;
            }
        }

        /// <summary>
        /// 更新（新增）某个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns>
        /// </returns>
        public int UpdateEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            var folder = GetFolderName<T>();
            var fileName = Path.Combine(folder, entity.Id + ".xml");
            if (!File.Exists(fileName))
            {
                Logs.Error("UpdateEntity<{0}> not find id:{1}", typeof(T).Name, entity.Id.ToString());
                return 0;
            }

            try
            {
                WriterXmlFile<T>(fileName, entity);

                return 1;
            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("UpdateEntity<{0}> fail", typeof(T).Name), ex);
                throw;
            }
        }

        /// <summary>
        /// 更新（新增）某个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns>
        /// </returns>
        public int InsertEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            var folder = GetFolderName<T>();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var fileName = Path.Combine(folder, entity.Id + ".xml");
            if (File.Exists(fileName))
            {
                Logs.Error("InsertEntity<{0}> id is exists:{1}", typeof(T).Name, entity.Id.ToString());
                return 0;
            }

            try
            {
                WriterXmlFile<T>(fileName, entity);

                return 1;
            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("UpdateEntity<{0}> fail", typeof(T).Name), ex);
                throw;
            }
        }

        /// <summary>
        /// 删除某个实体数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int DeleteEntity<T>(T entity) where T : class, IDataEntity, new()
        {
            var folder = GetFolderName<T>();
            var fileName = Path.Combine(folder, entity.Id + ".xml");
            if (!File.Exists(fileName))
            {
                Logs.Error("DeleteEntity<{0}> not find id:{1}", typeof(T).Name, entity.Id.ToString());
                return 0;
            }


            try
            {
                File.Delete(fileName);
                return 1;
            }
            catch (Exception ex)
            {
                Logs.Error(string.Format("DeleteEntity<{0}> fail", typeof(T).Name), ex);
                throw;
            }

        }

        /// <summary>
        /// 执行一组sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 执行并返回一个结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql)
        {
            throw new NotImplementedException();
        }
    }
}
