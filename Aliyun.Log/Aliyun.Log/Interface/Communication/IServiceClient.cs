using Aliyun.Log.Model.Communication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Interface.Communication
{
    internal interface IServiceClient
    {
        /// <summary>
        /// Sends a request to the service.
        /// </summary>
        /// <param name="request">The request data.</param>
        /// <param name="context">The execution context.</param>
        /// <returns>The response data.</returns>
        ServiceResponse Send(ServiceRequest request, ExecutionContext context);
    }
}
