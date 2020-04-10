using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Request
{
    public class LogRequest
    {
        /// <summary>
        /// default constructor of SLS Request.
        /// </summary>
        public LogRequest()
        {

        }

        /// <summary>
        /// LogRequest constructor with project name.
        /// </summary>
        /// <param name="project">project name to do SLS Request</param>
        public LogRequest(string project)
        {
            Project = project;
        }

        /// <summary>
        /// project name of the request
        /// </summary>
        public string Project { get; set; }

        internal bool IsSetProject()
        {
            return Project != null;
        }
    }
}
