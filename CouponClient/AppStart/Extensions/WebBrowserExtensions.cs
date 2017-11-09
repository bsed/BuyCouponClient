using CefSharp;
using CsQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CouponClient.AppStart.Extensions
{
    public static class WebBrowserExtensions
    {
        public static async Task Wait(this IWebBrowser browser, string select, int timeout, Action action)
        {
            var end = DateTime.Now.AddMilliseconds(timeout);
            Func<string, Task<bool>> selector = async s =>
           {
               var html = await browser.GetSourceAsync();
               var doc = CQ.CreateDocument(html);
               var result = doc.Select(s).Length > 0;
               return result;
           };
            while (!(await selector(select)))
            {
                Thread.Sleep(300);
                if (DateTime.Now > end)
                {
                    throw new Exception("time out");
                }
            }
            action();
        }
    }
}
