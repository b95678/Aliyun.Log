using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Aliyun.Log.Util
{
    public class TimeUtil
    {
        private static DateTime _1970StartDateTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0),TimeZoneInfo.Utc, TimeZoneInfo.Local);
        private const string _rfc822DateFormat = "ddd, dd MMM yyyy HH:mm:ss \\G\\M\\T";
        private const string _iso8601DateFormat = "yyyy-MM-dd'T'HH:mm:ss.fff'Z'";

        /// <summary>
        /// Formats an instance of <see cref="DateTime" /> to a GMT string.
        /// </summary>
        /// <param name="dt">The date time to format.</param>
        /// <returns></returns>
        public static string FormatRfc822Date(DateTime dt)
        {
            return dt.ToUniversalTime().ToString(_rfc822DateFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats a GMT date string to an object of <see cref="DateTime" />.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ParseRfc822Date(string dt)
        {
            Debug.Assert(!string.IsNullOrEmpty(dt));
            return DateTime.SpecifyKind(
                DateTime.ParseExact(dt, _rfc822DateFormat, CultureInfo.InvariantCulture), DateTimeKind.Utc);
        }

        /// <summary>
        /// Formats a date to a string in the format of ISO 8601 spec.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string FormatIso8601Date(DateTime dt)
        {
            return dt.ToUniversalTime().ToString(_iso8601DateFormat, CultureInfo.CreateSpecificCulture("en-US"));
        }

        /// <summary>
        /// convert time stamp to DateTime.
        /// </summary>
        /// <param name="timeStamp">seconds</param>
        /// <returns></returns>
        public static DateTime GetDateTime(uint timeStamp)
        {
            DateTime dtStart = _1970StartDateTime;
            long lTime = ((long)timeStamp * System.TimeSpan.TicksPerSecond);
            System.TimeSpan toNow = new System.TimeSpan(lTime);
            DateTime targetDt = dtStart.Add(toNow);
            return targetDt;
        }

        public static uint TimeSpan()
        {
            return (uint)Math.Round((DateTime.Now - _1970StartDateTime).TotalSeconds, MidpointRounding.AwayFromZero);
        }
    }
}
