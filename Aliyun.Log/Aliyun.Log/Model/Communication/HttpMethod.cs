using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Communication
{
    internal enum HttpMethod
    {
        /// <summary>
        /// Represents HTTP GET. Default value.
        /// </summary>
        Get = 0,

        /// <summary>
        /// Represents HTTP DELETE.
        /// </summary>
        Delete,

        /// <summary>
        /// Represents HTTP HEAD.
        /// </summary>
        Head,

        /// <summary>
        /// Represents HTTP POST.
        /// </summary>
        Post,

        /// <summary>
        /// Represents HTTP PUT.
        /// </summary>
        Put,

        /// <summary>
        /// Represents HTTP OPTIONS.
        /// </summary>
        Options,
    }
}
