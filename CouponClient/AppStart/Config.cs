using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace CouponClient
{
    public static class Config
    {
        private static string _stateLogPath = $"{RuningPath}/StateLog.json";

        private static string _configPath = $"{RuningPath}/ConfigPath.json";

        static Config()
        {
           
            _configSetting = new Models.Config();
            if (!File.Exists(_configPath))
            {
                //File.WriteAllText(_configPath, JsonConvert.SerializeObject(_configSetting));
            }
            else
            {
                _configSetting = JsonConvert.DeserializeObject<Models.Config>(File.ReadAllText(_configPath));
            }
            
        }

        private static void _stateLogs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //File.WriteAllText(_stateLogPath, JsonConvert.SerializeObject(StateLogs));
        }

        public static string RuningPath
        {
            get
            {
                return System.Environment.CurrentDirectory;
            }
        }

        public static string ServerAddress
        {
            get
            {
                string testServer = "http://39.108.225.40:21601/";
                string local = "http://localhost:62209/";
                string server = "http://www.immlm.cn/";
                return server;
            }
        }

     
        

        private static Models.Config _configSetting;

        public static Models.Config ConfigSetting
        {
            get
            {
                return _configSetting;
            }
            set
            {
                _configSetting = value;
                File.WriteAllText(_configPath, JsonConvert.SerializeObject(_configSetting));
            }
        }
    }


}
