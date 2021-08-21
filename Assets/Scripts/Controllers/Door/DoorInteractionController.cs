using Assets.Scripts.Components;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Controllers.Door
{
    [RequireComponent(typeof(DoorComponent))]
    public class DoorInteractionController : MonoBehaviour, IInteractional
    {
        [SerializeField]
        private DoorUI _doorUI;

        private DoorComponent _door;


        private void Start()
        {
            _door = GetComponent<DoorComponent>();
        }


        public void StartInteraction(PlayerComponent player)
        {
            _doorUI.Canvas.enabled = true;

            _doorUI.ResetButtons();

            _door.HandleUI(_doorUI, player, () => Interact(player));
        }

        public void Interact(PlayerComponent player)
        {
            StartInteraction(player);
        }

        public void FinishInteraction(PlayerComponent player)
        {
            _doorUI.Canvas.enabled = false;
        }
    }
}
