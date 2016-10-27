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

    [ConsoleCommand("petup")]
    public class CmdLevelUpPet : ICommand
    {
        private bool m_cheated;
        private string m_petId;
        private Player m_player;

        public CmdLevelUpPet(string[] serialized)
        {
            this.m_player = GameLogic.Binder.GameState.Player;
            this.m_petId = LangUtil.FirstLetterToUpper(serialized[0]);
            this.m_cheated = true;
        }

        public CmdLevelUpPet(Player player, string petId, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            this.m_player = player;
            this.m_petId = petId;
            this.m_cheated = cheated;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorA8 ra = new <executeRoutine>c__IteratorA8();
            ra.<>f__this = this;
            return ra;
        }

        public static void ExecuteStatic(Player player, string petId, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            if (!player.Pets.ownsPet(petId))
            {
                UnityEngine.Debug.LogWarning("Trying to level up pet that player doesn't own: " + petId);
            }
            else
            {
                PetInstance instance = player.Pets.getPetInstance(petId);
                int level = instance.Level + 1;
                if (level > ConfigGameplay.PET_MAX_LEVEL)
                {
                    UnityEngine.Debug.LogWarning("Trying to level up pet past level cap, skipping..");
                }
                else
                {
                    int num2 = App.Binder.ConfigMeta.PetRequiredDuplicatesForLevelUp(level);
                    if (instance.Duplicates < num2)
                    {
                        UnityEngine.Debug.LogWarning("Level-upping pet without sufficient duplicates: " + petId);
                    }
                    instance.Duplicates = Mathf.Clamp(instance.Duplicates - num2, 0, 0x7fffffff);
                    instance.Level = level;
                    GameLogic.Binder.EventBus.PetLevelUpped(player, petId, cheated);
                }
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorA8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdLevelUpPet <>f__this;

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
                    CmdLevelUpPet.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_petId, this.<>f__this.m_cheated);
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

