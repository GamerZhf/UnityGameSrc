namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdRerollItemPerks : ICommand
    {
        private CharacterInstance m_character;
        private ItemInstance m_itemInstance;

        public CmdRerollItemPerks(CharacterInstance character, ItemInstance itemInstance)
        {
            this.m_character = character;
            this.m_itemInstance = itemInstance;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator7C iteratorc = new <executeRoutine>c__Iterator7C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        public static void ExecuteStatic(CharacterInstance character, ItemInstance itemInstance)
        {
            Player owningPlayer = character.OwningPlayer;
            double diamondCost = App.Binder.ConfigMeta.ItemRerollDiamondCostCurve(itemInstance.Rarity, itemInstance.RerollCount);
            CmdGainResources.ExecuteStatic(owningPlayer, ResourceType.Diamond, -diamondCost, false, string.Empty, null);
            for (int i = 0; i < itemInstance.Perks.PerkInstances.Count; i++)
            {
                CmdRerollItemPerk.ExecuteStatic(itemInstance, i, owningPlayer);
            }
            itemInstance.RerollCount++;
            GameLogic.Binder.EventBus.ItemPerksRerolled(owningPlayer, itemInstance, diamondCost);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator7C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRerollItemPerks <>f__this;

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
                    CmdRerollItemPerks.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_itemInstance);
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

