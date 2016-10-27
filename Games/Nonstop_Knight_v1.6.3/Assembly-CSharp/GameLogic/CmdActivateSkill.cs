namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdActivateSkill : ICommand
    {
        private float m_buildUpTime;
        private CharacterInstance m_character;
        private SkillExecutionStats m_executionStats;
        private SkillType m_skillType;
        private List<SkillType> m_tempSkillTypeList = new List<SkillType>(3);

        public CmdActivateSkill(CharacterInstance character, SkillType skillType, float buildUpTime, SkillExecutionStats executionStats)
        {
            this.m_character = character;
            this.m_skillType = skillType;
            this.m_buildUpTime = buildUpTime;
            this.m_executionStats = executionStats;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator49 iterator = new <executeRoutine>c__Iterator49();
            iterator.<>f__this = this;
            return iterator;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator49 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Dictionary<SkillType, Coroutine>.Enumerator <$s_329>__3;
            internal CmdActivateSkill <>f__this;
            internal int <i>__6;
            internal IEnumerator <ie>__0;
            internal KeyValuePair<SkillType, Coroutine> <kv>__4;
            internal int <rank>__2;
            internal SkillInstance <si>__1;
            internal SkillType <skillType>__5;
            internal IEnumerator <waitIe>__7;

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
                        this.<ie>__0 = null;
                        this.<si>__1 = this.<>f__this.m_character.getSkillInstance(this.<>f__this.m_skillType);
                        if (this.<si>__1 == null)
                        {
                            this.<rank>__2 = 1;
                            break;
                        }
                        this.<rank>__2 = this.<si>__1.Rank;
                        break;

                    case 1:
                        goto Label_085A;

                    case 2:
                        goto Label_08C2;

                    case 3:
                        if (ConfigSkills.SHARED_DATA[this.<>f__this.m_skillType].ExternalControl)
                        {
                            this.<>f__this.m_character.assignToDefaultLayer();
                            this.<>f__this.m_character.ExternallyControlled = false;
                        }
                        this.<>f__this.m_character.SkillRoutines.Remove(this.<>f__this.m_skillType);
                        GameLogic.Binder.EventBus.CharacterSkillExecuted(this.<>f__this.m_character, this.<>f__this.m_skillType, this.<>f__this.m_executionStats);
                        goto Label_09A5;

                    default:
                        goto Label_09A5;
                }
                switch (this.<>f__this.m_skillType)
                {
                    case SkillType.Leap:
                        LeapSkill.PreExecute(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Omnislash:
                        OmnislashSkill.PreExecute(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;
                }
                if (this.<>f__this.m_executionStats.PreExecuteFailed)
                {
                    goto Label_09A5;
                }
                if (ConfigSkills.SHARED_DATA[this.<>f__this.m_skillType].ExternalControl)
                {
                    if (this.<>f__this.m_character.isAttacking())
                    {
                        CmdStopAttack.ExecuteStatic(this.<>f__this.m_character);
                    }
                    this.<>f__this.m_tempSkillTypeList.Clear();
                    this.<$s_329>__3 = this.<>f__this.m_character.SkillRoutines.GetEnumerator();
                    try
                    {
                        while (this.<$s_329>__3.MoveNext())
                        {
                            this.<kv>__4 = this.<$s_329>__3.Current;
                            this.<skillType>__5 = this.<kv>__4.Key;
                            if ((this.<skillType>__5 != this.<>f__this.m_skillType) && ConfigSkills.SHARED_DATA[this.<skillType>__5].ExternalControl)
                            {
                                this.<>f__this.m_tempSkillTypeList.Add(this.<skillType>__5);
                            }
                        }
                    }
                    finally
                    {
                        this.<$s_329>__3.Dispose();
                    }
                    this.<i>__6 = 0;
                    while (this.<i>__6 < this.<>f__this.m_tempSkillTypeList.Count)
                    {
                        CmdStopSkill.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_tempSkillTypeList[this.<i>__6]);
                        this.<i>__6++;
                    }
                }
                switch (this.<>f__this.m_skillType)
                {
                    case SkillType.Leap:
                        this.<ie>__0 = LeapSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Whirlwind:
                        this.<ie>__0 = WhirlwindSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Frenzy:
                        this.<ie>__0 = FrenzySkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2);
                        break;

                    case SkillType.Heal:
                        this.<ie>__0 = HealSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Fireball:
                        this.<ie>__0 = FireballSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2);
                        break;

                    case SkillType.Blast:
                        this.<ie>__0 = BlastSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Dash:
                        this.<ie>__0 = DashSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Decoy:
                        this.<ie>__0 = DecoySkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Vanish:
                        this.<ie>__0 = VanishSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Omnislash:
                        this.<ie>__0 = OmnislashSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Slam:
                        this.<ie>__0 = SlamSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Cluster:
                        this.<ie>__0 = ClusterSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Implosion:
                        this.<ie>__0 = ImplosionSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Clone:
                        this.<ie>__0 = CloneSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2);
                        break;

                    case SkillType.Midas:
                        this.<ie>__0 = MidasSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2);
                        break;

                    case SkillType.Shield:
                        this.<ie>__0 = ShieldSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2);
                        break;

                    case SkillType.BossSummoner:
                        this.<ie>__0 = BossSummonerSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.BossDefender:
                        this.<ie>__0 = BossDefenderSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.BossLeader:
                        this.<ie>__0 = BossLeaderSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.BossSplitter:
                        this.<ie>__0 = BossSplitterSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.BossSlam:
                        this.<ie>__0 = BossSlamSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.PoisonPuff:
                        this.<ie>__0 = PoisonPuffSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.BossBreeder:
                        this.<ie>__0 = BossBreederSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.BossEscaper:
                        this.<ie>__0 = BossEscaperSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.BossBreederEscaper:
                        this.<ie>__0 = BossBreederEscaperSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Charge:
                        this.<ie>__0 = ChargeSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;

                    case SkillType.Explosion:
                        this.<ie>__0 = ExplosionSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats, 0.25f);
                        break;

                    case SkillType.Escape:
                        this.<ie>__0 = EscapeSkill.ExecuteRoutine(this.<>f__this.m_character, this.<rank>__2, this.<>f__this.m_executionStats);
                        break;
                }
                GameLogic.Binder.EventBus.CharacterSkillActivated(this.<>f__this.m_character, this.<>f__this.m_skillType, this.<>f__this.m_buildUpTime, this.<>f__this.m_executionStats);
                this.<>f__this.m_character.ExternallyControlled = ConfigSkills.SHARED_DATA[this.<>f__this.m_skillType].ExternalControl;
                if (this.<>f__this.m_buildUpTime <= 0f)
                {
                    goto Label_086A;
                }
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.<>f__this.m_character, Vector3.zero, this.<>f__this.m_character.Facing);
                this.<waitIe>__7 = TimeUtil.WaitForFixedSeconds(this.<>f__this.m_buildUpTime);
            Label_085A:
                while (this.<waitIe>__7.MoveNext())
                {
                    this.$current = this.<waitIe>__7.Current;
                    this.$PC = 1;
                    goto Label_09A7;
                }
            Label_086A:
                GameLogic.Binder.EventBus.CharacterSkillBuildupCompleted(this.<>f__this.m_character, this.<>f__this.m_skillType, this.<>f__this.m_executionStats);
                if (this.<ie>__0 == null)
                {
                    UnityEngine.Debug.LogError("Skill not executable: " + this.<>f__this.m_skillType);
                    goto Label_08F6;
                }
            Label_08C2:
                while (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 2;
                    goto Label_09A7;
                }
            Label_08F6:
                this.$current = new WaitForFixedUpdate();
                this.$PC = 3;
                goto Label_09A7;
                this.$PC = -1;
            Label_09A5:
                return false;
            Label_09A7:
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

