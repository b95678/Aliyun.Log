using Aliyun.Log.Exception;
using Aliyun.Log.Model.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Response
{
    /// <summary>
    /// The response of the GetHistogram API from sls server
    /// </summary>
    public class GetHistogramsResponse : LogResponse
    {
        private static string[] validFields = { "count", "progress", "histograms" };

        private string _progress;
        private long _count;
        private List<Histogram> _histograms;

        /// <summary>
        /// constructor with http header and body from response
        /// </summary>
        /// <param name="headers">http header from respsone</param>
        /// <param name="jsonBody">http body (in json) from response</param>
        public GetHistogramsResponse(IDictionary<string, string> headers, JArray jsonBody) : base(headers)
        {
            string count;
            if (headers.TryGetValue(LogConst.NAME_HEADER_X_LOG_COUNT, out count))
            {
                _count = long.Parse(count);
            }
            headers.TryGetValue(LogConst.NAME_HEADER_X_LOG_PROGRESS, out _progress);
            ParseResponseBody(jsonBody);
        }
        public GetHistogramsResponse(IDictionary<string, string> headers, JObject jsonBody) : base(headers)
        {
            string count;
            if (headers.TryGetValue(LogConst.NAME_HEADER_X_LOG_COUNT, out count))
            {
                _count = long.Parse(count);
            }
            headers.TryGetValue(LogConst.NAME_HEADER_X_LOG_PROGRESS, out _progress);
            ParseResponseBody(jsonBody);
        }

        /// <summary>
        /// detect whether response are complete or not.
        /// </summary>
        /// <returns>true if response is complete. otherwise return false</returns>
        public bool IsCompleted()
        {
            return _progress == LogConst.STATUS_COMPLETE;
        }

        /// <summary>
        /// The count of histograms
        /// </summary>
        public long TotalCount
        {
            get { return _count; }
        }
        /// <summary>
        /// All of histograms
        /// </summary>
        public List<Histogram> Histograms
        {
            get { return _histograms; }
        }

        internal override void DeserializeFromJsonInternal(JArray json)
        {
            _histograms = Histogram.DeserializeFromJson(json);
            //if (_count == 0) _count = json.Count;
        }

        internal override void DeserializeFromJsonInternal(JObject json)
        {
            base.DeserializeFromJsonInternal(json);

            //判断Field有无出错
            foreach (var obj in json)
            {
                if (!Array.Exists(validFields, p => p == obj.Key))
                {
                    throw new LogException("LOGBadResponse", "The response is not valid json string : " + json, GetRequestId());

                }
            }

            JArray jArray = json["histograms"] as JArray;

            _histograms = Histogram.DeserializeFromJson(jArray);
        }
    }
}
