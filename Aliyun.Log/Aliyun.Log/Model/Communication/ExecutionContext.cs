using Aliyun.Log.Interface.Authenication;
using Aliyun.Log.Interface.Handler;
using Aliyun.Log.Model.Authenication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Communication
{
    internal class ExecutionContext
    {
        /// <summary>
        /// The default encoding (charset name).
        /// </summary>
        private const string DefaultEncoding = "utf-8";

        private IList<IResponseHandler> _responseHandlers = new List<IResponseHandler>();

        /// <summary>
        /// Gets or sets the charset.
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// Gets or sets the request signer.
        /// </summary>
        public IRequestSigner Signer { get; set; }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        public ServiceCredentials Credentials { get; set; }

        /// <summary>
        /// Gets the list of <see cref="IResponseHandler" />.
        /// </summary>
        public IList<IResponseHandler> ResponseHandlers
        {
            get { return _responseHandlers; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExecutionContext()
        {
            this.Charset = DefaultEncoding;
        }
    }
}
