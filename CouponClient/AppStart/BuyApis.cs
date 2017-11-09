using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponClient.Bll
{
    public static class BuyApis
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
    }


}
