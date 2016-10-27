namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ConsoleCommand("potion")]
    public class CmdGainPotions : ICommand
    {
        private int m_amount;
        private CharacterInstance m_character;
        private PotionType m_potionType;

        public CmdGainPotions(string[] serialized)
        {
            this.m_character = Binder.GameState.Player.ActiveCharacter;
            this.m_potionType = (PotionType) ((int) Enum.Parse(typeof(PotionType), LangUtil.FirstLetterToUpper(serialized[0].ToLower())));
            this.m_amount = 1;
        }

        public CmdGainPotions(CharacterInstance character, PotionType potionType, int amount)
        {
            this.m_character = character;
            this.m_potionType = potionType;
            this.m_amount = amount;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator9F iteratorf = new <executeRoutine>c__Iterator9F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public static void ExecuteStatic(CharacterInstance character, PotionType potionType, int amount)
        {
            Player owningPlayer = character.OwningPlayer;
            switch (potionType)
            {
                case PotionType.Revive:
                    character.Inventory.RevivePotions = Mathf.Clamp(character.Inventory.RevivePotions + amount, 0, 0x7fffffff);
                    break;

                case PotionType.Frenzy:
                    character.Inventory.FrenzyPotions = Mathf.Clamp(character.Inventory.FrenzyPotions + amount, 0, 0x7fffffff);
                    if (owningPlayer != null)
                    {
                        owningPlayer.Notifiers.PotionsInspected = false;
                    }
                    break;

                case PotionType.Xp:
                    character.Inventory.XpPotions = Mathf.Clamp(character.Inventory.XpPotions + amount, 0, 0x7fffffff);
                    break;

                case PotionType.Boss:
                    character.Inventory.BossPotions = Mathf.Clamp(character.Inventory.BossPotions + amount, 0, 0x7fffffff);
                    if (owningPlayer != null)
                    {
                        owningPlayer.Notifiers.PotionsInspected = false;
                    }
                    break;

                default:
                    UnityEngine.Debug.LogError("Unsupported potion type: " + potionType);
                    break;
            }
            Binder.EventBus.PotionsGained(character, potionType, amount);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator9F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdGainPotions <>f__this;

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
                    CmdGainPotions.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_potionType, this.<>f__this.m_amount);
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

