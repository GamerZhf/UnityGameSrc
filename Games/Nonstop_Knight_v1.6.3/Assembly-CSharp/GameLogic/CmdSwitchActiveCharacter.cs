namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdSwitchActiveCharacter : ICommand
    {
        private string m_newCharacterId;
        private Player m_player;

        public CmdSwitchActiveCharacter(Player player, string newCharacterId)
        {
            this.m_player = player;
            this.m_newCharacterId = newCharacterId;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorB8 rb = new <executeRoutine>c__IteratorB8();
            rb.<>f__this = this;
            return rb;
        }

        public static void ExecuteStatic(Player player, string newCharacterId)
        {
            player.ActiveCharacter.CharacterId = newCharacterId;
            player.ActiveCharacter.Character = Binder.CharacterResources.getResource(newCharacterId);
            Assert.IsTrue_Release(player.ActiveCharacter.Character != null, "Cannot link Character to CharacterInstance with id: " + newCharacterId);
            Binder.EventBus.PlayerActiveCharacterSwitched(player.ActiveCharacter);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorB8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSwitchActiveCharacter <>f__this;

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
                    CmdSwitchActiveCharacter.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_newCharacterId);
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

