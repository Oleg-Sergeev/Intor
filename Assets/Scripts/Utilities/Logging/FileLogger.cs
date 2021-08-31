using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utilities.Logging
{
    public class FileLogger : Logger
    {
        private static FileLogger Instance;

        private const string LogFormat = "{0:dd.MM.yyyy HH:mm:ss:fff} [{1}] {2}\r";
        private const string DateFormat = "dd.MM.yyyy";
        private const string TimeFormat = "HH-mm-ss";

        private static string RootFolderPath;
        private static string WorkFolderPath;
        private static string FilePath;


        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.LogWarning($"Removed duplicate {nameof(FileLogger)} ({name})");
                Destroy(gameObject);
                return;
            }

            RootFolderPath = $"{Application.persistentDataPath}/Logs";
            WorkFolderPath = $"{RootFolderPath}/{DateTime.Now.ToString(DateFormat)}";
            FilePath = $"{WorkFolderPath}/{DateTime.Now.ToString(TimeFormat)}.log";

            Application.logMessageReceivedThreaded += OnLogMessageReceived;
        }

        private void Start()
        {
            EnsureRootFolderCreated();
            EnsureWorkFolderCreated();
        }

        private void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= OnLogMessageReceived;
        }


        protected override void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (type != LogType.Exception) Log(new LogMessage(condition, type));
            else Log(new LogMessage($"{condition}{Environment.NewLine}{stacktrace}", type));
        }

        protected override async void Log(LogMessage logMessage)
        {
            if (ReorderLogType(logMessage.LogType) < ReorderLogType(LogType)) return;

            await LogAsync(logMessage);
        }

        private async Task LogAsync(LogMessage message)
        {
            var text = string.Format(LogFormat, message.Time, message.LogType, message.Content);

            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            using (var sourceStream = new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            }

        }


        private void EnsureRootFolderCreated()
        {
            if (!Directory.Exists(RootFolderPath)) Directory.CreateDirectory(RootFolderPath);
        }
        private void EnsureWorkFolderCreated()
        {
            if (!Directory.Exists(WorkFolderPath)) Directory.CreateDirectory(WorkFolderPath);
        }


        public void Open()
        {
            Application.OpenURL($"jar:file:///{WorkFolderPath}");
        }
    }
}