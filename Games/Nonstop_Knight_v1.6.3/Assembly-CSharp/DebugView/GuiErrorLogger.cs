namespace DebugView
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GuiErrorLogger : MonoBehaviour
    {
        private List<string> m_errors = new List<string>();
        private Vector2 m_scrollPosition = Vector2.zero;
        private List<string> m_warnings = new List<string>();

        protected void OnDisable()
        {
            Application.logMessageReceived -= new Application.LogCallback(this.onLogMessageReceived);
        }

        protected void OnEnable()
        {
            Application.logMessageReceived += new Application.LogCallback(this.onLogMessageReceived);
        }

        protected void OnGUI()
        {
            if (!Application.isEditor && ((this.m_errors.Count != 0) || (this.m_warnings.Count != 0)))
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;
                style.fontSize = (int) (Screen.height * 0.025f);
                int fontSize = style.fontSize;
                int num2 = 0;
                int num3 = 0;
                int num4 = Screen.width - 10;
                int num5 = (fontSize * (this.m_errors.Count + this.m_warnings.Count)) + 20;
                this.m_scrollPosition = GUI.BeginScrollView(new Rect(10f, Screen.height * 0.19f, (float) (Screen.width - 20), Screen.height - (Screen.height * 0.36f)), this.m_scrollPosition, new Rect(0f, 0f, 75000f, (float) num5));
                for (int i = 0; i < this.m_errors.Count; i++)
                {
                    style.normal.textColor = new Color(1f, 0.1f, 0.1f, 1f);
                    GUI.Label(new Rect((float) num2, (float) num3, (float) num4, (float) num5), this.m_errors[i], style);
                    num3 += fontSize;
                }
                for (int j = 0; j < this.m_warnings.Count; j++)
                {
                    style.normal.textColor = new Color(1f, 1f, 0f, 1f);
                    GUI.Label(new Rect((float) num2, (float) num3, (float) num4, (float) num5), this.m_warnings[j], style);
                    num3 += fontSize;
                }
                GUI.EndScrollView();
            }
        }

        private void onLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            condition = condition.Replace("\n", " ");
            stackTrace = stackTrace.Replace("\n", " ");
            if (((type == LogType.Error) || (type == LogType.Exception)) || (type == LogType.Assert))
            {
                this.m_errors.Add(condition + " -- " + stackTrace);
            }
            else if (type == LogType.Warning)
            {
                this.m_warnings.Add(condition + " -- " + stackTrace);
            }
        }
    }
}

