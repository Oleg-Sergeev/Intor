using System;
using Assets.Scripts.Data.Puzzles.Doors;

namespace Assets.Scripts.Interfaces
{
    public interface IDoorHackable
    {
        void Hack(DoorBaseSettings settings, Action<bool> onEndHack);
    }
}
