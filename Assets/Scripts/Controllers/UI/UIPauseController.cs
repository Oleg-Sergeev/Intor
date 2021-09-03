using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controllers.UI
{
    public class UIPauseController : UIBaseController
    {
        public override void Toggle()
        {
            base.Toggle();

            if (IsCanvasEnabled) Time.timeScale = 0;
            else Time.timeScale = 1;
        }


        public void Continue(UIGameplayController uiGameplay)
        {
            Toggle();
            uiGameplay.Toggle();
        }

        public void ReturnToLastCheckpoint()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OpenSettings(UISettingsController uiSettings)
        {
            uiSettings.Toggle();
            Disable();
        }

        public void ExitToMainMenu()
        {
            Debug.Log("ExitToMainMenu");
        }
    }
}