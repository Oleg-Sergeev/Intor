using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data.Items;

namespace Assets.Scripts.Data
{
    public class Inventory
    {
        public event Action<Slot> SlotAdded;
        public event Action<Slot> SlotUpdated;
        public event Action<Slot> SlotRemoved;


        public const int TotalSize = 15;


        public int CurrentSize => _slots.Count;


        private readonly Dictionary<string, Slot> _slots;


        public Inventory()
        {
            _slots = new Dictionary<string, Slot>();
        }


        public IList<Slot> GetSlots() => _slots.Values.ToList();

        public IList<Item> GetItems() => _slots.Values.Select(s => s.Item)?.ToList();


        public bool HasEmptySpace(Item sameItem = null) => CurrentSize < TotalSize || (sameItem != null && _slots.ContainsKey(sameItem.Id));


        public bool HasItem(Item templateItem) => templateItem != null && _slots.ContainsKey(templateItem.Id);



        public void Add(Item item)
        {
            if (item == null || !HasEmptySpace(item)) return;


            if (_slots.TryGetValue(item.Id, out var slot))
            {
                slot.Amount++;

                SlotUpdated?.Invoke(slot);
            }
            else
            {
                _slots.Add(item.Id, new Slot(item));

                SlotAdded?.Invoke(slot);
            }
        }

        public void Remove(Item item)
        {
            if (item == null || !_slots.TryGetValue(item.Id, out var slot)) return;


            slot.Amount--;

            if (slot.Amount <= 0)
            {
                _slots.Remove(item.Id);

                SlotRemoved?.Invoke(slot);
            }
            else SlotUpdated?.Invoke(slot);
        }
    }
}
