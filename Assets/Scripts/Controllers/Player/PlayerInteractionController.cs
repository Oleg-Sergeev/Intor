using System.Collections.Generic;
using Assets.Scripts.Components;
using Assets.Scripts.Data;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerInteractionController : MonoBehaviour
    {
        [SerializeField]
        private PlayerComponent _player;

        private HashSet<Collider> _overlaps;


        private void Awake()
        {
            _player.Inventory.SlotAdded += OnSlotAddedOrRemoved;
            _player.Inventory.SlotRemoved += OnSlotAddedOrRemoved;
        }

        private void Start()
        {
            _overlaps = new HashSet<Collider>();
        }

        private void OnDestroy()
        {
            _player.Inventory.SlotAdded -= OnSlotAddedOrRemoved;
            _player.Inventory.SlotRemoved -= OnSlotAddedOrRemoved;
        }


        private void OnTriggerEnter(Collider collider)
        {
            if (collider.GetComponent<IInteractional>() is IInteractional interactional)
            {
                _overlaps.Add(collider);

                interactional.StartInteraction(_player);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            if (collider.GetComponent<IInteractional>() is IInteractional interactional)
            {
                interactional.FinishInteraction(_player);

                _overlaps.Remove(collider);
            }
        }


        private void OnSlotAddedOrRemoved(Slot _)
        {
            foreach (var collider in _overlaps) OnTriggerEnter(collider);
        }
    }
}