using System.Collections.Generic;
using System.IO;

namespace PODTool.POD
{
    public class ResponseFile
    {
        public string PODFilename = "noname.pod";
        public string VolumeName = string.Empty;
        public readonly List<string> FileList = new List<string>();

        private void ParseResponseFile(string responseFilePath)
        {
            FileList.Clear();

            string[] lines = File.ReadAllLines(responseFilePath);
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("//") || string.IsNullOrEmpty(trimmed))
                    continue;

                if (trimmed.StartsWith("podFilename:"))
                {
                    PODFilename = trimmed.Substring(13);
                }
                else if (trimmed.StartsWith("volumeName:"))
                {
                    VolumeName = trimmed.Substring(12);
                }
                else
                {
                    FileList.Add(trimmed);
                }
            }
        }

        public ResponseFile(string filepath)
        {
            ParseResponseFile(filepath);
        }
    }
}
