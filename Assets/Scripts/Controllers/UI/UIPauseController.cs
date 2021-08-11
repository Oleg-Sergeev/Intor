using UnityEngine;

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


        public void ReturnToLastCheckpoint()
        {
            Debug.Log("ReturnToLastCheckpoint");
        }

        public void OpenSettings()
        {
            Debug.Log("OpenSettings");
        }

        public void ExitToMainMenu()
        {
            Debug.Log("ExitToMainMenu");
        }
    }
}