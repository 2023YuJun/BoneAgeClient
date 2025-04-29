using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public static class ZipHelper
    {
        public static string CreateZip(string sourceDir)
        {
            var destPath = Path.Combine(
                Path.GetTempPath(),
                $"logs_{DateTime.Now:yyyyMMddHHmmss}.zip");

            if (File.Exists(destPath)) File.Delete(destPath);
            ZipFile.CreateFromDirectory(sourceDir, destPath);
            return destPath;
        }
    }
}
