namespace PODTool
{
    static class StringExtensions
    {
        public static string TrimNullTerminated(this string s)
        {
            int nullIndex = s.IndexOf('\x00');
            if (nullIndex < 0)
                return s;
            return s.Substring(0, nullIndex);
        }
    }
}
