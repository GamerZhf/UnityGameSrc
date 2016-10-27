namespace GameLogic
{
    using PlayerView;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [ConsoleCommand("ascend")]
    public class CmdCheatAscend : ICommand
    {
        public CmdCheatAscend()
        {
        }

        public CmdCheatAscend(string[] serialized)
        {
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            return new <executeRoutine>c__Iterator80();
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator80 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Player <player>__0;
            internal Reward <trigChest>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (((this.$PC == 0) && !PlayerView.Binder.MenuSystem.InTransition) && !PlayerView.Binder.TransitionSystem.InCriticalTransition)
                {
                    this.<player>__0 = GameLogic.Binder.GameState.Player;
                    this.<trigChest>__1 = this.<player>__0.getFirstUnclaimedRetirementTriggerChest();
                    if (this.<trigChest>__1 != null)
                    {
                        if (this.<trigChest>__1.getTotalTokenAmount() <= 0.0)
                        {
                            this.<trigChest>__1.addResourceDrop(ResourceType.Token, 100.0);
                        }
                    }
                    else
                    {
                        Reward item = new Reward();
                        item.ChestType = ChestType.RetirementTrigger;
                        List<double> list = new List<double>();
                        list.Add(100.0);
                        item.TokenDrops = list;
                        this.<player>__0.UnclaimedRewards.Add(item);
                    }
                    PlayerView.Binder.TransitionSystem.retire();
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

