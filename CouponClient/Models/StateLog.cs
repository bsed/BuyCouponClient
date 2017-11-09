using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponClient.Models
{
    public class StateLog
    {
        public DateTime Create { get; set; }

        public Enums.StateLogType State { get; set; }

        public string Remark { get; set; }
    }
}
