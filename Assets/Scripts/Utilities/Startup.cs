using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Controllers.UI;
using UnityEngine;

namespace Assets.Scripts.Utilities
{
    [DefaultExecutionOrder(100)]
    public class Startup : MonoBehaviour
    {
        [SerializeField]
        private UIBaseController _activeUi;


        private void Start()
        {
            Time.timeScale = 1f;

            TogglePanels();
        }


        private void TogglePanels()
        {
            var uiPanels = FindObjectsOfType<UIBaseController>().Except(new List<UIBaseController>() { _activeUi });

            foreach (var panel in uiPanels)
                if (panel.IsCanvasEnabled)
                    panel.Toggle();

            if (!_activeUi.IsCanvasEnabled) _activeUi.Toggle();
        }
    }
}
