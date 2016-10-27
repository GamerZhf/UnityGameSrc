namespace DebugView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DebugViewContext : Context
    {
        [CompilerGenerated]
        private DebugView.EmailErrorLogger <EmailErrorLogger>k__BackingField;
        [CompilerGenerated]
        private DebugView.GuiErrorLogger <GuiErrorLogger>k__BackingField;

        [DebuggerHidden]
        protected override IEnumerator mapBindings(bool allocatePersistentObjectPools)
        {
            <mapBindings>c__Iterator3A iteratora = new <mapBindings>c__Iterator3A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        private void onCharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            if (critted)
            {
            }
        }

        private void onCharacterHpGained(CharacterInstance c, double amount, bool silent)
        {
        }

        private void onCharacterMeleeAttackEnded(CharacterInstance c, CharacterInstance targetCharacter, Vector3 contactWorldPt, int killCount)
        {
        }

        protected override void onCleanup()
        {
            this.GuiErrorLogger = null;
            this.EmailErrorLogger = null;
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnCharacterDealtDamage -= new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnCharacterHpGained -= new GameLogic.Events.CharacterHpGained(this.onCharacterHpGained);
            eventBus.OnCharacterMeleeAttackEnded -= new GameLogic.Events.CharacterMeleeAttackEnded(this.onCharacterMeleeAttackEnded);
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnCharacterDealtDamage += new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnCharacterHpGained += new GameLogic.Events.CharacterHpGained(this.onCharacterHpGained);
            eventBus.OnCharacterMeleeAttackEnded += new GameLogic.Events.CharacterMeleeAttackEnded(this.onCharacterMeleeAttackEnded);
        }

        public DebugView.EmailErrorLogger EmailErrorLogger
        {
            [CompilerGenerated]
            get
            {
                return this.<EmailErrorLogger>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<EmailErrorLogger>k__BackingField = value;
            }
        }

        public DebugView.GuiErrorLogger GuiErrorLogger
        {
            [CompilerGenerated]
            get
            {
                return this.<GuiErrorLogger>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<GuiErrorLogger>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <mapBindings>c__Iterator3A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal DebugViewContext <>f__this;

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
                    this.<>f__this.EmailErrorLogger = this.<>f__this.createPersistentGameObject<EmailErrorLogger>(this.<>f__this.Tm);
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

