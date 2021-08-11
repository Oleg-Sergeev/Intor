using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class ItemUI
    {
        [field: SerializeField]
        public Button PickUp { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI Name { get; private set; }

        //[field: SerializeField]
        //public TextMeshProUGUI Amount { get; private set; }
    }
}
