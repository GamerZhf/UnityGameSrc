using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TestResultRenderer
{
    [CompilerGenerated]
    private static Func<KeyValuePair<string, List<ITestResult>>, int> <>f__am$cache4;
    [CompilerGenerated]
    private static Func<ITestResult, bool> <>f__am$cache5;
    [CompilerGenerated]
    private static Func<ITestResult, string> <>f__am$cache6;
    private int m_FailureCount;
    private Vector2 m_ScrollPosition;
    private bool m_ShowResults;
    private readonly Dictionary<string, List<ITestResult>> m_TestCollection = new Dictionary<string, List<ITestResult>>();

    public void AddResults(string sceneName, ITestResult result)
    {
        if (!this.m_TestCollection.ContainsKey(sceneName))
        {
            this.m_TestCollection.Add(sceneName, new List<ITestResult>());
        }
        this.m_TestCollection[sceneName].Add(result);
        if (result.Executed && !result.IsSuccess)
        {
            this.m_FailureCount++;
        }
    }

    public void Draw()
    {
        if (this.m_ShowResults)
        {
            if (this.m_TestCollection.Count == 0)
            {
                GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(600f) };
                GUILayout.Label("All test succeeded", Styles.SucceedLabelStyle, options);
            }
            else
            {
                if (<>f__am$cache4 == null)
                {
                    <>f__am$cache4 = delegate (KeyValuePair<string, List<ITestResult>> testGroup) {
                        return testGroup.Value.Count;
                    };
                }
                GUILayout.Label(Enumerable.Sum<KeyValuePair<string, List<ITestResult>>>(this.m_TestCollection, <>f__am$cache4) + " tests failed!", Styles.FailedLabelStyle, new GUILayoutOption[0]);
                GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.ExpandWidth(true) };
                this.m_ScrollPosition = GUILayout.BeginScrollView(this.m_ScrollPosition, optionArray2);
                string text = string.Empty;
                foreach (KeyValuePair<string, List<ITestResult>> pair in this.m_TestCollection)
                {
                    text = text + "<b><size=18>" + pair.Key + "</size></b>\n";
                    if (<>f__am$cache5 == null)
                    {
                        <>f__am$cache5 = delegate (ITestResult result) {
                            return !result.IsSuccess;
                        };
                    }
                    if (<>f__am$cache6 == null)
                    {
                        <>f__am$cache6 = delegate (ITestResult result) {
                            object[] objArray1 = new object[] { result.Name, " ", result.ResultState, "\n", result.Message };
                            return string.Concat(objArray1);
                        };
                    }
                    text = text + string.Join("\n", Enumerable.ToArray<string>(Enumerable.Select<ITestResult, string>(Enumerable.Where<ITestResult>(pair.Value, <>f__am$cache5), <>f__am$cache6)));
                }
                GUILayout.TextArea(text, Styles.FailedMessagesStyle, new GUILayoutOption[0]);
                GUILayout.EndScrollView();
            }
            if (GUILayout.Button("Close", new GUILayoutOption[0]))
            {
                Application.Quit();
            }
        }
    }

    public int FailureCount()
    {
        return this.m_FailureCount;
    }

    public void ShowResults()
    {
        this.m_ShowResults = true;
        Cursor.visible = true;
    }

    private static class Styles
    {
        public static readonly GUIStyle FailedLabelStyle;
        public static readonly GUIStyle FailedMessagesStyle;
        public static readonly GUIStyle SucceedLabelStyle = new GUIStyle("label");

        static Styles()
        {
            SucceedLabelStyle.normal.textColor = Color.green;
            SucceedLabelStyle.fontSize = 0x30;
            FailedLabelStyle = new GUIStyle("label");
            FailedLabelStyle.normal.textColor = Color.red;
            FailedLabelStyle.fontSize = 0x20;
            FailedMessagesStyle = new GUIStyle("label");
            FailedMessagesStyle.wordWrap = false;
            FailedMessagesStyle.richText = true;
        }
    }
}

