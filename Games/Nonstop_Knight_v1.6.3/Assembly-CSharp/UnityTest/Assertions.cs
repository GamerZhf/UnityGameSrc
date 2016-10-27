namespace UnityTest
{
    using System;
    using UnityEngine;

    public static class Assertions
    {
        public static void CheckAssertions()
        {
            AssertionComponent[] assertions = UnityEngine.Object.FindObjectsOfType(typeof(AssertionComponent)) as AssertionComponent[];
            CheckAssertions(assertions);
        }

        public static void CheckAssertions(GameObject gameObject)
        {
            CheckAssertions(gameObject.GetComponents<AssertionComponent>());
        }

        public static void CheckAssertions(AssertionComponent assertion)
        {
            AssertionComponent[] assertions = new AssertionComponent[] { assertion };
            CheckAssertions(assertions);
        }

        public static void CheckAssertions(AssertionComponent[] assertions)
        {
            if (Debug.isDebugBuild)
            {
                foreach (AssertionComponent component in assertions)
                {
                    component.checksPerformed++;
                    if (!component.Action.Compare())
                    {
                        component.hasFailed = true;
                        component.Action.Fail(component);
                    }
                }
            }
        }
    }
}

