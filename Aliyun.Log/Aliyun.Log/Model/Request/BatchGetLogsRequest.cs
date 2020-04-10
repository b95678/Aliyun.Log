using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Request
{
    public class BatchGetLogsRequest : LogStoreRequest
    {
        private int _shard;
        private int _count;
        private string _cursor;
        public int Shard
        {
            get { return _shard; }
            set { _shard = value; }
        }
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }
        public string Cursor
        {
            get { return _cursor; }
            set { _cursor = value; }
        }
        public BatchGetLogsRequest(string project, string logstore, int shard, string cursor, int count)
            : base(project, logstore)
        {
            Shard = shard;
            Cursor = cursor;
            Count = count;
        }
        override public void AddSpecParamsTo(IDictionary<string, string> dic)
        {
            dic.Add("type", "log");
            dic.Add("cursor", Cursor);
            dic.Add("count", _count.ToString());
        }
        override public void AddSpecHeadersTo(IDictionary<string, string> dic)
        {
            dic.Add(LogConst.NAME_HEADER_ACCEPT_ENCODING, LogConst.VALUE_HEADER_COMPRESSTYPE_LZ4);
            dic.Add(LogConst.NAME_HEADER_ACCEPT, LogConst.PBVALUE_HEADER_CONTENTTYPE);
        }
    }
}
