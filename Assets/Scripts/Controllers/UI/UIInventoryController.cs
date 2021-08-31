using System.Collections.Generic;
using Assets.Scripts.Components;
using Assets.Scripts.Controllers.Player;
using Assets.Scripts.Data;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Controllers.UI
{
    public class UIInventoryController : UIBaseController
    {
        [SerializeField]
        private PlayerController _player;

        [SerializeField]
        private ItemInfoComponent _itemInfo;

        [SerializeField]
        private Transform _slotsParent;

        private SlotComponent _selectedSlot;

        private IList<SlotComponent> _slots;


        protected override void Awake()
        {
            base.Awake();

            _player.Inventory.SlotAdded += OnSlotAnyChange;
            _player.Inventory.SlotUpdated += OnSlotAnyChange;
            _player.Inventory.SlotRemoved += OnSlotAnyChange;
        }

        private void OnDestroy()
        {
            _player.Inventory.SlotAdded -= OnSlotAnyChange;
            _player.Inventory.SlotUpdated -= OnSlotAnyChange;
            _player.Inventory.SlotRemoved -= OnSlotAnyChange;
        }


        protected override void Init()
        {
            base.Init();

            _slots = _slotsParent.GetChilds<SlotComponent>();
        }


        public override void Toggle()
        {
            base.Toggle();

            if (!IsCanvasEnabled && _selectedSlot != null) DeselectSlot();
        }


        public void ShowItemInfo(SlotComponent slot)
        {
            if (_selectedSlot == slot)
            {
                DeselectSlot();
                return;
            }

            if (_selectedSlot != null) DeselectSlot();

            SelectSlot(slot);
        }

        private void SelectSlot(SlotComponent slot)
        {
            SetItemInfo(slot.Item);

            _selectedSlot = slot;

            slot.SetSelected(true);
        }
        private void DeselectSlot()
        {
            SetItemInfo(null);

            _selectedSlot.SetSelected(false);
            _selectedSlot = null;
        }
        private void SetItemInfo(Item item)
        {
            _itemInfo.Name.text = item?.Name;
            _itemInfo.Description.text = item?.Description;
        }


        private void OnSlotAnyChange(Slot updatedSlot)
        {
            var inventorySlots = _player.Inventory.GetSlots();


            for (int i = 0; i < inventorySlots.Count; i++)
            {
                var inventorySlot = inventorySlots[i];
                var uiSlot = _slots[i];

                uiSlot.Set(inventorySlot, ShowItemInfo);
            }

            for (int i = inventorySlots.Count; i < _slots.Count; i++)
            {
                var uiSlot = _slots[i];

                uiSlot.Clear();
            }
        }
    }
}
