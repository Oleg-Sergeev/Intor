using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Controllers.UI;
using Assets.Scripts.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utilities
{
    public class GameLogger : MonoBehaviour
    {
        private const int MaxContentLength = 45;

        private static GameLogger _instance;

        private static readonly Dictionary<LogType, Color> Colors = new Dictionary<LogType, Color>
        {
            { LogType.Assert, Color.green },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red }
        };

        private static readonly Queue<MessageInfo> _messages = new Queue<MessageInfo>();

        private static readonly Queue<CancellationTokenSource> _tokens = new Queue<CancellationTokenSource>();

        private static readonly List<UITextInfo> _texts = new List<UITextInfo>();

        private static int _maxCount;

        private static int _currentExtraInfoIndex;

        private static bool _isAutodelete;


        [SerializeField]
        private int _messageLifeTime = 10;

        [SerializeField]
        private LogType _logType = LogType.Log;

        [SerializeField]
        private TextMeshProUGUI _extraLogText;

        [SerializeField]
        private UIBaseController _extraLogUi;




        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(gameObject);

            Application.logMessageReceivedThreaded += OnLogMessageReceived;
        }

        private void Start()
        {
            int i = 0;
            foreach (var uiText in transform.GetChilds<TextMeshProUGUI>())
            {
                var t = i++;
                _texts.Add(new UITextInfo(uiText));
                uiText.GetComponent<Button>().onClick.AddListener(() => ShowExtraInfo(t));
            }

            _maxCount = _texts.Count;

            _currentExtraInfoIndex = -1;

            _isAutodelete = true;

            UpdateTexts();
        }


        public void OnClearAll()
        {
            var count = _messages.Count;

            foreach (var token in _tokens) token.Cancel();
            for (int i = 0; i < count; i++) DeleteMessage(0);
        }

        public void OnToggleAutodelete(bool isAutodelete)
        {
            _isAutodelete = isAutodelete;

            if (_isAutodelete)
            {
                _tokens.Clear();

                for (int i = 0; i < _messages.Count; i++)
                {
                    var tokenSrc = new CancellationTokenSource();
                    _tokens.Enqueue(tokenSrc);

                    DeleteMessage(_instance._messageLifeTime, tokenSrc.Token);
                }
            }
            else
            {
                foreach (var token in _tokens) token.Cancel();
            }
        }

        public void OnDropdownLogType(int type)
        {
            _logType = ReorderLogType((LogType)type);
        }


        private void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (type == LogType.Exception) LogMessage(new MessageInfo($"**EXCEPTION** {condition} {stacktrace}", type));
            else LogMessage(new MessageInfo(condition, type));
        }

        private void ShowExtraInfo(int index)
        {
            _extraLogText.text = $"[{_texts[index].MessageInfo.Time}] {_texts[index].MessageInfo.Content}";
            _extraLogText.color = Colors[_texts[index].MessageInfo.LogType];

            if (_currentExtraInfoIndex == index)
            {
                _extraLogUi.Toggle();

                return;
            }

            _currentExtraInfoIndex = index;

            if (!_instance._extraLogUi.IsCanvasEnabled) _instance._extraLogUi.Toggle();
        }


        private static void LogMessage(MessageInfo messageInfo)
        {
            if (ReorderLogType(messageInfo.LogType) < ReorderLogType(_instance._logType)) return;

            if (_messages.Count == _maxCount)
            {
                _messages.Dequeue();

                _tokens.Dequeue().Cancel();
            }

            _messages.Enqueue(messageInfo);

            var tokenSrc = new CancellationTokenSource();
            _tokens.Enqueue(tokenSrc);


            UpdateTexts();

            if (_isAutodelete)
            {
                DeleteMessage(_instance._messageLifeTime, tokenSrc.Token);
            }
        }

        private static void UpdateTexts()
        {
            if (_messages.Count == 0)
            {
               var uiText = _texts[0].UiText;

                uiText.raycastTarget = false;
                uiText.color = new Color(0.6f, 0.6f, 0.6f);
                uiText.text = "Debug";

                return;
            }

            var messagesArr = _messages.ToArray();

            for (int i = 0; i < messagesArr.Length; i++)
            {
                var uiText = _texts[i].UiText;
                var message = messagesArr[i];

                _texts[i].MessageInfo = message;

                uiText.raycastTarget = true;
                uiText.color = Colors[message.LogType];

                if (message.Content.Length <= MaxContentLength)
                {
                    uiText.text = message.Content;
                }
                else
                {
                    uiText.text = message.Content.Substring(0, MaxContentLength);
                }
            }

            for (int i = messagesArr.Length; i < _maxCount; i++)
            {
                var text = _texts[i].UiText;

                text.text = "";
                text.raycastTarget = false;

                if (i == _currentExtraInfoIndex)
                {
                    if (_instance._extraLogUi.IsCanvasEnabled)
                    {
                        _instance.ShowExtraInfo(i);
                    }
                    _currentExtraInfoIndex = -1;
                }
            }
        }

        private static async void DeleteMessage(int seconds, CancellationToken token = default)
        {
            try
            {
                await Task.Delay(seconds * 1000, token);
            }
            catch (TaskCanceledException)
            {
                return;
            }

            if (token.IsCancellationRequested || !Application.isPlaying) return;

            _messages.Dequeue();

            _tokens.Dequeue();

            UpdateTexts();
        }


        private static LogType ReorderLogType(LogType type)
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




        private struct MessageInfo
        {
            public string Time { get; }

            public string Content { get; }

            public LogType LogType { get; }


            public MessageInfo(string content, LogType logType)
            {
                Content = content;
                LogType = logType;
                Time = DateTime.Now.ToLongTimeString();
            }
        }


        private class UITextInfo
        {
            public TextMeshProUGUI UiText { get; }

            public MessageInfo MessageInfo { get; set; }


            public UITextInfo(TextMeshProUGUI uiText)
            {
                UiText = uiText;
            }
        }
    }
}
