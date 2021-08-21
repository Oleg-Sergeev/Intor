using System;
using Assets.Scripts.Data.Puzzles.Doors;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Controllers.UI.Puzzles.Doors
{
    public abstract class UIDoorBaseHackController : UIBaseController, IDoorHackable
    {
        public abstract void Hack(DoorBaseSettings settings, Action<bool> onEndHack);
    }
}
