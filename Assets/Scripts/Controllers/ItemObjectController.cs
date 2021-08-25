﻿using Assets.Scripts.Components;
using Assets.Scripts.Components.Triggers;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ItemObjectController : MonoBehaviour, IInteractional
    {
        private bool _isDeleted;

        [SerializeField]
        private Item _item;

        [SerializeField]
        private ItemUI _itemUI;

        [SerializeField]
        private GameObject _canvasItemUI;

        private Trigger _trigger;


        private void Start()
        {
            _isDeleted = false;

            _itemUI.Name.text = _item.Name;

            _canvasItemUI.SetActive(false);

            _trigger = GetComponent<Trigger>();
        }


        public void StartInteraction(PlayerComponent player)
        {
            _itemUI.PickUp.onClick.RemoveAllListeners();
            _itemUI.PickUp.interactable = player.Inventory.HasEmptySpace(_item);
            _itemUI.PickUp.onClick.AddListener(() => Interact(player));

            _canvasItemUI.SetActive(true);
        }

        public void Interact(PlayerComponent player)
        {
            _trigger?.Call();

            player.Inventory.Add(_item);

            FinishInteraction(player);

            gameObject.GetComponent<MeshRenderer>().enabled = false;

            _isDeleted = true;
        }

        public void FinishInteraction(PlayerComponent player)
        {
            _itemUI.PickUp.onClick.RemoveAllListeners();


            _canvasItemUI.SetActive(false);

            if (_isDeleted) gameObject.SetActive(false);
        }
    }
}