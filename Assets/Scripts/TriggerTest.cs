using System.Threading.Tasks;
using Assets.Scripts.Extensions;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public class TriggerTest : MonoBehaviour, IMoveable
    {
        [SerializeField]
        private Vector3 vector;


        public async void Move(Vector3 direction)
        {
            var startPos = transform.position;

            await Task.Delay(2000);

            transform.TranslateTo(direction, 0.5f, async () =>
            {
                await Task.Delay(2000);

                transform.TranslateTo(startPos, 0.5f);
            });
        }

        public void Move() => Move(vector);
    }
}