using System;
using Assets.Scripts.Extensions;
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


        public void ResetButtons()
        {
            ButtonOpen.Reset();
            ButtonClose.Reset();
            ButtonLock.Reset();
        }
    }
}
