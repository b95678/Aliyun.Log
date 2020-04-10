using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Request
{
    public class ListLogstoresRequest : LogRequest
    {

        /// <summary>
        /// default constructor
        /// </summary>
        public ListLogstoresRequest()
        {

        }
        /// <summary>
        /// constructor with project name
        /// </summary>
        /// <param name="project">project name</param>
        public ListLogstoresRequest(string project) : base(project)
        {
        }
    }
}
