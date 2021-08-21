using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Extensions;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public class GameDebugger : MonoBehaviour
    {
        private const int MessageLifeTime = 10;


        private static readonly Queue<MessageInfo> _messages = new Queue<MessageInfo>();

        private static readonly Queue<CancellationTokenSource> _tokens = new Queue<CancellationTokenSource>();

        private static readonly List<TextMeshProUGUI> _texts = new List<TextMeshProUGUI>();

        private static int _maxCount;


        private void Start()
        {
            _texts.AddRange(transform.GetChilds<TextMeshProUGUI>());

            _maxCount = _texts.Count;
        }


        public static void Log(object message) => Log(message.ToString());
        public static void Log(string message)
        {
            Debug.Log(message);

            LogMessage(new MessageInfo(message, Color.white));
        }

        public static void LogWarning(object message) => LogWarning(message.ToString());
        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);

            LogMessage(new MessageInfo(message, Color.yellow));
        }

        public static void LogError(object message) => LogError(message.ToString());
        public static void LogError(string message)
        {
            Debug.LogError(message);

            LogMessage(new MessageInfo(message, Color.red));
        }


        private static async void LogMessage(MessageInfo messageInfo)
        {
            if (_messages.Count == _maxCount)
            {
                _messages.Dequeue();

                _tokens.Dequeue().Cancel();
            }

            _messages.Enqueue(messageInfo);

            var tokenSrc = new CancellationTokenSource();
            _tokens.Enqueue(tokenSrc);

            UpdateTexts();

            await DeleteMessageAsync(MessageLifeTime, tokenSrc.Token);
        }

        private static void UpdateTexts()
        {
            var messagesArr = _messages.ToArray();

            for (int i = messagesArr.Length - 1, j = 0; i >= 0; i--, j++)
            {
                _texts[i].color = messagesArr[j].Color;
                _texts[i].text = messagesArr[j].Content;
            }

            for (int i = messagesArr.Length; i < _maxCount; i++)
                _texts[i].text = "";
        }


        private static async Task DeleteMessageAsync(int seconds, CancellationToken token)
        {
            await Task.Delay(seconds * 1000);

            if (token.IsCancellationRequested) return;

            _messages.Dequeue();

            _tokens.Dequeue();

            UpdateTexts();
        }


        private struct MessageInfo
        {
            public string Content { get; }

            public Color Color { get; }


            public MessageInfo(string content, Color color)
            {
                Content = content;
                Color = color;
            }
        }
    }
}
