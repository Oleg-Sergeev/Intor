using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public static class ResolutionScaler
    {
        public static float GetResolutionKoefficient(Vector2 referenceResolution, Vector2 currentResolution = default)
        {
            float referenceResolutionKoefficient = referenceResolution.x / referenceResolution.y;
            float currentResolutionKoefficient = currentResolution == default 
                ? (float)Screen.width / Screen.height 
                : currentResolution.x / currentResolution.y;

            var resolutionKoefficient = referenceResolutionKoefficient / currentResolutionKoefficient;

            return resolutionKoefficient;
        }

        public static float GetResolutionKoefficient(float referenceResolution, bool isAxisX = true, float currentResolution = default)
        {
            if (currentResolution == default) return referenceResolution / (isAxisX ? Screen.width : Screen.height);

            return referenceResolution / currentResolution;
        }
    }
}
