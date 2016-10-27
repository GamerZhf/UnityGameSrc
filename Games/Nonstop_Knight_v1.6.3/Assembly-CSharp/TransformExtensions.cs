using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;

[Extension]
public static class TransformExtensions
{
    private static void AddChildrenToListRecursively(List<Transform> list, Transform parent)
    {
        IEnumerator enumerator = parent.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                list.Add(current);
                AddChildrenToListRecursively(list, current);
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
    }

    [Extension]
    public static void CopyFrom(Transform self, Transform other, bool localSpace)
    {
        if (localSpace)
        {
            self.localPosition = other.localPosition;
            self.localRotation = other.localRotation;
            self.localScale = other.localScale;
        }
        else
        {
            self.position = other.position;
            self.rotation = other.rotation;
            SetWorldScale(self, other.lossyScale);
        }
    }

    [Extension]
    public static void DestroyChildren(Transform self)
    {
        Transform[] componentsInChildren = self.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            if (componentsInChildren[i] != self)
            {
                UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
            }
        }
    }

    [Extension]
    public static void DestroyChildrenImmediate(Transform self)
    {
        Transform[] componentsInChildren = self.GetComponentsInChildren<Transform>(true);
        for (int i = componentsInChildren.Length - 1; i >= 0; i--)
        {
            if (componentsInChildren[i] != self)
            {
                UnityEngine.Object.DestroyImmediate(componentsInChildren[i].gameObject);
            }
        }
    }

    [Extension]
    public static Transform FindChildRecursively(Transform self, string name)
    {
        foreach (Transform transform in self.GetComponentsInChildren<Transform>())
        {
            if (transform.name == name)
            {
                return transform;
            }
        }
        return null;
    }

    [Extension]
    public static Transform FindChildRegex(Transform self, string pattern, [Optional, DefaultParameterValue(false)] bool recursive)
    {
        Transform[] children = GetChildren(self, recursive);
        for (int i = 0; i < children.Length; i++)
        {
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(children[i].name))
            {
                return children[i];
            }
        }
        return null;
    }

    [Extension]
    public static Transform FindChildRegex(Transform self, string[] patterns, [Optional, DefaultParameterValue(false)] bool recursive)
    {
        Transform[] children = GetChildren(self, recursive);
        for (int i = 0; i < patterns.Length; i++)
        {
            for (int j = 0; j < children.Length; j++)
            {
                Regex regex = new Regex(patterns[i]);
                if (regex.IsMatch(children[j].name))
                {
                    return children[j];
                }
            }
        }
        return null;
    }

    [Extension]
    public static Transform FindChildWithTag(Transform self, string tag)
    {
        IEnumerator enumerator = self.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                if (current.gameObject.CompareTag(tag))
                {
                    return current;
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
    public static Transform FindChildWithTagRecursively(Transform self, string tag)
    {
        foreach (Transform transform in self.GetComponentsInChildren<Transform>())
        {
            if (transform.gameObject.CompareTag(tag))
            {
                return transform;
            }
        }
        return null;
    }

    [Extension]
    public static Transform GetChildByIndex(Transform self, int index)
    {
        Transform[] childrenSorted = GetChildrenSorted(self, null);
        return ((index >= childrenSorted.Length) ? null : childrenSorted[index]);
    }

    [Extension]
    public static Transform[] GetChildren(Transform self, bool recursive)
    {
        List<Transform> list = new List<Transform>();
        if (recursive)
        {
            AddChildrenToListRecursively(list, self);
        }
        else
        {
            IEnumerator enumerator = self.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    if (current != self)
                    {
                        list.Add(current);
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
        }
        return list.ToArray();
    }

    [Extension]
    public static Transform[] GetChildrenSorted(Transform self, [Optional, DefaultParameterValue(null)] IComparer<Transform> sorter)
    {
        List<Transform> list = new List<Transform>(GetChildren(self, false));
        if (sorter == null)
        {
            list.Sort(new Comparison<Transform>(TransformExtensions.SortAlphabetically));
        }
        else
        {
            list.Sort(sorter);
        }
        return list.ToArray();
    }

    [Extension]
    public static int GetRecursiveChildCount(Transform self)
    {
        return (self.GetComponentsInChildren<Transform>().Length - 1);
    }

    [Extension]
    public static Transform[] GetSiblings(Transform self)
    {
        if (self.parent == null)
        {
            return new Transform[0];
        }
        List<Transform> list = new List<Transform>();
        IEnumerator enumerator = self.parent.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                if ((current != self) && (current != self.parent))
                {
                    list.Add(current);
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
        return list.ToArray();
    }

    [Extension]
    public static bool IsSiblingOf(Transform self, Transform other)
    {
        return (self.parent == other.parent);
    }

    [Extension]
    public static void ScaleBy(Transform self, float scaleBy)
    {
        self.localScale = (Vector3) (self.localScale * scaleBy);
    }

    [Extension]
    public static void ScaleBy(Transform self, Vector3 scaleBy)
    {
        Vector3 localScale = self.localScale;
        localScale.Scale(scaleBy);
        self.localScale = localScale;
    }

    [Extension]
    public static void SetParentAndMaintainLocalTm(Transform self, Transform parent)
    {
        Vector3 localPosition = self.localPosition;
        Quaternion localRotation = self.localRotation;
        Vector3 localScale = self.localScale;
        self.SetParent(parent);
        self.localPosition = localPosition;
        self.localRotation = localRotation;
        self.localScale = localScale;
    }

    [Extension]
    public static void SetWorldScale(Transform self, Vector3 scale)
    {
        self.localScale = Vector3.one;
        Vector3 lossyScale = self.lossyScale;
        if (lossyScale.x != 0f)
        {
            scale.x /= lossyScale.x;
        }
        if (lossyScale.y != 0f)
        {
            scale.y /= lossyScale.y;
        }
        if (lossyScale.z != 0f)
        {
            scale.z /= lossyScale.z;
        }
        self.localScale = scale;
    }

    private static int SortAlphabetically(Transform a, Transform b)
    {
        return a.name.CompareTo(b.name);
    }
}

