using Aliyun.Log.Interface.Authenication;
using Aliyun.Log.Model.Communication;
using Aliyun.Log.Util;
using System;

namespace Aliyun.Log.Model.Authenication
{
    internal class LogRequestSigner:IRequestSigner
    {
        private readonly string httpResource;
        private readonly HttpMethod httpMethod;

        public LogRequestSigner(string httpResource, HttpMethod httpMethod)
        {
            this.httpResource = httpResource;
            this.httpMethod = httpMethod;
        }

        public void Sign(ServiceRequest request, ServiceCredentials credentials)
        {
            request.Headers.Add(LogConst.NAME_HEADER_AUTH, LogClientTools.Signature(request.Headers, request.Parameters, this.httpMethod, this.httpResource, credentials.AccessId, credentials.AccessKey));
        }
    }
}
