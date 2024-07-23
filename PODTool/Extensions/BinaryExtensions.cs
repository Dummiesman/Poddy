using System.IO;
using System.Text;

namespace PODTool
{
    static class BinaryExtensions
    {
        public static string ReadNullTerminatedString(this BinaryReader reader)
        {
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                char c = reader.ReadChar();
                if (c == '\x00')
                    break;
                builder.Append(c);
            }
            return builder.ToString();
        }

        public static string ReadFixedSizeCharArray(this BinaryReader reader, int size)
        {
            return new string(reader.ReadChars(size)).TrimNullTerminated();
        }

        public static void WriteFixedSizeCharArray(this BinaryWriter writer, string str, int len)
        {
            for (int i = 0; i < len; i++)
            {
                char c = (i >= str.Length) ? '\x00' : str[i];
                writer.Write(c);
            }
        }
    }
}
