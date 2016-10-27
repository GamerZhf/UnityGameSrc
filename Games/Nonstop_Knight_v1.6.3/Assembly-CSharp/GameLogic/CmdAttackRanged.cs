namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdAttackRanged : ICommand
    {
        private CharacterInstance m_sourceCharacter;
        private Vector3 m_targetWorldPt;
        public const float PROJECTILE_LAUNCH_BUILDUP = 0.2f;

        public CmdAttackRanged(CharacterInstance sourceCharacter, Vector3 targetWorldPt)
        {
            this.m_sourceCharacter = sourceCharacter;
            this.m_targetWorldPt = targetWorldPt;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator4B iteratorb = new <executeRoutine>c__Iterator4B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator4B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdAttackRanged <>f__this;
            internal float <buffModifier>__6;
            internal IEnumerator <ie>__0;
            internal Projectile <p>__4;
            internal float <projectileCurveHeight>__3;
            internal Vector3 <projectileSpawnPt>__1;
            internal Vector3 <projectileTargetPt>__2;
            internal Projectile.ProjectileProperties <props>__5;

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
                        this.<>f__this.m_sourceCharacter.resetAttackTimer();
                        this.<>f__this.m_sourceCharacter.AttackSourceCounter++;
                        GameLogic.Binder.EventBus.CharacterRangedAttackStarted(this.<>f__this.m_sourceCharacter, this.<>f__this.m_targetWorldPt);
                        this.<ie>__0 = TimeUtil.WaitForFixedSeconds(0.2f);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0360;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                if (this.<>f__this.m_sourceCharacter.IsDead)
                {
                    UnityEngine.Debug.LogError("Mid-CmdAttackRanged source character is dead (this coroutine should've been stopped already): " + this.<>f__this.m_sourceCharacter.Id);
                }
                else
                {
                    ConfigGameplay.GetRangedCharacterProjectileSetup(this.<>f__this.m_sourceCharacter, this.<>f__this.m_targetWorldPt, out this.<projectileSpawnPt>__1, out this.<projectileTargetPt>__2, out this.<projectileCurveHeight>__3);
                    this.<p>__4 = GameLogic.Binder.ProjectilePool.getObject();
                    this.<p>__4.gameObject.SetActive(true);
                    this.<props>__5 = new Projectile.ProjectileProperties(ConfigGameplay.PROJECTILE_PROTOTYPE_PROPERTIES[this.<>f__this.m_sourceCharacter.Character.RangedProjectileType]);
                    this.<props>__5.DamagePerHit = this.<>f__this.m_sourceCharacter.DamagePerHit(true);
                    this.<buffModifier>__6 = GameLogic.Binder.BuffSystem.getBaseStatModifierSumFromActiveBuffs(this.<>f__this.m_sourceCharacter, BaseStatProperty.AttacksPerSecond);
                    if (this.<buffModifier>__6 > 0f)
                    {
                        this.<props>__5.Speed *= 1f + this.<buffModifier>__6;
                    }
                    if ((this.<>f__this.m_sourceCharacter.IsBoss && (this.<>f__this.m_sourceCharacter.getPerkInstanceCount(PerkType.BossCriticalHit) > 0)) && ((this.<>f__this.m_sourceCharacter.AttackSourceCounter % Mathf.RoundToInt(this.<>f__this.m_sourceCharacter.getGenericModifierForPerkType(PerkType.BossCriticalHit))) == 0))
                    {
                        this.<props>__5.DamagePerHit *= this.<>f__this.m_sourceCharacter.CriticalHitMultiplier(true);
                        this.<props>__5.IsCriticalHit = true;
                    }
                    if ((this.<>f__this.m_sourceCharacter.IsBoss && (this.<>f__this.m_sourceCharacter.getPerkInstanceCount(PerkType.BossStun) > 0)) && ((this.<>f__this.m_sourceCharacter.AttackSourceCounter % Mathf.RoundToInt(this.<>f__this.m_sourceCharacter.getGenericModifierForPerkType(PerkType.BossStun))) == 0))
                    {
                        this.<props>__5.StunDurationSeconds = ConfigPerks.SHARED_DATA[PerkType.BossStun].DurationSeconds;
                    }
                    this.<p>__4.spawnUsingCurve(this.<>f__this.m_sourceCharacter, this.<projectileSpawnPt>__1, this.<projectileTargetPt>__2, this.<projectileCurveHeight>__3, this.<props>__5);
                    GameLogic.Binder.EventBus.CharacterRangedAttackEnded(this.<>f__this.m_sourceCharacter, this.<>f__this.m_targetWorldPt, this.<p>__4);
                    GameLogic.Binder.CommandProcessor.stopCommand(this.<>f__this.m_sourceCharacter.PhysicsBody, ref this.<>f__this.m_sourceCharacter.AttackRoutine);
                    goto Label_0360;
                    this.$PC = -1;
                }
            Label_0360:
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

