using UnityEngine;

namespace Assets.Scripts.Controllers.UI
{
    public class UIBaseController : MonoBehaviour
    {
        protected bool IsCanvasEnabled { get; set; }

        protected Canvas Canvas { get; set; }


        protected virtual void Start()
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
    }
}