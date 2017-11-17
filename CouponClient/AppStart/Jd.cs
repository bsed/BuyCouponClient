using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CsQuery;
using System.Net;

namespace CouponClient.Bll
{
    public static class Jd
    {
        private static string jdCidLogPath = Config.RuningPath + "/jdLog.json";
        private static string jdTempCoupon = Config.RuningPath + "/jdTempCoupon.json";

        private static FileInfo filLog;
        static Jd()
        {
            setJdCidUrl();
            filLog = new FileInfo(jdCidLogPath);
            if (filLog.Exists)
            {
                _logs = JsonConvert.DeserializeObject<ObservableCollection<Models.JdCidLog>>(File.ReadAllText(jdCidLogPath));
            }
            else
            {
                _logs = new ObservableCollection<Models.JdCidLog>();
            }
            _logs.CollectionChanged += _logs_CollectionChanged;
        }

        private static void setJdCidUrl()
        {
            _allCids = new Models.JdCidUrl[] {
                new Models.JdCidUrl("男装+夹克","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9730"),
                new Models.JdCidUrl("男装+休闲裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9736"),
                new Models.JdCidUrl("男装+T恤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=1349"),
                new Models.JdCidUrl("男装+衬衫","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=1348"),
                new Models.JdCidUrl("男装+牛仔裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9735"),
                new Models.JdCidUrl("男装+卫衣","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9732"),
                new Models.JdCidUrl("男装+针织衫","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=1350"),
                new Models.JdCidUrl("男装+羽绒服","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=3982"),
                new Models.JdCidUrl("男装+棉服","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9724"),
                new Models.JdCidUrl("男装+毛呢大衣","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9729"),
                new Models.JdCidUrl("男装+西服套装","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9738"),
                new Models.JdCidUrl("男装+马甲/背心","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9734"),
                new Models.JdCidUrl("男装+风衣","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9728"),
                new Models.JdCidUrl("男装+西裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9737"),
                new Models.JdCidUrl("男装+羊毛衫","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=12089"),
                new Models.JdCidUrl("男装+仿皮皮衣","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9725"),
                new Models.JdCidUrl("男装+中老年男装","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9740"),
                new Models.JdCidUrl("男装+西服","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9731"),
                new Models.JdCidUrl("男装+羊绒衫","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9726"),
                new Models.JdCidUrl("男装+POLO衫","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9733"),
                new Models.JdCidUrl("男装+卫裤/运动裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=12003"),
                new Models.JdCidUrl("男装+短裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=12004"),
                new Models.JdCidUrl("男装+唐装/中山装","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1342&cat3=9741"),
                new Models.JdCidUrl("女装+连衣裙","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9719"),
                new Models.JdCidUrl("女装+针织衫","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=1356"),
                new Models.JdCidUrl("女装+毛呢大衣","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9706"),
                new Models.JdCidUrl("女装+T恤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=1355"),
                new Models.JdCidUrl("女装+卫衣","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9710"),
                new Models.JdCidUrl("女装+半身裙","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9720"),
                new Models.JdCidUrl("女装+休闲裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9717"),
                new Models.JdCidUrl("女装+大码女装","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9722"),
                new Models.JdCidUrl("女装+打底裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9716"),
                new Models.JdCidUrl("女装+羽绒服","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=3983"),
                new Models.JdCidUrl("女装+打底衫","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=11985"),
                new Models.JdCidUrl("女装+短外套","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9712"),
                new Models.JdCidUrl("女装+衬衫","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=1354"),
                new Models.JdCidUrl("女装+风衣","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9708"),
                new Models.JdCidUrl("女装+牛仔裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9715"),
                new Models.JdCidUrl("女装+短裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=11991"),
                new Models.JdCidUrl("女装+棉服","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9705"),
                new Models.JdCidUrl("女装+毛衣","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=11999"),
                new Models.JdCidUrl("女装+礼服","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=11996"),
                new Models.JdCidUrl("女装+中老年女装","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1343&cat3=9721"),
                new Models.JdCidUrl("内衣+内衣睡衣/家居服","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=1371"),
                new Models.JdCidUrl("内衣+内衣文胸","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=1364"),
                new Models.JdCidUrl("内衣+男式内裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=9744"),
                new Models.JdCidUrl("内衣+休闲棉袜 ","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=12010"),
                new Models.JdCidUrl("内衣+女式内裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=9743"),
                new Models.JdCidUrl("内衣+保暖内衣","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=1369"),
                new Models.JdCidUrl("内衣+秋衣秋裤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=12015"),
                new Models.JdCidUrl("内衣+塑身美体","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=9747"),
                new Models.JdCidUrl("内衣+商务男袜","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=9745"),
                new Models.JdCidUrl("内衣+连裤袜/丝袜","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=9748"),
                new Models.JdCidUrl("内衣+美腿袜","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=9749"),
                new Models.JdCidUrl("内衣+打底裤袜","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=12013"),
                new Models.JdCidUrl("内衣+吊带/背心","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1345&cat3=1365"),
                new Models.JdCidUrl("服饰配件+太阳镜","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=9790"),
                new Models.JdCidUrl("服饰配件+男士腰带/礼盒","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=12029"),
                new Models.JdCidUrl("服饰配件+女士丝巾/围巾/披肩","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=12021"),
                new Models.JdCidUrl("服饰配件+棒球帽","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=9792"),
                new Models.JdCidUrl("服饰配件+毛线手套","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=12027"),
                new Models.JdCidUrl("服饰配件+毛线帽","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=9793"),
                new Models.JdCidUrl("服饰配件+光学镜架/镜片","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=9789"),
                new Models.JdCidUrl("服饰配件+真皮手套","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=12026"),
                new Models.JdCidUrl("服饰配件+领带/领结/领带夹","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=12039"),
                new Models.JdCidUrl("服饰配件+男士丝巾/围巾","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=12022"),
                new Models.JdCidUrl("服饰配件+遮阳帽","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=9794"),
                new Models.JdCidUrl("服饰配件+礼帽","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=12025"),
                new Models.JdCidUrl("服饰配件+袖扣","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1315&cat2=1346&cat3=1378"),
                new Models.JdCidUrl("家装建材+厨房卫浴","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9855&cat2=9857&cat3="),
                new Models.JdCidUrl("家装建材+五金工具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9855&cat2=9858&cat3="),
                new Models.JdCidUrl("家装建材+灯饰照明","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9855&cat2=9856&cat3="),
                new Models.JdCidUrl("家装建材+电工电料","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9855&cat2=9859&cat3="),
                new Models.JdCidUrl("家装建材+墙地面材料","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9855&cat2=9860&cat3="),
                new Models.JdCidUrl("家装建材+装饰材料","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9855&cat2=9861&cat3="),
                new Models.JdCidUrl("汽车用品+汽车装饰","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6728&cat2=6745&cat3="),
                new Models.JdCidUrl("汽车用品+维修保养","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6728&cat2=6742&cat3="),
                new Models.JdCidUrl("汽车用品+车载电器","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6728&cat2=6740&cat3="),
                new Models.JdCidUrl("汽车用品+美容清洗","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6728&cat2=6743&cat3="),
                new Models.JdCidUrl("汽车用品+安全自驾","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6728&cat2=6747&cat3="),
                new Models.JdCidUrl("运动户外+健身训练","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1318&cat2=1463&cat3="),
                new Models.JdCidUrl("运动户外+户外装备","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1318&cat2=1462&cat3="),
                new Models.JdCidUrl("运动户外+运动鞋包","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1318&cat2=12099&cat3="),
                new Models.JdCidUrl("运动户外+体育用品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1318&cat2=1466&cat3="),
                new Models.JdCidUrl("运动户外+户外鞋服","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1318&cat2=2628&cat3="),
                new Models.JdCidUrl("运动户外+垂钓用品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1318&cat2=12147&cat3="),
                new Models.JdCidUrl("运动户外+运动服饰","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1318&cat2=12102&cat3="),
                new Models.JdCidUrl("运动户外+骑行运动","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1318&cat2=12115&cat3="),
                new Models.JdCidUrl("运动户外+游泳用品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1318&cat2=12154&cat3="),
                new Models.JdCidUrl("家居家纺+家纺","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1620&cat2=1621&cat3="),
                new Models.JdCidUrl("家居家纺+家装软饰","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1620&cat2=11158&cat3="),
                new Models.JdCidUrl("家居家纺+生活日用","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1620&cat2=1624&cat3="),
                new Models.JdCidUrl("家居家纺+收纳用品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1620&cat2=13780&cat3="),
                new Models.JdCidUrl("礼品箱包+礼品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1672&cat2=2599&cat3="),
                new Models.JdCidUrl("礼品箱包+精品男包","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1672&cat2=2576&cat3="),
                new Models.JdCidUrl("礼品箱包+潮流女包","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1672&cat2=2575&cat3="),
                new Models.JdCidUrl("礼品箱包+功能箱包","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1672&cat2=2577&cat3="),
                new Models.JdCidUrl("礼品箱包+火机烟具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1672&cat2=14226&cat3="),
                new Models.JdCidUrl("美妆个护+面部护肤","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1316&cat2=1381&cat3="),
                new Models.JdCidUrl("美妆个护+清洁用品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1316&cat2=1625&cat3="),
                new Models.JdCidUrl("美妆个护+香水彩妆","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1316&cat2=1387&cat3="),
                new Models.JdCidUrl("美妆个护+洗发护发","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1316&cat2=1386&cat3="),
                new Models.JdCidUrl("美妆个护+身体护理","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1316&cat2=1383&cat3="),
                new Models.JdCidUrl("美妆个护+女性护理","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1316&cat2=1385&cat3="),
                new Models.JdCidUrl("美妆个护+口腔护理","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1316&cat2=1384&cat3="),
                new Models.JdCidUrl("厨具+水具酒具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6196&cat2=6219&cat3="),
                new Models.JdCidUrl("厨具+厨房配件","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6196&cat2=6214&cat3="),
                new Models.JdCidUrl("厨具+茶具/咖啡具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6196&cat2=11143&cat3="),
                new Models.JdCidUrl("厨具+餐具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6196&cat2=6227&cat3="),
                new Models.JdCidUrl("厨具+烹饪锅具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6196&cat2=6197&cat3="),
                new Models.JdCidUrl("厨具+刀剪菜板","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6196&cat2=6198&cat3="),
                new Models.JdCidUrl("食品饮料+休闲食品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1320&cat2=1583&cat3="),
                new Models.JdCidUrl("食品饮料+茗茶","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1320&cat2=12202&cat3="),
                new Models.JdCidUrl("食品饮料+粮油调味","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1320&cat2=1584&cat3="),
                new Models.JdCidUrl("食品饮料+饮料冲调","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1320&cat2=1585&cat3="),
                new Models.JdCidUrl("食品饮料+进口食品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1320&cat2=5019&cat3="),
                new Models.JdCidUrl("食品饮料+地方特产","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1320&cat2=1581&cat3="),
                new Models.JdCidUrl("食品饮料+食品礼券","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1320&cat2=2641&cat3="),
                new Models.JdCidUrl("家用电器+生活电器","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=737&cat2=738&cat3="),
                new Models.JdCidUrl("家用电器+个护健康","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=737&cat2=1276&cat3="),
                new Models.JdCidUrl("家用电器+大 家 电","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=737&cat2=794&cat3="),
                new Models.JdCidUrl("家用电器+厨房小电","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=737&cat2=752&cat3="),
                new Models.JdCidUrl("家用电器+厨卫大电","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=737&cat2=13297&cat3="),
                new Models.JdCidUrl("母婴+童装","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1319&cat2=11842&cat3="),
                new Models.JdCidUrl("母婴+妈妈专区","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1319&cat2=4997&cat3="),
                new Models.JdCidUrl("母婴+寝居服饰","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1319&cat2=6313&cat3="),
                new Models.JdCidUrl("母婴+童车童床","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1319&cat2=1528&cat3="),
                new Models.JdCidUrl("母婴+喂养用品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1319&cat2=1526&cat3="),
                new Models.JdCidUrl("母婴+洗护用品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1319&cat2=1527&cat3="),
                new Models.JdCidUrl("母婴+童鞋","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1319&cat2=14941&cat3="),
                new Models.JdCidUrl("母婴+尿裤湿巾","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1319&cat2=1525&cat3="),
                new Models.JdCidUrl("母婴+营养辅食","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1319&cat2=1524&cat3="),
                new Models.JdCidUrl("母婴+安全座椅","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=1319&cat2=12193&cat3="),
                new Models.JdCidUrl("手机+手机配件","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9987&cat2=830&cat3="),
                new Models.JdCidUrl("手机+手机通讯","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9987&cat2=653&cat3="),
                new Models.JdCidUrl("流行男鞋+休闲鞋","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11730&cat3=6908"),
                new Models.JdCidUrl("流行男鞋+男靴","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11730&cat3=6912"),
                new Models.JdCidUrl("流行男鞋+商务休闲鞋","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11730&cat3=6907"),
                new Models.JdCidUrl("流行男鞋+正装鞋","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11730&cat3=6906"),
                new Models.JdCidUrl("流行男鞋+功能鞋","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11730&cat3=9781"),
                new Models.JdCidUrl("流行男鞋+鞋配件","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11730&cat3=6913"),
                new Models.JdCidUrl("流行男鞋+帆布鞋","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11730&cat3=9783"),
                new Models.JdCidUrl("流行男鞋+增高鞋","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11730&cat3=12066"),
                new Models.JdCidUrl("时尚女鞋+女靴","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11731&cat3=9776"),
                new Models.JdCidUrl("时尚女鞋+单鞋","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11731&cat3=6914"),
                new Models.JdCidUrl("时尚女鞋+休闲鞋","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11731&cat3=6916"),
                new Models.JdCidUrl("时尚女鞋+高跟鞋","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11731&cat3=9772"),
                new Models.JdCidUrl("时尚女鞋+马丁靴","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11731&cat3=12060"),
                new Models.JdCidUrl("时尚女鞋+凉鞋","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11731&cat3=6917"),
                new Models.JdCidUrl("时尚女鞋+拖鞋/人字拖","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11731&cat3=9775"),
                new Models.JdCidUrl("时尚女鞋+靴子","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=11729&cat2=11731&cat3=6919"),
                new Models.JdCidUrl("电脑、办公+办公设备","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=670&cat2=716&cat3="),
                new Models.JdCidUrl("电脑、办公+外设产品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=670&cat2=686&cat3="),
                new Models.JdCidUrl("电脑、办公+文具/耗材","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=670&cat2=729&cat3="),
                new Models.JdCidUrl("电脑、办公+电脑整机","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=670&cat2=671&cat3="),
                new Models.JdCidUrl("电脑、办公+电脑配件","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=670&cat2=677&cat3="),
                new Models.JdCidUrl("电脑、办公+网络产品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=670&cat2=699&cat3="),
                new Models.JdCidUrl("珠宝首饰+时尚饰品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6144&cat2=6182&cat3="),
                new Models.JdCidUrl("珠宝首饰+银饰","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6144&cat2=6155&cat3="),
                new Models.JdCidUrl("珠宝首饰+翡翠玉石","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6144&cat2=6167&cat3="),
                new Models.JdCidUrl("珠宝首饰+水晶玛瑙","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6144&cat2=6172&cat3="),
                new Models.JdCidUrl("珠宝首饰+木手串/把件","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6144&cat2=12041&cat3="),
                new Models.JdCidUrl("珠宝首饰+发饰","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6144&cat2=13915&cat3="),
                new Models.JdCidUrl("珠宝首饰+钻石","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6144&cat2=6160&cat3="),
                new Models.JdCidUrl("珠宝首饰+铂金","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6144&cat2=12040&cat3="),
                new Models.JdCidUrl("家具+客厅家具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9847&cat2=9849&cat3="),
                new Models.JdCidUrl("家具+卧室家具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9847&cat2=9848&cat3="),
                new Models.JdCidUrl("家具+书房家具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9847&cat2=9851&cat3="),
                new Models.JdCidUrl("家具+餐厅家具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9847&cat2=9850&cat3="),
                new Models.JdCidUrl("家具+储物家具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9847&cat2=9852&cat3="),
                new Models.JdCidUrl("家具+阳台/户外","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9847&cat2=9853&cat3="),
                new Models.JdCidUrl("家具+办公家具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9847&cat2=9854&cat3="),
                new Models.JdCidUrl("家具+儿童家具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9847&cat2=13533&cat3="),
                new Models.JdCidUrl("医药保健+保健器械","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9192&cat2=9197&cat3="),
                new Models.JdCidUrl("医药保健+传统滋补","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9192&cat2=9195&cat3="),
                new Models.JdCidUrl("医药保健+营养健康","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9192&cat2=9193&cat3="),
                new Models.JdCidUrl("医药保健+营养成分","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9192&cat2=9194&cat3="),
                new Models.JdCidUrl("医药保健+护理护具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=9192&cat2=12190&cat3="),
                new Models.JdCidUrl("数码+影音娱乐","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=652&cat2=828&cat3="),
                new Models.JdCidUrl("数码+智能设备","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=652&cat2=12345&cat3="),
                new Models.JdCidUrl("数码+数码配件","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=652&cat2=829&cat3="),
                new Models.JdCidUrl("数码+摄影摄像","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=652&cat2=654&cat3="),
                new Models.JdCidUrl("数码+电子教育","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=652&cat2=12346&cat3="),
                new Models.JdCidUrl("玩具乐器+毛绒布艺","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6233&cat2=6236&cat3="),
                new Models.JdCidUrl("玩具乐器+益智玩具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6233&cat2=6271&cat3="),
                new Models.JdCidUrl("玩具乐器+积木拼插","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6233&cat2=6275&cat3="),
                new Models.JdCidUrl("玩具乐器+乐器","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6233&cat2=6291&cat3="),
                new Models.JdCidUrl("玩具乐器+遥控/电动","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6233&cat2=6235&cat3="),
                new Models.JdCidUrl("玩具乐器+健身玩具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6233&cat2=6260&cat3="),
                new Models.JdCidUrl("玩具乐器+动漫玩具","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6233&cat2=6264&cat3="),
                new Models.JdCidUrl("玩具乐器+绘画/DIY","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6233&cat2=6279&cat3="),
                new Models.JdCidUrl("生鲜+海鲜水产","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=12218&cat2=12222&cat3="),
                new Models.JdCidUrl("生鲜+水果","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=12218&cat2=12221&cat3="),
                new Models.JdCidUrl("生鲜+禽肉蛋品","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=12218&cat2=13586&cat3="),
                new Models.JdCidUrl("生鲜+猪牛羊肉","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=12218&cat2=13581&cat3="),
                new Models.JdCidUrl("生鲜+蔬菜","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=12218&cat2=13553&cat3="),
                new Models.JdCidUrl("钟表+钟表","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=5025&cat2=5026&cat3="),
                new Models.JdCidUrl("中外名酒+葡萄酒","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=12259&cat2=12260&cat3=9438"),
                new Models.JdCidUrl("中外名酒+白酒","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=12259&cat2=12260&cat3=9435"),
                new Models.JdCidUrl("中外名酒+黄酒/养生酒","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=12259&cat2=12260&cat3=9436"),
                new Models.JdCidUrl("中外名酒+洋酒","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=12259&cat2=12260&cat3=9437"),
                new Models.JdCidUrl("中外名酒+啤酒","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=12259&cat2=12260&cat3=9439"),
                new Models.JdCidUrl("宠物生活+医疗保健","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6994&cat2=6997&cat3="),
                new Models.JdCidUrl("宠物生活+宠物主粮","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6994&cat2=6995&cat3="),
                new Models.JdCidUrl("宠物生活+家居日用","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6994&cat2=6998&cat3="),
                new Models.JdCidUrl("宠物生活+出行装备","https://media.jd.com/gotoadv/pickupdate?type=1&pageIndex=&pageSize=50&property=&sort=&goodsView=&cat1=6994&cat2=7000&cat3="),

            };

            var coupTypes = BuyApis.GetJdCouponType();
            foreach (var item in _allCids)
            {
                item.ID = coupTypes
                    .FirstOrDefault(s => s.Keyword.SplitToArray<string>(',')
                    .Any(x => x == item.Cid))?.ID;
            }
        }

