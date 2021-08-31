using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Controllers.UI;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Utilities.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Utilities
{
    [DefaultExecutionOrder(100)]
    public class Startup : MonoBehaviour
    {
        [SerializeField]
        private UIBaseController _activeUi;

        [SerializeField]
        private UnityEvent _onGameInitialized;


        private void Start()
        {
            Time.timeScale = 1f;

            TogglePanels();

            _onGameInitialized?.Invoke();
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
