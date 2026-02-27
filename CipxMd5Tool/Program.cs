using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace CipxMd5Tool
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 检查是否有文件拖入
            if (args.Length == 0)
            {
                return; // 没有文件，直接退出
            }

            string filePath = args[0];

            // 检查文件是否存在且后缀为.cipx
            if (!File.Exists(filePath) ||
                !Path.GetExtension(filePath).Equals(".cipx", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                // 计算MD5
                string md5Hash = CalculateMD5(filePath);

                // 获取文件名
                string fileName = Path.GetFileName(filePath);

                // 构建输出格式
                string output = $"<!-- CLASSISLAND_PKG_MD5 {{\"{fileName}\":\"{md5Hash}\"}} -->";

                // 复制到剪贴板
                Clipboard.SetText(output);
            }
            catch
            {
                // 出错静默退出
            }
        }

        static string CalculateMD5(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hash = md5.ComputeHash(stream);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2")); // X2 表示大写十六进制
                }
                return sb.ToString();
            }
        }
    }
}