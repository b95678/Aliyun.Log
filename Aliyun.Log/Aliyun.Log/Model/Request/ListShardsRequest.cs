using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Request
{
    public class ListShardsRequest : LogStoreRequest
    {
        public ListShardsRequest(string projrct, string logstore) : base(projrct, logstore)
        {
        }
        override public void AddSpecParamsTo(IDictionary<string, string> dic)
        {

        }
        override public void AddSpecHeadersTo(IDictionary<string, string> dic)
        {

        }
    }
}
