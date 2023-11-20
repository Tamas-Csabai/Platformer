using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public static class Extender
    {

        [HideInCallstack]
        public static T Log<T>(this T o, string message = "")
        {
            if (message == string.Empty || message == "")
                Debug.Log(o.ToString());
            else
                Debug.Log(message);

            return o;
        }

        [HideInCallstack]
        public static T Log<T>(this T o, UnityEngine.Object context)
        {
            Debug.Log(o.ToString(), context);
            return o;
        }

        [HideInCallstack]
        public static UnityEngine.Object Log(this UnityEngine.Object o, string message = "")
        {
            if (message == string.Empty || message == "")
                Debug.Log(o.ToString(), o);
            else
                Debug.Log(message, o);

            return o;
        }

        public static void For<T>(this T[] a, Action<T> action)
        {
            for (int i = 0; i < a.Length; i++)
                action.Invoke(a[i]);
        }

        public static T First<T>(this List<T> e) where T : class
        {
            if (e == null || e.Count == 0)
                return null;

            return e[0];
        }

        public static T First<T>(this T[] a) where T : class
        {
            if (a == null || a.Length == 0)
                return null;

            return a[0];
        }

        public static T Last<T>(this List<T> e) where T : class
        {
            if (e == null || e.Count == 0)
                return null;

            return e[e.Count - 1];
        }

        public static T Last<T>(this T[] a) where T : class
        {
            if (a == null || a.Length == 0)
                return null;

            return a[a.Length - 1];
        }

        public static bool HasIndex<T>(this T[] a, int i) where T : class
        {
            return a.Length > 0 && i >= 0 && i < a.Length;
        }

        public static bool HasIndex<T>(this List<T> a, int i) where T : class
        {
            return a.Count > 0 && i >= 0 && i < a.Count;
        }

        public static bool HasElement<T>(this T[] a, T e) where T : class
        {
            for (int i = 0; i < a.Length; i++)
                if (a[i].Equals(e))
                    return true;

            return false;
        }

        public static bool HasElement<T>(this List<T> a, T e) where T : class
        {
            foreach(T t in a)
                if (t.Equals(e))
                    return true;

            return false;
        }

        public static T Find<T>(this T[] a, Predicate<T> predicate)
        {
            for (int i = 0; i < a.Length; i++)
                if (predicate.Invoke(a[i]))
                    return a[i];

            return default;
        }

        public static bool Contains<T>(this T[] a, Predicate<T> predicate)
        {
            for (int i = 0; i < a.Length; i++)
                if (predicate.Invoke(a[i]))
                    return true;

            return false;
        }

        public static float Remap(this float f, float fromMin, float fromMax, float toMin, float toMax)
        {
            return ((Mathf.Clamp(f, fromMin, fromMax) - fromMin) / (fromMax - fromMin) * (toMax - toMin)) + toMin;
        }

        public static Transform GetClosestInChildren(this Transform t, Transform to)
        {
            if(t.childCount == 0)
                return null;

            float closestSqrDistance = float.MaxValue;
            Transform closestTransform = null;

            foreach(Transform child in t)
            {
                float sqrDistance = (to.position - child.position).sqrMagnitude;

                if (sqrDistance < closestSqrDistance)
                {
                    closestTransform = child;
                    closestSqrDistance = sqrDistance;
                }
            }

            return closestTransform;
        }

        public static void ResetLocal(this Transform t)
        {
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
        }

        public static void ResetWorld(this Transform t)
        {
            t.position = Vector3.zero;
            t.rotation = Quaternion.identity;
        }

    }
}
