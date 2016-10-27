namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdInitializePlayer : ICommand
    {
        private Player m_player;

        public CmdInitializePlayer(Player player)
        {
            this.m_player = player;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorA2 ra = new <executeRoutine>c__IteratorA2();
            ra.<>f__this = this;
            return ra;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorA2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdInitializePlayer <>f__this;
            internal CharacterInstance <character>__7;
            internal int <i>__1;
            internal int <i>__3;
            internal int <i>__6;
            internal int <j>__8;
            internal ConfigRunestones.SharedData <runestoneData>__2;
            internal long <secondsNow>__0;
            internal ConfigSkills.SharedData <skillData>__5;
            internal SkillType <skillType>__4;
            internal SkillType <skillType>__9;

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
                    this.<secondsNow>__0 = Service.Binder.ServerTime.GameTime;
                    this.<>f__this.m_player.Version = ConfigApp.InternalClientVersion;
                    this.<>f__this.m_player.LastSerializationTimestampDuringDeserialization = this.<>f__this.m_player.LastSerializationTimestamp;
                    this.<>f__this.m_player.Runestones.Player = this.<>f__this.m_player;
                    this.<i>__1 = 0;
                    while (this.<i>__1 < ConfigRunestones.RUNESTONES.Length)
                    {
                        this.<runestoneData>__2 = ConfigRunestones.RUNESTONES[this.<i>__1];
                        if ((this.<>f__this.m_player.Rank >= this.<runestoneData>__2.UnlockRank) && !this.<>f__this.m_player.Runestones.ownsRunestone(this.<runestoneData>__2.Id))
                        {
                            CmdGainRunestone.ExecuteStatic(this.<>f__this.m_player, this.<runestoneData>__2.Id, false);
                        }
                        this.<i>__1++;
                    }
                    this.<i>__3 = 0;
                    while (this.<i>__3 < ConfigSkills.ALL_HERO_SKILLS.Count)
                    {
                        this.<skillType>__4 = ConfigSkills.ALL_HERO_SKILLS[this.<i>__3];
                        this.<skillData>__5 = ConfigSkills.SHARED_DATA[this.<skillType>__4];
                        if (((this.<>f__this.m_player.Rank >= this.<skillData>__5.UnlockRank) && !this.<>f__this.m_player.UnlockedSkills.Contains(this.<skillType>__4)) && !this.<>f__this.m_player.PendingSkillUnlocks.Contains(this.<skillType>__4))
                        {
                            CmdUnlockSkill.ExecuteStatic(this.<>f__this.m_player, this.<skillType>__4, false);
                        }
                        this.<i>__3++;
                    }
                    this.<i>__6 = 0;
                    while (this.<i>__6 < this.<>f__this.m_player.CharacterInstances.Count)
                    {
                        this.<character>__7 = this.<>f__this.m_player.CharacterInstances[this.<i>__6];
                        this.<j>__8 = 0;
                        while (this.<j>__8 < this.<>f__this.m_player.UnlockedSkills.Count)
                        {
                            this.<skillType>__9 = this.<>f__this.m_player.UnlockedSkills[this.<j>__8];
                            if (this.<character>__7.getSkillInstance(this.<skillType>__9) == null)
                            {
                                SkillInstance item = new SkillInstance();
                                item.Rank = 1;
                                item.SkillType = this.<skillType>__9;
                                this.<character>__7.SkillInstances.Add(item);
                            }
                            this.<j>__8++;
                        }
                        this.<i>__6++;
                    }
                    if (this.<>f__this.m_player.LastPassiveRewardClaimTimestamp <= 0L)
                    {
                        this.<>f__this.m_player.LastPassiveRewardClaimTimestamp = this.<secondsNow>__0;
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

