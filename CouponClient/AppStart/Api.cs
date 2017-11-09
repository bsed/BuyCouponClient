using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace CouponClient.Api
{
    public class BaseApi
    {
        public BaseApi() { }

        public BaseApi(string url, string type)
        {
            Url = url;
            Type = type.ToUpper();
        }


        public BaseApi(string url, string type, Object data)
        {
            Url = url;
            Type = type.ToUpper();
            Data = data;
        }



        public string Url { get; set; }

        public string Type { get; set; }

        public object Data { get; set; }

        public Dictionary<string, string> Files { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 创建请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="type">类别</param>
        /// <param name="p">参数</param>
        /// <returns></returns>
        public virtual Stream CreateRequest()
        {

            using (var client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 30, 0);
                HttpContent httpContent;
                HttpResponseMessage result;
                if (Type.ToLower() == "post")
                {

                    using (var formData = new MultipartFormDataContent())
                    {
                        if (Files.Count > 0)
                        {
                            foreach (var item in Files)
                            {
                                var stream = File.OpenRead(item.Value);
                                HttpContent content = new StreamContent(stream);

                                content.Headers.Add("Content-Type", "application/octet-stream");
                                content.Headers.Add("Content-Disposition", $"form-data; name=\"{item.Key}\"; filename=\"{DateTime.Now.ToFileTime()}{new FileInfo(item.Value).Extension}\"");
                                formData.Add(content, item.Key);
                            }

                            if (Data != null)
                            {
                                var items = (JObject)JToken.FromObject(Data);
                                foreach (var item in items)
                                {
                                    formData.Add(new StringContent(item.Value.ToString()), item.Key);
                                }
                            }
                            httpContent = formData;
                        }
                        else if (Data != null)
                        {
                            httpContent = new StringContent(JsonConvert.SerializeObject(Data), System.Text.Encoding.UTF8, "application/json");
                        }
                        else
                        {
                            httpContent = new MultipartFormDataContent();
                        }
                        result = client.PostAsync(Url, httpContent).Result;
                    }

                }
                else
                {
                    result = client.GetAsync(Url).Result;

                }
                return result.Content.ReadAsStreamAsync().Result;
            }
        }

        /// <summary>
        /// 创建请求返回Jobject
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="type">类别</param>
        /// <param name="p">参数</param>
        /// <returns></returns>
        public virtual JObject CreateRequestReturnJson()
        {
            var steam = CreateRequest();
            string txtData = "";
            if (steam == null)
            {
                return null;

            }
            using (var reader = new StreamReader(steam))
            {
                txtData = reader.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<JObject>(txtData);
        }


        public virtual string CreateRequestReturnString()
        {
            var steam = CreateRequest();
            string txtData = "";
            using (var reader = new StreamReader(steam))
            {
                txtData = reader.ReadToEnd();
            }
            return txtData;
        }
    }

    public class BuyApiResult<T>
    {
        public string State { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }
    }

    public class BuyApi : BaseApi
    {
        public BuyApi(string action, string control, object p = null, string type = "POST")
        {
            if (type.ToLower() == "get")
            {
                var pProp = p?.GetType().GetProperties();

                if (pProp?.Count() > 0)
                {
                    var pNames = pProp.Select(s => $"{s.Name}={s.GetValue(p)}");
                    var strP = string.Join("&", pNames);

                    Url = $"{Config.ServerAddress}{control}/{action}?{strP}";
                }
                else
                {
                    Url = $"{Config.ServerAddress}{control}/{action}";
                }
            }
            else
            {
                Url = $"{Config.ServerAddress}{control}/{action}";
            }
            Data = p;
            Type = type;
        }


        public virtual BuyApiResult<T> CreateRequestReturnBuyResult<T>()
        {
            var result = CreateRequestReturnJson();
            return new BuyApiResult<T>
            {
                Message = result["Message"].Value<string>(),
                Result = result["Result"].Value<JToken>().ToObject<T>(),
                State = result["State"].Value<string>()
            };
        }
    }

    public class BuyUploadApi : BaseApi
    {
        public BuyUploadApi(Dictionary<string, string> files)
        {
            Url = $"{Config.ServerAddress}Uploader/Upload";
            Files = files;
            Type = "POST";
        }

        public string[] CreateRequestReturnUrls()
        {
            var result = CreateRequestReturnJson();
            return result["Result"]["FileUrls"].Values<string>().ToArray();
        }
    }


}