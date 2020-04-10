using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Exception
{
    public class LogException : ApplicationException
    {
        private string _errorCode;
        private string _requestId;
        /// <summary>
        /// Get Sls sever requestid.
        /// </summary>
        public string RequestId
        {
            get
            {
                return _requestId;
            }
        }
        /// <summary>
        /// Get LogException error code.
        /// </summary>
        public string ErrorCode
        {
            get
            {
                return _errorCode;
            }
        }

        public LogException(string code,string message,string requestid = "") : base(message)
        {
            _errorCode = code;
            _requestId = requestid;
        }

        public LogException(string code,string message,System.Exception innerException, string requestid = "") :base(message,innerException)
        {
            _errorCode = code;
            _requestId = requestid;
        }

        public override string ToString()
        {
            return $"ErrorCode:{ErrorCode},ErrorMsg:{Message},ErrorRequestId:{RequestId}";
        }
    }
}
