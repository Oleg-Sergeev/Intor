using System;
using System.Linq;
using System.Threading;
using Assets.Scripts.Extensions;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class TransformTranslatorComponent : MonoBehaviour, IMoveable, IRotatable
    {
        protected Vector3 StartPosition { get; private set; }

        protected Quaternion StartRotation { get; private set; }

        protected event Action Callback;

        private CancellationTokenSource _cts;


        [field: SerializeField]
        protected Vector3 ToPosition { get; set; }

        [field: SerializeField]
        protected Vector3 ToRotation { get; set; }

        [SerializeField]
        private SerializableEvent _serializedCallback;

        [field: SerializeField]
        protected float Speed { get; set; }

        [field: SerializeField]
        protected bool IsLocalPosition { get; private set; }

        [field: SerializeField]
        protected bool IsRelativePosition { get; private set; }

        [field: SerializeField]
        protected bool IsRelativeRotation { get; private set; }


        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            _cts.Cancel();
        }

        private void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            StartPosition = IsLocalPosition ? transform.localPosition : transform.position;

            StartRotation = IsLocalPosition ? transform.localRotation : transform.rotation;

            if (IsRelativePosition) ToPosition += StartPosition;
            if (IsRelativeRotation) ToRotation += StartRotation.eulerAngles;


            if (!string.IsNullOrEmpty(_serializedCallback.methodName))
            {
                Callback = (Action)_serializedCallback.target
                    .GetType()
                    .GetMethods()
                        .Where(x => x.Name == _serializedCallback.methodName)
                        .First(x => x.GetParameters().Length == 0)
                    .CreateDelegate(typeof(Action), _serializedCallback.target);
            }
        }


        public void MoveToStart() => Move(StartPosition);
        public void MoveToDestination() => Move(ToPosition);
        public void ResetPosition() => Move(StartPosition, Speed, null, IsLocalPosition);
        public virtual void Move(Vector3 direction) => Move(direction, Speed, Callback, IsLocalPosition);
        private void Move(Vector3 direction, float speed, Action callback = default, bool isLocalPosition = true)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            transform.TranslateTo(direction, speed, callback, isLocalPosition, _cts.Token);
        }

        public void RotateToStart() => Rotate(StartRotation);
        public void RotateToDestination() => Rotate(Quaternion.Euler(ToRotation));
        public void ResetRotation() => Rotate(StartRotation, Speed, null, IsLocalPosition);
        public virtual void Rotate(Quaternion rotation) => Rotate(rotation, Speed, Callback, IsLocalPosition);
        private void Rotate(Quaternion rotation, float speed, Action callback = default, bool isLocalPosition = true)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            transform.TranslateTo(rotation, speed, callback, isLocalPosition, _cts.Token);
        }
    }
}