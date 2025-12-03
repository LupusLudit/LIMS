using System.Diagnostics;
using System.IO;

namespace LIMS.Debugging
{
    public enum LogType
    {
        Info,
        Warning,
        Error
    }

    /// <include file='../Docs/LIMSClassesDocs.xml' path='ClassDocs/ClassMembers[@name="Logger"]/*'/>
    public static class Logger
    {

        private static readonly object lockObject = new object();
        private static string logDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        static Logger()
        {
            if (!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }
        }

        private static string GetTodaysFilePath()
        {
            string path = Path.Combine(logDirectoryPath, $"{DateTime.Now.ToString("yyyy-MM-dd")}.log");
            return path;
        }

        private static void Write(LogType type, string message)
        {
            string line = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] <{type}>: {message}";
            string filePath = GetTodaysFilePath();

            lock (lockObject)
            {
                using (StreamWriter logWriter = new StreamWriter(filePath, true))
                {
                    logWriter.WriteLine(line);
                }
            }
        }

        public static void Info(string message) => Write(LogType.Info, message);
        public static void Warning(string message) => Write(LogType.Warning, message);
        public static void Error(string message) => Write(LogType.Error, message);
    }
}
