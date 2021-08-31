﻿using Assets.Scripts.Utilities.Localization;
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


        public void Continue()
        {
            Toggle();
        }

        public void ReturnToLastCheckpoint()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OpenSettings()
        {
            Debug.Log("OpenSettings");
            LocalizationController.ChangeLanguage("Ru");
        }

        public void ExitToMainMenu()
        {
            Debug.Log("ExitToMainMenu");
            LocalizationController.ChangeLanguage("en");
        }
    }
}