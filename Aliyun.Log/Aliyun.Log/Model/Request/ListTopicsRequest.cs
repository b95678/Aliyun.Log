using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Request
{
    /// <summary>
    /// The request used to list topic from sls server
    /// </summary>
    public class ListTopicsRequest : LogRequest
    {
        private string _logstore;
        private string _token;
        private int? _lines;

        /// <summary>
        /// default constructor.
        /// please set required fileds(project, logstore) initialized by this default constructor before using it to 
        /// send request. Otherwise, request will be failed with exception.
        /// </summary>
        public ListTopicsRequest()
        {

        }

        /// <summary>
        /// constructor with all required fileds
        /// </summary>
        /// <param name="project">project name</param>
        /// <param name="logstore">logstore name</param>
        public ListTopicsRequest(string project, string logstore)
            : base(project)
        {
            _logstore = logstore;
        }

        /// <summary>
        /// constructor with all possible fileds
        /// </summary>
        /// <param name="project">project name</param>
        /// <param name="logstore">logstore name</param>
        /// <param name="token">token to list more topics</param>
        /// <param name="lines">count of topics to request</param>
        public ListTopicsRequest(string project, string logstore, string token, Int32 lines)
            : base(project)
        {
            _logstore = logstore;
            _token = token;
            _lines = lines;
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
        /// The token to list more topics
        /// </summary>
        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        internal bool IsSetToken()
        {
            return _token != null;
        }

        /// <summary>
        /// The count of topics to request
        /// </summary>
        public int Lines
        {
            get { return _lines ?? default(int); }
            set { _lines = value; }
        }

        internal bool IsSetLines()
        {
            return _lines.HasValue;
        }
    }
}
