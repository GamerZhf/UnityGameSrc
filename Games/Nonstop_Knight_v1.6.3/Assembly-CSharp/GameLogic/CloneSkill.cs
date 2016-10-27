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

    public class CloneSkill
    {
        public static string CLONE_CHARACTER_ID = "Support001";

        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank)
        {
            <ExecuteRoutine>c__IteratorCA rca = new <ExecuteRoutine>c__IteratorCA();
            rca.c = c;
            rca.<$>c = c;
            return rca;
        }

        public static void Summon(CharacterInstance sourceCharacter, Vector3 spawnPos, [Optional, DefaultParameterValue(null)] object sourceObject, [Optional, DefaultParameterValue(0x7fffffff)] int sourceQuota)
        {
            int num = 0;
            CharacterInstance target = null;
            for (int i = 0; i < GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveCharacters.Count; i++)
            {
                CharacterInstance instance2 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveCharacters[i];
                if ((!instance2.IsDead && (instance2.Prefab == CharacterPrefab.KnightClone)) && (instance2.Source == sourceObject))
                {
                    num++;
                    if ((target == null) || ((instance2.FutureTimeOfDeath > 0f) && (instance2.FutureTimeOfDeath < target.FutureTimeOfDeath)))
                    {
                        target = instance2;
                    }
                }
            }
            if (num >= sourceQuota)
            {
                GameLogic.Binder.DeathSystem.killCharacter(target, null, false, false, SkillType.NONE);
            }
            string id = CLONE_CHARACTER_ID;
            Character summonedCharacterPrototype = GameLogic.Binder.CharacterResources.getResource(id);
            float num3 = sourceCharacter.getLimitedLifetimeForSummon(summonedCharacterPrototype);
            float num4 = (sourceCharacter.getPerkInstanceCount(PerkType.SkillUpgradeClone3) <= 0) ? ConfigSkills.Clone.BaseHPMultiplier : ConfigSkills.Clone.DecoyHPMultiplier;
            CmdSpawnCharacter.SpawningData data2 = new CmdSpawnCharacter.SpawningData();
            data2.CharacterPrototype = summonedCharacterPrototype;
            data2.SpawnWorldPos = spawnPos;
            data2.SpawnWorlRot = sourceCharacter.PhysicsBody.Transform.rotation;
            data2.IsPlayerCharacter = true;
            data2.IsPlayerSupportCharacter = true;
            data2.NormalizedStartingHp = num4;
            data2.LimitedLifetimeSeconds = num3;
            data2.SupportRunestonePerkSnapshotSource = sourceCharacter.OwningPlayer;
            data2.Source = sourceObject;
            CmdSpawnCharacter.SpawningData data = data2;
            CharacterInstance instance3 = CmdSpawnCharacter.ExecuteStatic(data);
            Buff buff3 = new Buff();
            buff3.HudSprite = "skill_buff_icon";
            buff3.DurationSeconds = num3;
            buff3.SourceCharacter = instance3;
            Buff buff = buff3;
            GameLogic.Binder.BuffSystem.startBuff(sourceCharacter, buff);
            if (sourceCharacter.getPerkInstanceCount(PerkType.SkillUpgradeClone1) > 0)
            {
                buff3 = new Buff();
                buff3.HealingPerSecond = sourceCharacter.MaxLife(true) * sourceCharacter.getGenericModifierForPerkType(PerkType.SkillUpgradeClone1);
                buff3.DurationSeconds = num3;
                buff3.FromPerk = PerkType.SkillUpgradeClone1;
                buff3.SourceCharacter = instance3;
                Buff buff2 = buff3;
                GameLogic.Binder.BuffSystem.startBuff(sourceCharacter, buff2);
            }
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorCA : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal Vector3 <charPosXz>__1;
            internal int <i>__0;
            internal IEnumerator <ie>__5;
            internal Vector3 <spawnPos>__4;
            internal Vector3 <targetPosXz>__2;
            internal Vector3 <toTargetDirXz>__3;
            internal IEnumerator <waitIe>__6;
            internal CharacterInstance c;

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
                        CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                        this.<i>__0 = 0;
                        goto Label_0285;

                    case 1:
                        break;

                    case 2:
                        goto Label_02C7;

                    default:
                        goto Label_02E3;
                }
            Label_0267:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 1;
                    goto Label_02E5;
                }
            Label_0277:
                this.<i>__0++;
            Label_0285:
                if (this.<i>__0 < ConfigSkills.Clone.NumberOfClones)
                {
                    this.<charPosXz>__1 = Vector3Extensions.ToXzVector3(this.c.PhysicsBody.Transform.position);
                    if ((this.c.TargetCharacter != null) && (Vector3.Distance(this.c.PhysicsBody.Transform.position, this.c.TargetCharacter.PhysicsBody.Transform.position) <= ConfigSkills.Clone.BehindSummonDistance))
                    {
                        this.<targetPosXz>__2 = Vector3Extensions.ToXzVector3(this.c.TargetCharacter.PhysicsBody.Transform.position);
                        Vector3 vector = this.<targetPosXz>__2 - this.<charPosXz>__1;
                        this.<toTargetDirXz>__3 = vector.normalized;
                        this.<spawnPos>__4 = this.<targetPosXz>__2 + ((Vector3) (this.<toTargetDirXz>__3 * 1.25f));
                    }
                    else
                    {
                        this.<spawnPos>__4 = this.<charPosXz>__1 + ((Vector3) (this.c.PhysicsBody.Transform.forward * ConfigSkills.Clone.DefaultSummonDistance));
                        this.<spawnPos>__4.x += UnityEngine.Random.Range((float) -1.5f, (float) 1.5f);
                        this.<spawnPos>__4.z += UnityEngine.Random.Range((float) -1.5f, (float) 1.5f);
                    }
                    int? mask = null;
                    this.<spawnPos>__4 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.calculateNearestEmptySpot(this.<spawnPos>__4, this.c.PhysicsBody.Transform.position - this.<spawnPos>__4, 1f, 1f, 6f, mask);
                    CloneSkill.Summon(this.c, this.<spawnPos>__4, this.c.getSkillInstance(SkillType.Clone), 1 + this.c.getSkillExtraCharges(SkillType.Clone));
                    if ((ConfigSkills.Clone.NumberOfClones <= 1) || (ConfigSkills.Clone.WaitBetweenSummons <= 0f))
                    {
                        goto Label_0277;
                    }
                    this.<ie>__5 = TimeUtil.WaitForFixedSeconds(ConfigSkills.Clone.WaitBetweenSummons);
                    goto Label_0267;
                }
                this.<waitIe>__6 = TimeUtil.WaitForFixedSeconds(ConfigSkills.Clone.WaitAfterSummon);
            Label_02C7:
                while (this.<waitIe>__6.MoveNext())
                {
                    this.$current = this.<waitIe>__6.Current;
                    this.$PC = 2;
                    goto Label_02E5;
                }
                goto Label_02E3;
                this.$PC = -1;
            Label_02E3:
                return false;
            Label_02E5:
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

