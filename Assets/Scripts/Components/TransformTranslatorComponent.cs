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
        private Vector3 _startPosition;

        private Quaternion _startRotation;

        private Action _callback;

        private CancellationTokenSource _cts;


        [SerializeField]
        private Vector3 _toPosition;

        [SerializeField]
        private Vector3 _toRotation;

        [SerializeField]
        private SerializableEvent _serializedCallback;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private bool _isLocalPosition;


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
            _startPosition = _isLocalPosition ? transform.localPosition : transform.position;

            _startRotation = _isLocalPosition ? transform.localRotation : transform.rotation;


            if (!string.IsNullOrEmpty(_serializedCallback.methodName))
            {
                _callback = (Action)_serializedCallback.target
                    .GetType()
                    .GetMethods()
                        .Where(x => x.Name == _serializedCallback.methodName)
                        .First(x => x.GetParameters().Length == 0)
                    .CreateDelegate(typeof(Action), this);
            }
        }


        public void MoveToStart() => Move(_startPosition);
        public void MoveToDestination() => Move(_toPosition);
        public void Move(Vector3 direction)
        {
            transform.TranslateTo(direction, _speed, _callback, _isLocalPosition, _cts.Token);
        }

        public void RotateToStart() => Rotate(_startRotation);
        public void RotateToDestination() => Rotate(Quaternion.Euler(_toRotation));
        public void Rotate(Quaternion rotation)
        {
            transform.TranslateTo(rotation, _speed, _callback, _isLocalPosition, _cts.Token);
        }
    }
}