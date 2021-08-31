using System.Collections.Generic;

namespace Assets.Scripts.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool ChangeKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey oldKey, TKey newKey)
        {
            if (!dict.TryGetValue(oldKey, out TValue value))
                return false;

            dict.Remove(oldKey);
            dict.Add(newKey, value);

            return true;
        }
    }
}
