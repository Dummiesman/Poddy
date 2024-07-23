using System;

namespace PODTool
{
    public static class TimeExtensions
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        }

        public static long ToUnixTime(DateTime time)
        {
            return ((DateTimeOffset)time).ToUnixTimeSeconds();
        }
    }
}
