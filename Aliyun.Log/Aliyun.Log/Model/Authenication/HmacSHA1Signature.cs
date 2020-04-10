using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Aliyun.Log.Model.Authenication
{
    internal class HmacSHA1Signature : ServiceSignature
    {
        private Encoding _encoding = Encoding.UTF8;

        public override string SignatureMethod
        {
            get { return "HmacSHA1"; }
        }

        public override string SignatureVersion
        {
            get { return "1"; }
        }

        public HmacSHA1Signature()
        {
        }

        protected override string ComputeSignatureCore(string key, string data)
        {
            Debug.Assert(!string.IsNullOrEmpty(data));

            using (KeyedHashAlgorithm algorithm = KeyedHashAlgorithm.Create(this.SignatureMethod.ToString().ToUpperInvariant()))
            {
                algorithm.Key = _encoding.GetBytes(key.ToCharArray());
                return Convert.ToBase64String(algorithm.ComputeHash(_encoding.GetBytes(data.ToCharArray())));
            }
        }

    }
}
