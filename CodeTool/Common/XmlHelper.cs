using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeTool.Common
{
    public class XmlHelper
    {
        /// <summary>
        /// 保存数据到文件
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="fileName">文件完整名称</param>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static bool Save2File<T>(string fileName,T obj)
        {
            try
            {
                using (Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(stream, obj);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 从文件加载数据到Object
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="fileName">文件完整名称</param>
        /// <returns></returns>
        public static T Load2Object<T>(string fileName)
        {
            try
            {
                using (Stream stream=new FileStream(fileName,FileMode.Open,FileAccess.Read,FileShare.None))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stream);
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
