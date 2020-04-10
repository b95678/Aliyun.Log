using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace Aliyun.Log.Model.Communication
{
    internal abstract class ServiceResponse : ServiceMessage, IDisposable
    {
        public abstract HttpStatusCode StatusCode
        {
            get; set;
        }
        public abstract System.Exception Failure { get; }

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSuccessful()
        {
            return (int)this.StatusCode / 100 == (int)HttpStatusCode.OK / 100;
        }

        public virtual void EnsureSuccessful()
        {
            if (!IsSuccessful())
            {
                // Disposing the content should help users: If users call EnsureSuccessStatusCode(), an exception is
                // thrown if the response status code is != 2xx. I.e. the behavior is similar to a failed request (e.g.
                // connection failure). Users don't expect to dispose the content in this case: If an exception is
                // thrown, the object is responsible fore cleaning up its state.
                if (Content != null)
                {
                    Content.Dispose();
                }

                Debug.Assert(this.Failure != null);
                throw this.Failure;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServiceResponse()
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
