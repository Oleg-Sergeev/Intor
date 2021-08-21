using System;
using System.Collections;
using Assets.Scripts.Data.Puzzles.Doors;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.UI.Puzzles.Doors
{
    public class UIDoorMechanicalHackController : UIDoorBaseHackController
    {
        private const float Smoothing = 7f;

        [SerializeField]
        private UIGameplayController _gameplayUI;

        [SerializeField]
        private PlayerInput _input;

        [SerializeField]
        private Slider _progressBar;

        [SerializeField]
        private RectTransform _point;

        private RectTransform _handle;

        private DoorMechanicalSettings _mechanicalSettings;

        private Vector3 _pointRawDirection;
        private Vector3 _pointSmoothDirection;

        private Vector3 _handleRawDirection;
        private Vector3 _handleSmoothDirection;

        private int _movementDirection;

        private bool _hasAnyDirection;


        protected override void Start()
        {
            base.Start();

            _handle = _progressBar.handleRect;
        }


        public override void Hack(DoorBaseSettings settings, Action<bool> onEndHack)
        {
            Toggle();
            _gameplayUI.Toggle();

            SetupPuzzle(settings);

            StartCoroutine(MovePoint());
            StartCoroutine(MoveHandle(onEndHack));
        }


        public void OnMovePoint(int direction)
        {
            _hasAnyDirection = _movementDirection != 0;

            _movementDirection = direction;
        }
        public void OnMovePoint(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<float>();

            OnMovePoint((int)direction);
        }

        public void StopMovePoint()
        {
            if (!_hasAnyDirection) OnMovePoint(0);

            _hasAnyDirection = false;
        }


        private IEnumerator ReverseHandle(float time)
        {
            while (time > 0f && Canvas.enabled)
            {
                yield return null;

                time -= Time.deltaTime;


                _handleRawDirection -= _mechanicalSettings.HandleReverseAcceleration * Vector3.right * Time.deltaTime;

                _handleRawDirection = Vector3.ClampMagnitude(_handleRawDirection, _mechanicalSettings.HandleReverseSpeed);

                _handleSmoothDirection = Vector3.Lerp(_handleSmoothDirection, _handleRawDirection, Time.deltaTime * Smoothing);

                _progressBar.value += _handleSmoothDirection.x * Time.deltaTime;
            }
        }

        private IEnumerator MoveHandle(Action<bool> onEndHack)
        {
            while (_progressBar.value < _mechanicalSettings.PuzzleTime && Canvas.enabled)
            {
                yield return null;


                if (IsPointInsideTheHandle())
                {
                    _handleRawDirection += Vector3.right * Time.deltaTime;
                }
                else
                {
                    yield return StartCoroutine(ReverseHandle(_mechanicalSettings.HandleReverseTime));
                }

                _handleRawDirection = Vector3.ClampMagnitude(_handleRawDirection, 1f);

                _handleSmoothDirection = Vector3.Lerp(_handleSmoothDirection, _handleRawDirection, Time.deltaTime * Smoothing);

                _progressBar.value += _handleSmoothDirection.x * Time.deltaTime;
            }

            if (Canvas.enabled) Toggle();
            _gameplayUI.Toggle();

            onEndHack?.Invoke(_progressBar.value >= _mechanicalSettings.PuzzleTime);


            _input.SwitchCurrentActionMap("Main");
        }

        private IEnumerator MovePoint()
        {
            var handleAreaLength = _handle.parent.GetComponent<RectTransform>().rect.width / 2;

            while (_progressBar.value < _mechanicalSettings.PuzzleTime && Canvas.enabled)
            {
                yield return null;

                _pointSmoothDirection = Vector3.Lerp(_pointSmoothDirection, _pointRawDirection * _movementDirection, Time.deltaTime * Smoothing);


                _point.Translate(_pointSmoothDirection * Time.deltaTime);

                _point.localPosition = Vector3.ClampMagnitude(_point.localPosition, handleAreaLength);
            }
        }


        private bool IsPointInsideTheHandle() =>
            Mathf.Abs(_point.localPosition.x - _handle.localPosition.x) <= _mechanicalSettings.HandleWidth / 2;


        private void SetupPuzzle(DoorBaseSettings settings)
        {
            _input.SwitchCurrentActionMap("PuzzleDoorMechanical");

            _point.anchoredPosition = Vector3.zero;
            _handle.anchoredPosition = Vector3.zero;

            _pointSmoothDirection = Vector3.zero;
            _handleSmoothDirection = Vector3.zero;

            _progressBar.value = 0f;

            _movementDirection = 0;

            _hasAnyDirection = false;


            _mechanicalSettings = (DoorMechanicalSettings)settings;

            _progressBar.maxValue = _mechanicalSettings.PuzzleTime;

            _handle.sizeDelta = new Vector2(_mechanicalSettings.HandleWidth, 0);


            var resilutionKoef = 1f / Utilities.ResolutionScaler.GetResolutionKoefficient(2160);

            _pointRawDirection = _mechanicalSettings.PointSpeed * Vector3.right * resilutionKoef;

            _handleRawDirection = Vector3.right * resilutionKoef;
        }
    }
}
