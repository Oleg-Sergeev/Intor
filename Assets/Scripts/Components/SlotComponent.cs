using System;
using Assets.Scripts.Data;
using Assets.Scripts.Data.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    public class SlotComponent : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Image _selectedSlot;

        [SerializeField]
        private TextMeshProUGUI _amount;


        public Item Item { get; private set; }


        public void SetSelected(bool selected)
        {
            _selectedSlot.enabled = selected;
        }

        public void Set(Slot slot, Action<SlotComponent> slotSelectedAction)
        {
            if (slot == null) return;

            Item = slot.Item;

            _amount.text = slot.Amount > 1 ? slot.Amount.ToString() : "";

            _icon.enabled = true;
            //_icon = slot.Item.Icon;

            _button.interactable = true;
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => slotSelectedAction?.Invoke(this));
        }

        public void Clear()
        {
            SetSelected(false);

            Item = null;

            _amount.text = "";

            _icon.enabled = false;

            _button.interactable = false;
        }
    }
}
