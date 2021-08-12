using Assets.Scripts.Components;
using Assets.Scripts.Data;
using Assets.Scripts.Data.Items;
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


        private void Start()
        {
            _isDeleted = false;

            _itemUI.Name.text = _item.Name;

            _canvasItemUI.SetActive(false);
        }


        public void StartInteraction(PlayerComponent player)
        {
            if (_isDeleted) return;

            _itemUI.PickUp.onClick.RemoveAllListeners();
            _itemUI.PickUp.interactable = player.Inventory.HasEmptySpace(_item);
            _itemUI.PickUp.onClick.AddListener(() => Interact(player));

            _canvasItemUI.SetActive(true);
        }

        public void Interact(PlayerComponent player)
        {
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