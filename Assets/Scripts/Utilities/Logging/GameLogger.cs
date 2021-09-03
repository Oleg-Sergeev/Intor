using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Controllers.UI;
using Assets.Scripts.Extensions;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utilities.Logging
{
    public class GameLogger : Logger
    {
        private const string TimeFormat = "HH:mm:ss:fff";

        private const int MaxContentLength = 45;

        private static GameLogger Instance;


        private static readonly Dictionary<LogType, Color> _colors = new Dictionary<LogType, Color>
        {
            { LogType.Assert, Color.green },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red }
        };

        private static readonly Queue<LogMessage> _messages = new Queue<LogMessage>();

        private static readonly Queue<CancellationTokenSource> _tokens = new Queue<CancellationTokenSource>();

        private static readonly List<LogText> _texts = new List<LogText>();


        private static int _messagesCount;

        private static bool _isAutodelete;


        [SerializeField]
        private int _messageLifeTime = 10;

        [SerializeField]
        private LogText _extraLogText;

        [SerializeField]
        private UIBaseController _extraLogUi;

        [SerializeField]
        private Transform _logTextsParent;



        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.LogWarning($"Removed duplicate {nameof(GameLogger)} ({name})");
                DestroyImmediate(gameObject);
                return;
            }

            Application.logMessageReceivedThreaded += OnLogMessageReceived;
        }

        private void Start()
        {
            foreach (var uiText in _logTextsParent.GetChilds<TextMeshProUGUI>())
            {
                var logText = new LogText(uiText);
                _texts.Add(logText);
                uiText.GetComponent<Button>().onClick.AddListener(() => ShowExtraInfo(logText));
            }

            _messagesCount = _texts.Count;

            _isAutodelete = true;

            UpdateTexts();
        }

        private void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= OnLogMessageReceived;

            _messages.Clear();
            foreach (var tokenSrc in _tokens) tokenSrc.Cancel();
            _tokens.Clear();
            _texts.Clear();
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

                    DeleteMessage(_messageLifeTime, tokenSrc.Token);
                }
            }
            else
            {
                foreach (var token in _tokens) token.Cancel();
            }
        }


        protected override void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            if (type == LogType.Exception) Log(new LogMessage($"**EXCEPTION** {condition} {stacktrace}", type));
            else Log(new LogMessage(condition, type));
        }

        protected override void Log(LogMessage message)
        {
            if (ReorderLogType(message.LogType) < ReorderLogType(LogType)) return;

            if (_messages.Count == _messagesCount)
            {
                _messages.Dequeue();

                _tokens.Dequeue().Cancel();
            }

            _messages.Enqueue(message);

            var tokenSrc = new CancellationTokenSource();
            _tokens.Enqueue(tokenSrc);


            UpdateTexts();

            if (_isAutodelete)
            {
                DeleteMessage(_messageLifeTime, tokenSrc.Token);
            }
        }


        private void ShowExtraInfo(LogText text)
        {
            if (text == null || text.LogMessage == _extraLogText.LogMessage)
            {
                _extraLogUi.Disable();

                _extraLogText.ResetUiText();

                return;
            }

            _extraLogText.SetUiText(text.LogMessage, true);

            _extraLogUi.Enable();
        }

        private void UpdateTexts()
        {
            if (_messages.Count == 0)
            {
                var uiText = _texts[0].UiText;

                uiText.raycastTarget = false;
                uiText.color = new Color(0.6f, 0.6f, 0.6f);
                uiText.text = "Debug";

                _extraLogUi.Disable();

                return;
            }


            var messagesArr = _messages.ToArray();

            var isExtraMessageExists = false;


            for (int i = 0; i < messagesArr.Length; i++)
            {
                _texts[i].SetUiText(messagesArr[i]);

                if (_extraLogUi.IsCanvasEnabled && !isExtraMessageExists && _texts[i].LogMessage == _extraLogText.LogMessage)
                    isExtraMessageExists = true;
            }

            for (int i = messagesArr.Length; i < _messagesCount; i++)
            {
                _texts[i].ResetUiText();
            }


            if (!isExtraMessageExists) _extraLogUi.Disable();
        }

        private async void DeleteMessage(int seconds, CancellationToken token = default)
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


        [Serializable]
        private class LogText
        {
            [field: SerializeField]
            public TextMeshProUGUI UiText { get; private set; }

            public LogMessage LogMessage { get; private set; }


            public LogText() {}
            public LogText(TextMeshProUGUI uiText)
            {
                UiText = uiText;
            }


            public void SetUiText(LogMessage message, bool extraContent = false)
            {
                LogMessage = message;

                if (extraContent) UiText.text = $"[{message.Time.ToString(TimeFormat)}] {message.Content}";
                else  UiText.text = message.Content.Length <= MaxContentLength
                        ? message.Content
                        : message.Content.Substring(0, MaxContentLength);
                UiText.color = _colors[message.LogType];
                UiText.raycastTarget = true;
            }

            public void ResetUiText()
            {
                UiText.text = string.Empty;
                UiText.raycastTarget = false;
                LogMessage = default;
            }
        }
    }
}
