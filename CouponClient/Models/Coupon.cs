using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponClient.Models
{
    public class Coupon
    {
        public int ID { get; set; }


        public virtual int? TypeID { get; set; }

        public virtual string ProductType { get; set; }


        public virtual string ProductID { get; set; }


        public Enums.CouponPlatform Platform { get; set; }


        public virtual string ShopName { get; set; }


        public virtual string Name { get; set; }


        public virtual string Image { get; set; }




        public virtual string Subtitle { get; set; }

        
        public virtual decimal Price { get; set; }


        public virtual decimal OriginalPrice { get; set; }

        public virtual string Value { get; set; }



        public virtual DateTime StartDateTime { get; set; }


        public virtual DateTime EndDateTime { get; set; }


        public virtual string DataJson { get; set; }

        public virtual DateTime CreateDateTime { get; set; }


        public virtual int Sales { get; set; }


        public string UrlLisr { get; set; }


        public decimal Commission { get; set; }


        public decimal CommissionRate { get; set; }


        public int Left { get; set; }


        public int Total { get; set; }


        public string UserID { get; set; }


        public string Link { get; set; }

        
        public string PCouponID { get; set; }

        public string Cid { get; set; }

        public string PLink { get; set; }
    }
}
