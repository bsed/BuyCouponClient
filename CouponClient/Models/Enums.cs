using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponClient.Enums
{
    public enum Platform
    {
        TaoBao,
        JD,
        MGJ
    }

    public enum StateLogType
    {
        TaoBaoSignSuccess = 0,
        TaoBaoCouponDownloadStart = 10,
        TaoBaoCouponDownloadComplated = 11,
        TaoBaoCouponDownloadFail = 12,
        TaoBaoCouponDownloaded = 13,
        TaoBaoCouponUploadStart = 20,
        TaoBaoCouponUploadComplated = 21,
        TaoBaoCouponUploadFail = 22,
        TaoBaoCouponUploaded = 23,
        TaoBaoCouponDeleteTemp = 24,
        TaoBaoCouponAddDbStart = 30,
        TaoBaoCouponAddDbComplated = 31,
        TaoBaoCouponAddDbFail = 32,
        TaoBaoCouponAddedDb = 33,
        MoGuJieSignSuccess = 140,
        MoGuJieGetCouponStart = 150,
        MoGuJieGetCouponComplated = 151,
        MoGuJieGetCouponError = 152,
        MoGuJieCouponAddDbStart = 160,
        MoGuJieCouponAddDbComplated = 161,
        MoGuJieCouponAddDbError = 162,
        MoGuJieCouponDeleteTemp = 170,
        JdSignSuccess = 200,
        JdCouponDownloadStart = 210,
        JdCouponDownloadComplated = 211,
        JdCouponDownloadFail = 212,
        JdCouponDownloaded = 213,
        JdCouponUploadStart = 220,
        JdCouponUploadComplated = 221,
        JdCouponUploadFail = 222,
        JdCouponUploaded = 223,
        JdCouponDeleteTemp = 224,
        JdCouponAddDbStart = 230,
        JdCouponAddDbComplated = 231,
        JdCouponAddDbFail = 232,
        JdCouponAddedDb = 233,
        JdCouponColumnError = 240
    }
}
