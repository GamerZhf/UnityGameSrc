namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdPushCharacter : ICommand
    {
        public const float DRAG_PER_SECOND = 8f;
        private CharacterInstance m_character;
        private Vector3 m_pushVelocity;

        public CmdPushCharacter(CharacterInstance character, Vector3 pushVelocity)
        {
            this.m_character = character;
            this.m_pushVelocity = pushVelocity;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator5E iteratore = new <executeRoutine>c__Iterator5E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator5E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdPushCharacter <>f__this;
            internal Vector3 <vel>__0;

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
                        if (!this.<>f__this.m_character.IsDead && !this.<>f__this.m_character.isInvulnerable())
                        {
                            this.<>f__this.m_character.ExternallyControlled = true;
                            this.<vel>__0 = this.<>f__this.m_pushVelocity;
                            break;
                        }
                        goto Label_0125;

                    case 1:
                        break;

                    default:
                        goto Label_0125;
                }
                while (this.<vel>__0.sqrMagnitude > 1f)
                {
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.<>f__this.m_character, this.<vel>__0, this.<>f__this.m_character.Facing);
                    PhysicsUtil.ApplyDrag(ref this.<vel>__0, 8f, Time.fixedDeltaTime);
                    this.$current = new WaitForFixedUpdate();
                    this.$PC = 1;
                    return true;
                }
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.<>f__this.m_character, Vector3.zero, this.<>f__this.m_character.Facing);
                this.<>f__this.m_character.ExternallyControlled = false;
                goto Label_0125;
                this.$PC = -1;
            Label_0125:
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

