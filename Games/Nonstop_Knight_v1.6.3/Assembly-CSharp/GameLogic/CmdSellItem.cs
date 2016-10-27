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

    public class CmdSellItem : ICommand
    {
        private CharacterInstance m_character;
        private RectTransform m_flyToHudOrigin;
        private ItemInstance m_itemInstance;

        public CmdSellItem(CharacterInstance character, ItemInstance itemInstance, [Optional, DefaultParameterValue(null)] RectTransform flyToHudOrigin)
        {
            this.m_character = character;
            this.m_itemInstance = itemInstance;
            this.m_flyToHudOrigin = flyToHudOrigin;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator8D iteratord = new <executeRoutine>c__Iterator8D();
            iteratord.<>f__this = this;
            return iteratord;
        }

        public static void ExecuteStatic(CharacterInstance character, ItemInstance itemInstance, [Optional, DefaultParameterValue(null)] RectTransform flyToHudOrigin)
        {
            double num3;
            double num4;
            Player owningPlayer = character.OwningPlayer;
            CharacterInstance activeCharacter = owningPlayer.ActiveCharacter;
            bool flag = false;
            List<ItemInstance> list = character.getItemInstances(false);
            if (list.Contains(itemInstance))
            {
                list.Remove(itemInstance);
                flag = true;
            }
            bool flag2 = false;
            List<ItemSlot> list2 = character.getItemSlots(false);
            for (int i = 0; i < list2.Count; i++)
            {
                ItemSlot slot = list2[i];
                if (slot.ItemInstance == itemInstance)
                {
                    flag2 = true;
                    slot.ItemInstance = null;
                    break;
                }
            }
            if (flag)
            {
            }
            double num2 = 0.0;
            App.Binder.ConfigMeta.ItemSellCurve(activeCharacter, itemInstance.Item.Type, itemInstance.Rarity, itemInstance.Level, itemInstance.Rank, out num3, out num4);
            bool visualizationManuallyControlled = flyToHudOrigin != null;
            if (num3 > 0.0)
            {
                CmdGainResources.ExecuteStatic(owningPlayer, ResourceType.Token, num3, visualizationManuallyControlled, "TRACKING_ID_GAMEPLAY_LOOT_GAIN", null);
                num2 = num3;
            }
            else if (num4 > 0.0)
            {
                CmdGainResources.ExecuteStatic(owningPlayer, ResourceType.Coin, num4, visualizationManuallyControlled, "TRACKING_ID_GAMEPLAY_LOOT_GAIN", null);
                num2 = num4;
            }
            GameLogic.Binder.EventBus.ItemSold(character, itemInstance, num2, flyToHudOrigin);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator8D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSellItem <>f__this;

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
                    CmdSellItem.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_itemInstance, this.<>f__this.m_flyToHudOrigin);
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

