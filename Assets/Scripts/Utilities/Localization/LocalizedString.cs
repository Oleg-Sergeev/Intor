using TMPro;
using UnityEngine;

namespace Assets.Scripts.Utilities.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedString : MonoBehaviour, ILocalizableText
    {
        [field: SerializeField]
        public string Key { get; private set; }

        public string Value
        {
            get => _text.text;
            set => _text.text = value;
        }

        private TextMeshProUGUI _text;


        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }


        public void Localize(string localizedString)
        {
            Value = localizedString;
        }
    }
}