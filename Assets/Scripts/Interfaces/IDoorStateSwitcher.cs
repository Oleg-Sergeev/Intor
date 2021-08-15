using Assets.Scripts.States.Door;

namespace Assets.Scripts.Interfaces
{
    public interface IDoorStateSwitcher
    {
        void SwitchState<T>() where T : BaseDoorState;
    }
}
