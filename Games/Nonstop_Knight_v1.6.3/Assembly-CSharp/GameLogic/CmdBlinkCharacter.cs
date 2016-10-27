namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdBlinkCharacter : ICommand
    {
        private CharacterInstance m_character;
        private Vector3 m_targetWorldPt;
        private float m_waitBefore;

        public CmdBlinkCharacter(CharacterInstance character, Vector3 targetWorldPt, float waitBefore)
        {
            this.m_character = character;
            this.m_targetWorldPt = targetWorldPt;
            this.m_waitBefore = waitBefore;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator4C iteratorc = new <executeRoutine>c__Iterator4C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator4C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdBlinkCharacter <>f__this;
            internal ActiveDungeon <ad>__1;
            internal Vector3 <finalPosXz>__2;
            internal IEnumerator <ie>__3;
            internal IEnumerator <waitIe>__0;

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
                        this.<>f__this.m_character.ExternallyControlled = true;
                        CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.<>f__this.m_character, Vector3.zero, this.<>f__this.m_character.Facing);
                        if (this.<>f__this.m_waitBefore <= 0f)
                        {
                            goto Label_00C4;
                        }
                        this.<waitIe>__0 = TimeUtil.WaitForFixedSeconds(this.<>f__this.m_waitBefore);
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_016B;

                    case 3:
                        this.<>f__this.m_character.PhysicsBody.Transform.position = this.<finalPosXz>__2;
                        CmdGainHp.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_character.MaxLife(true), true);
                        this.$current = new WaitForFixedUpdate();
                        this.$PC = 4;
                        goto Label_0284;

                    case 4:
                        Binder.EventBus.CharacterBlinked(this.<>f__this.m_character);
                        this.<ie>__3 = this.<>f__this.m_character.PhysicsBody.spinAroundRoutine(1, 0.2f, Easing.Function.OUT_CUBIC);
                        goto Label_0244;

                    case 5:
                        goto Label_0244;

                    default:
                        goto Label_0282;
                }
                if (this.<waitIe>__0.MoveNext())
                {
                    this.$current = this.<waitIe>__0.Current;
                    this.$PC = 1;
                    goto Label_0284;
                }
            Label_00C4:
                this.<ad>__1 = Binder.GameState.ActiveDungeon;
                this.<finalPosXz>__2 = this.<ad>__1.ActiveRoom.calculateNearestEmptySpot(this.<>f__this.m_targetWorldPt, Vector3.zero, 1f, 1f, 6f, null);
                Binder.EventBus.CharacterPreBlink(this.<>f__this.m_character);
                this.<ie>__3 = this.<>f__this.m_character.PhysicsBody.spinAroundRoutine(1, 0.2f, Easing.Function.IN_CUBIC);
            Label_016B:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 2;
                    goto Label_0284;
                }
                this.$current = new WaitForFixedUpdate();
                this.$PC = 3;
                goto Label_0284;
            Label_0244:
                while (this.<ie>__3.MoveNext())
                {
                    this.$current = this.<ie>__3.Current;
                    this.$PC = 5;
                    goto Label_0284;
                }
                this.<>f__this.m_character.ExternallyControlled = false;
                this.<>f__this.m_character.BlinkRoutine = null;
                goto Label_0282;
                this.$PC = -1;
            Label_0282:
                return false;
            Label_0284:
                return true;
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

