namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CheatConsole : MonoBehaviour
    {
        [CompilerGenerated]
        private static Func<System.Type, bool> <>f__am$cache4;
        [CompilerGenerated]
        private string <LastConsoleCommand>k__BackingField;
        [CompilerGenerated]
        private bool <Visible>k__BackingField;
        private Dictionary<string, System.Type> m_commandIdMapping = new Dictionary<string, System.Type>();
        private ICommandProcessor m_commandProcessor;

        private void findCheatCommandAttributes()
        {
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate (System.Type t) {
                    return t.IsClass;
                };
            }
            IEnumerator<System.Type> enumerator = Enumerable.Where<System.Type>(Assembly.GetExecutingAssembly().GetTypes(), <>f__am$cache4).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    System.Type current = enumerator.Current;
                    foreach (object obj2 in current.GetCustomAttributes(typeof(ConsoleCommandAttribute), true))
                    {
                        ConsoleCommandAttribute attribute = (ConsoleCommandAttribute) obj2;
                        this.m_commandIdMapping.Add(attribute.CommandId.ToLower(), current);
                    }
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
        }

        protected void OnGUI()
        {
            if (this.Visible)
            {
                if (string.IsNullOrEmpty(this.LastConsoleCommand))
                {
                    this.LastConsoleCommand = string.Empty;
                }
                float num = ((float) Screen.height) / 1242f;
                this.LastConsoleCommand = GUI.TextField(new Rect(20f * num, 20f * num, 900f * num, 80f * num), this.LastConsoleCommand, 50);
                bool flag = false;
                if (Event.current.isKey)
                {
                    switch (Event.current.keyCode)
                    {
                        case KeyCode.Return:
                        case KeyCode.KeypadEnter:
                            flag = true;
                            break;
                    }
                }
                if (flag)
                {
                    this.processCommand(this.LastConsoleCommand);
                    GUI.FocusControl(string.Empty);
                    this.Visible = false;
                }
                GUI.SetNextControlName(string.Empty);
            }
        }

        private void processCommand(string command)
        {
            try
            {
                char[] separator = new char[] { ' ' };
                string[] collection = command.Split(separator);
                string key = collection[0].ToLower();
                if (this.m_commandIdMapping.ContainsKey(key))
                {
                    string[] strArray2 = new List<string>(collection).GetRange(1, collection.Length - 1).ToArray();
                    System.Type type = this.m_commandIdMapping[key];
                    object[] args = new object[] { strArray2 };
                    ICommand command2 = (ICommand) Activator.CreateInstance(type, args);
                    this.m_commandProcessor.execute(command2, 0f);
                }
            }
            catch (Exception)
            {
            }
        }

        protected void Start()
        {
            this.m_commandProcessor = GameLogic.Binder.CommandProcessor;
            this.findCheatCommandAttributes();
            this.Visible = false;
        }

        protected void Update()
        {
            if ((Input.GetKeyDown(KeyCode.Backslash) || Input.GetKeyDown(KeyCode.Less)) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.Visible = !this.Visible;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                this.processCommand(this.LastConsoleCommand);
            }
        }

        private string LastConsoleCommand
        {
            [CompilerGenerated]
            get
            {
                return this.<LastConsoleCommand>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<LastConsoleCommand>k__BackingField = value;
            }
        }

        public bool Visible
        {
            [CompilerGenerated]
            get
            {
                return this.<Visible>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Visible>k__BackingField = value;
            }
        }
    }
}

