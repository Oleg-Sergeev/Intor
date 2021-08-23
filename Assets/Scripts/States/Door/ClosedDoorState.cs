using System;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Extensions;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.States.Door
{
    public class ClosedDoorState : BaseDoorState
    {
        public ClosedDoorState(Key requiredKey, IDoorStateSwitcher stateSwitcher) : base(requiredKey, stateSwitcher)
        {
        }


        public override void Open()
        {
            Debug.Log("Door opened");

            StateSwitcher.SwitchState<OpenedDoorState>();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override void Lock()
        {
            Debug.Log("Door locked");

            StateSwitcher.SwitchState<LockedDoorState>();
        }

        public override void HandleUI(DoorUI doorUI, bool hasKey, params UnityAction[] callbacks)
        {
            doorUI.ButtonOpen.AddListenersWithEnable(Open);
            doorUI.ButtonOpen.AddListeners(callbacks);

            if (hasKey)
            {
                doorUI.ButtonLock.AddListenersWithEnable(Lock);
                doorUI.ButtonLock.AddListeners(callbacks);
            }
        }
    }
}
