using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerInteractionController : MonoBehaviour
    {
        private PlayerController _player;

        private HashSet<Collider> _overlaps;


        private void Awake()
        {
            _player = transform.parent.GetComponent<PlayerController>();

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
            if (!(collider.GetComponent<IInteractional>() is IInteractional interactional)) return;

            //var ray = new Ray(transform.position, collider.transform.position - transform.position);
            //if (Physics.Raycast(ray, out var hit) && hit.collider != collider) return;


            _overlaps.Add(collider);

            interactional.StartInteraction(_player);
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