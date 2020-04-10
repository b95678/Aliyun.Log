using Aliyun.Log.Model;
using Aliyun.Log.Util;
using Aliyun.Log.Exception;
using Aliyun.Log.Model.Communication;
using System;
using Aliyun.Log.Model.Response;
using Aliyun.Log.Model.Request;
using Aliyun.Log.Model.Authenication;
using Newtonsoft.Json.Linq;
using Aliyun.Log.Model.Config;
using Google.Protobuf;
using System.IO;
using System.Diagnostics;

namespace Aliyun.Log
{
    public class LogClient
    {
        private ServiceClient serviceClient;

        private object _slsClientLockObj = new object();
        private string _hostName;
        private string _uriScheme;
        private int _port;

        private string _localMachinePrivateIp;
        private string _securityToken;


        /// <summary>
        /// readonly property, AccessKeyId of LogClient
        /// </summary>
        public string AccessKeyId { get; }

        /// <summary>
        /// readonly property, AccessKey of LogClient
        /// </summary>
        public string AccessKey { get; }


        /// <summary>
        /// readonly property, Endpoint of LogClient
        /// </summary>
        public string Endpoint { get; private set; }

        /// <summary>
        /// Read/Write Timeouf for underlying HTTPWebRequest.ReadWriteTimeout
        /// </summary>
        public int ReadWriteTimeout
        {
            get
            {
                return serviceClient.Configuration.ReadWriteTimeout;
            }
            set
            {
                if (value > 0)
                {
                    lock (_slsClientLockObj)
                        serviceClient.Configuration.ReadWriteTimeout = value;
                }
            }
        }

        /// <summary>
        /// Connection Timeout for underlying HttpWebRequest.Timeout
        /// </summary>
        public int ConnectionTimeout
        {
            get
            {
                return serviceClient.Configuration.ConnectionTimeout;
            }
            set
            {
                if (value > 0)
                {
                    lock (_slsClientLockObj)
                        serviceClient.Configuration.ConnectionTimeout = value;
                }
            }
        }

        /// <summary>
        /// Construct the sls client with accessId, accessKey and server address, 
        /// all other parameters will be set to default value
        /// </summary>
        /// <param name="endpoint">the sls server address(e.g.,http://cn-hangzhou.sls.aliyuncs.com)</param>
        /// <param name="accessKeyId">aliyun accessId</param>
        /// <param name="accessKey">aliyun accessKey</param>
        public LogClient(string endpoint, string accessKeyId, string accessKey)
        {
            if (!endpoint.StartsWith("http://") && !endpoint.StartsWith("https://"))
                endpoint = "http://" + endpoint;
            setEndpoint(endpoint);
            if (IpUtil.IsIpAddress(this._hostName))
                throw new LogException("LogClientError", "client error happens");

            _localMachinePrivateIp = IpUtil.GetLocalMachinePrivateIp();
            AccessKeyId = accessKeyId;
            AccessKey = accessKey;

            serviceClient = ServiceClient.Create(new ClientConfiguration());
            serviceClient.Configuration.ConnectionTimeout = LogConst.DEFAULT_SLS_CONNECT_TIMEOUT;
            serviceClient.Configuration.ReadWriteTimeout = LogConst.DEFAULT_SLS_READWRT_TIMEOUT;
        }

        /// <summary>
        /// Construct the sls client with accessId, accessKey and server address, 
        /// all other parameters will be set to default value
        /// </summary>
        /// <param name="endpoint">the sls server address(e.g.,http://cn-hangzhou.sls.aliyuncs.com)</param>
        /// <param name="accessKeyId">aliyun accessId</param>
        /// <param name="accessKey">aliyun accessKey</param>
        /// <param name="securityToken">aliyun securityToken</param>
        public LogClient(string endpoint, string accessKeyId, string accessKey, string securityToken)
        {
            if (!endpoint.StartsWith("http://") && !endpoint.StartsWith("https://"))
                endpoint = "http://" + endpoint;
            setEndpoint(endpoint);
            if (IpUtil.IsIpAddress(this._hostName))
                throw new LogException("LogClientError", "client error happens");

            _localMachinePrivateIp = IpUtil.GetLocalMachinePrivateIp();
            AccessKeyId = accessKeyId;
            AccessKey = accessKey;
            _securityToken = securityToken;
            serviceClient = ServiceClient.Create(new ClientConfiguration());
            serviceClient.Configuration.ConnectionTimeout = LogConst.DEFAULT_SLS_CONNECT_TIMEOUT;
            serviceClient.Configuration.ReadWriteTimeout = LogConst.DEFAULT_SLS_READWRT_TIMEOUT;
        }

