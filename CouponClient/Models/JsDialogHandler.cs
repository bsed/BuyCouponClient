using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouponClient.Models
{
    public class JsDialogHandler : IJsDialogHandler
    {
        public void OnDialogClosed(IWebBrowser browserControl, IBrowser browser)
        {
            //throw new NotImplementedException();
        }

        public bool OnJSBeforeUnload(IWebBrowser browserControl, IBrowser browser, string message, bool isReload, IJsDialogCallback callback)
        {
            return true;
            //throw new NotImplementedException();
        }

        public bool OnJSDialog(IWebBrowser browserControl, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {
            OnJSDialogShow?.Invoke(messageText);
            return messageText == "下载速度太快，请稍后再试！";
            //throw new NotImplementedException();
        }

        public void OnResetDialogState(IWebBrowser browserControl, IBrowser browser)
        {

        }

        public event JsDialogEventHandler OnJSDialogShow;
    }

    public delegate void JsDialogEventHandler(string message);
}
