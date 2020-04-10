using System;
using System.Collections.Generic;
using System.Text;

namespace Aliyun.Log.Model.Data
{
    /// <summary>
    /// This class presents one log content in logItem
    /// </summary>
    public class LogContent
    {
        private string _key = string.Empty;
        private string _value = string.Empty;

        /// <summary>
        /// default constructor
        /// </summary>
        public LogContent()
        {

        }

        /// <summary>
        /// constructure with specified parameters
        /// </summary>
        /// <param name="key">log content's key </param>
        /// <param name="value">log content's value </param>
        public LogContent(string key, string value)
        {
            _key = key;
            _value = value;
        }

        /// <summary>
        /// the logcontent's key
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// the logcontent's value
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
