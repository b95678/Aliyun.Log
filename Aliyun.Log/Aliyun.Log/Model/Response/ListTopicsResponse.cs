using Aliyun.Log.Exception;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Aliyun.Log.Model.Response
{
    public class ListTopicsResponse : LogResponse
    {
        private static string[] validFields = { "count", "next_token", "topics" };

        private long _count;
        private string _nextToken;
        private List<string> _topics;

        /// <summary>
        /// constructor with http header and body from response
        /// </summary>
        /// <param name="headers">http header from respsone</param>
        /// <param name="jsonBody">http body (in json) from response</param>
        public ListTopicsResponse(IDictionary<string, string> headers, JArray jsonBody) : base(headers)
        {
            headers.TryGetValue(LogConst.NAME_HEADER_X_LOG_NEXT_TOKEN, out _nextToken);
            string tmpCount;
            if (headers.TryGetValue(LogConst.NAME_HEADER_X_LOG_COUNT, out tmpCount))
            {
                _count = int.Parse(tmpCount);
            }
            ParseResponseBody(jsonBody);
        }

        public ListTopicsResponse(IDictionary<string, string> headers, JObject jsonBody) : base(headers)
        {
            headers.TryGetValue(LogConst.NAME_HEADER_X_LOG_NEXT_TOKEN, out _nextToken);
            string tmpCount;
            if (headers.TryGetValue(LogConst.NAME_HEADER_X_LOG_COUNT, out tmpCount))
            {
                _count = int.Parse(tmpCount);
            }
            ParseResponseBody(jsonBody);
        }



        /// <summary>
        /// The count of log topics in the response
        /// </summary>
        public long Count
        {
            get { return _count; }
        }

        /// <summary>
        /// The next token property in the response. It is used to list more topics in next ListTopics request. 
        /// If there is no more topics to list, it will return an empty string.
        /// </summary>
        public string NextToken
        {
            get { return _nextToken; }
        }

        /// <summary>
        /// All log topics in the response
        /// </summary>
        public List<string> Topics
        {
            get { return _topics; }
        }
        internal override void DeserializeFromJsonInternal(JArray json)
        {
            _topics = JsonConvert.DeserializeObject<List<string>>(json.ToString());
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
            JArray jArray = json["topics"] as JArray;
            _topics = JsonConvert.DeserializeObject<List<string>>(jArray.ToString());
            if (_count == 0) _count = _topics.Count;
        }
    }
}
