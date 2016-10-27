namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BossSplitterSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank, SkillExecutionStats executionStats)
        {
            <ExecuteRoutine>c__IteratorC6 rc = new <ExecuteRoutine>c__IteratorC6();
            rc.c = c;
            rc.<$>c = c;
            return rc;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorC6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal Vector3 <centerPos>__1;
            internal string <charId>__0;
            internal float <normalizedStartingHp>__3;
            internal CmdSpawnCharacter.SpawningData <sd>__4;
            internal Vector3 <spawnPos>__2;
            internal CharacterInstance c;

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
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.c, Vector3.zero, this.c.Facing);
                    this.c.CurrentHp *= ConfigSkills.BossSplitter.HpMultiplierPerSplit;
                    this.<charId>__0 = this.c.CharacterId;
                    this.<centerPos>__1 = this.c.PhysicsBody.Transform.position;
                    this.<spawnPos>__2 = this.<centerPos>__1 + ((Vector3) (this.c.PhysicsBody.Transform.right * 2f));
                    this.<spawnPos>__2 = GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.calculateNearestEmptySpot(this.<spawnPos>__2, this.<centerPos>__1 - this.<spawnPos>__2, 1f, 1f, 6f, null);
                    this.<normalizedStartingHp>__3 = Mathf.Clamp01((float) (this.c.CurrentHp / this.c.MaxLife(true)));
                    CmdSpawnCharacter.SpawningData data = new CmdSpawnCharacter.SpawningData();
                    data.CharacterPrototype = GameLogic.Binder.CharacterResources.getResource(this.<charId>__0);
                    data.Rank = this.c.Rank;
                    data.SpawnWorldPos = this.<spawnPos>__2;
                    data.SpawnWorlRot = this.c.PhysicsBody.Transform.rotation;
                    data.IsBoss = true;
                    data.IsWildBoss = this.c.IsWildBoss;
                    data.IsBossClone = true;
                    data.NormalizedStartingHp = this.<normalizedStartingHp>__3;
                    this.<sd>__4 = data;
                    CmdSpawnCharacter.ExecuteStatic(this.<sd>__4);
                    this.c.PhysicsBody.Transform.position = this.<centerPos>__1 - ((Vector3) (this.c.PhysicsBody.Transform.right * 2f));
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

