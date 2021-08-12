using Assets.Scripts.Components;

namespace Assets.Scripts.Interfaces
{
    public interface IInteractional
    {
        void StartInteraction(PlayerComponent player);
        void Interact(PlayerComponent player);
        void FinishInteraction(PlayerComponent player);
    }
}
