using Aliyun.Log.Model.Authenication;
using Aliyun.Log.Model.Communication;


namespace Aliyun.Log.Interface.Authenication
{
    internal interface IRequestSigner
    {
        void Sign(ServiceRequest request, ServiceCredentials credentials);
    }
}
