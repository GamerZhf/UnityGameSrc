namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdGainMaterials : ICommand
    {
        private int m_amount;
        private string m_itemId;
        private Player m_player;
        public static int PLAYER_ITEM_COUNT_CAP = 0xf423f;

        public CmdGainMaterials(Player player, string itemId, int amount)
        {
            this.m_player = player;
            this.m_itemId = itemId;
            this.m_amount = amount;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator9D iteratord = new <executeRoutine>c__Iterator9D();
            iteratord.<>f__this = this;
            return iteratord;
        }

        public static void ExecuteStatic(Player player, string itemId, int amount)
        {
            if (!player.Materials.ContainsKey(itemId))
            {
                player.Materials.Add(itemId, 0);
            }
            player.Materials[itemId] = Mathf.Clamp(player.Materials[itemId] + amount, 0, PLAYER_ITEM_COUNT_CAP);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator9D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdGainMaterials <>f__this;

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
                    CmdGainMaterials.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_itemId, this.<>f__this.m_amount);
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

