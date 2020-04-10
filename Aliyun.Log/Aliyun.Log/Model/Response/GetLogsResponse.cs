using Aliyun.Log.Exception;
using Aliyun.Log.Model.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Response
{
    /// <summary>
    /// The response of the GetLog API from sls server
    /// </summary>
    public class GetLogsResponse : LogResponse
    {
        private static string[] validFields = { "count", "progress", "logs" };


        private long _count;
        private string _progress;
        private List<QueriedLog> _logs;

        /// <summary>
        /// constructor with http header and body from response
        /// </summary>
        /// <param name="headers">http header from response</param>
        /// <param name="jsonBody">http body (in json) from response</param>
        public GetLogsResponse(IDictionary<string, string> headers, JArray jsonBody) : base(headers)
        {
            string count;
            if (headers.TryGetValue(LogConst.NAME_HEADER_X_LOG_COUNT, out count))
            {
                _count = long.Parse(count);
            }
            headers.TryGetValue(LogConst.NAME_HEADER_X_LOG_PROGRESS, out _progress);
            ParseResponseBody(jsonBody);
        }
        public GetLogsResponse(IDictionary<string, string> headers, JObject jsonBody) : base(headers)
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
        /// The count of logs
        /// </summary>
        public long Count
        {
            get { return _count; }
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
        /// List of logs
        /// </summary>
        public List<QueriedLog> Logs
        {
            get { return _logs; }
        }

        internal override void DeserializeFromJsonInternal(JArray json)
        {
            _logs = QueriedLog.DeserializeFromJson(json);
            if (_count == 0) _count = _logs.Count;
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
            JArray jArray = json["logs"] as JArray;
            _logs = QueriedLog.DeserializeFromJson(jArray);
            if (_count == 0) _count = _logs.Count;
        }

        //used only in testing project
        internal string Print()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("{count:" + _count + "," + "progress:" + _progress + ",");
            if (_logs != null)
            {
                strBuilder.Append("{");
                foreach (QueriedLog log in _logs)
                    strBuilder.Append("[" + log.Print() + "]");
                strBuilder.Append("}");
            }
            strBuilder.Append("}");
            return strBuilder.ToString();
        }

    }
}
