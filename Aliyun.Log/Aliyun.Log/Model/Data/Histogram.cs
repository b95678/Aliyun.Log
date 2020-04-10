using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Data
{
    /// <summary>
    /// The log status(histogram info)
    /// </summary>
    public class Histogram
    {
        private UInt32 _from;
        private UInt32 _to;
        private long _count;
        private string _progress;

        /// <summary>
        /// The begin timestamp of time range
        /// </summary>
        public UInt32 From
        {
            get { return _from; }
            set { _from = value; }
        }

        /// <summary>
        /// The end timestamp of time range
        /// </summary>
        public UInt32 To
        {
            get { return _to; }
            set { _to = value; }
        }

        /// <summary>
        /// The log count
        /// </summary>
        public long Count
        {
            get { return _count; }
            set { _count = value; }
        }

        /// <summary>
        /// detect whether histogram is complete or not.
        /// </summary>
        /// <returns>true if return histogram is complete. otherwise return false</returns>
        public bool IsCompleted()
        {
            return _progress == LogConst.STATUS_COMPLETE;
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public Histogram()
        {

        }

        internal static List<Histogram> DeserializeFromJson(JArray json)
        {
            List<Histogram> hlst = new List<Histogram>();
            if (json != null)
            {
                for (int i = 0; i < json.Count; ++i)
                {
                    Histogram htg = new Histogram();
                    htg._from = (UInt32)json[i][LogConst.NAME_GETSTATUS_FROM];
                    htg._to = (UInt32)json[i][LogConst.NAME_GETSTATUS_TO];
                    htg._count = (UInt32)json[i][LogConst.NAME_GETSTATUS_COUNT];
                    htg._progress = (string)json[i][LogConst.NAME_GETSTATUS_PROGRESS];
                    hlst.Add(htg);
                }
            }
            return hlst;
        }
    }
}
