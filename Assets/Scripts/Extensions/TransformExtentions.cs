using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class TransformExtentions
    {
        public static async void TranslateTo(this Transform transform, Vector3 endPos, float speed = 1.5f, Action callback = default, bool isLocal = false, CancellationToken token = default)
        {
            var t = 0f;

            var (x1, y1, z1) = !isLocal ? transform.position : transform.localPosition;
            var (x2, y2, z2) = endPos;

            while (t <= 1f)
            {
                await Task.Yield();

                token.ThrowIfCancellationRequested();

                var pos = new Vector3(Mathf.SmoothStep(x1, x2, t), Mathf.SmoothStep(y1, y2, t), Mathf.SmoothStep(z1, z2, t));

                if (!isLocal) transform.position = pos;
                else transform.localPosition = pos;

                t += speed * Time.deltaTime;
            }

            callback?.Invoke();
        }
        public static async void TranslateTo(this Transform transform, Quaternion endRot, float speed = 1.5f, Action callback = default, bool isLocal = false, CancellationToken token = default)
        {
            var t = 0f;

            var (x0, y0, z0, w0) = transform.rotation;
            var (x, y, z, w) = endRot;

            while (t <= 1)
            {
                await Task.Yield();

                token.ThrowIfCancellationRequested();

                var rot = new Quaternion(Mathf.SmoothStep(x0, x, t), Mathf.SmoothStep(y0, y, t), Mathf.SmoothStep(z0, z, t), Mathf.SmoothStep(w0, w, t));

                if (!isLocal) transform.rotation = rot;
                else transform.localRotation = rot;

                t += speed * Time.deltaTime;
            }

            callback?.Invoke();
        }
        public static void TranslateTo(this Transform transform, Vector3 endPos, Quaternion endRot, float speed = 1.5f, Action callback = default, bool isLocal = false, CancellationToken token = default)
        {
            transform.TranslateTo(endPos, speed, default, isLocal, token);
            transform.TranslateTo(endRot, speed, callback, isLocal, token);
        }



        public static async void RotateWhile(this Transform transform, Vector3 direction, Func<bool> predicate, Action callback = default)
        {
            direction = new Vector3(direction.x, direction.y, -direction.z);

            while (predicate())
            {
                await Task.Yield();

                transform.Rotate(direction * Time.deltaTime);
            }

            callback?.Invoke();
        }

        public static async void ScaleTo(this Transform transform, Vector3 endScale, float speed = 1.5f, Action callback = default)
        {
            var (x1, y1, z1) = transform.localScale;
            var (x2, y2, z2) = endScale;

            var t = 0f;

            while (t <= 1)
            {
                await Task.Yield();

                transform.localScale = new Vector3(Mathf.SmoothStep(x1, x2, t), Mathf.SmoothStep(y1, y2, t), Mathf.SmoothStep(z1, z2, t));

                t += speed * Time.deltaTime;
            }

            callback?.Invoke();
        }


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
