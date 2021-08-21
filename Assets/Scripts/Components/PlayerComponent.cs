using Assets.Scripts.Data;
using Assets.Scripts.Utilities;
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
    }
}
