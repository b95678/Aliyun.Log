using Aliyun.Log.Exception;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Response
{
    public class LogResponse
    {

        // Http header of the response
        private Dictionary<string, string> _headers = new Dictionary<string, string>();

        /// <summary>
        /// LogResponse constructor with HTTP response headers
        /// </summary>
        /// <param name="httpHeaders">HTTP response header from SLS server</param>        
        public LogResponse(IDictionary<string, string> httpHeaders)
        {
            _headers = new Dictionary<string, string>(httpHeaders);
        }

        /// <summary>
        /// Get the value from the head of response using key
        /// </summary>
        /// <returns>Value of specified http header</returns>
        public string GetHeader(string key)
        {
            string res = null;
            _headers.TryGetValue(key, out res);
            return res;
        }

        /// <summary>
        /// Get request Id for current response generated on server-side. it is useful to track any potential issues
        /// for this request.
        /// </summary>
        /// <returns>request Id generated on server-side</returns>
        public string GetRequestId()
        {
            string requestId = string.Empty;
            _headers.TryGetValue(LogConst.NAME_HEADER_REQUESTID, out requestId);
            return requestId;
        }

        /// <summary>
        /// Get all the http response headers
        /// </summary>
        /// <returns>Key-pair map for http headers</returns>
        public Dictionary<string, string> GetAllHeaders()
        {
            return new Dictionary<string, string>(_headers);
        }

        //internal helper function to consolidate logic to throw exception when parsing json string in http response.
        internal void ParseResponseBody(JObject jsonBody)
        {
            try
            {
                DeserializeFromJsonInternal(jsonBody);
            }
            catch(LogException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw new LogException("LOGBadResponse", "The response is not valid json string : " + jsonBody, ex, GetRequestId());
            }
        }
        internal void ParseResponseBody(JArray jsonBody)
        {
            try
            {
                DeserializeFromJsonInternal(jsonBody);
            }
            catch (System.Exception ex)
            {
                throw new LogException("LOGBadResponse", "The response is not valid json string : " + jsonBody, ex, GetRequestId());
            }
        }

        internal virtual void DeserializeFromJsonInternal(JObject json) {
            //判断有无出错
            if (!string.IsNullOrWhiteSpace(Convert.ToString(json[LogConst.NAME_ERROR_CODE])))
            {
                throw new LogException(json[LogConst.NAME_ERROR_CODE].ToString(), json[LogConst.NAME_ERROR_MESSAGE].ToString(), GetRequestId());
            }
            


        }
        internal virtual void DeserializeFromJsonInternal(JArray json) { }
    }
}
