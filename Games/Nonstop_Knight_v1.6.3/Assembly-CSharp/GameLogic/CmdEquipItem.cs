namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdEquipItem : ICommand
    {
        private CharacterInstance m_character;
        private ItemInstance m_itemInstance;

        public CmdEquipItem(CharacterInstance character, ItemInstance itemInstance)
        {
            this.m_character = character;
            this.m_itemInstance = itemInstance;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator88 iterator = new <executeRoutine>c__Iterator88();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance character, ItemInstance itemInstance)
        {
            List<ItemInstance> list = character.getItemInstances(false);
            if (list.Contains(itemInstance))
            {
                list.Remove(itemInstance);
            }
            else
            {
                UnityEngine.Debug.LogWarning("Trying to equip item which is not in character inventory: " + itemInstance.ItemId);
                return;
            }
            float num = Mathf.Clamp01((float) (character.CurrentHp / character.MaxLife(true)));
            ItemInstance item = null;
            bool flag = false;
            ItemType type = itemInstance.Item.Type;
            List<ItemSlot> list2 = character.getItemSlots(false);
            for (int i = 0; i < list2.Count; i++)
            {
                ItemSlot slot = list2[i];
                if (slot.CompatibleItemType == type)
                {
                    if (slot.ItemInstance != null)
                    {
                        item = slot.ItemInstance;
                        list.Add(item);
                    }
                    slot.ItemInstance = itemInstance;
                    flag = true;
                    break;
                }
            }
            character.CurrentHp = num * character.MaxLife(true);
            Binder.EventBus.ItemEquipped(character, itemInstance, item);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator88 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdEquipItem <>f__this;

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
                    CmdEquipItem.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_itemInstance);
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

