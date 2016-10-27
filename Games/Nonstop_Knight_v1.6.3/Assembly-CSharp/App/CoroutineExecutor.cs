namespace App
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class CoroutineExecutor : MonoBehaviour, ICoroutineExecutor
    {
        Coroutine ICoroutineExecutor.StartCoroutine(IEnumerator routine)
        {
            return base.StartCoroutine(routine);
        }

        void ICoroutineExecutor.StopAllCoroutines()
        {
            base.StopAllCoroutines();
        }
    }
}

