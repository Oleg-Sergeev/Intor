using UnityEngine;

namespace Assets.Scripts.Components
{
    public class MovementModelsComponent : MonoBehaviour
    {
        public float Constant() => 1;

        public float Linear(float value) => value;

        public float SymmetricSimplePeriod(float value) => Mathf.Sin(value);

        public float AsymetricSimplePeriod(float value) => Mathf.Pow(Mathf.Sin(value), 2) - Mathf.Cos(2 * value);

        public float SymmetricHeartbeatPeriod(float value) => Mathf.Pow(Mathf.Sin(value), 2) * Mathf.Cos(10 * value);

        public float AsymmetricHeartbeatPeriod(float value) => 2 * Mathf.Pow(Mathf.Sin(6 * value), 3) * Mathf.Pow(Mathf.Cos(5 * value), 3) + Mathf.Abs(0.1f * Mathf.Cos(2 * value));
    }
}
