using System;
using System.Collections.Generic;
using Assets.Scripts.Controllers.UI.Puzzles.Doors;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.Puzzles.Doors;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Interfaces;
using Assets.Scripts.States.Door;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Components
{
    [Serializable]
    public class DoorComponent : MonoBehaviour, IDoorStateSwitcher
    {
        private BaseDoorState _currentState;

        private List<BaseDoorState> _allStates;

        [SerializeField]
        private Key _requiredKey;

        [SerializeField]
        private DoorMechanicalSettings _settings;

        [SerializeField]
        private UIDoorBaseHackController _doorHackable;

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
        }


        public void Open()
        {
            _currentState.Open();
        }

        public void Close()
        {
            _currentState.Close();
        }

        public void Lock()
        {
            _currentState.Lock();
        }

        public void HandleUI(DoorUI doorUI, PlayerComponent player, params UnityAction[] callbacks)
        {
            var hasKey = player.Inventory.HasItem(_requiredKey);

            _currentState.HandleUI(doorUI, hasKey, callbacks);
        }

        public void SwitchState<T>() where T : BaseDoorState
        {
            var newState = _allStates.Find(state => state is T);
            _currentState = newState;
        }



        private enum States
        {
            Opened,
            Closed,
            Locked
        }
    }
}
