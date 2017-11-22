using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponClient.Bll
{
    public static class Buy
    {
        public static Models.BuyUserInfo Login(string userName, string password)
        {
            var api = new Api.BuyApi("LoginClient", "Account", new
            {
                UserName = userName,
                Password = password
            });
            var result = api.CreateRequestReturnBuyResult<JObject>();
            if (result.State != "Success")
            {
                throw new Exception(result.Message);
            }
            var data = result.Result;
            return new Models.BuyUserInfo
            {
                NickName = data["NickName"].Value<string>(),
                Code = data["Code"].Value<string>(),
                ID = data["ID"].Value<string>(),
                UserName = userName,
                PhoneNumber = data["PhoneNumber"].Value<string>()
            };
        }

        public static IEnumerable<Models.ProxyCouponCount> GetProxyCouponCount(string userID, Enums.Platform platform, DateTime? date = null)
        {
            string couponPlatform;
            switch (platform)
            {
                default:
                case Enums.Platform.TaoBao:
                    couponPlatform = "0,1";
                    break;
                case Enums.Platform.JD:
                    couponPlatform = "2";
                    break;
                case Enums.Platform.MGJ:
                    couponPlatform = "4";
                    break;
            }
            var api = new Api.BuyApi("GetCouponCount", "Proxy", new
            {
                UserID = userID,
                Platform = couponPlatform,
                Date = date
            }, "GET");
            var result = api.CreateRequestReturnBuyResult<IEnumerable<Models.ProxyCouponCount>>().Result;
            return result;
        }

        public static IEnumerable<Models.CouponType> GetJdCouponType()
        {
            var api = new Api.BuyApi("GetCid", "Jd", type: "GET");
            var result = api.CreateRequestReturnBuyResult<IEnumerable<Models.CouponType>>().Result;
            return result;
        }

        /// <summary>
        /// 在超时时候通过这个来检测是否完成导入
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="p"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool GetCouponUserTempsCount(string userID, Enums.Platform p, DateTime date)
        {
            string strP = null;
            switch (p)
            {
                case Enums.Platform.TaoBao:
                    strP = "0,1";
                    break;
                case Enums.Platform.JD:
                    strP = "2";
                    break;
                case Enums.Platform.MGJ:
                    strP = "4";
                    break;
                default:
                    break;
            }
            var api = new Api.BuyApi("GetCouponUserTempsCount", "Coupon", new
            {
                UserID = userID,
                Platforms = strP,
                Date = date
            }, "Get");
            return api.CreateRequestReturnBuyResult<int>().Result == 0;
        }

        public static void LoopCheckCouponUserTemps(string userID, Enums.Platform p)
        {
            var date = DateTime.Now;
            bool check = false;
            int time = 0;
            do
            {
                check = Bll.Buy.GetCouponUserTempsCount(userID, p, date);
                time++;
                System.Threading.Thread.Sleep(1000 * 60);
                if (time > 60)
                {
                    throw new Exception("数据未处理完");
                }
            } while (!check);

        }
    }


}
