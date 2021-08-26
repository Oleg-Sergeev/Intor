using System.Collections;
using Assets.Scripts.Utilities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controllers.UI
{
    public class UIDebugController : UIBaseController
    {
        private const int StartFps = 60;
        private const int MinFps = 30;
        private const int MaxFps = 120;

        [SerializeField]
        private TextMeshProUGUI _textFps;

        [SerializeField]
        private TextMeshProUGUI _textFpsLimit;

        [SerializeField]
        private FpsCounter _counter;

        private Coroutine _coroutineFps;


        protected override void Init()
        {
            base.Init();

            Application.targetFrameRate = StartFps;
        }


        public override void Toggle()
        {
            base.Toggle();

            if (IsCanvasEnabled) _coroutineFps = StartCoroutine(ShowFps());
            else if (_coroutineFps != null) StopCoroutine(_coroutineFps);
        }


        public void ChangeFpsLimit()
        {
            Application.targetFrameRate = MinFps + (Application.targetFrameRate % MaxFps);

            _textFpsLimit.text = $"{Application.targetFrameRate} Fps";
        }


        public void DebugAction()
        {
        }


        private IEnumerator ShowFps()
        {
            var delay = new WaitForSecondsRealtime(0.2f);

            while (IsCanvasEnabled)
            {
                yield return delay;

                var fps = _counter.AverageFPS;

                _textFps.text = $"{fps} fps";
            }
        }
    }
}
