using Assets.Scripts.Data.SaveData;
using Assets.Scripts.Utilities.Saving;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class MainCameraController : MonoBehaviour, ISaveable
    {
        public string Id => "MainCamera";


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


        public void SetItemData(ItemData itemData)
        {
            var cameraData = (CameraData)itemData;

            transform.position = cameraData.Position;
            transform.rotation = Quaternion.Euler(cameraData.Rotation);
        }

        public ItemData GetItemData() => new CameraData(Id)
        {
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles
        };
    }
}