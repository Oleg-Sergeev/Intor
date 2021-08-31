using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Extensions;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.States.Door
{
    public class OpenedDoorState : BaseDoorState
    {
        public OpenedDoorState(int id, Key requiredKey, IDoorStateSwitcher stateSwitcher) : base(id, requiredKey, stateSwitcher)
        {
        }

        public override void Open()
        {
            Debug.Log("Door already locked");
        }

        public override void Close()
        {
            Debug.Log("Door closed");

            StateSwitcher.SwitchState<ClosedDoorState>();
        }

        public override void Lock()
        {
            Debug.Log("Door locked");

            StateSwitcher.SwitchState<LockedDoorState>();
        }

        public override void HandleUI(DoorUI doorUI, bool hasKey, bool hasHackModule, params UnityAction[] callbacks)
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
