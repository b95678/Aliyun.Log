using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Request
{
    /// <summary>
    /// The Request used to get histograms of a query from sls server
    /// </summary>
    public class GetHistogramsRequest : LogRequest
    {
        private string _logstore;
        private string _topic;
        private uint? _from;
        private uint? _to;
        private string _query;

        /// <summary>
        /// default constructor.
        /// please set required fileds(project, logstore, from, to) initialized by this default constructor before  
        /// using it to send request. Otherwise, request will be failed with exception.
        /// </summary>
        public GetHistogramsRequest()
        {

        }

        /// <summary>
        /// constructor with all required fileds
        /// </summary>
        /// <param name="project">project name</param>
        /// <param name="logstore">logstore name</param>
        /// <param name="from">begin timestamp of time range to query</param>
        /// <param name="to">end timestamp of time range to query</param>
        public GetHistogramsRequest(string project, string logstore, uint from, uint to)
            : base(project)
        {
            _logstore = logstore;
            _from = from;
            _to = to;
        }

        /// <summary>
        /// constructor with all possible fileds
        /// </summary>
        /// <param name="project">project name</param>
        /// <param name="logstore">logstore name</param>
        /// <param name="from">begin timestamp of time range to query</param>
        /// <param name="to">end timestamp of time range to query</param>
        /// <param name="topic">log topic to query</param>
        /// <param name="query">query string to run</param>
        public GetHistogramsRequest(string project, string logstore, uint from, uint to, string topic, string query)
            : base(project)
        {
            _logstore = logstore;
            _from = from;
            _to = to;
            _topic = topic;
            _query = query;
        }

        /// <summary>
        /// The logstore name
        /// </summary>
        public string Logstore
        {
            get { return _logstore; }
            set { _logstore = value; }
        }

        internal bool IsSetLogstore()
        {
            return _logstore != null;
        }

        /// <summary>
        /// The log topic to query
        /// </summary>
        public string Topic
        {
            get { return _topic; }
            set { _topic = value; }
        }

        internal bool IsSetTopic()
        {
            return _topic != null;
        }

        /// <summary>
        /// The begin timestamp of time range to query
        /// </summary>
        public uint From
        {
            get { return _from ?? default(uint); }
            set { _from = value; }
        }

        internal bool IsSetFrom()
        {
            return _from.HasValue;
        }

        /// <summary>
        /// The end timestamp of time range to query
        /// </summary>
        public uint To
        {
            get { return _to ?? default(uint); }
            set { _to = value; }
        }

        internal bool IsSetTo()
        {
            return _to.HasValue;
        }

        /// <summary>
        /// The query string to run
        /// </summary>
        public string Query
        {
            get { return _query; }
            set { _query = value; }
        }

        internal bool IsSetQuery()
        {
            return _query != null;
        }
    }
}
