using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.UI
{
    public class UIElevatorController : UIBaseController
    {
        [field: SerializeField]
        public Button ButtonUp { get; private set; }

        [field: SerializeField]
        public Button ButtonDown { get; private set; }

        [SerializeField]
        private GameObject _notWorking;


        public void ToggleUp()
        {
            ButtonUp.interactable = true;
            ButtonDown.interactable = false;
        }

        public void ToggleDown()
        {
            ButtonUp.interactable = false;
            ButtonDown.interactable = true;
        }

        public void EnableButtons()
        {
            ButtonUp.interactable = true;
            ButtonDown.interactable = true;
        }

        public void DisableButtons()
        {
            ButtonUp.interactable = false;
            ButtonDown.interactable = false;
        }


        public void SetWorking(bool working)
        {
            DisableButtons();

            ButtonUp.gameObject.SetActive(working);
            ButtonDown.gameObject.SetActive(working);

            _notWorking.SetActive(!working);
        }
    }
}
