using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FloatingWindowApp
{
    public static class ConfigHelper
    {
        private static readonly object lockObject = new object();
        private static string ConfigPath;

        static ConfigHelper()
        {
            string projectName = Assembly.GetExecutingAssembly().GetName().Name;
            string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            ConfigPath = Path.Combine(solutionPath, projectName, "App.config");
        }

        public static void GetSetting(string key, out string value)
        {
            value = "";
            try
            {
                lock (lockObject)
                {
                    if (!File.Exists(ConfigPath))
                    {
                        CreateDefaultConfig();
                    }

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigPath);

                    XmlNode appSettingsNode = xmlDoc.SelectSingleNode("/configuration/appSettings");
                    XmlNode node = appSettingsNode.SelectSingleNode($"add[@key='{key}']");

                    if (node != null)
                    {
                        value = node.Attributes["value"]?.Value ?? "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"加载配置文件时发生错误: {ex.Message}");
            }
        }

        public static void SetSetting(string key, string value)
        {
            try
            {
                lock (lockObject)
                {
                    if (!File.Exists(ConfigPath))
                    {
                        CreateDefaultConfig();
                    }

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(ConfigPath);

                    XmlNode appSettingsNode = xmlDoc.SelectSingleNode("/configuration/appSettings");
                    XmlNode node = appSettingsNode.SelectSingleNode($"add[@key='{key}']");

                    if (node != null)
                    {
                        node.Attributes["value"].Value = value;
                    }
                    else
                    {
                        // 如果节点不存在，创建新节点
                        XmlElement newSetting = xmlDoc.CreateElement("add");
                        newSetting.SetAttribute("key", key);
                        newSetting.SetAttribute("value", value);
                        appSettingsNode.AppendChild(newSetting);
                    }

                    xmlDoc.Save(ConfigPath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"保存配置文件时发生错误: {ex.Message}");
            }
        }

        private static void CreateDefaultConfig()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(declaration);

            XmlNode configurationNode = xmlDoc.CreateElement("configuration");
            xmlDoc.AppendChild(configurationNode);

            XmlNode appSettingsNode = xmlDoc.CreateElement("appSettings");
            configurationNode.AppendChild(appSettingsNode);

            XmlElement setting = xmlDoc.CreateElement("add");
            setting.SetAttribute("key", "FormLocationX");
            setting.SetAttribute("value", "800");
            appSettingsNode.AppendChild(setting);

            setting = xmlDoc.CreateElement("add");
            setting.SetAttribute("key", "FormLocationY");
            setting.SetAttribute("value", "50");
            appSettingsNode.AppendChild(setting);

            setting = xmlDoc.CreateElement("add");
            setting.SetAttribute("key", "AutoOpen");
            setting.SetAttribute("value", "false");
            appSettingsNode.AppendChild(setting);

            setting = xmlDoc.CreateElement("add");
            setting.SetAttribute("key", "ResultResident");
            setting.SetAttribute("value", "false");
            appSettingsNode.AppendChild(setting);

            xmlDoc.Save(ConfigPath);
        }
    }
}
