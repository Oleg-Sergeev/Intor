using Assets.Scripts.PropertyAttributes;
using Assets.Scripts.Utilities.Localization;
using UnityEngine;


namespace Assets.Scripts.Data.Items
{
    [CreateAssetMenu(fileName = "New item", menuName = "Items/Item", order = 0)]
    public class Item : ScriptableObject, ILocalizableText
    {
        private const char TextSeparator = ';';

        [field: SerializeField]
        public string Key { get; private set; }


        [field: SerializeField]
        [field: BeginReadOnlyGroup, AutoGenerateId, EndReadOnlyGroup]
        public string Id { get; private set; }

        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; private set; }

        [field: SerializeField]
        public Sprite Icon { get; private set; }




        public void Localize(string localizedText)
        {
            var splitted = localizedText.Split(TextSeparator);

            var (name, description) = (splitted[0], splitted[1]);

            Name = name;
            Description = description;
        }
    }
}
