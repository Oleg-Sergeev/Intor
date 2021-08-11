using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public class LookAtScreenCenter : MonoBehaviour
    {
        private Camera _camera;


        private void Start() => _camera = Camera.main;

        private void LateUpdate() => transform.LookAt((transform.position * 2) - _camera.transform.position);
    }
}
