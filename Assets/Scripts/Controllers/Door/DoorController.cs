using System;
using System.Collections.Generic;
using Assets.Scripts.Controllers.Player;
using Assets.Scripts.Controllers.UI.Puzzles.Doors;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.Puzzles.Doors;
using Assets.Scripts.Data.SaveData;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Extensions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.PropertyAttributes;
using Assets.Scripts.States.Door;
using Assets.Scripts.Utilities.Saving;
using UnityEngine;

namespace Assets.Scripts.Door.Controllers
{
    public class DoorController : MonoBehaviour, IDoorStateSwitcher, IInteractional, ISaveable
    {
        [field: SerializeField]
        [field: BeginReadOnlyGroup, AutoGenerateId, EndReadOnlyGroup]
        public string Id { get; private set; }


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
        private State _startState;


        private void Start()
        {
            _allStates = new List<BaseDoorState>
            {
                new OpenedDoorState(0, _requiredKey, this),
                new ClosedDoorState(1, _requiredKey, this),
                new LockedDoorState(2, _requiredKey, this, _settings, _doorHackable)
            };

            SwitchState(_startState);

            if (_requiredKey == null)
            {
                _doorUI.HideLockAndHackButtons();

                if (_startState == State.Locked)
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
            if (_currentState is OpenedDoorState) transform.parent.TranslateTo(Quaternion.Euler(0, -90, 0), 120);
            else transform.parent.TranslateTo(Quaternion.Euler(0, 0, 0), 120);
        }
        private void SwitchState(State state)
        {
            switch (state)
            {
                case State.Opened: SwitchState<OpenedDoorState>(); break;
                case State.Closed: SwitchState<ClosedDoorState>(); break;
                case State.Locked: SwitchState<LockedDoorState>(); break;
                default: throw new ArgumentException("Unknown state", nameof(_startState));
            }
        }


        public void StartInteraction(PlayerController player)
        {
            _doorUI.Canvas.enabled = true;

            _doorUI.ResetButtons();


            var hasKey = player.Inventory.HasItem(_requiredKey);
            var hasHackModule = _requiredKey != null && player.Inventory.HasItem(_requiredHackModule);

            _currentState.HandleUI(_doorUI, hasKey, hasHackModule, () => Interact(player));
        }

        public void Interact(PlayerController player)
        {
            StartInteraction(player);
        }

        public void FinishInteraction(PlayerController player)
        {
            _doorUI.Canvas.enabled = false;
        }


        public void SetItemData(ItemData itemData)
        {
            var doorData = (DoorData)itemData;

            SwitchState((State)doorData.State);
        }

        public ItemData GetItemData() => new DoorData(Id)
        {
            State = _currentState.Id
        };


        private enum State
        {
            Opened = 0,
            Closed = 1,
            Locked = 2
        }
    }
}
