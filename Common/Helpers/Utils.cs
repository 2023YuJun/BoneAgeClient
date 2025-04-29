using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class Utils
    {
        public static string GenerateRandomFileName(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }

        public static string GetNonLocalhostIPsAsString()
        {
            List<string> ipAddresses = new List<string>();

            // 获取所有网络接口
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                // 获取网络接口的 IP 属性
                IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

                // 获取 IPv4 地址
                foreach (UnicastIPAddressInformation ip in ipProperties.UnicastAddresses)
                {
                    // 检查是否为 IPv4 地址且不是回环地址
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                        ip.Address.ToString() != "127.0.0.1")
                    {
                        ipAddresses.Add(ip.Address.ToString());
                    }
                }
            }

            // 将 IP 地址列表转换为以逗号分隔的字符串
            return string.Join(",", ipAddresses);
        }

    }
}
