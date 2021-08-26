using UnityEngine;

namespace Assets.Scripts.Controllers.UI
{
    public class UIBaseController : MonoBehaviour
    {
        public bool IsCanvasEnabled { get; protected set; }

        protected Canvas Canvas { get; set; }


        protected virtual void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            Canvas = GetComponent<Canvas>();

            IsCanvasEnabled = Canvas.enabled;
        }

        [ContextMenu("Toggle")]
        public virtual void Toggle()
        {
            IsCanvasEnabled = !IsCanvasEnabled;

            Canvas.enabled = IsCanvasEnabled;
        }

        public void Enable()
        {
            IsCanvasEnabled = true;

            Canvas.enabled = true;
        }

        public void Disable()
        {
            IsCanvasEnabled = false;

            Canvas.enabled = false;
        }
    }
}