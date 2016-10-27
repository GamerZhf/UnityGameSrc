namespace Facebook.Unity.Example
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class ConsoleBase : MonoBehaviour
    {
        [CompilerGenerated]
        private Texture2D <LastResponseTexture>k__BackingField;
        protected static int ButtonHeight = (!Constants.IsMobile ? 0x18 : 60);
        private GUIStyle buttonStyle;
        private const int DpiScalingFactor = 160;
        private GUIStyle labelStyle;
        private string lastResponse = string.Empty;
        protected static int MainWindowFullWidth = (!Constants.IsMobile ? 760 : Screen.width);
        protected static int MainWindowWidth = (!Constants.IsMobile ? 700 : (Screen.width - 30));
        protected static int MarginFix = (!Constants.IsMobile ? 0x30 : 0);
        private static Stack<string> menuStack = new Stack<string>();
        private float? scaleFactor;
        private Vector2 scrollPosition = Vector2.zero;
        private string status = "Ready";
        private GUIStyle textInputStyle;
        private GUIStyle textStyle;

        protected virtual void Awake()
        {
            Application.targetFrameRate = 60;
        }

        protected bool Button(string label)
        {
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.MinHeight(ButtonHeight * this.ScaleFactor), GUILayout.MaxWidth((float) MainWindowWidth) };
            return GUILayout.Button(label, this.ButtonStyle, options);
        }

        protected void GoBack()
        {
            if (Enumerable.Any<string>(menuStack))
            {
                Application.LoadLevel(menuStack.Pop());
            }
        }

        protected bool IsHorizontalLayout()
        {
            return (Screen.orientation == ScreenOrientation.LandscapeLeft);
        }

        protected void LabelAndTextField(string label, ref string text)
        {
            GUILayout.BeginHorizontal(new GUILayoutOption[0]);
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.MaxWidth(200f * this.ScaleFactor) };
            GUILayout.Label(label, this.LabelStyle, options);
            GUILayoutOption[] optionArray2 = new GUILayoutOption[] { GUILayout.MaxWidth((float) (MainWindowWidth - 150)) };
            text = GUILayout.TextField(text, this.TextInputStyle, optionArray2);
            GUILayout.EndHorizontal();
        }

        protected void SwitchMenu(System.Type menuClass)
        {
            menuStack.Push(base.GetType().Name);
            Application.LoadLevel(menuClass.Name);
        }

        protected GUIStyle ButtonStyle
        {
            get
            {
                if (this.buttonStyle == null)
                {
                    this.buttonStyle = new GUIStyle(GUI.skin.button);
                    this.buttonStyle.fontSize = this.FontSize;
                }
                return this.buttonStyle;
            }
        }

        protected int FontSize
        {
            get
            {
                return (int) Math.Round((double) (this.ScaleFactor * 16f));
            }
        }

        protected GUIStyle LabelStyle
        {
            get
            {
                if (this.labelStyle == null)
                {
                    this.labelStyle = new GUIStyle(GUI.skin.label);
                    this.labelStyle.fontSize = this.FontSize;
                }
                return this.labelStyle;
            }
        }

        protected string LastResponse
        {
            get
            {
                return this.lastResponse;
            }
            set
            {
                this.lastResponse = value;
            }
        }

        protected Texture2D LastResponseTexture
        {
            [CompilerGenerated]
            get
            {
                return this.<LastResponseTexture>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<LastResponseTexture>k__BackingField = value;
            }
        }

        protected static Stack<string> MenuStack
        {
            get
            {
                return menuStack;
            }
            set
            {
                menuStack = value;
            }
        }

        protected float ScaleFactor
        {
            get
            {
                if (!this.scaleFactor.HasValue)
                {
                    this.scaleFactor = new float?(Screen.dpi / 160f);
                }
                return this.scaleFactor.Value;
            }
        }

        protected Vector2 ScrollPosition
        {
            get
            {
                return this.scrollPosition;
            }
            set
            {
                this.scrollPosition = value;
            }
        }

        protected string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        protected GUIStyle TextInputStyle
        {
            get
            {
                if (this.textInputStyle == null)
                {
                    this.textInputStyle = new GUIStyle(GUI.skin.textField);
                    this.textInputStyle.fontSize = this.FontSize;
                }
                return this.textInputStyle;
            }
        }

        protected GUIStyle TextStyle
        {
            get
            {
                if (this.textStyle == null)
                {
                    this.textStyle = new GUIStyle(GUI.skin.textArea);
                    this.textStyle.alignment = TextAnchor.UpperLeft;
                    this.textStyle.wordWrap = true;
                    this.textStyle.padding = new RectOffset(10, 10, 10, 10);
                    this.textStyle.stretchHeight = true;
                    this.textStyle.stretchWidth = false;
                    this.textStyle.fontSize = this.FontSize;
                }
                return this.textStyle;
            }
        }
    }
}

