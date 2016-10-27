using System;
using UnityEngine;

[ExcludePlatform(new RuntimePlatform[] { RuntimePlatform.Android, RuntimePlatform.LinuxPlayer }), SucceedWithAssertions, DynamicTest("ExampleIntegrationTests"), Timeout(1f), ExpectExceptions(false, new System.Type[] { typeof(ArgumentException) })]
public class DynamicIntegrationTest : MonoBehaviour
{
    public void Start()
    {
        IntegrationTest.Pass(base.gameObject);
    }
}

