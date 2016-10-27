namespace Facebook.Unity.Example
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class LogView : ConsoleBase
    {
        private static string datePatt = "M/d/yyyy hh:mm:ss tt";
        private static IList<string> events = new List<string>();

        public static void AddLog(string log)
        {
            events.Insert(0, string.Format("{0}\n{1}\n", DateTime.Now.ToString(datePatt), log));
        }

        protected void OnGUI()
        {
            GUILayout.BeginVertical(new GUILayoutOption[0]);
            if (base.Button("Back"))
            {
                base.GoBack();
            }
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                Vector2 scrollPosition = base.ScrollPosition;
                scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
                base.ScrollPosition = scrollPosition;
            }
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.MinWidth((float) ConsoleBase.MainWindowFullWidth) };
            base.ScrollPosition = GUILayout.BeginScrollView(base.ScrollPosition, options);
            GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.ExpandHeight(true), GUILayout.MaxWidth((float) ConsoleBase.MainWindowWidth) };
            GUILayout.TextArea(string.Join("\n", Enumerable.ToArray<string>(events)), base.TextStyle, optionArray2);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}

