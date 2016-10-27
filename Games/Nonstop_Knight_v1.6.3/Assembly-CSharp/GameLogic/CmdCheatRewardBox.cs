namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [ConsoleCommand("box")]
    public class CmdCheatRewardBox : ICommand
    {
        public CmdCheatRewardBox()
        {
        }

        public CmdCheatRewardBox(string[] serialized)
        {
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            return new <executeRoutine>c__Iterator84();
        }

        public static void ExecuteStatic()
        {
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            if (player.getFirstUnclaimedRetirementTriggerChest() == null)
            {
                Reward reward2 = new Reward();
                reward2.ChestType = ChestType.RetirementTrigger;
                List<double> list = new List<double>();
                list.Add(App.Binder.ConfigMeta.RetirementTokenReward(activeCharacter, 50));
                reward2.TokenDrops = list;
                Reward item = reward2;
                player.UnclaimedRewards.Add(item);
            }
            CmdRollDropLootTable.ExecuteStatic(App.Binder.ConfigLootTables.BossAdditionalDropLootTable, player, player.ActiveCharacter.PhysicsBody.Transform.position, CharacterType.UNSPECIFIED, ChestType.RewardBoxCommon.ToString(), ChestType.NONE);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator84 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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
                    CmdCheatRewardBox.ExecuteStatic();
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

