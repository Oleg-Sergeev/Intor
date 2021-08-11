using UnityEngine;

namespace Assets.Scripts.Data.Items
{
    [CreateAssetMenu(fileName = "New key", menuName = "Items/Key")]
    public class Key : Item
    {
        [field: SerializeField]
        public bool IsReusable { get; private set; }
    }
}
