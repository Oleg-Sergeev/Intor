using System.Collections;
using Assets.Scripts.Utilities;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controllers.UI
{
    public class UIDebugController : UIBaseController
    {
        [SerializeField]
        private TextMeshProUGUI _textFps;

        [SerializeField]
        private FpsCounter _counter;

        private Coroutine _coroutineFps;


        public override void Toggle()
        {
            base.Toggle();

            if (IsCanvasEnabled) _coroutineFps = StartCoroutine(ShowFps());
            else if (_coroutineFps != null) StopCoroutine(_coroutineFps);
        }


        int i = 0;
        public void DebugAction()
        {
            Debug.Log($"{i++} Very long string {new string('x', Random.Range(1, 150))} {new string('y', Random.Range(1, 150))}");

            int a = 0;
            int b = 1 / a;
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
