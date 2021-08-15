using System;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Extensions;
using Assets.Scripts.Interfaces;
using UnityEngine.Events;

namespace Assets.Scripts.States.Door
{
    public class LockedDoorState : BaseDoorState
    {
        public LockedDoorState(Key requiredKey, IDoorStateSwitcher stateSwitcher) : base(requiredKey, stateSwitcher)
        {
        }


        public override void Open()
        {
            UnityEngine.Debug.Log("Door opened");

            StateSwitcher.SwitchState<OpenedDoorState>();
        }

        public override void Close()
        {
            UnityEngine.Debug.Log("Door unlocked");

            StateSwitcher.SwitchState<ClosedDoorState>();
        }

        public override void Lock()
        {
            throw new NotImplementedException();
        }

        public override void HandleUI(DoorUI doorUI, bool hasKey, params UnityAction[] callbacks)
        {
            if (!hasKey) return;


            doorUI.ButtonClose.AddListenersAndEnable(Close);
            doorUI.ButtonClose.AddListeners(callbacks);

            doorUI.ButtonOpen.AddListenersAndEnable(Open);
            doorUI.ButtonOpen.AddListeners(callbacks);
        }
    }
}
