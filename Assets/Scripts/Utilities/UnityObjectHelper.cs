using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Utilities
{
    public static class UnityObjectHelper
    {
        public static IEnumerable<T> FindObjectsOfType<T>(bool includeActive = true)
            => Object.FindObjectsOfType<MonoBehaviour>(includeActive)
            .Select(x => x.GetComponent<T>())
            .Where(x => x != null);

    }
}
