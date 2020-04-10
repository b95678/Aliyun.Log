using Aliyun.Log.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Aliyun.Log.Model.Response
{
    public class BatchGetLogsResponse : LogResponse
    {
        private string _nextCursor;
        private int _logCount;
        private int _rawSize;
        private LogGroupList _logGroupList;

        public BatchGetLogsResponse(IDictionary<string, string> headers, Stream body)
            : base(headers)
        {
            headers.TryGetValue(LogConst.NAME_HEADER_NEXT_CURSOR, out _nextCursor);
            string tmpLogCount, tmpRawSize, tmpContentLength;
            if (headers.TryGetValue(LogConst.NAME_HEADER_LOG_COUNT, out tmpLogCount))
            {
                int.TryParse(tmpLogCount, out _logCount);
            }
            if (headers.TryGetValue(LogConst.NAME_HEADER_LOG_BODY_RAW_SIZE, out tmpRawSize))
            {
                int.TryParse(tmpRawSize, out _rawSize);
            }
            int contentLength = 0;
            if (headers.TryGetValue("Content-Length", out tmpContentLength))
            {
                int.TryParse(tmpContentLength, out contentLength);
            }
            _logGroupList = LogGroupList.Parser.ParseFrom(LogClientTools.DecompressFromLZ4(body, _rawSize));
        }

        public string NextCursor
        {
            get { return _nextCursor; }
        }
        public int LogCount
        {
            get { return _logCount; }
        }
        public int RawSize
        {
            get { return _rawSize; }
        }
        public LogGroupList LogGroupList
        {
            get { return _logGroupList; }
        }
    }
}
