using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Response
{
    public class ListShardsResponse : LogResponse
    {
        private List<int> _shards;

        public ListShardsResponse(IDictionary<string, string> httpHeaders, JArray body)
            : base(httpHeaders)
        {
            ParseResponseBody(body);
        }

        public ListShardsResponse(IDictionary<string,string> httpHeaders,JObject body):base(httpHeaders) {
            ParseResponseBody(body);
        }

        public List<int> Shards
        {
            get { return _shards; }
        }
        internal override void DeserializeFromJsonInternal(JArray json)
        {
            _shards = new List<int>();
            foreach (JObject obj in json.Children<JObject>())
            {
                _shards.Add(int.Parse(obj.GetValue("shardID").ToString()));
            }
        }
    }
}
