namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdSaveGameStateToPersistentStorage : ICommand
    {
        public const string FILENAME = "gameState.json";
        private GameState m_gameState;

        public CmdSaveGameStateToPersistentStorage(GameState gs)
        {
            this.m_gameState = gs;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator42 iterator = new <executeRoutine>c__Iterator42();
            iterator.<>f__this = this;
            return iterator;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator42 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSaveGameStateToPersistentStorage <>f__this;
            internal string <json>__0;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    this.<json>__0 = JsonUtils.Serialize(this.<>f__this.m_gameState);
                    if (!IOUtil.SaveToPersistentStorage(this.<json>__0, "gameState.json", ConfigApp.PersistentStorageEncryptionEnabled, true))
                    {
                        UnityEngine.Debug.LogError("Failed to store game state to persistent storage.");
                    }
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

