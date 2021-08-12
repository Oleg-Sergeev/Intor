using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class ItemInfoComponent : MonoBehaviour
    {
        [field: SerializeField]
        public TextMeshProUGUI Name { get; private set; }

        [field: SerializeField]
        public TextMeshProUGUI Description { get; private set; }
    }
}
