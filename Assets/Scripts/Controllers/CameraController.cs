using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        [SerializeField]
        private Vector3 _offset = new Vector3(-5f, -9.3f, 5f);

        [SerializeField]
        private float _followSpeed;


        private void Start()
        {
            if (_offset == Vector3.zero) _offset = _target.position - transform.position;

            transform.position = _target.position - _offset;
        }


        private void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _target.position - _offset, _followSpeed * Time.deltaTime);
        }
    }
}