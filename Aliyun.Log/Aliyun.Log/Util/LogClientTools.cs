using Aliyun.Log.Exception;
using Aliyun.Log.Model;
using Aliyun.Log.Model.Communication;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using LZ4;
using ICSharpCode.SharpZipLib.GZip;
using System.Net;
using Aliyun.Log.Interface.Authenication;
using Aliyun.Log.Model.Authenication;
using System.Runtime.CompilerServices;

namespace Aliyun.Log.Util
{
    internal class LogClientTools
    {
        /// <summary>
        /// 压缩到ZIP
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] CompressToZlib(byte[] buffer)
        {
            using (MemoryStream compressStream = new MemoryStream())
            {
                using (GZipOutputStream compressedzipStream = new GZipOutputStream(compressStream))
                {
                    compressedzipStream.Write(buffer, 0, buffer.Length);
                }
                return compressStream.ToArray();
            }
        }

        /// <summary>
        /// 从ZIP解压
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="rawSize"></param>
        /// <returns></returns>
        public static byte[] DecompressFromZlib(Stream stream, int rawSize)
        {
            using (stream)
            {
                using (GZipInputStream gzi = new GZipInputStream(stream))
                {
                    using (MemoryStream ms = new MemoryStream(rawSize))
                    {
                        int count;
                        byte[] data = new byte[rawSize];
                        while ((count = gzi.Read(data, 0, data.Length)) != 0)
                        {
                            ms.Write(data, 0, count);
                        }
                        byte[] overarr = ms.ToArray();
                        return overarr;
                    }
                }

            }
        }
        public static byte[] CompressToLz4(byte[] buffer)
        {
            //using (MemoryStream ms=new MemoryStream(buffer))
            //{
            //    using (LZ4Stream sr = new LZ4Stream(ms, LZ4StreamMode.Compress,LZ4StreamFlags.HighCompression))
            //    {
            //        ms.CopyTo(sr);


            //    }
            //    //var result = ms.ToArray();
            //    //return result;

            //}

            return LZ4Codec.Encode(buffer, 0, buffer.Length);

        }
        public static byte[] DecompressFromLZ4(Stream stream, int rawLength)
        {
            using (stream)
            {
                using (LZ4Stream streamInner = new LZ4Stream(stream,LZ4StreamMode.Decompress))
                {

                    byte[] output = new byte[rawLength];
                    streamInner.Read(output, 0, rawLength);
                    return output;
                }
            }
        }
        public static string GetMd5Value(byte[] buffer)
        {
            return GetMd5Value(buffer, 0, buffer.Length);
        }
        public static string GetMd5Value(byte[] buffer, int offset, int count)
        {
            MD5 hash = MD5.Create(LogConst.NAME_MD5);
            StringBuilder returnStr = new StringBuilder();
            byte[] md5hash = hash.ComputeHash(buffer, offset, count);
            if (md5hash != null)
            {
                for (int i = 0; i < md5hash.Length; i++)
                {
                    returnStr.Append(md5hash[i].ToString("X2").ToUpper());
                }
            }
            return returnStr.ToString();
        }

