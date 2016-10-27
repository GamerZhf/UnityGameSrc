namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CmdHitDestructibles : ICommand
    {
        private SkillType m_fromSkill;
        private Vector3 m_position;
        private float m_radius;
        private CharacterInstance m_sourceCharacter;

        public CmdHitDestructibles(CharacterInstance sourceCharacter, Vector3 position, float radius, [Optional, DefaultParameterValue(0)] SkillType fromSkill)
        {
            this.m_sourceCharacter = sourceCharacter;
            this.m_position = position;
            this.m_radius = radius;
            this.m_fromSkill = fromSkill;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator58 iterator = new <executeRoutine>c__Iterator58();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance sourceCharacter, Vector3 position, float radius, [Optional, DefaultParameterValue(0)] SkillType fromSkill)
        {
            if (sourceCharacter.IsPlayerCharacter)
            {
                foreach (Collider collider in Physics.OverlapSphere(position, radius, Layers.DestructibleLayerMask))
                {
                    DestructiblePhysicsBody component = collider.GetComponent<DestructiblePhysicsBody>();
                    if (component != null)
                    {
                        component.Hit(fromSkill);
                    }
                }
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator58 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdHitDestructibles <>f__this;

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
                    CmdHitDestructibles.ExecuteStatic(this.<>f__this.m_sourceCharacter, this.<>f__this.m_position, this.<>f__this.m_radius, this.<>f__this.m_fromSkill);
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

