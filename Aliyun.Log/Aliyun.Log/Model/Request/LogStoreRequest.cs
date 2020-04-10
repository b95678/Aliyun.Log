using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Request
{
    public abstract class LogStoreRequest : LogRequest
    {
        /// <summary>
        /// The logstore name
        /// </summary>
        public string Logstore { get; set; }
        public LogStoreRequest(string project, string logstore) : base(project)
        {
            Logstore = logstore;
        }
        public abstract void AddSpecParamsTo(IDictionary<string, string> dic);
        public abstract void AddSpecHeadersTo(IDictionary<string, string> dic);
    }
}
