using Assets.Scripts.Utilities.Localization;

namespace Assets.Scripts.Controllers.UI
{
    public class UISettingsController : UIBaseController
    {
        private string[] _langs = { "EN", "RU" };


        public void ChangeLanguage(int index)
        {
            LocalizationController.ChangeLanguage(_langs[index]);
        }

        public void ReturnToPause(UIPauseController uiPause)
        {
            uiPause.Enable();
            Disable();
        }
    }
}
