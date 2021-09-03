using System;
using System.Linq;
using System.Threading;
using Assets.Scripts.Extensions;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Components
{
    public class TransformTranslatorComponent : MonoBehaviour, IMoveable, IRotatable
    {
        protected Vector3 CurrentPosition => IsLocalPosition ? transform.localPosition : transform.position;

        protected Quaternion CurrentRotation => IsLocalRotation ? transform.localRotation : transform.rotation;


        protected Vector3 StartPosition { get; private set; }

        protected Quaternion StartRotation { get; private set; }

        protected event Action Callback;
        protected event Action CallbackOnDestination;
        protected event Action CallbackOnStart;

        private CancellationTokenSource _cts;


        [field: SerializeField]
        protected Vector3 ToPosition { get; set; }

        [field: SerializeField]
        protected Vector3 ToRotation { get; set; }


        [SerializeField]
        private SerializableEvent _callback;

        [SerializeField]
        private SerializableEvent _callbackOnDestination;

        [SerializeField]
        private SerializableEvent _callbackOnStart;


        [field: SerializeField]
        protected float MoveSpeed { get; set; }

        [field: SerializeField]
        protected float RotateSpeed { get; set; }

        [field: SerializeField]
        protected bool IsLocalPosition { get; private set; }

        [field: SerializeField]
        protected bool IsLocalRotation { get; private set; }

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


            if (!string.IsNullOrEmpty(_callback.methodName)) 
                Callback = CreateAction(_callback);

            if (!string.IsNullOrEmpty(_callbackOnDestination.methodName)) 
                CallbackOnDestination = CreateAction(_callbackOnDestination);

            if (!string.IsNullOrEmpty(_callbackOnStart.methodName)) 
                CallbackOnStart = CreateAction(_callbackOnStart);
        }


        public void MoveToStart() => Move(StartPosition, Callback + CallbackOnStart);
        public void MoveToDestination() => Move(ToPosition, Callback + CallbackOnDestination);
        public void ResetPosition() => Move(StartPosition, null, null);
        public virtual void Move(Vector3 direction, Action callback = default, float? speed = null) => Move(direction, speed ?? MoveSpeed, callback, IsLocalPosition);
        private void Move(Vector3 direction, float speed, Action callback = default, bool isLocalPosition = true)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            transform.TranslateTo(direction, speed, callback, isLocalPosition, _cts.Token);
        }

        public void RotateToStart() => Rotate(StartRotation, Callback + CallbackOnStart);
        public void RotateToDestination() => Rotate(Quaternion.Euler(ToRotation), Callback + CallbackOnDestination);
        public void ResetRotation() => Rotate(StartRotation, null, null);
        public virtual void Rotate(Quaternion rotation, Action callback = default, float? speed = null) => Rotate(rotation, speed ?? RotateSpeed, callback, IsLocalRotation);
        private void Rotate(Quaternion rotation, float speed, Action callback = default, bool isLocalRotation = true)
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            transform.TranslateTo(rotation, speed, callback, isLocalRotation, _cts.Token);
        }


        private Action CreateAction(SerializableEvent serializableEvent) =>
            (Action)serializableEvent.target
            .GetType()
            .GetMethods()
                .Where(x => x.Name == serializableEvent.methodName)
                .First(x => x.GetParameters().Length == 0)
            .CreateDelegate(typeof(Action), serializableEvent.target);
    }
}