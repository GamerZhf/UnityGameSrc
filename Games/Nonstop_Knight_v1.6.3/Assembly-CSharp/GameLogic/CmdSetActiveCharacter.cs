namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdSetActiveCharacter : ICommand
    {
        private string m_characterId;
        private Player m_player;

        public CmdSetActiveCharacter(Player player, string characterId)
        {
            this.m_player = player;
            this.m_characterId = characterId;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorB6 rb = new <executeRoutine>c__IteratorB6();
            rb.<>f__this = this;
            return rb;
        }

        public static void ExecuteStatic(Player player, string characterId)
        {
            int num = -1;
            for (int i = 0; i < player.CharacterInstances.Count; i++)
            {
                if (player.CharacterInstances[i].CharacterId == characterId)
                {
                    num = i;
                    break;
                }
            }
            if (num != -1)
            {
                player.ActiveCharacterIndex = num;
            }
            else
            {
                player.ActiveCharacterIndex = 0;
            }
            Binder.EventBus.PlayerActiveCharacterChanged(player.ActiveCharacter);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorB6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSetActiveCharacter <>f__this;

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
                    CmdSetActiveCharacter.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_characterId);
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

