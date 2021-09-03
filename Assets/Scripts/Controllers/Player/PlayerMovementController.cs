using System;
using Assets.Scripts.Data;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerMovementController : MonoBehaviour, IMoveable, IRotatable
    {
        [SerializeField]
        private MoveSettings _moveSettings;

        private Rigidbody _rigidbody;

        private Transform _cameraTransform;


        private Vector3 _rawMovementDirection;
        private Vector3 _smoothMovementDirection;


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _cameraTransform = Camera.main.transform;
        }


        private void Update()
        {
            _smoothMovementDirection = Vector3.Lerp(_smoothMovementDirection, _rawMovementDirection, Time.deltaTime * _moveSettings.MovementSmoothing);
        }

        private void FixedUpdate()
        {
            Move(GetDirection());

            if (_smoothMovementDirection.sqrMagnitude > 0.01f) Rotate(GetRotation());
        }


        public void Move(Vector3 direction, Action callback = default, float? speed = null)
        {
            _rigidbody.MovePosition(transform.position + direction);
        }

        public void Rotate(Quaternion rotation, Action callback = default, float? speed = null)
        {
            _rigidbody.MoveRotation(rotation);
        }


        public void OnMovement(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>();

            _rawMovementDirection = new Vector3(input.x, 0f, input.y);
        }


        private Vector3 GetDirection() => NormilizeCameraDirection(_smoothMovementDirection) * _moveSettings.MovementSpeed * Time.deltaTime;

        private Quaternion GetRotation()
        {
            const float turnSpeed2Lerp = 0.01f;


            var lerpTurningSpeed = _moveSettings.TurningSpeed * turnSpeed2Lerp;

            var cameraDirection = NormilizeCameraDirection(_smoothMovementDirection);

            var lookRotation = Quaternion.LookRotation(cameraDirection);

            var rotation = Quaternion.Slerp(
                _rigidbody.rotation,
                lookRotation,
                lerpTurningSpeed);


            return rotation;
        }


        private Vector3 NormilizeCameraDirection(Vector3 movementDirection)
        {
            var cameraForward = _cameraTransform.forward;
            var cameraRight = _cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            var cameraDirection = cameraForward * movementDirection.z + cameraRight * movementDirection.x;

            return cameraDirection;
        }
    }
}
