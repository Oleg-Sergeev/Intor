using System;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Extensions;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Utilities;
using UnityEngine.Events;

namespace Assets.Scripts.States.Door
{
    public class OpenedDoorState : BaseDoorState
    {
        public OpenedDoorState(Key requiredKey, IDoorStateSwitcher stateSwitcher) : base(requiredKey, stateSwitcher)
        {
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            GameDebugger.Log("Door closed");

            StateSwitcher.SwitchState<ClosedDoorState>();
        }

        public override void Lock()
        {
            GameDebugger.Log("Door locked");

            StateSwitcher.SwitchState<LockedDoorState>();
        }

        public override void HandleUI(DoorUI doorUI, bool hasKey, params UnityAction[] callbacks)
        {
            doorUI.ButtonClose.AddListenersWithEnable(Close);
            doorUI.ButtonClose.AddListeners(callbacks);

            if (hasKey)
            {
                doorUI.ButtonLock.AddListenersWithEnable(Lock);
                doorUI.ButtonLock.AddListeners(callbacks);
            }
        }
    }
}
