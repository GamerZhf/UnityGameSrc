namespace App
{
    using System;
    using System.Collections;
    using UnityEngine;

    public interface ICoroutineExecutor
    {
        Coroutine StartCoroutine(IEnumerator routine);
        void StopAllCoroutines();
    }
}

