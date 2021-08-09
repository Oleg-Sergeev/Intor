using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField]
    [Range(0, 100f)]
    private float _turningSpeed = 15;

    [SerializeField]
    [Range(0, 100f)]
    private float _movementSmoothing = 5;

    [SerializeField]
    [Range(0, 100f)]
    private float _movementSpeed = 5;


    private Rigidbody _rigidbody;


    private Vector3 _movementDirection;
    private Vector3 _smoothMovementDirection;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        _smoothMovementDirection = Vector3.Lerp(_smoothMovementDirection, _movementDirection, Time.deltaTime * _movementSmoothing);
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }



    public void OnMovement(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();

        _movementDirection = new Vector3(input.x, 0f, input.y);
    }


    private void Move()
    {
        var movementDirection = NormilizeCameraDirection(_smoothMovementDirection) * _movementSpeed * Time.deltaTime;

        _rigidbody.MovePosition(transform.position + movementDirection);
    }

    private void Rotate()
    {
        if (_smoothMovementDirection.sqrMagnitude <= 0.01f) return;


        var lerpTurningSpeed = _turningSpeed / 100f;

        var cameraDirection = NormilizeCameraDirection(_smoothMovementDirection);

        var lookRotation = Quaternion.LookRotation(cameraDirection);

        var rotation = Quaternion.Slerp(
            _rigidbody.rotation,
            lookRotation,
            lerpTurningSpeed);


        _rigidbody.MoveRotation(rotation);
    }


    private Vector3 NormilizeCameraDirection(Vector3 movementDirection)
    {
        var cameraForward = Camera.main.transform.forward;
        var cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        var cameraDirection = cameraForward * movementDirection.z + cameraRight * movementDirection.x;

        return cameraDirection;
    }
}
