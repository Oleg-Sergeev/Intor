using Assets.Scripts.Controllers.Player;

namespace Assets.Scripts.Interfaces
{
    public interface IInteractional
    {
        void StartInteraction(PlayerController player);
        void Interact(PlayerController player);
        void FinishInteraction(PlayerController player);
    }
}
