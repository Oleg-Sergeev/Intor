using System.Threading.Tasks;
using Assets.Scripts.Controllers.UI;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class ElevatorComponent : TransformTranslatorComponent
    {
        private const float Epsilon = 0.01f;

        private Vector3 _positionDown;
        private Vector3 _positionUp;


        [SerializeField]
        private TransformTranslatorComponent _elevatorButton;

        [SerializeField]
        private UIElevatorController _elevatorUi;

        [SerializeField]
        private float _moveDelay = 1.5f;

        [SerializeField]
        private bool _isStartAtTheDown = true;

        [SerializeField]
        private bool _isWorking = true;



        protected override void Init()
        {
            base.Init();

            if (_isWorking)
            {
                _elevatorUi.ButtonUp.interactable = _isStartAtTheDown;
                _elevatorUi.ButtonDown.interactable = !_isStartAtTheDown;
            }
            else _elevatorUi.SetWorking(false);


            if (_isStartAtTheDown)
            {
                _positionDown = StartPosition;
                _positionUp = ToPosition;
            }
            else
            {
                _positionDown = ToPosition;
                _positionUp = StartPosition;
            }


            Callback += OnEndMove;
        }

        private void OnDestroy()
        {
            Callback -= OnEndMove;
        }


        public override async void Move(Vector3 direction)
        {
            if (!_isWorking) return;

            var isAtTheUp = IsAtTheTop(direction);
            var isAtTheDown = IsAtTheDown(direction);
            if ((IsAtTheDown() && isAtTheDown) || (IsAtTheTop() && isAtTheUp)) return;

            if (isAtTheDown) _elevatorButton.RotateToStart();
            else _elevatorButton.RotateToDestination();

            _elevatorUi.DisableButtons();

            await Task.Delay((int)(_moveDelay * 1000));

            base.Move(direction);
        }


        public void Enable() => SetWorking(true);
        public void Disable() => SetWorking(false);

        private void SetWorking(bool isWorking)
        {
            _isWorking = isWorking;

            _elevatorUi.SetWorking(isWorking);


            if (_isWorking) OnEndMove();
        }


        public void OnEndMove()
        {
            if (IsAtTheDown()) _elevatorUi.ToggleUp();
            else if (IsAtTheTop()) _elevatorUi.ToggleDown();
            else _elevatorUi.EnableButtons();
        }


        private bool IsAtTheDown() => IsAtTheDown(IsLocalPosition ? transform.localPosition : transform.position);
        private bool IsAtTheDown(Vector3 position) => Vector3.Distance(_positionDown, position) <= Epsilon;

        private bool IsAtTheTop() => IsAtTheTop(IsLocalPosition ? transform.localPosition : transform.position);
        private bool IsAtTheTop(Vector3 position) => Vector3.Distance(_positionUp, position) <= Epsilon;
    }
}