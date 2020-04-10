using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aliyun.Log.Model.Communication
{
    internal class ServiceMessage
    {
        // HTTP header keys are case-insensitive.
        protected IDictionary<string, string> _headers;

        /// <summary>
        /// Gets the dictionary of HTTP headers.
        /// </summary>
        public virtual IDictionary<string, string> Headers
        {
            get { return _headers; }
        }

        /// <summary>
        /// Gets or sets the content stream.
        /// </summary>
        public virtual Stream Content { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServiceMessage()
        {
        }
    }
}
