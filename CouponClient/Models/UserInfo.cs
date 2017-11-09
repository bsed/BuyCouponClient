using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponClient.Models
{
    public class ThridPlatformUserInfo
    {

        public string NickName { get; set; }

        public string AccessToken { get; set; }


        public Enums.Platform Platform { get; set; }

    }

    public class BuyUserInfo
    {
        public string ID { get; set; }

        public string NickName { get; set; }

        public string UserName { get; set; }

        public string Code { get; set; }


        public string PhoneNumber { get; set; }
    }

    public class ProxyCouponCount
    {
        public string UserID { get; set; }

        public string PhoneNumber { get; set; }

        public int Count { get; set; }
    }

}
