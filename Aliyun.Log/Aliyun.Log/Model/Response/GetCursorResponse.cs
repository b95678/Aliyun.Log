using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Response
{
    public class GetCursorResponse : LogResponse
    {
        public string Cursor { get; set; }
        public GetCursorResponse(IDictionary<string, string> headers, string cursor) : base(headers)
        {
            Cursor = cursor;
        }
    }
}
