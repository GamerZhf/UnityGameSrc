namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DecoySkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorCD rcd = new <ExecuteRoutine>c__IteratorCD();
            rcd.c = c;
            rcd.executionStats = executionStats;
            rcd.<$>c = c;
            rcd.<$>executionStats = executionStats;
            return rcd;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorCD : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal SkillExecutionStats <$>executionStats;
            internal string <charId>__0;
            internal Vector3 <dashDir>__3;
            internal IEnumerator <ie>__6;
            internal CmdSpawnCharacter.SpawningData <sd>__2;
            internal Vector3 <spawnPos>__1;
            internal Vector3 <vel>__4;
            internal float <velSqrMag>__5;
            internal CharacterInstance c;
            internal SkillExecutionStats executionStats;

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
                    {
                        this.<charId>__0 = "Support002";
                        this.<spawnPos>__1 = this.c.PhysicsBody.Transform.position;
                        this.<spawnPos>__1 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.calculateNearestEmptySpot(this.<spawnPos>__1, Vector3.zero, 1f, 1f, 6f, null);
                        CmdSpawnCharacter.SpawningData data = new CmdSpawnCharacter.SpawningData();
                        data.CharacterPrototype = GameLogic.Binder.CharacterResources.getResource(this.<charId>__0);
                        data.SpawnWorldPos = this.<spawnPos>__1;
                        data.SpawnWorlRot = this.c.PhysicsBody.Transform.rotation;
                        data.IsPlayerCharacter = true;
                        data.IsPlayerSupportCharacter = true;
                        this.<sd>__2 = data;
                        CmdSpawnCharacter.ExecuteStatic(this.<sd>__2);
                        GameLogic.Binder.EventBus.CharacterSkillExecutionMidpoint(this.c, SkillType.Decoy, this.executionStats);
                        this.<dashDir>__3 = -this.c.PhysicsBody.Transform.forward;
                        this.c.PhysicsBody.gameObject.layer = Layers.IGNORE_CHARACTERS;
                        this.<vel>__4 = (Vector3) (this.<dashDir>__3 * ConfigSkills.Decoy.MovementForce);
                        this.<velSqrMag>__5 = this.<vel>__4.sqrMagnitude;
                        break;
                    }
                    case 1:
                        break;

                    case 2:
                        goto Label_0226;

                    default:
                        goto Label_0242;
                }
                if (this.<velSqrMag>__5 > 25f)
                {
                    this.<velSqrMag>__5 = this.<vel>__4.sqrMagnitude;
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, this.<vel>__4, this.c.Facing);
                    PhysicsUtil.ApplyDrag(ref this.<vel>__4, ConfigSkills.Decoy.DragPerSecond, Time.fixedDeltaTime);
                    this.$current = new WaitForFixedUpdate();
                    this.$PC = 1;
                    goto Label_0244;
                }
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                this.<ie>__6 = TimeUtil.WaitForFixedSeconds(ConfigSkills.Decoy.PostLeapWaitTime);
            Label_0226:
                while (this.<ie>__6.MoveNext())
                {
                    this.$current = this.<ie>__6.Current;
                    this.$PC = 2;
                    goto Label_0244;
                }
                goto Label_0242;
                this.$PC = -1;
            Label_0242:
                return false;
            Label_0244:
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

