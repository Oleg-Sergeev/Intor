using Assets.Scripts.Components;
using UnityEngine;

namespace Assets.Scripts.Controllers.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerComponent _player;


        private void Start()
        {
            _player = GetComponent<PlayerComponent>();
        }
    }
}