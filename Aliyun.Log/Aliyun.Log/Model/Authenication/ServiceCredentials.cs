using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Authenication
{
    internal class ServiceCredentials
    {
        /// <summary>
        /// Gets the access ID.
        /// </summary>
        public string AccessId { get; private set; }

        /// <summary>
        /// Gets the access key.
        /// </summary>
        public string AccessKey { get; private set; }

        /// <summary>
        /// Initialize an new instance of <see cref="ServiceCredentials"/>.
        /// </summary>
        /// <param name="accessId">The access ID.</param>
        /// <param name="accessKey">The access key.</param>
        public ServiceCredentials(string accessId, string accessKey)
        {
            if (string.IsNullOrEmpty(accessId))
                throw new ArgumentException(Properties.Resources.ExceptionIfArgumentStringIsNullOrEmpty, "accessId");

            AccessId = accessId;
            AccessKey = accessKey;
        }
    }
}
