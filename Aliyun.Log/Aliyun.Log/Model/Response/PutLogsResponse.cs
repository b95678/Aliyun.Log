using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Response
{
    /// <summary>
    /// The response of the PutLogs API from sls server
    /// </summary>
    public class PutLogsResponse : LogResponse
    {
        /// <summary>
        /// default constructor for PutLogsResponse
        /// </summary>
        /// <param name="header">header information in http response</param>
        public PutLogsResponse(IDictionary<string, string> header)
            : base(header)
        {

        }
    }
}
