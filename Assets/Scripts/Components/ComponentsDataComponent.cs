using System;
using System.Collections.Generic;
using Assets.Scripts.Data.SaveData;
using Assets.Scripts.PropertyAttributes;
using Assets.Scripts.Utilities.Saving;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class ComponentsDataComponent : MonoBehaviour, ISaveable
    {
        [field: SerializeField]
        [field: BeginReadOnlyGroup, AutoGenerateId, EndReadOnlyGroup]
        public string Id { get; private set; }

        private List<Type> _componentTypesToAdd = new List<Type>();
        private List<Type> _componentTypesToRemove = new List<Type>();


        public void AddComponent<T>() where T : Component
        {
            _componentTypesToAdd.Add(gameObject.AddComponent<T>().GetType());
        }

        public void RemoveComponent<T>() where T : Component
        {
            var component = gameObject.GetComponent<T>();
            _componentTypesToRemove.Add(component.GetType());

            Destroy(component);
        }


        public void SetItemData(ItemData itemData)
        {
            var componentsData = (ComponentsData)itemData;

            _componentTypesToAdd = componentsData.ComponentsToAdd;
            _componentTypesToRemove = componentsData.ComponentsToRemove;

            transform.position = componentsData.Position;
            transform.rotation = Quaternion.Euler(componentsData.Rotation);

            foreach (var type in _componentTypesToAdd) gameObject.AddComponent(type);
            foreach (var type in _componentTypesToRemove) Destroy(gameObject.GetComponent(type));
        }

        public ItemData GetItemData() => new ComponentsData(Id)
        {
            ComponentsToAdd = _componentTypesToAdd,
            ComponentsToRemove = _componentTypesToRemove,
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles
        };
    }
}