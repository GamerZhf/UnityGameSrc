namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [ConsoleCommand("petInspect")]
    public class CmdInspectPet : ICommand
    {
        private bool m_cheated;
        private string m_petId;
        private Player m_player;

        public CmdInspectPet(string[] serialized)
        {
            this.m_player = Binder.GameState.Player;
            this.m_petId = LangUtil.FirstLetterToUpper(serialized[0]);
            this.m_cheated = true;
        }

        public CmdInspectPet(Player player, string petId, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            this.m_player = player;
            this.m_petId = petId;
            this.m_cheated = cheated;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorA4 ra = new <executeRoutine>c__IteratorA4();
            ra.<>f__this = this;
            return ra;
        }

        public static void ExecuteStatic(Player player, string petId, [Optional, DefaultParameterValue(false)] bool cheated)
        {
            if (!player.Pets.ownsPet(petId))
            {
                UnityEngine.Debug.LogWarning("Trying to inspect pet that player doesn't own: " + petId);
            }
            else
            {
                player.Pets.getPetInstance(petId).InspectedByPlayer = true;
                Binder.EventBus.PetInspected(player, petId, cheated);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorA4 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdInspectPet <>f__this;

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
                    CmdInspectPet.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_petId, this.<>f__this.m_cheated);
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

