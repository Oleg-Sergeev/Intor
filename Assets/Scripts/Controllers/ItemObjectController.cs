using Assets.Scripts.Components.Triggers;
using Assets.Scripts.Controllers.Player;
using Assets.Scripts.Data.Items;
using Assets.Scripts.Data.SaveData;
using Assets.Scripts.Data.UI;
using Assets.Scripts.Interfaces;
using Assets.Scripts.PropertyAttributes;
using Assets.Scripts.Utilities.Localization;
using Assets.Scripts.Utilities.Saving;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class ItemObjectController : MonoBehaviour, IInteractional, ISaveable
    {
        [field: SerializeField]
        [field: BeginReadOnlyGroup, AutoGenerateId, EndReadOnlyGroup]
        public string Id { get; private set; }


        private bool _isDeleted;

        [SerializeField]
        private Item _item;

        [SerializeField]
        private ItemUI _itemUI;

        [SerializeField]
        private GameObject _canvasItemUI;

        private Trigger _trigger;


        private void Awake()
        {
            LocalizationController.LanguageChanged += OnLanguageChanged;
        }

        private void Start()
        {
            _isDeleted = false;

            _itemUI.Name.text = _item.Name;

            _canvasItemUI.SetActive(false);

            _trigger = GetComponent<Trigger>();
        }

        private void OnDestroy()
        {
            LocalizationController.LanguageChanged -= OnLanguageChanged;
        }


        public void StartInteraction(PlayerController player)
        {
            _itemUI.PickUp.onClick.RemoveAllListeners();
            _itemUI.PickUp.interactable = player.Inventory.HasEmptySpace(_item);
            _itemUI.PickUp.onClick.AddListener(() => Interact(player));

            _canvasItemUI.SetActive(true);
        }

        public void Interact(PlayerController player)
        {
            player.Inventory.Add(_item);

            FinishInteraction(player);

            gameObject.GetComponent<MeshRenderer>().enabled = false;

            _isDeleted = true;


            _trigger?.Call();
        }

        public void FinishInteraction(PlayerController player)
        {
            _itemUI.PickUp.onClick.RemoveAllListeners();


            _canvasItemUI.SetActive(false);

            if (_isDeleted) gameObject.SetActive(false);
        }


        public void SetItemData(ItemData itemData)
        {
            var data = (ItemObjectData)itemData;

            _isDeleted = data.IsDeleted;

            if (_isDeleted) FinishInteraction(null);
        }

        public ItemData GetItemData() => new ItemObjectData(Id)
        {
            IsDeleted = _isDeleted
        };


        private void OnLanguageChanged()
        {
            _itemUI.Name.text = _item.Name;
        }
    }
}