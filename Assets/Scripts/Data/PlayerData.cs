using UnityEngine;

namespace Assets.Scripts.Data
{
    [DefaultExecutionOrder(-10)]
    public class PlayerData : MonoBehaviour
    {
        [field: SerializeField]
        public string Name { get; private set; }

        public Inventory Inventory { get; private set; }


        private void Awake()
        {
            Inventory = new Inventory();
        }
    }
}
