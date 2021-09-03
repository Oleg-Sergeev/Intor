using System.Linq;
using Assets.Scripts.Data;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.SaveData;
using Assets.Scripts.PropertyAttributes;
using Assets.Scripts.Utilities.Saving;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerController : MonoBehaviour, ISaveable
    {
        [field: SerializeField]
        [field: BeginReadOnlyGroup, AutoGenerateId, EndReadOnlyGroup]
        public string Id { get; private set; }


        [field: SerializeField]
        public string Name { get; private set; }

        public Inventory Inventory { get; private set; }


        private void Awake()
        {
            Inventory = new Inventory();
        }


        public void SetItemData(ItemData itemData)
        {
            var items = Resources.LoadAll<Item>("Items").ToDictionary(i => i.Id, i => i);

            var playerData = (PlayerData)itemData;

            foreach (var slot in playerData.Slots)
                Inventory.Add(items[slot.Key], slot.Value);


            transform.position = playerData.Position;

            transform.rotation = Quaternion.Euler(playerData.Rotation);
        }

        public ItemData GetItemData() => new PlayerData(Id)
        {
            Slots = Inventory.Slots.ToDictionary(s => s.Item.Id, s => s.Amount),
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles
        };
    }
}