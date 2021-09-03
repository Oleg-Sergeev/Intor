using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Data.Items;
using Newtonsoft.Json;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class Inventory
    {
        public event Action<Slot> SlotAdded;
        public event Action<Slot> SlotUpdated;
        public event Action<Slot> SlotRemoved;


        public const int TotalSize = 15;

        [JsonIgnore]
        public int CurrentSize => _slots.Count;

        public IReadOnlyList<Slot> Slots => _slots.Values.ToList();

        [JsonIgnore]
        public IReadOnlyList<Item> Items => _slots.Values.Select(s => s.Item)?.ToList();

        private readonly Dictionary<string, Slot> _slots;


        public Inventory()
        {
            _slots = new Dictionary<string, Slot>();
        }

        [JsonConstructor]
        private Inventory(IReadOnlyList<Slot> slots)
        {
            _slots = slots.ToDictionary(s => s.Item.Id, s => s);
        }


        public bool HasEmptySpace(Item sameItem = null) => CurrentSize < TotalSize || (sameItem != null && _slots.ContainsKey(sameItem.Id));


        public bool HasItem(Item templateItem) => templateItem != null && _slots.ContainsKey(templateItem.Id);



        public void Add(Item item, int amount = 1)
        {
            if (item == null || !HasEmptySpace(item)) return;


            if (_slots.TryGetValue(item.Id, out var slot))
            {
                slot.Amount++;

                SlotUpdated?.Invoke(slot);
            }
            else
            {
                _slots.Add(item.Id, new Slot(item, amount));

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
