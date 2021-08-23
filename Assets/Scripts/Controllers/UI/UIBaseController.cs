using UnityEngine;

namespace Assets.Scripts.Controllers.UI
{
    public class UIBaseController : MonoBehaviour
    {
        public bool IsCanvasEnabled { get; protected set; }

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