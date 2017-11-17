using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponClient.Models
{
    public class JdCidUrl
    {
        public JdCidUrl() { }

        public JdCidUrl(string cid, string url)
        {
            Cid = cid;
            Url = url;
        }

        public string Cid { get; set; }


        public string Url { get; set; }

        public int? ID { get; set; }

    }

    public class JdUserCidUrl
    {
        public string UserID { get; set; }

        public string PhoneNumber { get; set; }

        public JdCidUrl[] CidUrls { get; set; }
    }

    public class JdCidLog
    {
        public string Cid { get; set; }

        public string UserID { get; set; }

        public DateTime CreateDateTime { get; set; }
    }

    public class JdPage
    {
        public int Number { get; set; }


        public int Total { get; set; }


        public bool IsLast { get { return Number == Total; } }
    }
}
