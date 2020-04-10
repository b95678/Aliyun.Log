using Aliyun.Log.Model.Communication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Interface.Handler
{
    internal interface IResponseHandler
    {
        void Handle(ServiceResponse response);
    }
}
