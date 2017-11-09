using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponClient
{
    public static class SystemBase
    {
        public static void WriteLog(string content, string type = null)
        {
            string dir = $@"{Config.RuningPath}\log\{type ?? "default"}";
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            System.IO.File.AppendAllText($@"{dir}\{DateTime.Now.Date:yyyyMMdd}.log", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {content}\r\n");

        }

    }
}
