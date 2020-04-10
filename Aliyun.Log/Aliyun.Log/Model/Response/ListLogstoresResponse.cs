using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Response
{
    public class ListLogstoresResponse : LogResponse
    {
        private int _count;
        private List<string> _logstores;

        /// <summary>
        /// constructor with http header and body from response
        /// </summary>
        /// <param name="headers">http header from respsone</param>
        /// <param name="jsonBody">http body (in json) from response</param>
        public ListLogstoresResponse(IDictionary<string, string> headers, JObject jsonBody)
            : base(headers)
        {
            ParseResponseBody(jsonBody);
        }

        /// <summary>
        /// Count of the logstores
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// All of the logstores
        /// </summary>
        public List<string> Logstores
        {
            get { return _logstores; }
        }

        internal override void DeserializeFromJsonInternal(JObject json)
        {
            _count = (int)json[LogConst.NAME_LISTLOGSTORE_TOTAL];
            _logstores = JsonConvert.DeserializeObject<List<string>>(json[LogConst.NAME_LISTLOGSTORE_ITEM].ToString());
        }

    }
}
