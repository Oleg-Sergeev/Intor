using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class TransformExtentions
    {
        public static T GetChild<T>(this Transform transform) where T : Component
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<T>() is T component && component) return component;
            }

            return null;
        }

        public static IList<T> GetChilds<T>(this Transform transform) where T : Component
        {
            if (typeof(T) == typeof(Transform)) return transform.GetChilds() as IList<T>;

            var childs = new List<T>();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<T>() is T component && component) childs.Add(component);
            }

            return childs;
        }
        public static IList<Transform> GetChilds(this Transform transform)
        {
            var childs = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++) childs.Add(transform.GetChild(i));

            return childs;
        }
    }
}
