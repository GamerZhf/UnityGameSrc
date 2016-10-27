namespace UnityTest.IntegrationTestRunner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityTest;

    internal class IntegrationTestsProvider
    {
        [CompilerGenerated]
        private static Func<ITestComponent, ITestComponent> <>f__am$cache3;
        internal ITestComponent currentTestGroup;
        internal Dictionary<ITestComponent, HashSet<ITestComponent>> testCollection = new Dictionary<ITestComponent, HashSet<ITestComponent>>();
        internal IEnumerable<ITestComponent> testToRun;

        public IntegrationTestsProvider(IEnumerable<ITestComponent> tests)
        {
            this.testToRun = tests;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = new Func<ITestComponent, ITestComponent>(IntegrationTestsProvider.<IntegrationTestsProvider>m__5C);
            }
            IEnumerator<ITestComponent> enumerator = Enumerable.OrderBy<ITestComponent, ITestComponent>(tests, <>f__am$cache3).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    ITestComponent current = enumerator.Current;
                    if (current.IsTestGroup())
                    {
                        throw new Exception(current.Name + " is test a group");
                    }
                    this.AddTestToList(current);
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            if (this.currentTestGroup == null)
            {
                this.currentTestGroup = this.FindInnerTestGroup(TestComponent.NullTestComponent);
            }
        }

        [CompilerGenerated]
        private static ITestComponent <IntegrationTestsProvider>m__5C(ITestComponent component)
        {
            return component;
        }

        private void AddTestToList(ITestComponent test)
        {
            ITestComponent testGroup = test.GetTestGroup();
            if (!this.testCollection.ContainsKey(testGroup))
            {
                this.testCollection.Add(testGroup, new HashSet<ITestComponent>());
            }
            this.testCollection[testGroup].Add(test);
            if (testGroup != TestComponent.NullTestComponent)
            {
                this.AddTestToList(testGroup);
            }
        }

        public bool AnyTestsLeft()
        {
            return (this.testCollection.Count != 0);
        }

        private ITestComponent FindInnerTestGroup(ITestComponent group)
        {
            HashSet<ITestComponent> set = this.testCollection[group];
            foreach (ITestComponent component in set)
            {
                if (component.IsTestGroup())
                {
                    component.EnableTest(true);
                    return this.FindInnerTestGroup(component);
                }
            }
            return group;
        }

        private ITestComponent FindNextTestGroup(ITestComponent testGroup)
        {
            if (testGroup == null)
            {
                throw new Exception("No test left");
            }
            if (Enumerable.Any<ITestComponent>(this.testCollection[testGroup]))
            {
                testGroup.EnableTest(true);
                return this.FindInnerTestGroup(testGroup);
            }
            this.testCollection.Remove(testGroup);
            testGroup.EnableTest(false);
            ITestComponent component = testGroup.GetTestGroup();
            if (component == null)
            {
                return null;
            }
            this.testCollection[component].Remove(testGroup);
            return this.FindNextTestGroup(component);
        }

        public void FinishTest(ITestComponent test)
        {
            try
            {
                test.EnableTest(false);
                this.currentTestGroup = this.FindNextTestGroup(this.currentTestGroup);
            }
            catch (MissingReferenceException exception)
            {
                Debug.LogException(exception);
            }
        }

        public ITestComponent GetNextTest()
        {
            ITestComponent item = Enumerable.First<ITestComponent>(this.testCollection[this.currentTestGroup]);
            this.testCollection[this.currentTestGroup].Remove(item);
            item.EnableTest(true);
            return item;
        }

        public List<ITestComponent> GetRemainingTests()
        {
            List<ITestComponent> list = new List<ITestComponent>();
            foreach (KeyValuePair<ITestComponent, HashSet<ITestComponent>> pair in this.testCollection)
            {
                list.AddRange(pair.Value);
            }
            return list;
        }
    }
}

