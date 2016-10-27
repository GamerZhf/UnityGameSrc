namespace OrbCreationExtensions
{
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [Extension]
    public static class TransformExtensions
    {
        [Extension]
        public static Transform FindFirstChildWhereNameContains(Transform trans, string childName)
        {
            if (trans.gameObject.name.IndexOf(childName) >= 0)
            {
                return trans;
            }
            IEnumerator enumerator = trans.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform transform2 = FindFirstChildWhereNameContains((Transform) enumerator.Current, childName);
                    if (transform2 != null)
                    {
                        return transform2;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            return null;
        }

        [Extension]
        public static Transform FindFirstChildWithName(Transform trans, string childName)
        {
            if (trans.gameObject.name == childName)
            {
                return trans;
            }
            IEnumerator enumerator = trans.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform transform2 = FindFirstChildWithName((Transform) enumerator.Current, childName);
                    if (transform2 != null)
                    {
                        return transform2;
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            return null;
        }

        [Extension]
        public static T GetFirstComponentInParents<T>(Transform trans) where T: MonoBehaviour
        {
            T component = trans.gameObject.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
            if ((trans.parent != null) && (trans.parent != trans))
            {
                return GetFirstComponentInParents<T>(trans.parent);
            }
            return null;
        }

        [Extension]
        public static bool IsPartOf(Transform trans, Transform aTransform)
        {
            return ((trans == aTransform) || (((trans.parent != null) && (trans.parent != trans)) && IsPartOf(trans.parent, aTransform)));
        }

        [Extension]
        public static Vector3 PointToLocalSpace(Transform trans, Vector3 p)
        {
            return trans.InverseTransformPoint(p);
        }

        [Extension]
        public static Vector3 PointToWorldSpace(Transform trans, Vector3 p)
        {
            return trans.TransformPoint(p);
        }
    }
}