        public static void ResponseErrorCheck(ServiceResponse response, ServiceCredentials credentials)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                string requestId = "";
                response.Headers.TryGetValue(LogConst.NAME_HEADER_REQUESTID, out requestId);
                //if (requestId == null) requestId = Guid.NewGuid().ToString();
                JObject body = ParserResponseToJObject(response.Content);
                throw new LogException(body[LogConst.NAME_ERROR_CODE].ToString(), body[LogConst.NAME_ERROR_MESSAGE].ToString(), requestId);
            }
        }
        internal class KeyValueComparer : IComparer<KeyValuePair<string, string>>
        {
            public int Compare(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
            {
                int rtu = string.Compare(x.Key, y.Key, StringComparison.Ordinal);
                return rtu == 0 ? string.Compare(x.Value, y.Value, StringComparison.Ordinal) : rtu;
            }
        }
        private static string MapEnumMethodToString(HttpMethod httpMethod)
        {
            switch (httpMethod)
            {
                case HttpMethod.Get:
                    return LogConst.NAME_HTTP_GET;
                case HttpMethod.Post:
                    return LogConst.NAME_HTTP_POST;
                case HttpMethod.Put:
                    return LogConst.NAME_HTTP_PUT;
                case HttpMethod.Head:
                    return LogConst.NAME_HTTP_HEAD;
                case HttpMethod.Delete:
                    return LogConst.NAME_HTTP_DELETE;
                case HttpMethod.Options:
                    return LogConst.NAME_HTTP_OPTIONS;
                default:
                    Debug.Assert(false, "invalid http method");
                    return "";
            }
        }
        private static string GetRequestString(IEnumerable<KeyValuePair<string, string>> parameters, string kvDelimiter, string separator)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            if (parameters != null)
            {
                bool isFirst = true;
                foreach (var p in parameters)
                {
                    if (!isFirst)
                    {
                        stringBuilder.Append(separator);
                    }
                    isFirst = false;
                    stringBuilder.Append(p.Key);
                    if (p.Value != null)
                    {
                        stringBuilder.Append(kvDelimiter).Append(p.Value);
                    }
                }
            }
            return stringBuilder.ToString();
        }
        internal static string BuildHeaderSigStr(IDictionary<string, string> headers)
        {
            List<KeyValuePair<string, string>> headerLst = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<string, string> pair in headers)
            {
                if ((pair.Key.StartsWith("x-log-") && pair.Key.CompareTo(LogConst.NAME_HEADER_DATE) != 0)
                    || pair.Key.StartsWith("x-acs-"))
                {
                    headerLst.Add(new KeyValuePair<string, string>(pair.Key.Trim().ToLower(), pair.Value.Trim()));
                }
            }
            headerLst.Sort(new KeyValueComparer());
            StringBuilder reqUri = new StringBuilder();
            reqUri.Append(LogClientTools.GetValeFromDic(headers, LogConst.NAME_HEADER_MD5)).Append("\n")
                .Append(LogClientTools.GetValeFromDic(headers, LogConst.NAME_HEADER_CONTENTTYPE)).Append("\n")
                .Append(LogClientTools.GetValeFromDic(headers, LogConst.NAME_HEADER_DATE)).Append("\n")
                .Append(GetRequestString(headerLst, ":", "\n"));
            return reqUri.ToString();
        }

        internal static string SigInternal(string source, string accessKeyId, string accessKey)
        {
            ServiceSignature signAlthm = ServiceSignature.Create();
            return LogConst.PREFIX_VALUE_HEADER_AUTH + accessKeyId + ":" + signAlthm.ComputeSignature(accessKey, source);
        }
        public static string Signature(IDictionary<string, string> headers, string accessKeyId, string accessKey)
        {
            return SigInternal(BuildHeaderSigStr(headers), accessKeyId, accessKey);
        }
        public static string Signature(IDictionary<string, string> headers, IDictionary<string, string> paramDic, HttpMethod method, string resource, string accessKeyId, string accessKey)
        {
            List<KeyValuePair<string, string>> paramLst = new List<KeyValuePair<string, string>>(paramDic);

            paramLst.Sort(new KeyValueComparer());

            StringBuilder reqUri = new StringBuilder();
            reqUri.Append(MapEnumMethodToString(method)).Append("\n")
                .Append(BuildHeaderSigStr(headers)).Append("\n")
                .Append(resource)
                .Append((paramLst != null && paramLst.Count > 0) ? ("?" + GetRequestString(paramLst, "=", "&")) : (""));
            return SigInternal(reqUri.ToString(), accessKeyId, accessKey);
        }
        internal static string GetValeFromDic(IDictionary<string, string> dic, string keyName)
        {
            string value = null;
            if (dic.TryGetValue(keyName, out value))
                return value;
            return "";
        }
        public static JArray ParserResponseToJArray(Stream response)
        {
            using (response)
            {
                StreamReader sr = null;
                string json = null;
                try
                {
                    sr = new StreamReader(response, Encoding.UTF8);
                }
                catch (System.Exception e)
                {
                    if (sr != null)
                        sr.Close();
                    throw new LogException("LOGBadResponse", "The response from the server is empty", e);
                }
                try
                {
                    json = sr.ReadToEnd();
                }
                catch (IOException e)
                {
                    throw new LogException("LOGBadResponse", "Io exception happened when parse the response data : ", e);
                }
                catch (OutOfMemoryException e)
                {
                    throw new LogException("LOGBadResponse", "There is not enough memory to continue the execution of parsing the response data : ", e);
                }
                finally
                {
                    sr.Close();
                }
                try
                {
                    JArray obj = JArray.Parse(json);
                    return obj;
                }
                catch (System.Exception e)
                {
                    //如果是Server返回的错误
                    if (json.Contains(LogConst.NAME_ERROR_CODE))
                    {
                        JObject jo = JObject.Parse(json);
                        throw new LogException(jo[LogConst.NAME_ERROR_CODE].ToString(), jo[LogConst.NAME_ERROR_MESSAGE].ToString());
                    }
                    throw new LogException("LOGBadResponse", "The response is not valid json string : " + json, e);
                }
            }
        }
        public static JObject ParserResponseToJObject(Stream response)
        {
            using (response)
            {
                StreamReader sr = null;
                string json = null;
                try
                {
                    sr = new StreamReader(response, Encoding.UTF8);
                }
                catch (System.Exception e)
                {
                    if (sr != null)
                        sr.Close();
                    throw new LogException("LOGBadResponse", "The response from the server is empty", e);
                }
                try
                {
                    json = sr.ReadToEnd();
                }
                catch (IOException e)
                {
                    throw new LogException("LOGBadResponse", "Io exception happened when parse the response data : ", e);
                }
                catch (OutOfMemoryException e)
                {
                    throw new LogException("LOGBadResponse", "There is not enough memory to continue the execution of parsing the response data : ", e);
                }
                finally
                {
                    sr.Close();
                }
                try
                {
                    JObject obj = JObject.Parse(json);
                    return obj;
                }
                catch (System.Exception e)
                {
                    throw new LogException("LOGBadResponse", "The response is not valid json string : " + json, e);
                }
            }
        }
    }
}
