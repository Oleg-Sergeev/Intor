using UnityEngine;

namespace Assets.Scripts.Data.Items
{
    [CreateAssetMenu(fileName = "New item", menuName = "Items/Default item", order = 0)]
    public class Item : ScriptableObject
    {
        [field: SerializeField]
        public ulong Id { get; private set; }

        [field: SerializeField]
        public string Name { get; private set; }


        public string Description { get; private set; }
    }
}
