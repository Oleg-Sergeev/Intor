using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerData _playerData;


        private void Start()
        {
            _playerData = GetComponent<PlayerData>();
        }
    }
}