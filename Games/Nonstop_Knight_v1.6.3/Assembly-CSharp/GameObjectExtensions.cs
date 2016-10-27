using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[Extension]
public static class GameObjectExtensions
{
    [Extension]
    public static T AddOrGetComponent<T>(GameObject self) where T: Component
    {
        T component = self.GetComponent<T>();
        if (component == null)
        {
            component = self.AddComponent<T>();
        }
        return component;
    }

    [Extension]
    public static T FindComponentInChildren<T>(GameObject self, [Optional, DefaultParameterValue(true)] bool includeInactive) where T: Component
    {
        T[] componentsInChildren = self.GetComponentsInChildren<T>(includeInactive);
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            return componentsInChildren[i];
        }
        return null;
    }

    [Extension]
    public static GameObject FindSecurely(GameObject self, string name, [Optional, DefaultParameterValue(true)] bool recursive)
    {
        GameObject gameObject = null;
        Transform transform = self.transform;
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.gameObject.name == name)
            {
                gameObject = child.gameObject;
                break;
            }
            if (recursive)
            {
                Transform transform3 = TransformExtensions.FindChildRecursively(child, name);
                if (transform3 != null)
                {
                    gameObject = transform3.gameObject;
                    break;
                }
            }
        }
        if (gameObject == null)
        {
            Debug.LogError("GameObject `" + name + "' not found!");
        }
        return gameObject;
    }

    [Extension]
    public static T GetComponentAbove<T>(GameObject self) where T: Component
    {
        Transform parent = self.transform.parent;
        T component = null;
        while (parent != null)
        {
            component = parent.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
            parent = parent.parent;
        }
        return component;
    }

    [Extension]
    public static Component GetComponentAbove(GameObject self, System.Type type)
    {
        Transform parent = self.transform.parent;
        Component component = null;
        while (parent != null)
        {
            component = parent.GetComponent(type);
            if (component != null)
            {
                return component;
            }
            parent = parent.parent;
        }
        return component;
    }

    [Extension]
    public static void SetActiveIfDifferent(GameObject self, bool active)
    {
        if (self.activeSelf != active)
        {
            self.SetActive(active);
        }
    }

    [Extension]
    public static void SetLayerRecursively(GameObject self, int layer)
    {
        self.layer = layer;
        Transform[] children = TransformExtensions.GetChildren(self.transform, true);
        for (int i = children.Length - 1; i >= 0; i--)
        {
            children[i].gameObject.layer = layer;
        }
    }

    [Extension]
    public static void SetStaticRecursively(GameObject self, bool flag)
    {
        self.isStatic = flag;
        Transform[] children = TransformExtensions.GetChildren(self.transform, true);
        for (int i = children.Length - 1; i >= 0; i--)
        {
            children[i].gameObject.isStatic = flag;
        }
    }
}

