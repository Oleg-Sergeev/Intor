using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Interfaces;
using UnityEngine.Events;

namespace Assets.Scripts.States.Door
{
    public abstract class BaseDoorState
    {
        protected Key RequiredKey { get; }

        protected IDoorStateSwitcher StateSwitcher { get; }


        protected BaseDoorState(Key requiredKey, IDoorStateSwitcher stateSwitcher)
        {
            RequiredKey = requiredKey;

            StateSwitcher = stateSwitcher;
        }


        public abstract void Open();

        public abstract void Close();

        public abstract void Lock();

        public abstract void HandleUI(DoorUI doorUI, bool hasKey, bool hasHackModule, params UnityAction[] callbacks);
    }
}
