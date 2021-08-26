using UnityEngine;

namespace Assets.Scripts.Utilities.Logging
{
    public abstract class Logger : MonoBehaviour
    {
        [field: SerializeField]
        protected LogType LogType { get; private set; } = LogType.Log;


        public void OnDropdownLogType(int type)
        {
            LogType = ReorderLogType((LogType)type);
        }

        protected LogType ReorderLogType(LogType type)
        {
            switch (type)
            {
                case LogType.Log: return 0;
                case LogType.Assert: return (LogType)1;
                case LogType.Warning: return (LogType)2;
                case LogType.Error: return (LogType)3;
                case LogType.Exception: return (LogType)4;
                default: return 0;
            }
        }


        protected abstract void OnLogMessageReceived(string condition, string stacktrace, LogType type);

        protected abstract void Log(LogMessage message);
    }
}
