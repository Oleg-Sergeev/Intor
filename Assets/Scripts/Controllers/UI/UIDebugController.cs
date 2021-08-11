using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controllers.UI
{
    public class UIDebugController : UIBaseController
    {
        [SerializeField]
        private TextMeshProUGUI _textFps;

        private Coroutine _coroutineFps;


        public override void Toggle()   
        {
            base.Toggle();

            if (IsCanvasEnabled) _coroutineFps = StartCoroutine(CountFps());
            else StopCoroutine(_coroutineFps);
        }


        private IEnumerator CountFps()
        {
            var delay = new WaitForSecondsRealtime(0.5f);

            while (IsCanvasEnabled)
            {
                yield return delay;

                var fps = 1f / Time.unscaledDeltaTime;

                _textFps.text = $"{fps:f1} fps";
            }
        }
    }
}
