using System;
using UnityEngine;

namespace Assets.Scripts.Utilities.Logging
{
    public struct LogMessage
    {
        public DateTime Time { get; }

        public string Content { get; }

        public LogType LogType { get; }


        public LogMessage(string content, LogType logType)
        {
            Content = content;
            LogType = logType;
            Time = DateTime.Now;
        }


        public static bool operator ==(LogMessage m1, LogMessage m2) => m1.Equals(m2);
        public static bool operator !=(LogMessage m1, LogMessage m2) => !m1.Equals(m2);


        public override bool Equals(object obj)
        {
            if (obj is LogMessage message)  
                return Time == message.Time 
                && Content == message.Content 
                && LogType == message.LogType;

            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
