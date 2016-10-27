namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdSetCharacterVelocityAndFacing : ICommand
    {
        private CharacterInstance m_character;
        private Vector3 m_facing;
        private Vector3 m_velocity;

        public CmdSetCharacterVelocityAndFacing(CharacterInstance character, Vector3 velocity, Vector3 facing)
        {
            this.m_character = character;
            this.m_velocity = velocity;
            this.m_facing = facing;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator67 iterator = new <executeRoutine>c__Iterator67();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance character, Vector3 velocity, Vector3 facing)
        {
            character.Velocity = velocity;
            character.Facing = facing;
            Binder.EventBus.CharacterVelocityUpdated(character);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator67 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSetCharacterVelocityAndFacing <>f__this;

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
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_velocity, this.<>f__this.m_facing);
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

