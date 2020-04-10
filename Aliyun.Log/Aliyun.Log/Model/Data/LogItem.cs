using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Data
{
    /// <summary>
    /// This class presents one log event item that will put into SLS
    /// </summary>
    public class LogItem
    {
        private UInt32 _time;
        private List<LogContent> _contents;

        /// <summary>
        /// the log's timestamp
        /// </summary>
        public UInt32 Time
        {
            get { return _time; }
            set { _time = value; }
        }

        /// <summary>
        /// logcontents in logs 
        /// </summary>
        public List<LogContent> Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public LogItem()
        {
            _contents = new List<LogContent>();
        }

        /// <summary>
        /// method to append log content by key/value pair
        /// </summary>
        /// <param name="key">user define field to name the value</param>
        /// <param name="value">the value of field key</param>
        public void PushBack(string key, string value)
        {
            _contents.Add(new LogContent(key, value));
        }

        /// <summary>
        /// method to append log content by key/value pair
        /// </summary>
        /// <param name="content">log content</param>
        public void PushBack(LogContent content)
        {
            _contents.Add(content);
        }
    }
}
