using System;
using Assets.Scripts.Extensions;
using Assets.Scripts.Utilities.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Data.UI
{
    [Serializable]
    public class DoorUI
    {
        [field: SerializeField]
        public Canvas Canvas { get; private set; }

        [field: SerializeField]
        public Button ButtonOpen { get; private set; }

        [field: SerializeField]
        public Button ButtonClose { get; private set; }

        [field: SerializeField]
        public Button ButtonLock { get; private set; }

        [field: SerializeField]
        public Button ButtonHack { get; private set; }


        public void ResetButtons()
        {
            ButtonOpen.Reset();
            ButtonClose.Reset();
            ButtonLock.Reset();
            ButtonHack.Reset();
        }

        public void HideLockAndHackButtons()
        {
            ButtonLock.gameObject.SetActive(false);
            ButtonHack.gameObject.SetActive(false);
        }
    }
}
