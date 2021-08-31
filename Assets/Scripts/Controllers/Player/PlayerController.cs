using Assets.Scripts.Data;
using Assets.Scripts.Data.SaveData;
using Assets.Scripts.Utilities.Saving;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerController : MonoBehaviour, ISaveable
    {
        public int Id => gameObject.GetInstanceID();


        [field: SerializeField]
        public string Name { get; private set; }

        public Inventory Inventory { get; private set; }


        private void Awake()
        {
            Inventory = new Inventory();
        }


        public void SetItemData(ItemData itemData)
        {
            var playerData = (PlayerData)itemData;

            Inventory = playerData.Inventory;

            transform.position = playerData.Position;

            transform.rotation = Quaternion.Euler(playerData.Rotation);
        }

        public ItemData GetItemData() => new PlayerData(Id)
        {
            Inventory = Inventory,
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles
        };
    }
}