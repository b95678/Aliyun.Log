using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Authenication
{
    internal abstract class ServiceSignature
    {
        public abstract string SignatureMethod { get; }

        public abstract string SignatureVersion { get; }

        protected ServiceSignature()
        {
        }

        public string ComputeSignature(string key, string data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException(Properties.Resources.ExceptionIfArgumentStringIsNullOrEmpty, "key");
            if (string.IsNullOrEmpty(data))
                throw new ArgumentException(Properties.Resources.ExceptionIfArgumentStringIsNullOrEmpty, "data");

            return ComputeSignatureCore(key, data);
        }

        protected abstract string ComputeSignatureCore(string key, string data);

        public static ServiceSignature Create()
        {
            return new HmacSHA1Signature();
        }
    }
}
