using System;
using System.Collections.Generic;
using Assets.Scripts.Data.Items;

namespace Assets.Scripts.Data
{
    public class Inventory
    {
        public event Action<Slot> SlotAdded;
        public event Action<Slot> SlotUpdated;
        public event Action<Slot> SlotRemoved;


        public const int TotalSize = 15;


        public int CurrentSize => _items.Count;


        private readonly Dictionary<ulong, Slot> _items;


        public Inventory()
        {
            _items = new Dictionary<ulong, Slot>();
        }



        public bool HasEmptySpace(Item sameItem = null) => CurrentSize < TotalSize || (sameItem != null && _items.ContainsKey(sameItem.Id));


        public bool HasItem(Item templateItem) => _items.ContainsKey(templateItem.Id);



        public void Add(Item item)
        {
            if (item == null || !HasEmptySpace(item)) return;


            if (_items.TryGetValue(item.Id, out var slot))
            {
                slot.Amount++;

                SlotUpdated?.Invoke(slot);
            }
            else
            {
                _items.Add(item.Id, new Slot(item));

                SlotAdded?.Invoke(slot);
            }
        }

        public void Remove(Item item)
        {
            if (item == null || !_items.TryGetValue(item.Id, out var slot)) return;


            slot.Amount--;

            if (slot.Amount <= 0)
            {
                _items.Remove(item.Id);

                SlotRemoved?.Invoke(slot);
            }
            else SlotUpdated?.Invoke(slot);
        }
    }
}
