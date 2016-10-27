namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ConsoleCommand("aug")]
    public class CmdGainAugmentation : ICommand
    {
        private string m_id;
        private Player m_player;

        public CmdGainAugmentation(string[] serialized)
        {
            this.m_player = Binder.GameState.Player;
            this.m_id = LangUtil.FirstLetterToUpper(serialized[0]);
        }

        public CmdGainAugmentation(Player player, string id)
        {
            this.m_player = player;
            this.m_id = id;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator9C iteratorc = new <executeRoutine>c__Iterator9C();
            iteratorc.<>f__this = this;
            return iteratorc;
        }

        public static void ExecuteStatic(Player player, string id)
        {
            PlayerAugmentations augmentations = player.Augmentations;
            if (augmentations.hasAugmentation(id))
            {
                UnityEngine.Debug.LogWarning("Trying to re-gain player augmentation: " + id);
            }
            else
            {
                PlayerAugmentationInstance ai = new PlayerAugmentationInstance(id);
                augmentations.addAugmentation(ai);
                Binder.EventBus.PlayerAugmentationGained(player, id);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator9C : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdGainAugmentation <>f__this;

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
                    CmdGainAugmentation.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_id);
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

