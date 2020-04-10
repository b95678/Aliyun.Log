using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model
{
    public class LogConst
    {
        public const string CONST_USER_AGENT_PREFIX = "log-dotnet-sdk-v-";

        public const int LIMIT_LOG_SIZE = 3 * 1024 * 1024;
        public const int LIMIT_LOG_COUNT = 4 * 1024;
        public const int DEFAULT_SLS_RETRY_TIME = 3;
        public const int DEFAULT_SLS_CONNECT_TIMEOUT = 5 * 1000;
        public const int DEFAULT_SLS_READWRT_TIMEOUT = 20 * 1000;
        public const int DEFAULT_SLS_RETRY_INTERVALBASE = 100;

        public const string NAME_ERROR_CODE = "error_code";
        public const string NAME_ERROR_MESSAGE = "error_message";
        public const string NAME_LISTLOGSTORE_ITEM = "logstores";
        public const string NAME_LISTLOGSTORE_TOTAL = "count";
        public const string NAME_LISTTOPIC_COUNT = "count";
        public const string NAME_LISTTOPIC_TOPICS = "topics";
        public const string NAME_LISTTOPIC_NEXTTOKEN = "next_token";
        public const string NAME_GETSTATUS_PROGRESS = "progress";
        public const string NAME_GETSTATUS_COUNT = "count";
        public const string NAME_GETSTATUS_FROM = "from";
        public const string NAME_GETSTATUS_TO = "to";
        public const string NAME_GETSTATUS_HISTOGRAM = "histograms";
        public const string NAME_GETDATA_COUNT = "count";
        public const string NAME_GETDATA_PROGRESS = "progress";
        public const string NAME_GETDATA_LOGS = "logs";
        public const string NAME_GETDATA_TIME = "__time__";
        public const string NAME_GETDATA_SOURCE = "__source__";

        public const string NAME_MD5 = "MD5";

        public const string NAME_HTTP_GET = "GET";
        public const string NAME_HTTP_POST = "POST";
        public const string NAME_HTTP_PUT = "PUT";
        public const string NAME_HTTP_DELETE = "DELETE";
        public const string NAME_HTTP_PATCH = "PATCH";
        public const string NAME_HTTP_HEAD = "HEAD";
        public const string NAME_HTTP_OPTIONS = "OPTIONS";

        public const string NAME_HEADER_AUTH = HttpHeaders.Authorization;
        public const string PREFIX_VALUE_HEADER_AUTH = "LOG" + " ";
        public const string NAME_HEADER_CONTENTTYPE = HttpHeaders.ContentType;
        public const string JSONVALUE_HEADER_CONTENTTYPE = "application/json";
        public const string PBVALUE_HEADER_CONTENTTYPE = "application/x-protobuf";
        public const string NAME_HEADER_MD5 = HttpHeaders.ContentMd5;
        public const string NAME_HEADER_HOST = "Host";
        public const string NAME_HEADER_APIVERSION = "x-log-apiversion";
        public const string VALUE_HEADER_APIVERSION = "0.6.0";
        public const string NAME_HEADER_ACCESSKEYID = "x-log-accesskeyid";
        public const string NAME_HEADER_COMPRESSTYPE = "x-log-compresstype";
        public const string NAME_HEADER_REQUESTID = "x-log-requestid";
        public const string NAME_HEADER_DATE = "x-log-date";
        public const string NAME_HEADER_X_LOG_COUNT = "x-log-count";
        public const string NAME_HEADER_X_LOG_NEXT_TOKEN = "x-log-nexttoken";
        public const string NAME_HEADER_X_LOG_PROGRESS = "x-log-progress";
        public const string NAME_HEADER_ACCEPT_ENCODING = "Accept-Encoding";
        public const string NAME_HEADER_ACCEPT = "Accept";
        public const string VALUE_HEADER_COMPRESSTYPE_DEFLATE = "deflate";
        public const string VALUE_HEADER_COMPRESSTYPE_LZ4 = "lz4";
        public const string NAME_HEADER_BODYRAWSIZE = "x-log-bodyrawsize";
        public const string NAME_HEADER_NEXT_CURSOR = "x-log-cursor";
        public const string NAME_HEADER_LOG_COUNT = "x-log-count";
        public const string NAME_HEADER_LOG_BODY_RAW_SIZE = "x-log-bodyrawsize";
        public const string NAME_HEADER_SIGMETHOD = "x-log-signaturemethod";
        public const string VALUE_HEADER_SIGMETHOD = "hmac-sha1";
        public const string NAME_HEADER_ACS_SECURITY_TOKEN = "x-acs-security-token";
        public const string RESOURCE_SEPARATOR = "/";
        public const string RESOURCE_LOGSTORES = RESOURCE_SEPARATOR + "logstores";
        public const string RESOURCE_SHARDS = RESOURCE_SEPARATOR + "shards";
        public const string PARAMETER_OFFSET = "offset";
        public const string PARAMETER_LINES = "line";
        public const string RESOURCE_TOPIC = "topic";
        public const string PARAMETER_TOKEN = "token";
        public const string PARAMETER_TYPE = "type";
        public const string VALUE_TYPE_CONTENT = "log";
        public const string VALUE_TYPE_STATUS = "histogram";
        public const string PARAMETER_TOPIC = "topic";
        public const string PARAMETER_FROM = "from";
        public const string PARAMETER_TO = "to";
        public const string PARAMETER_QUERY = "query";
        public const string PARAMETER_REVERSE = "reverse";

        public const string STATUS_COMPLETE = "Complete";
        public const string STATUS_INCOMPLETE = "InComplete";
    }
}
