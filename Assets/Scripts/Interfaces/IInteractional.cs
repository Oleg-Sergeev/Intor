using Assets.Scripts.Data;

namespace Assets.Scripts.Interfaces
{
    public interface IInteractional
    {
        void StartInteraction(PlayerData player);
        void Interact(PlayerData player);
        void FinishInteraction(PlayerData player);
    }
}
