using System;
using UnityEngine;
using UnityTest;

[DynamicTest("ExampleIntegrationTests"), SucceedWithAssertions]
public class CodeBasedAssertionExample : MonoBehaviour
{
    public float FloatField = 3f;
    public GameObject goReference;

    public void Awake()
    {
        IAssertionComponentConfigurator configurator;
        FloatComparer comparer = AssertionComponent.Create<FloatComparer>(out configurator, CheckMethod.Update | CheckMethod.Start, base.gameObject, "CodeBasedAssertionExample.FloatField", 3f);
        configurator.UpdateCheckRepeatFrequency = 5;
        comparer.floatingPointError = 0.1;
        comparer.compareTypes = FloatComparer.CompareTypes.Equal;
        AssertionComponent.Create<ValueDoesNotChange>(CheckMethod.Update | CheckMethod.Start, base.gameObject, "CodeBasedAssertionExample.FloatField");
        base.transform.position = new Vector3(0f, 3f, 0f);
        AssertionComponent.Create<FloatComparer>(CheckMethod.Update, base.gameObject, "CodeBasedAssertionExample.FloatField", base.gameObject, "transform.position.y");
        this.goReference = base.gameObject;
        AssertionComponent.Create<GeneralComparer>(CheckMethod.Update, base.gameObject, "CodeBasedAssertionExample.goReference", null).compareType = GeneralComparer.CompareType.ANotEqualsB;
    }
}

