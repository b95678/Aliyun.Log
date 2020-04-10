using Aliyun.Log.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Aliyun.Log.Model.Communication
{
    internal class ServiceRequest:ServiceMessage,IDisposable
    {
        private bool _disposed;

        private IDictionary<string, string> parameters =   new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the endpoint.
        /// </summary>
        public Uri Endpoint { get; set; }

        /// <summary>
        /// Gets or sets the resource path of the request URI.
        /// </summary>
        public string ResourcePath { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        public HttpMethod Method { get; set; }

        public HttpWebRequest HttpRequest { get; set; }

        /// <summary>
        /// Gets the dictionary of the request parameters.
        /// </summary>
        public IDictionary<string, string> Parameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// Gets whether the request can be repeated.
        /// </summary>
        public bool IsRepeatable
        {
            get { return this.Content == null || this.Content.CanSeek; }
        }

        /// <summary>
        /// Constuctor.
        /// </summary>
        public ServiceRequest()
        {
            _headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Build the request URI from the request message.
        /// </summary>
        /// <returns></returns>
        public string BuildRequestUri()
        {
            const string delimiter = "/";
            string uri = Endpoint.ToString();
            if (!uri.EndsWith(delimiter)
                && (ResourcePath == null ||
                    !ResourcePath.StartsWith(delimiter)))
            {
                uri += delimiter;
            }
            else if (uri.EndsWith(delimiter) && (ResourcePath != null && ResourcePath.StartsWith(delimiter)))
            {
                uri = uri.Substring(0, uri.Length - 1);
            }

            if (ResourcePath != null)
            {
                uri += ResourcePath;
            }

            if (IsParameterInUri())
            {
                string paramString = HttpUtil.GetRequestParameterString(parameters);
                if (!string.IsNullOrEmpty(paramString))
                {
                    uri += "?" + paramString;
                }
            }
            return uri;
        }

        public Stream BuildRequestContent()
        {
            if (!IsParameterInUri())
            {
                string paramString = HttpUtil.GetRequestParameterString(parameters);
                if (!string.IsNullOrEmpty(paramString))
                {
                    byte[] buffer = Encoding.GetEncoding("utf-8").GetBytes(paramString);
                    Stream content = new MemoryStream();
                    content.Write(buffer, 0, buffer.Length);
                    content.Flush();
                    // Move the marker to the beginning for further read.
                    content.Seek(0, SeekOrigin.Begin);
                    return content;
                }
            }

            return this.Content;
        }

        private bool IsParameterInUri()
        {
            bool requestHasPayload = this.Content != null;
            bool requestIsPost = this.Method == HttpMethod.Post;
            bool putParamsInUri = !requestIsPost || requestHasPayload;
            return putParamsInUri;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (Content != null)
                {
                    Content.Close();
                    Content = null;
                }
                if (HttpRequest != null)
                {
                    // force close connection group
                    //Console.WriteLine(HttpRequest.ConnectionGroupName + "[]" + HttpRequest.ConnectionGroupName.Length.ToString() + "[]" + HttpRequest.ServicePoint.CurrentConnections.ToString());
                    HttpRequest.ServicePoint.CloseConnectionGroup(HttpRequest.ConnectionGroupName);
                    HttpRequest.Abort();
                    HttpRequest = null;
                }
                _disposed = true;
            }
        }
    }
}
