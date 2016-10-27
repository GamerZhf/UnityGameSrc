using System;
using System.Collections;
using UnityEngine;

public static class UnityUtils
{
    public static bool CoroutineRunning(ref Coroutine coroutine)
    {
        return (coroutine != null);
    }

    public static T InstantiateGameObjectWithComponent<T>(Transform parent) where T: Component
    {
        GameObject obj2 = new GameObject(typeof(T).ToString());
        obj2.transform.SetParent(parent, false);
        obj2.transform.localPosition = Vector3.zero;
        return obj2.AddComponent<T>();
    }

    public static void SetActiveStateIfDifferent(GameObject go, bool active)
    {
        if (go.activeSelf != active)
        {
            go.SetActive(active);
        }
    }

    public static void SetEnabledStateIfDifferent(MonoBehaviour mb, bool enabled)
    {
        if (mb.enabled != enabled)
        {
            mb.enabled = enabled;
        }
    }

    public static Coroutine StartCoroutine(MonoBehaviour owner, IEnumerator ie)
    {
        return owner.StartCoroutine(ie);
    }

    public static void StopCoroutine(MonoBehaviour owner, ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            owner.StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}

