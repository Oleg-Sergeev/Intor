using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [DefaultExecutionOrder(-10)]
    public class PlayerComponent : MonoBehaviour
    {
        [field: SerializeField]
        public string Name { get; private set; }

        public Inventory Inventory { get; private set; }


        private void Awake()
        {
            Inventory = new Inventory();
        }

        public void Clear()
        {
            var t1 = Inventory.GetItems();
            var t = t1?.Count > 0 ? t1[0] : null;
            Inventory.Remove(t);
        }
    }
}