        private static void _logs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            File.WriteAllText(jdCidLogPath, JsonConvert.SerializeObject(_logs));
        }

        private static ObservableCollection<Models.JdCidLog> _logs;

        public static ObservableCollection<Models.JdCidLog> Logs { get { return _logs; } }

        public static Models.JdCidUrl[] _allCids;

        public static Models.JdCidUrl[] AllCids
        {
            get
            {

                return _allCids;
            }
        }



        /// <summary>
        /// 读取JD的EXCEL表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="cid"></param>
        /// <param name="html"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<Models.Coupon> GetCouponsFromExcel(string userID, Models.JdCidUrl cid, string html, string path)
        {
            var dtable = new ExcelHelper(path).ExcelToDataTable(null, true);
            var doc = CQ.CreateDocument(html);
            List<Models.Coupon> coupons = new List<Models.Coupon>();
            Func<string, string> decode = t =>
            {
                return WebUtility.HtmlDecode(t.Trim());
            };
            foreach (System.Data.DataRow item in dtable.Rows)
            {
                try
                {
                    var strDate = item["推广时间"].ToString();
                    var dates = strDate.SplitToArray<DateTime>('至');
                    var id = item["商品详情页"].ToString()
                        .Replace("http://item.jd.com/", "")
                        .Replace(".html", "");
                    var hItem = CQ.CreateDocument(doc.Select($"[skuid={id}]")[0].OuterHTML);
                    var img = hItem.Select(".img-wrap img")[0].GetAttribute("src");
                    var oPrice = Convert.ToDecimal(decode(hItem.Select(".pc_unitPrice .pri").FirstOrDefault()?.InnerText).Remove(0, 1));
                    var price = Convert.ToDecimal(decode(hItem.Select(".price-exc .price em").FirstOrDefault()?.InnerText ?? "$0").Remove(0, 1));
                    if (price == 0)
                    {
                        price = oPrice;
                    }
                    var link = item["我要推广"].ToString();
                    var docShopName = hItem.Select(".shop-name");
                    var shopName = docShopName.Count() > 0 ? decode(docShopName[0].InnerText) : "京东自营";
                    var c = new Models.Coupon
                    {
                        TypeID = cid.ID,
                        Cid = cid.Cid,
                        Commission = Convert.ToDecimal(item["无线佣金（元）"]),
                        CommissionRate = Convert.ToDecimal(item["无线佣金比例（%）"]),
                        CreateDateTime = DateTime.Now,
                        EndDateTime = dates[1],
                        Image = img,
                        PCouponID = id,
                        Left = 1000,
                        Link = link,
                        Name = item["商品名称"].ToString(),
                        OriginalPrice = oPrice,
                        Platform = Enums.CouponPlatform.Jd,
                        PLink = link,
                        Price = price,
                        ProductID = id,
                        Sales = Convert.ToInt32(item["30天引入订单"]),
                        ShopName = shopName,
                        Total = 1000,
                        StartDateTime = dates[0],
                        Value = $"-{oPrice - price}",
                        ProductType = cid.Cid,
                        UserID = userID,

                    };
                    coupons.Add(c);
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }
            return coupons;
        }

        public static void Import(List<Models.Coupon> coupon, Action<Enums.StateLogType> stateChange)
        {

            var json = JsonConvert.SerializeObject(coupon);
            var path = $"{Config.RuningPath}\\temp\\Jd_{DateTime.Now:yyyyMMddHHmm}.json";
            File.WriteAllText(path, json);
            var files = new Dictionary<string, string>();
            files.Add("file", path);

            stateChange(Enums.StateLogType.JdCouponUploadStart);
            string[] url;
            try
            {
                url = new Api.BuyUploadApi(files).CreateRequestReturnUrls();
                stateChange(Enums.StateLogType.JdCouponUploadComplated);
            }
            catch (Exception)
            {
                stateChange(Enums.StateLogType.JdCouponUploadFail);
                return;
            }
            try
            {
                stateChange(Enums.StateLogType.JdCouponAddDbStart);
                var import = new Api.BuyApi("Import", "Jd", new { Url = url[0] }).CreateRequestReturnBuyResult<object>();

                if (import.State == "Success")
                {
                    stateChange(Enums.StateLogType.JdCouponAddDbComplated);
                }
                else
                {
                    stateChange(Enums.StateLogType.JdCouponAddDbFail);
                }
                TempCoupons.Clear();
            }
            catch (Exception)
            {
                stateChange(Enums.StateLogType.JdCouponAddDbFail);
            }

        }


        public static class TempCoupons
        {

            private static List<Models.Coupon> _tempCoupon;

            /// <summary>
            /// 缓存中的优惠券，待上传
            /// </summary>
            public static List<Models.Coupon> Data
            {
                get
                {
                    if (_tempCoupon == null)
                    {
                        if (File.Exists(jdTempCoupon))
                        {
                            string strCoupon = File.ReadAllText(jdTempCoupon);
                            _tempCoupon = JsonConvert.DeserializeObject<List<Models.Coupon>>(strCoupon);
                        }
                        else
                        {
                            _tempCoupon = new List<Models.Coupon>();
                        }
                    }
                    return _tempCoupon;
                }

            }


            public static void Update(IEnumerable<Models.Coupon> newCoupons)
            {
                var filter = newCoupons
                    .Where(s => s.Commission > 0)
                    .Where(s => s.OriginalPrice >= s.Price);
                Data.AddRange(filter);
                string strCoupon = JsonConvert.SerializeObject(Data);
                File.WriteAllText(jdTempCoupon, strCoupon);

            }

            public static void Clear()
            {
                _tempCoupon = new List<Models.Coupon>();
                if (File.Exists(jdTempCoupon))
                {
                    File.Delete(jdTempCoupon);
                }

            }
        }
    }


}
