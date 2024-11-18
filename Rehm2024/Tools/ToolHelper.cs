using Newtonsoft.Json;
using Rehm2024.Common.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Rehm2024.Tools
{
    public static class ToolHelper
    {

        public static AppSettings LoadSettings(string FileName= "appsettings.json")
        {
            try
            {

                string filePath = Path.Combine(Environment.CurrentDirectory, FileName);

                if (!File.Exists(filePath))
                {
                    // 如果文件不存在，则创建
                    var appSettings = new AppSettings
                    {
                        Url = "192.168.45.91:30050",
                        WorkshopNo = "1厂",
                        LineNo="SMT502",
                        EquipmentName ="Reflow",
                        EquipmentNo= "VXS734N",
                        UserNo="admin"
                    };

                    var json = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
                    File.WriteAllText("appsettings.json", json);
                    Console.WriteLine($"Configuration file '{filePath}' has been created.");

                }
                else
                {
                    Console.WriteLine($"Configuration file '{filePath}' already exists.");
                }


                using (StreamReader r = new StreamReader(FileName))
                {
                    string json = r.ReadToEnd();
                    var _appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
                    return _appSettings;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}");
                return null;
            }
        }

       
        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="appSettings"></param>
       public static void SaveConfiguration(AppSettings appSettings, string FileName = "appsettings.json")
        {
            var json = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            File.WriteAllText(FileName, json);
        }


        public static string ObjectToXml<T>(T obj)
        {
            using (StringWriter textWriter = new StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(textWriter, obj, ns);
                return textWriter.ToString();
            }
        }


        /// <summary>
        /// 字符串转16进制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StringToHexString(string input)
        {
            if (input == null) return null;
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
        /// <summary>
        /// 将十六进制字符串转换为字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string hexString)
        {
            if(hexString == null) { return null; }
            int length = hexString.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }



        //assemblyDirPath + CONFIG_FILE
        public static void WriteLog(string strLog)
        {
            try
            {
                string sFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Log\" + DateTime.Now.ToString("yyyyMM"); //文件夹名
                string sFileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                sFileName = sFilePath + "\\" + sFileName;
                if (!Directory.Exists(sFilePath))
                {
                    Directory.CreateDirectory(sFilePath);

                }
                FileStream fs;
                StreamWriter sw;
                if (File.Exists(sFileName))
                {
                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                }
                else
                {
                    fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
                }
                sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   ---   " + strLog + "\r\n");
                sw.Close();
                fs.Close();
            }
            catch
            {

            }

        }
    }

    
}
  




      
           
    