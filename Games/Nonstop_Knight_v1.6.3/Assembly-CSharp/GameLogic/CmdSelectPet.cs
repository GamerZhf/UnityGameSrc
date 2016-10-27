namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdSelectPet : ICommand
    {
        private string m_characterId;
        private Player m_player;

        public CmdSelectPet(Player player, string characterId)
        {
            this.m_player = player;
            this.m_characterId = characterId;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorB3 rb = new <executeRoutine>c__IteratorB3();
            rb.<>f__this = this;
            return rb;
        }

        public static void ExecuteStatic(Player player, string characterId)
        {
            PetInstance instance = player.Pets.getSelectedPetInstance();
            if ((instance != null) && (instance.SpawnedCharacterInstance != null))
            {
                Binder.DeathSystem.killCharacter(instance.SpawnedCharacterInstance, null, false, true, SkillType.NONE);
            }
            PetInstance pet = null;
            if (!string.IsNullOrEmpty(characterId))
            {
                if (!player.Pets.ownsPet(characterId))
                {
                    UnityEngine.Debug.LogError("Trying to select a non-owned pet: " + characterId);
                    return;
                }
                pet = player.Pets.getPetInstance(characterId);
                player.Pets.SelectedPet = player.Pets.getPetIndex(pet);
            }
            else
            {
                player.Pets.SelectedPet = -1;
            }
            Binder.EventBus.PetSelected(player, pet);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorB3 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSelectPet <>f__this;

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
                    CmdSelectPet.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_characterId);
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

