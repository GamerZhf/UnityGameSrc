namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdRankUpItem : ICommand
    {
        private CharacterInstance m_character;
        private bool m_free;
        private ItemInstance m_itemInstance;
        private int m_rankUpCount;

        public CmdRankUpItem(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free)
        {
            this.m_character = character;
            this.m_itemInstance = itemInstance;
            this.m_rankUpCount = rankUpCount;
            this.m_free = free;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator7A iteratora = new <executeRoutine>c__Iterator7A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        public static void ExecuteStatic(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free)
        {
            Player player = Binder.GameState.Player;
            if (itemInstance.isAtMaxRank())
            {
                UnityEngine.Debug.LogWarning("Item instance already at max rank, cannot rank-up: " + itemInstance.ItemId);
            }
            else
            {
                float num = Mathf.Clamp01((float) (character.CurrentHp / character.MaxLife(true)));
                int rank = itemInstance.Rank + rankUpCount;
                itemInstance.Rank = rank;
                character.CurrentHp = num * character.MaxLife(true);
                if (!free)
                {
                    double num4 = player.ActiveCharacter.getAdjustedItemUpgradeCost(itemInstance.Item.Type, player.getRiggedItemLevel(itemInstance), rank);
                    CmdGainResources.ExecuteStatic(player, ResourceType.Coin, -num4, false, string.Empty, null);
                }
                player.ActiveCharacter.updateHighestLevelItemOwned();
                Binder.EventBus.ItemRankUpped(character, itemInstance, rankUpCount, free);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator7A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRankUpItem <>f__this;

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
                    CmdRankUpItem.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_itemInstance, this.<>f__this.m_rankUpCount, this.<>f__this.m_free);
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

