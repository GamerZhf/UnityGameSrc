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

    public class CmdSpawnCharacter : ICommand
    {
        private static int sm_spawnCounter;

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            return new <executeRoutine>c__Iterator6B();
        }

        public static CharacterInstance ExecuteStatic(SpawningData data)
        {
            CharacterInstance character = GameLogic.Binder.CharacterPool.getObject();
            if (data.CharacterInstancePrototype != null)
            {
                character.copyFrom(data.CharacterInstancePrototype);
            }
            else
            {
                character.CharacterId = data.CharacterPrototype.Id;
            }
            character.postDeserializeInitialization();
            GameLogic.Binder.EventBus.CharacterSpawnStarted(character);
            if (data.BossPerks != null)
            {
                character.BossPerks.PerkInstances.AddRange(data.BossPerks);
            }
            if (data.SupportRunestonePerkSnapshotSource != null)
            {
                Player supportRunestonePerkSnapshotSource = data.SupportRunestonePerkSnapshotSource;
                for (int i = 0; i < supportRunestonePerkSnapshotSource.Runestones.SelectedRunestones.Count; i++)
                {
                    string id = supportRunestonePerkSnapshotSource.Runestones.SelectedRunestones[i].Id;
                    SkillType skillTypeForRunestone = ConfigRunestones.GetSkillTypeForRunestone(id);
                    ConfigRunestones.SharedData runestoneData = ConfigRunestones.GetRunestoneData(id);
                    if (((skillTypeForRunestone != SkillType.NONE) && supportRunestonePerkSnapshotSource.ActiveCharacter.isSkillActive(skillTypeForRunestone)) && (runestoneData.PerkInstance != null))
                    {
                        character.SupportPerks.PerkInstances.Add(runestoneData.PerkInstance);
                    }
                }
            }
            character.Rank = data.Rank;
            character.OwningPlayer = !data.IsPlayerCharacter ? null : GameLogic.Binder.GameState.Player;
            character.IsDead = false;
            character.IsPlayerCharacter = data.IsPlayerCharacter;
            character.IsSupport = data.IsPlayerSupportCharacter;
            character.IsPet = data.IsPet;
            if (character.IsPet)
            {
                character.IsSupport = true;
            }
            character.IsBoss = data.IsBoss;
            character.IsEliteBoss = data.IsEliteBoss;
            if (character.IsEliteBoss)
            {
                character.IsBoss = true;
            }
            character.IsWildBoss = data.IsWildBoss;
            if (character.IsWildBoss)
            {
                character.IsBoss = true;
            }
            character.IsBossClone = data.IsBossClone;
            if (data.IsBossClone)
            {
                character.IsBoss = true;
            }
            character.Id = !character.IsPlayerCharacter ? ("Enemy_" + sm_spawnCounter++) : ("Player_" + sm_spawnCounter++);
            if (data.NormalizedStartingHp > 0f)
            {
                character.CurrentHp = character.MaxLife(true) * data.NormalizedStartingHp;
            }
            else
            {
                character.CurrentHp = character.MaxLife(true);
            }
            character.PhysicsBody.name = "CharacterBody_" + character.Id;
            if (character.IsBoss)
            {
                character.PhysicsBody.CharacterController.radius = character.Radius * 1.5f;
            }
            else
            {
                character.PhysicsBody.CharacterController.radius = character.Radius;
            }
            character.PhysicsBody.CharacterController.height = 4f;
            character.PhysicsBody.CharacterController.enabled = true;
            character.assignToDefaultLayer();
            character.PhysicsBody.transform.position = data.SpawnWorldPos;
            character.PhysicsBody.transform.rotation = data.SpawnWorlRot;
            if (character.IsSupport)
            {
                character.PhysicsBody.tag = Tags.SUPPORT_CHARACTER;
            }
            else if (character.IsPlayerCharacter)
            {
                character.PhysicsBody.tag = Tags.HERO_CHARACTER;
            }
            else
            {
                character.PhysicsBody.tag = Tags.MONSTER_CHARACTER;
            }
            if (character.IsSupport)
            {
                character.PhysicsBody.Seeker.traversableTags = 0x20000001;
            }
            else if (character.IsPlayerCharacter)
            {
                character.PhysicsBody.Seeker.traversableTags = -536870911;
            }
            else
            {
                character.PhysicsBody.Seeker.traversableTags = -1073741823;
            }
            character.IsPersistent = data.IsPersistent;
            if (data.IsPersistent)
            {
                GameLogic.Binder.GameState.PersistentCharacters.Add(character);
                character.PhysicsBody.gameObject.SetActive(false);
            }
            else
            {
                GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveCharacters.Add(character);
                character.PhysicsBody.gameObject.SetActive(true);
            }
            if (data.LimitedLifetimeSeconds > 0f)
            {
                character.FutureTimeOfDeath = Time.fixedTime + data.LimitedLifetimeSeconds;
            }
            else
            {
                character.FutureTimeOfDeath = 0f;
            }
            character.Source = data.Source;
            character.resetAttackTimer();
            character.completeAttackTimer();
            GameLogic.Binder.EventBus.CharacterSpawned(character);
            return character;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator6B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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

        [StructLayout(LayoutKind.Sequential)]
        public struct SpawningData
        {
            public Character CharacterPrototype;
            public CharacterInstance CharacterInstancePrototype;
            public int Rank;
            public Vector3 SpawnWorldPos;
            public Quaternion SpawnWorlRot;
            public bool IsPlayerCharacter;
            public bool IsPlayerSupportCharacter;
            public bool IsPet;
            public bool IsBoss;
            public bool IsEliteBoss;
            public bool IsWildBoss;
            public bool IsBossClone;
            public bool IsPersistent;
            public float NormalizedStartingHp;
            public float LimitedLifetimeSeconds;
            public List<PerkInstance> BossPerks;
            public Player SupportRunestonePerkSnapshotSource;
            public object Source;
        }
    }
}

