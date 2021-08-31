using Assets.Scripts.Data.SaveData;
using Assets.Scripts.Utilities.Saving;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class LeverComponent : TransformTranslatorComponent, ISaveable
    {
        public int Id => gameObject.GetInstanceID();

        public void SetItemData(ItemData itemData)
        {
            var leverData = (LeverData)itemData;


            if (IsLocalPosition) transform.localPosition = leverData.Position;
            else transform.position = leverData.Position;

            if (IsLocalRotation) transform.localRotation = Quaternion.Euler(leverData.Rotation);
            else transform.rotation = Quaternion.Euler(leverData.Rotation);
        }

        public ItemData GetItemData() => new LeverData(Id)
        {
            Position = CurrentPosition,
            Rotation = CurrentRotation.eulerAngles
        };
    }
}