        public GetCursorResponse GetCursor(GetCursorRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                sReq.ResourcePath = LogConst.RESOURCE_LOGSTORES
                    + LogConst.RESOURCE_SEPARATOR
                    + request.Logstore
                    + LogConst.RESOURCE_SHARDS
                    + LogConst.RESOURCE_SEPARATOR
                    + request.Shard;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                request.AddSpecHeadersTo(sReq.Headers);
                request.AddSpecParamsTo(sReq.Parameters);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    JObject body = LogClientTools.ParserResponseToJObject(response.Content);
                    GetCursorResponse getCursorResp = new GetCursorResponse(response.Headers, body.GetValue("cursor").ToString());
                    return getCursorResp;
                }
            }
        }

        public ListShardsResponse ListShards(ListShardsRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                sReq.ResourcePath = LogConst.RESOURCE_LOGSTORES
                    + LogConst.RESOURCE_SEPARATOR
                    + request.Logstore
                    + LogConst.RESOURCE_SHARDS;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                request.AddSpecHeadersTo(sReq.Headers);
                request.AddSpecParamsTo(sReq.Parameters);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    //JArray body = LogClientTools.ParserResponseToJArray(response.Content);
                    JObject body = LogClientTools.ParserResponseToJObject(response.Content);
                    ListShardsResponse listShardsResp = new ListShardsResponse(response.Headers, body);
                    return listShardsResp;
                }
            }
        }
        public BatchGetLogsResponse BatchGetLogs(BatchGetLogsRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                sReq.ResourcePath = LogConst.RESOURCE_LOGSTORES
                    + LogConst.RESOURCE_SEPARATOR
                    + request.Logstore
                    + LogConst.RESOURCE_SHARDS
                    + LogConst.RESOURCE_SEPARATOR
                    + request.Shard;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                request.AddSpecHeadersTo(sReq.Headers);
                request.AddSpecParamsTo(sReq.Parameters);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    BatchGetLogsResponse batchGetLogsResp = new BatchGetLogsResponse(response.Headers, response.Content);
                    return batchGetLogsResp;
                }
            }
        }

        public ListLogstoresResponse ListLogstores(ListLogstoresRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);
                sReq.ResourcePath = LogConst.RESOURCE_LOGSTORES;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    JObject body = LogClientTools.ParserResponseToJObject(response.Content);
                    ListLogstoresResponse res = new ListLogstoresResponse(response.Headers, body);
                    return res;
                }
            }
        }

        /// <summary>
        /// put logs into sls server
        /// </summary>
        /// <param name="request">The request to put logs </param>
        /// <exception>LogException</exception>
        /// <returns>The response to put logs</returns>
        public PutLogsResponse PutLogs(PutLogsRequest request)
        {
            LogGroup lg = new LogGroup();

            if (request.IsSetTopic())
            {
                lg.Topic = request.Topic;
            }
            if (request.IsSetSource())
            {
                lg.Source = request.Source;
            }
            else
            {
                lg.Source = _localMachinePrivateIp;
            }

            if (request.IsSetLogItems())
            {
                foreach (var item in request.LogItems)
                {
                    Log log = new Log();
                    log.Time = item.Time;
                    foreach (var kv in item.Contents)
                    {
                        Log.Types.Content content = new Log.Types.Content
                        {
                            Key = kv.Key,
                            Value = kv.Value
                        };

                        log.Contents.Add(content);
                    }
                    lg.Logs.Add(log);
                }
            }

            return PutLogs(request,lg);
        }

        internal PutLogsResponse PutLogs(PutLogsRequest request, LogGroup logGroup)
        {
            if (logGroup.Logs.Count > LogConst.LIMIT_LOG_COUNT)
                throw new LogException("InvalidLogSize", "logItems' length exceeds maximum limitation： " + LogConst.LIMIT_LOG_COUNT + " lines.");
            else if (logGroup.CalculateSize() > LogConst.LIMIT_LOG_SIZE)
                throw new LogException("InvalidLogSize", "logItems' size exceeds maximum limitation: " + LogConst.LIMIT_LOG_SIZE + " byte.");
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Post;
                sReq.Endpoint = BuildReqEndpoint(request);

                //use empty string to replace Logstore if not set by user explicitly
                string logstore = request.IsSetLogstore() ? request.Logstore : string.Empty;
                sReq.ResourcePath = LogConst.RESOURCE_LOGSTORES + LogConst.RESOURCE_SEPARATOR + logstore;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);
                sReq.Headers.Add(LogConst.NAME_HEADER_CONTENTTYPE, LogConst.PBVALUE_HEADER_CONTENTTYPE);
                byte[] logBytes = logGroup.ToByteArray();
                sReq.Headers[LogConst.NAME_HEADER_BODYRAWSIZE] = logBytes.Length.ToString();
                sReq.Headers.Add(LogConst.NAME_HEADER_COMPRESSTYPE, LogConst.VALUE_HEADER_COMPRESSTYPE_LZ4);
                logBytes = LogClientTools.CompressToLz4(logBytes);
                sReq.Headers.Add(LogConst.NAME_HEADER_MD5, LogClientTools.GetMd5Value(logBytes));
                sReq.Content = new MemoryStream(logBytes);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Post);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    PutLogsResponse putLogResp = new PutLogsResponse(response.Headers);
                    return putLogResp;
                }
            }

        }

        /// <summary>
        /// Get the topics in the logtstore
        /// </summary>
        /// <param name="request">The list topics request</param>
        /// <exception>LogException</exception>
        /// <returns>The List topics response</returns>
        public ListTopicsResponse ListTopics(ListTopicsRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                //use empty string to replace Logstore if not set by user explicitly
                string logstore = request.IsSetLogstore() ? request.Logstore : string.Empty;
                sReq.ResourcePath = LogConst.RESOURCE_LOGSTORES + LogConst.RESOURCE_SEPARATOR + logstore;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                sReq.Parameters.Add(LogConst.PARAMETER_TYPE, LogConst.RESOURCE_TOPIC);

                if (request.IsSetToken())
                    sReq.Parameters.Add(LogConst.PARAMETER_TOKEN, request.Token);

                if (request.IsSetLines())
                    sReq.Parameters.Add(LogConst.PARAMETER_LINES, request.Lines.ToString());

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    //JArray body = LogClientTools.ParserResponseToJArray(response.Content);
                    JObject body = LogClientTools.ParserResponseToJObject(response.Content);
                    ListTopicsResponse res = new ListTopicsResponse(response.Headers, body);
                    return res;
                }
            }
        }

        /// <summary>
        ///  Get The sub set of logs data from sls server which match input
        ///  parameters. 
        /// </summary>
        /// <param name="request">The get logs request</param>
        /// <exception>LogException</exception>
        /// <returns>The get Logs response</returns>
        public GetLogsResponse GetLogs(GetLogsRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                //use empty string to replace Logstore if not set by user explicitly
                string logstore = request.IsSetLogstore() ? request.Logstore : string.Empty;
                sReq.ResourcePath = LogConst.RESOURCE_LOGSTORES + LogConst.RESOURCE_SEPARATOR + logstore;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                sReq.Parameters.Add(LogConst.PARAMETER_TYPE, LogConst.VALUE_TYPE_CONTENT);

                if (request.IsSetTopic())
                    sReq.Parameters.Add(LogConst.PARAMETER_TOPIC, request.Topic);

                if (request.IsSetFrom())
                    sReq.Parameters.Add(LogConst.PARAMETER_FROM, request.From.ToString());

                if (request.IsSetTo())
                    sReq.Parameters.Add(LogConst.PARAMETER_TO, request.To.ToString());

                if (request.IsSetQuery())
                    sReq.Parameters.Add(LogConst.PARAMETER_QUERY, request.Query);

                if (request.IsSetLines())
                    sReq.Parameters.Add(LogConst.PARAMETER_LINES, request.Lines.ToString());

                if (request.IsSetOffset())
                    sReq.Parameters.Add(LogConst.PARAMETER_OFFSET, request.Offset.ToString());

                if (request.IsSetReverse())
                    sReq.Parameters.Add(LogConst.PARAMETER_REVERSE, request.Reverse.ToString());

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    JArray body = LogClientTools.ParserResponseToJArray(response.Content);
                    //JObject body = LogClientTools.ParserResponseToJObject(response.Content);
                    GetLogsResponse res = new GetLogsResponse(response.Headers, body);
                    return res;
                }
            }
        }

        /// <summary>
        /// Get The log status(histogram info) from sls server which match input
        /// parameters. All the logs with logstore and topic in [from, to) which
        /// contain the keys in query are the matched data.
        /// </summary>
        /// <param name="request">The get histograms request</param>
        /// <exception>LogException</exception>
        /// <returns>The get histograms response</returns>
        public GetHistogramsResponse GetHistograms(GetHistogramsRequest request)
        {
            using (ServiceRequest sReq = new ServiceRequest())
            {
                sReq.Method = HttpMethod.Get;
                sReq.Endpoint = BuildReqEndpoint(request);

                //use empty string to replace Logstore if not set by user explicitly
                string logstore = request.IsSetLogstore() ? request.Logstore : string.Empty;
                sReq.ResourcePath = LogConst.RESOURCE_LOGSTORES + LogConst.RESOURCE_SEPARATOR + logstore;

                FillCommonHeaders(sReq);
                FillCommonParameters(sReq);

                sReq.Parameters.Add(LogConst.PARAMETER_TYPE, LogConst.VALUE_TYPE_STATUS);

                if (request.IsSetTopic())
                    sReq.Parameters.Add(LogConst.PARAMETER_TOPIC, request.Topic);

                if (request.IsSetFrom())
                    sReq.Parameters.Add(LogConst.PARAMETER_FROM, request.From.ToString());

                if (request.IsSetTo())
                    sReq.Parameters.Add(LogConst.PARAMETER_TO, request.To.ToString());

                if (request.IsSetQuery())
                    sReq.Parameters.Add(LogConst.PARAMETER_QUERY, request.Query);

                ExecutionContext context = new ExecutionContext();
                context.Signer = new LogRequestSigner(sReq.ResourcePath, HttpMethod.Get);
                context.Credentials = new ServiceCredentials(this.AccessKeyId, this.AccessKey);

                using (ServiceResponse response = serviceClient.Send(sReq, context))
                {
                    LogClientTools.ResponseErrorCheck(response, context.Credentials);
                    JArray body = LogClientTools.ParserResponseToJArray(response.Content);
                    //JObject body = LogClientTools.ParserResponseToJObject(response.Content);
                    GetHistogramsResponse res = new GetHistogramsResponse(response.Headers, body);
                    return res;
                }
            }
        }

        //used for unit testing
        internal void SetWebSend(ServiceClientImpl.WebSend send)
        {
            ((ServiceClientImpl)serviceClient).SendMethod = send;
        }

        private void setEndpoint(string endpoint)
        {
            try
            {
                Uri endpointUri = new Uri(endpoint);
                Endpoint = endpointUri.ToString();
                _hostName = endpointUri.Host;
                _uriScheme = endpointUri.Scheme;
                _port = endpointUri.Port;
            }
            catch (System.Exception)
            {
                throw new LogException("LogClientError", "client error happens");
            }
        }

        private void FillCommonHeaders(ServiceRequest sReq)
        {
            sReq.Headers.Add(LogConst.NAME_HEADER_DATE, TimeUtil.FormatRfc822Date(DateTime.Now));
            sReq.Headers.Add(LogConst.NAME_HEADER_APIVERSION, LogConst.VALUE_HEADER_APIVERSION);
            sReq.Headers.Add(LogConst.NAME_HEADER_BODYRAWSIZE, "0");
            sReq.Headers.Add(LogConst.NAME_HEADER_SIGMETHOD, LogConst.VALUE_HEADER_SIGMETHOD);
            if (_securityToken != null && _securityToken.Length != 0)
            {
                sReq.Headers.Add(LogConst.NAME_HEADER_ACS_SECURITY_TOKEN, _securityToken);
            }
        }

        private void FillCommonParameters(ServiceRequest sReq)
        {
            //TODO: add any additional parameters    
        }

        private Uri BuildReqEndpoint(LogRequest request)
        {
            //use empty string as project name if not set (expection will be thrown when do request)
            string project = request.IsSetProject() ? request.Project : string.Empty;
            return new Uri(this._uriScheme + "://" + project + "." + this._hostName + ":" + this._port);
        }
    }
}
