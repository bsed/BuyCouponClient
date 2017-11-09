using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace CouponClient.Models
{
    public class DownloadHandler : IDownloadHandler
    {
        public delegate void DownloadUploadEventHandler(IBrowser browser, DownloadItem downloadItem);

        public delegate void DownloadCompleteEventHandler(DownloadItem downloadItem, string path);

        public event DownloadUploadEventHandler OnDownloadStart;
        public event DownloadUploadEventHandler OnDownloading;
        public event DownloadCompleteEventHandler OnDownloadComplete;



        public DownloadHandler()
        {
            _path = $"{System.Environment.CurrentDirectory}\\temp";
        }

        public void Set(string prefix = null, string suffix = null, string newName = null, string path = null)
        {
            _prefix = prefix;
            _suffix = suffix;
            _newName = newName;
            if (!string.IsNullOrWhiteSpace(path))
            {
                _path = $"{_path}\\{path}";
            }

        }

        private string _path;
        private string _prefix, _suffix, _newName;

        public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            var fileInfo = new FileInfo(downloadItem.SuggestedFileName);
            string fileName = fileName = $"{_prefix}{_newName ?? fileInfo.Name.Replace(fileInfo.Extension, "")}{_suffix}{fileInfo.Extension}";
            string filePath = $"{_path}\\{fileName}";
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    OnDownloadStart(browser, downloadItem);
                    if (File.Exists(filePath))
                    {
                        downloadItem.IsCancelled = true;
                        OnDownloadComplete(downloadItem, filePath);
                    }
                    else
                    {
                        callback.Continue(filePath, false);
                    }

                }
            }
        }

        public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            if (downloadItem.IsInProgress)
            {
                OnDownloading(browser, downloadItem);
            }
            if (downloadItem.IsComplete || downloadItem.IsCancelled)
            {
                OnDownloadComplete(downloadItem, $"{downloadItem.FullPath}");
            }

        }
    }





}
