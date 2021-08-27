using System;
using System.Collections.Generic;
using Assets.Scripts.Components;
using Assets.Scripts.Controllers.UI.Puzzles.Doors;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.Puzzles.Doors;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Extensions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.States.Door;
using UnityEngine;

namespace Assets.Scripts.Door.Controllers
{
    public class DoorController : MonoBehaviour, IDoorStateSwitcher, IInteractional
    {
        private BaseDoorState _currentState;

        private List<BaseDoorState> _allStates;

        [SerializeField]
        private Key _requiredKey;

        [SerializeField]
        private Module _requiredHackModule;

        [SerializeField]
        private DoorMechanicalSettings _settings;

        [SerializeField]
        private UIDoorBaseHackController _doorHackable;

        [SerializeField]
        private DoorUI _doorUI;

        [SerializeField]
        private States _startState;


        private void Start()
        {
            _allStates = new List<BaseDoorState>
            {
                new OpenedDoorState(_requiredKey, this),
                new ClosedDoorState(_requiredKey, this),
                new LockedDoorState(_requiredKey, this, _settings, _doorHackable)
            };

            switch (_startState)
            {
                case States.Opened: SwitchState<OpenedDoorState>(); break;
                case States.Closed: SwitchState<ClosedDoorState>(); break;
                case States.Locked: SwitchState<LockedDoorState>(); break;
                default: throw new ArgumentException("Unknown state", nameof(_startState));
            }

            if (_requiredKey == null)
            {
                _doorUI.HideLockAndHackButtons();

                if (_startState == States.Locked)
                    SwitchState<ClosedDoorState>();
            }


            _doorUI.ResetButtons();
            _doorUI.Canvas.enabled = false;
        }


        public void Open() => _currentState.Open();
        public void Close() => _currentState.Close();
        public void Lock() => _currentState.Lock();


        public void SwitchState<T>() where T : BaseDoorState
        {
            var newState = _allStates.Find(state => state is T);
            _currentState = newState;


            //???
            if (_currentState is OpenedDoorState) transform.parent.TranslateTo(Quaternion.Euler(0, -90, 0), 110);
            else transform.parent.TranslateTo(Quaternion.Euler(0, 0, 0), 110);
        }


        public void StartInteraction(PlayerComponent player)
        {
            _doorUI.Canvas.enabled = true;

            _doorUI.ResetButtons();


            var hasKey = player.Inventory.HasItem(_requiredKey);
            var hasHackModule = _requiredKey != null && player.Inventory.HasItem(_requiredHackModule);

            _currentState.HandleUI(_doorUI, hasKey, hasHackModule, () => Interact(player));
        }

        public void Interact(PlayerComponent player)
        {
            StartInteraction(player);
        }

        public void FinishInteraction(PlayerComponent player)
        {
            _doorUI.Canvas.enabled = false;
        }



        private enum States
        {
            Opened,
            Closed,
            Locked
        }
    }
}
