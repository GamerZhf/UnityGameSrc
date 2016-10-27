namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ConsoleCommand("pet")]
    public class CmdGainPet : ICommand
    {
        private bool m_cheated;
        private int m_count;
        private string m_petId;
        private Player m_player;

        public CmdGainPet(string[] serialized)
        {
            this.m_player = Binder.GameState.Player;
            this.m_petId = LangUtil.FirstLetterToUpper(serialized[0]);
            this.m_count = 5;
            this.m_cheated = false;
        }

        public CmdGainPet(Player player, string petId, int count, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            this.m_player = player;
            this.m_petId = petId;
            this.m_count = count;
            this.m_cheated = cheated;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator9E iteratore = new <executeRoutine>c__Iterator9E();
            iteratore.<>f__this = this;
            return iteratore;
        }

        public static void ExecuteStatic(Player player, string petId, int count, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            if (count > 0)
            {
                if (player.Pets.ownsPet(petId))
                {
                    PetInstance instance = player.Pets.getPetInstance(petId);
                    instance.Duplicates += count;
                    if (cheated)
                    {
                        instance.InspectedByPlayer = true;
                    }
                }
                else
                {
                    PetInstance instance3 = new PetInstance();
                    instance3.CharacterId = petId;
                    instance3.Duplicates = count;
                    instance3.InspectedByPlayer = cheated;
                    PetInstance item = instance3;
                    item.postDeserializeInitialization();
                    player.Pets.Instances.Add(item);
                }
                Binder.EventBus.PetGained(player, petId, cheated);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator9E : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdGainPet <>f__this;

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
                    CmdGainPet.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_petId, this.<>f__this.m_count, this.<>f__this.m_cheated);
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

