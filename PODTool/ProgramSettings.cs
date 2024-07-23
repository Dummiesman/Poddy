using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PODTool
{
    /// <summary>
    /// Lazy settings class
    /// </summary>
    class ProgramSettings
    {
        public static string AuditLogUsername = "user";
        public static PODVersion DefaultPODVersion = PODVersion.POD2;
        public static bool EnableAuditLogWarning = true;
        public static readonly List<string> RecentFiles = new List<string>();

        const string SETTINGS_SAVE_PATH = "settings_v2.txt";
        const int MAX_RECENT_FILES = 10;

        public static void AddRecentFile(string file)
        {
            // if it's already in the list, remove it. we'll bump it to the end.
            for(int i=RecentFiles.Count - 1; i >= 0; i--)
            {
                string recentFile = RecentFiles[i];
                if (recentFile.ToLowerInvariant() == file.ToLowerInvariant())
                {
                    RecentFiles.RemoveAt(i);
                }
            }

            RecentFiles.Add(file);
            while (RecentFiles.Count > MAX_RECENT_FILES)
                RecentFiles.RemoveAt(0);
            Save();
        }

        private static void Load()
        {
            if (!File.Exists(SETTINGS_SAVE_PATH))
                return;

            string[] settings = File.ReadAllLines(SETTINGS_SAVE_PATH);
            string currentSection = null;

            foreach(string line in settings)
            {
                if (string.IsNullOrWhiteSpace(line)) 
                    continue;
                if(line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line.Substring(1, line.Length - 2);
                    continue;
                }

                switch (currentSection)
                {
                    case "audit_log_username":
                        AuditLogUsername = line;
                        break;
                    case "default_pod_version":
                        DefaultPODVersion = (PODVersion)Enum.Parse(typeof(PODVersion), line, true);
                        break;
                    case "enable_audit_warning":
                        bool.TryParse(line, out EnableAuditLogWarning);
                        break;
                    case "recent_files":
                        RecentFiles.Add(line);
                        break;
                }
            }
        }

        public static void Init()
        {
            if (File.Exists(SETTINGS_SAVE_PATH))
            {
                Load();
            }
            else
            {
                AuditLogUsername = Environment.UserName;
                Save();
            }
        }

        public static void Save()
        {
            var builder = new StringBuilder();

            builder.AppendLine("[audit_log_username]");
            builder.AppendLine(AuditLogUsername);
            builder.AppendLine("[default_pod_version]");
            builder.AppendLine(DefaultPODVersion.ToString());
            builder.AppendLine("[enable_audit_warning]");
            builder.AppendLine(EnableAuditLogWarning.ToString());

            builder.AppendLine("[recent_files]");
            foreach (var file in RecentFiles)
                builder.AppendLine(file);

            File.WriteAllText(SETTINGS_SAVE_PATH, builder.ToString());
        }
    }
}
