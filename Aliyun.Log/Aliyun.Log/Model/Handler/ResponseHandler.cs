using Aliyun.Log.Interface.Handler;
using Aliyun.Log.Model.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Aliyun.Log.Model.Handler
{
    internal class ResponseHandler : IResponseHandler
    {
        public ResponseHandler()
        {
        }

        public virtual void Handle(ServiceResponse response)
        {
            Debug.Assert(response != null);
        }
    }
}
