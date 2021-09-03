using UnityEngine;

namespace Assets.Scripts.Components
{
    public class ElevatorDestroyer : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _elevatorPosition;

        [SerializeField]
        private Vector3 _elevatorRotation;

        [SerializeField]
        private ComponentsDataComponent _rope;

        [SerializeField]
        private ComponentsDataComponent _elevatorWall;

        [SerializeField]
        private ComponentsDataComponent[] _leverTriggers;

        private ElevatorComponent _elevator;


        private void Start()
        {
            _elevator = GetComponent<ElevatorComponent>();
        }


        public void DestroyElevator()
        {
            _rope.AddComponent<BoxCollider>();
            _rope.AddComponent<Rigidbody>();

            for (int i = 0; i < _leverTriggers.Length; i++)
            {
                _leverTriggers[i].RemoveComponent<Triggers.TriggerComponent>();
            }

            _elevator.MoveImmediately(_elevatorPosition, speed: 3);
            _elevator.Rotate(Quaternion.Euler(_elevatorRotation), speed: 120);

            _elevator.Disable();

            _elevatorWall.transform.localScale = new Vector3(1, 1, 0.995f);
            _elevatorWall.AddComponent<Rigidbody>();
        }
    }
}