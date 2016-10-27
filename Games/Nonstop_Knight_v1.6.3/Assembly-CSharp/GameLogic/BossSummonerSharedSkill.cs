namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BossSummonerSharedSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(SkillType skillType, CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorC7 rc = new <ExecuteRoutine>c__IteratorC7();
            rc.c = c;
            rc.skillType = skillType;
            rc.executionStats = executionStats;
            rc.<$>c = c;
            rc.<$>skillType = skillType;
            rc.<$>executionStats = executionStats;
            return rc;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorC7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal SkillType <$>skillType;
            internal ActiveDungeon <ad>__0;
            internal float <angle>__12;
            internal Vector3 <backPosition>__6;
            internal Vector3 <backPositionOffset>__7;
            internal int <count>__3;
            internal DamageType <damageType>__4;
            internal int <i>__8;
            internal IEnumerator <ie>__14;
            internal string <minionCharacterId>__10;
            internal DamageType <minionDamageType>__9;
            internal int <numAllowedUntilLimit>__2;
            internal Vector3 <position>__5;
            internal Vector2 <random>__13;
            internal ConfigSkills.BossSummonerSkillSharedData <skillData>__1;
            internal Vector3 <summonPosition>__11;
            internal CharacterInstance c;
            internal SkillExecutionStats executionStats;
            internal SkillType skillType;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<skillData>__1 = ConfigSkills.BOSS_SUMMONER_SKILLS[this.skillType];
                        this.<numAllowedUntilLimit>__2 = ConfigSkills.BossSummoner.SummonCountLimit - this.<ad>__0.ActiveRoom.numberOfFriendlyCharactersAlive(this.c, false);
                        this.<count>__3 = Mathf.Min(this.<numAllowedUntilLimit>__2, this.<skillData>__1.SummonCount);
                        if (this.<count>__3 != 0)
                        {
                            this.<damageType>__4 = !this.c.UsesRangedAttack ? DamageType.Melee : DamageType.Ranged;
                            this.<position>__5 = this.c.PhysicsBody.Transform.position;
                            this.<backPosition>__6 = this.<position>__5 + ((Vector3) (this.c.PhysicsBody.Transform.forward * -ConfigSkills.BossSummoner.SummonRadius));
                            this.<backPositionOffset>__7 = Vector3Extensions.ToXzVector3(this.<backPosition>__6 - this.<position>__5);
                            this.<i>__8 = 0;
                            while (this.<i>__8 < this.<count>__3)
                            {
                                this.<minionDamageType>__9 = DamageType.UNSPECIFIED;
                                switch (this.<skillData>__1.MinionType)
                                {
                                    case BossSummonerMinionType.Inherit:
                                        this.<minionDamageType>__9 = this.<damageType>__4;
                                        break;

                                    case BossSummonerMinionType.Melee:
                                        this.<minionDamageType>__9 = DamageType.Melee;
                                        break;

                                    case BossSummonerMinionType.Ranged:
                                        this.<minionDamageType>__9 = DamageType.Ranged;
                                        break;

                                    case BossSummonerMinionType.Mixed:
                                        this.<minionDamageType>__9 = (UnityEngine.Random.value <= 0.5f) ? DamageType.Ranged : DamageType.Melee;
                                        break;
                                }
                                if (!GameLogic.Binder.CharacterResources.hasDamageTypeCharacterIds(this.c.Type, this.<minionDamageType>__9))
                                {
                                    this.<minionDamageType>__9 = this.<damageType>__4;
                                }
                                this.<minionCharacterId>__10 = GameLogic.Binder.CharacterResources.getRandomCharacterId(this.<minionDamageType>__9, this.c.Type, this.<ad>__0.Dungeon.Theme);
                                Vector3 vector = new Vector3();
                                this.<summonPosition>__11 = vector;
                                if (this.<minionDamageType>__9 == DamageType.Ranged)
                                {
                                    this.<angle>__12 = ((this.<i>__8 % 2) != 0) ? -17f : 17f;
                                    this.<summonPosition>__11 = this.<position>__5 + (Quaternion.Euler(0f, this.<angle>__12 * this.<i>__8, 0f) * this.<backPositionOffset>__7);
                                }
                                else
                                {
                                    this.<random>__13 = (Vector2) (UnityEngine.Random.insideUnitCircle * ConfigSkills.BossSummoner.SummonRadius);
                                    this.<summonPosition>__11.x = this.<position>__5.x + this.<random>__13.x;
                                    this.<summonPosition>__11.z += this.<position>__5.z + this.<random>__13.y;
                                }
                                int? mask = null;
                                this.<summonPosition>__11 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.calculateNearestEmptySpot(this.<summonPosition>__11, this.<position>__5 - this.<summonPosition>__11, 1f, 1f, 6f, mask);
                                CmdSpawnCharacter.SpawningData data = new CmdSpawnCharacter.SpawningData();
                                data.CharacterPrototype = GameLogic.Binder.CharacterResources.getResource(this.<minionCharacterId>__10);
                                data.Rank = Mathf.Clamp(this.c.Rank - 2, 1, 0x7fffffff);
                                data.SpawnWorldPos = this.<summonPosition>__11;
                                data.SpawnWorlRot = this.c.PhysicsBody.Transform.rotation;
                                CmdSpawnCharacter.ExecuteStatic(data);
                                this.<i>__8++;
                            }
                            GameLogic.Binder.EventBus.CharacterSkillExecutionMidpoint(this.c, this.skillType, this.executionStats);
                            if (this.<skillData>__1.EscapeAfterCasting)
                            {
                                this.<ie>__14 = this.c.PhysicsBody.leapBackRoutine(0.1f);
                                break;
                            }
                        }
                        goto Label_0433;

                    case 1:
                        break;

                    default:
                        goto Label_0433;
                }
                while (this.<ie>__14.MoveNext())
                {
                    this.$current = this.<ie>__14.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_0433;
                this.$PC = -1;
            Label_0433:
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

