using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FunGames.Tools.Debugging
{
    public class FGDebugConsole : FGSingleton<FGDebugConsole>
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private ScrollRect _scrollRect;
        private string _lastMessage;

        private Dictionary<int, StringBuilder> LogsDictionary = new Dictionary<int, StringBuilder>();

        private StringBuilder currentLogBuilder;
        private int currentIndex = 0;

        [Range(1000, 16000)] public int MaxLogSize = 16000;

        public void Init()
        {
            Application.logMessageReceivedThreaded += OnLogReceived;
        }

        private void OnLogReceived(string message, string stacktrace, LogType type)
        {
            if (message == _lastMessage)
            {
                return;
            }

            if (type == LogType.Exception || type == LogType.Error)
            {
                AddMessage($"<color=red>{message}</color>\n");
            }
            else if (type == LogType.Warning)
            {
                AddMessage($"<color=yellow>{message}</color>\n");
            }
            else
            {
                AddMessage($"{message}\n");
            }

            _lastMessage = message;
        }

        public void Clear()
        {
            _text.text = string.Empty;
        }

        public void AddMessage(string text)
        {
            // if (currentLogBuilder == null) currentLogBuilder = new StringBuilder();
            //
            // if (currentLogBuilder.Length > MaxLogSize)
            // {
            //     LogsDictionary.Add(currentIndex++, currentLogBuilder);
            //     currentLogBuilder = new StringBuilder();
            //     Clear();
            // }

            if(_text!=null) _text.text += text;
           
            // currentLogBuilder.Append(text);
            //
            // _scrollRect.verticalNormalizedPosition = 0f;
        }

        public void OnDestroy () {
            // base.OnDestroy();
            Application.logMessageReceivedThreaded -= OnLogReceived;
        }
    }
}