using System;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.Puzzles.Doors;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Extensions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utilities;
using UnityEngine.Events;

namespace Assets.Scripts.States.Door
{
    public class LockedDoorState : BaseDoorState
    {
        private IDoorHackable _doorHacker;

        private DoorBaseSettings _doorSettings;


        public LockedDoorState(Key requiredKey, IDoorStateSwitcher stateSwitcher, DoorBaseSettings doorSettings, IDoorHackable doorHacker) : base(requiredKey, stateSwitcher)
        {
            _doorSettings = doorSettings;
            _doorHacker = doorHacker;
        }


        public override void Open()
        {
            GameDebugger.Log("Door opened");

            StateSwitcher.SwitchState<OpenedDoorState>();
        }

        public override void Close()
        {
            GameDebugger.Log("Door unlocked");

            StateSwitcher.SwitchState<ClosedDoorState>();
        }

        public override void Lock()
        {
            GameDebugger.Log("Door already locked");
        }

        private void Hack(params UnityAction[] callbacks)
        {
            _doorHacker.Hack(_doorSettings, (success) =>
            {
                if (success) Close();

                foreach (var callback in callbacks) callback?.Invoke();
            });
        }


        public override void HandleUI(DoorUI doorUI, bool hasKey, params UnityAction[] callbacks)
        {
            doorUI.ButtonHack.AddListenersWithEnable(() => Hack(callbacks));

            if (!hasKey) return;


            doorUI.ButtonClose.AddListenersWithEnable(Close);
            doorUI.ButtonClose.AddListeners(callbacks);

            doorUI.ButtonOpen.AddListenersWithEnable(Open);
            doorUI.ButtonOpen.AddListeners(callbacks);
        }
    }
}
