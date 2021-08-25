using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Components.Triggers
{
    public class TriggerComponent : Trigger
    {
        [SerializeField]
        private TriggerType _triggerType;

        [SerializeField]
        private ColliderCondition _colliderType;


        [SerializeField]
        private Collider _targetCollider;

        [SerializeField]
        private List<Collider> _targetColliders;

        private List<Collider> _enteredColliders;


        [SerializeField]
        private UnityEvent _onTriggered;

        [SerializeField]
        private UnityEvent _onTriggerEnter;

        [SerializeField]
        private UnityEvent _onTriggerExit;


        private void Start()
        {
            _enteredColliders = new List<Collider>();
        }


        public override void Call()
        {
            _onTriggered?.Invoke();
        }


        private void OnTriggerEnter(Collider collider)
        {
            if (_colliderType == ColliderCondition.CollidersListAll) _enteredColliders.Add(collider);

            if (CheckColliderBasedOnSetup(collider)) _onTriggerEnter?.Invoke();
        }

        private void OnTriggerExit(Collider collider)
        {
            if (_colliderType == ColliderCondition.CollidersListAll) _enteredColliders.Remove(collider);

            if (CheckColliderBasedOnSetup(collider)) _onTriggerExit?.Invoke();
        }


        private bool CheckColliderBasedOnSetup(Collider collider)
        {
            switch (_colliderType)
            {
                case ColliderCondition.AnyCollider: return true;
                case ColliderCondition.OneCollider: return _targetCollider == collider;
                case ColliderCondition.CollidersListAny: return _targetColliders.Contains(collider);
                case ColliderCondition.CollidersListAll: return !_targetColliders.Except(_enteredColliders).Any();
                default: throw new ArgumentException($"Unknown ColliderType enum value \"{_colliderType}\"", nameof(_colliderType));
            }
        }


        public enum TriggerType
        {
            OnTrigger,
            ExternalTrigger
        }

        public enum ColliderCondition
        {
            AnyCollider,
            OneCollider,
            CollidersListAny,
            CollidersListAll
        }
    }
}
