using System.Collections;
using TMPro;
using UnityEngine;

public class DebugUiController : MonoBehaviour
{
    private bool _isEnabled;

    private Canvas _canvas;

    [SerializeField]
    private TextMeshProUGUI _textFps;

    private Coroutine _coroutineFps;


    private void Start()
    {
        _canvas = GetComponent<Canvas>();

        _isEnabled = _canvas.enabled;
    }

    public void ToggleDebugMenu()
    {
        _isEnabled = !_isEnabled;

        _canvas.enabled = _isEnabled;

        if (_isEnabled) _coroutineFps = StartCoroutine(CountFps());
        else StopCoroutine(_coroutineFps);
    }


    private IEnumerator CountFps()
    {
        var delay = new WaitForSeconds(0.5f);

        while (_isEnabled)
        {
            yield return delay;

            var fps = 1f / Time.unscaledDeltaTime;

            _textFps.text = $"{fps:f1} fps";
        }
    }
}
