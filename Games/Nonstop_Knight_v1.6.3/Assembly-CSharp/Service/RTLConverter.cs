namespace Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class RTLConverter : MonoBehaviour
    {
        private List<IRTLProcessor> customProcessors = new List<IRTLProcessor>();
        private const char LINE_BREAK = '\n';
        private List<UILineInfo> m_lines = new List<UILineInfo>();
        private bool m_lock;
        private string m_processedText;
        private Text m_textComp;

        [DebuggerHidden]
        private IEnumerator AddForcedLineBrakes(string input)
        {
            <AddForcedLineBrakes>c__Iterator231 iterator = new <AddForcedLineBrakes>c__Iterator231();
            iterator.input = input;
            iterator.<$>input = input;
            iterator.<>f__this = this;
            return iterator;
        }

        private void ExecuteProcessorsOnEnd(ref string output)
        {
            foreach (IRTLProcessor processor in this.customProcessors)
            {
                if (!processor.OnEnd(ref output))
                {
                    break;
                }
            }
        }

        private bool ExecuteProcessorsOnStart(ref string input)
        {
            foreach (IRTLProcessor processor in this.customProcessors)
            {
                if (!processor.OnStart(ref input))
                {
                    return false;
                }
            }
            return true;
        }

        private void FixAlignment(ref Text text)
        {
            if (text.alignment == TextAnchor.LowerLeft)
            {
                text.alignment = TextAnchor.LowerRight;
            }
            else if (text.alignment == TextAnchor.UpperLeft)
            {
                text.alignment = TextAnchor.UpperRight;
            }
            else if (text.alignment == TextAnchor.MiddleLeft)
            {
                text.alignment = TextAnchor.MiddleRight;
            }
        }

        private char[] GetCharSubset(int startIndex, int length, char[] input)
        {
            char[] chArray = new char[length];
            int index = startIndex;
            for (int i = 0; index < (startIndex + length); i++)
            {
                chArray[i] = input[index];
                index++;
            }
            return chArray;
        }

        public void Init(Text textComp, [Optional, DefaultParameterValue(null)] List<IRTLProcessor> customProcessors)
        {
            this.m_textComp = textComp;
            this.m_textComp.RegisterDirtyVerticesCallback(new UnityAction(this.OnDirty));
            if (customProcessors != null)
            {
                this.customProcessors = customProcessors;
            }
        }

        private void OnDirty()
        {
            this.ProcessTextComp();
        }

        public void ProcessTextComp()
        {
            if (this.m_textComp != null)
            {
                this.m_textComp.cachedTextGenerator.GetLines(this.m_lines);
                if (((this.m_lines.Count >= 2) && !string.Equals(this.m_processedText, this.m_textComp.text)) && !this.m_lock)
                {
                    this.m_lock = true;
                    this.FixAlignment(ref this.m_textComp);
                    Binder.TaskManager.StartTask(this.AddForcedLineBrakes(this.m_textComp.text), null);
                    Binder.TaskManager.StartTask(this.Unlock(), null);
                }
            }
        }

        public static string Reverse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            char[] chArray = new char[input.Length];
            int index = 0;
            for (int i = input.Length - 1; index < input.Length; i--)
            {
                if ((((input[i] >= 0xdc00) && (input[i] <= 0xdfff)) && ((i > 0) && (input[i - 1] >= 0xd800))) && (input[i - 1] <= 0xdbff))
                {
                    chArray[index + 1] = input[i];
                    chArray[index] = input[i - 1];
                    index++;
                    i--;
                }
                else
                {
                    chArray[index] = input[i];
                }
                index++;
            }
            return new string(chArray);
        }

        private void SetTextToComp(string text)
        {
            if (this.m_textComp != null)
            {
                this.m_textComp.text = text;
            }
        }

        [DebuggerHidden]
        private IEnumerator Unlock()
        {
            <Unlock>c__Iterator230 iterator = new <Unlock>c__Iterator230();
            iterator.<>f__this = this;
            return iterator;
        }

        [CompilerGenerated]
        private sealed class <AddForcedLineBrakes>c__Iterator231 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string <$>input;
            internal RTLConverter <>f__this;
            internal List<char[]> <charLines>__5;
            internal char[] <charsWithLineBreak>__13;
            internal Vector2 <copy>__2;
            internal int <i>__14;
            internal int <i>__6;
            internal int <idx>__12;
            internal char[] <inputCharsReversed>__3;
            internal int <j>__16;
            internal int <j>__7;
            internal int <lenght>__10;
            internal char[] <line>__11;
            internal char[] <line>__15;
            internal UILineInfo <lineInfo>__8;
            internal int <nextCharIdx>__9;
            internal Vector2 <rect>__1;
            internal string <reversed>__0;
            internal int <startChar>__4;
            internal string input;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        if (this.<>f__this.ExecuteProcessorsOnStart(ref this.input))
                        {
                            this.<reversed>__0 = RTLConverter.Reverse(this.input);
                            this.<>f__this.m_textComp.cachedTextGenerator.Invalidate();
                            this.<rect>__1 = this.<>f__this.m_textComp.rectTransform.rect.size;
                            this.<copy>__2 = new Vector2(this.<rect>__1.x * 0.95f, this.<rect>__1.y);
                            this.<>f__this.m_textComp.cachedTextGenerator.Populate(this.<reversed>__0, this.<>f__this.m_textComp.GetGenerationSettings(this.<copy>__2));
                            this.<>f__this.m_textComp.cachedTextGenerator.GetLines(this.<>f__this.m_lines);
                            if (this.<>f__this.m_lines.Count < 2)
                            {
                                this.<>f__this.m_processedText = this.input;
                                this.<>f__this.ExecuteProcessorsOnEnd(ref this.<>f__this.m_processedText);
                                this.<>f__this.SetTextToComp(this.<>f__this.m_processedText);
                            }
                            else
                            {
                                this.<inputCharsReversed>__3 = this.<reversed>__0.ToCharArray();
                                this.<startChar>__4 = 0;
                                this.<charLines>__5 = new List<char[]>();
                                this.<i>__6 = 1;
                                this.<j>__7 = 0;
                                while (this.<i>__6 < this.<>f__this.m_lines.Count)
                                {
                                    this.<lineInfo>__8 = this.<>f__this.m_lines[this.<i>__6];
                                    this.<nextCharIdx>__9 = this.<lineInfo>__8.startCharIdx;
                                    this.<lenght>__10 = this.<nextCharIdx>__9 - this.<startChar>__4;
                                    this.<lenght>__10 = Math.Min(this.<lenght>__10, this.<inputCharsReversed>__3.Length - this.<startChar>__4);
                                    this.<line>__11 = this.<>f__this.GetCharSubset(this.<startChar>__4, this.<lenght>__10, this.<inputCharsReversed>__3);
                                    this.<startChar>__4 = this.<lineInfo>__8.startCharIdx;
                                    this.<charLines>__5.Add(this.<line>__11);
                                    this.<i>__6++;
                                    this.<j>__7++;
                                }
                                this.<charLines>__5.Add(this.<>f__this.GetCharSubset(this.<startChar>__4, this.<inputCharsReversed>__3.Length - this.<startChar>__4, this.<inputCharsReversed>__3));
                                this.<charLines>__5.Reverse();
                                this.<idx>__12 = 0;
                                this.<charsWithLineBreak>__13 = new char[(this.<inputCharsReversed>__3.Length + this.<>f__this.m_lines.Count) - 1];
                                this.<i>__14 = this.<charLines>__5.Count - 1;
                                while (this.<i>__14 >= 0)
                                {
                                    this.<line>__15 = this.<charLines>__5[this.<i>__14];
                                    Array.Reverse(this.<line>__15);
                                    this.<j>__16 = 0;
                                    while (this.<j>__16 < this.<line>__15.Length)
                                    {
                                        this.<charsWithLineBreak>__13[this.<idx>__12++] = this.<line>__15[this.<j>__16];
                                        this.<j>__16++;
                                    }
                                    if (this.<i>__14 != 0)
                                    {
                                        this.<charsWithLineBreak>__13[this.<idx>__12++] = '\n';
                                    }
                                    this.<i>__14--;
                                }
                                this.<>f__this.m_processedText = new string(this.<charsWithLineBreak>__13);
                                this.<>f__this.ExecuteProcessorsOnEnd(ref this.<>f__this.m_processedText);
                                this.<>f__this.SetTextToComp(this.<>f__this.m_processedText);
                                this.$current = 0;
                                this.$PC = 1;
                                return true;
                            }
                            break;
                        }
                        this.<>f__this.m_processedText = this.input;
                        this.<>f__this.SetTextToComp(this.<>f__this.m_processedText);
                        break;

                    case 1:
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <Unlock>c__Iterator230 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RTLConverter <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = 0;
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.<>f__this.m_lock = false;
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

