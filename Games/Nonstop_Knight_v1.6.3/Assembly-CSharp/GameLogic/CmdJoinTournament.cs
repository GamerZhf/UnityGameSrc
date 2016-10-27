namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdJoinTournament : ICommand
    {
        private Player m_player;
        private TournamentInfo m_tournamentInfo;

        public CmdJoinTournament(Player player, TournamentInfo tournamentInfo)
        {
            this.m_player = player;
            this.m_tournamentInfo = tournamentInfo;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorA7 ra = new <executeRoutine>c__IteratorA7();
            ra.<>f__this = this;
            return ra;
        }

        public static TournamentInstance ExecuteStatic(Player player, TournamentInfo tournamentInfo)
        {
            if (player.Tournaments.ActiveInstances.ContainsKey(tournamentInfo.Id))
            {
                UnityEngine.Debug.LogError("Tried to join a tournament that is already an active tournament");
                return null;
            }
            TournamentInstance instance = new TournamentInstance(tournamentInfo, player);
            instance.postDeserializeInitialization();
            DungeonRuleset rulesetForId = ConfigDungeonModifiers.GetRulesetForId(tournamentInfo.TournamentRulesetId);
            if (rulesetForId != null)
            {
                for (int i = 0; i < rulesetForId.DungeonModifiers.Count; i++)
                {
                    DungeonModifierType type = rulesetForId.DungeonModifiers[i];
                    switch (type)
                    {
                        case DungeonModifierType.StartingItemLegionnaire:
                        case DungeonModifierType.StartingItemBeast:
                        case DungeonModifierType.StartingItemGoldie:
                        case DungeonModifierType.StartingItemSpike:
                        case DungeonModifierType.StartingItemSureshot:
                        case DungeonModifierType.StartingItemGrande:
                        case DungeonModifierType.StartingItemNightmare:
                        case DungeonModifierType.StartingItemBaron:
                        case DungeonModifierType.StartingItemMojo:
                        case DungeonModifierType.StartingItemSnowflake:
                        {
                            DungeonModifier modifier = ConfigDungeonModifiers.MODIFIERS[type];
                            for (int j = 0; j < instance.ItemSlots.Count; j++)
                            {
                                if (instance.ItemSlots[j].CompatibleItemType == modifier.Parameter_ItemType)
                                {
                                    ItemInstance instance2 = new ItemInstance(modifier.Parameter_String, 1, modifier.Parameter_Int - 1, 0, player);
                                    instance2.InspectedByPlayer = true;
                                    instance2.Unlocked = true;
                                    instance.ItemSlots[j].ItemInstance = instance2;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            player.Tournaments.ActiveInstances.Add(tournamentInfo.Id, instance);
            CmdRefreshVendorInventory.ExecuteStatic(player, true);
            return instance;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorA7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdJoinTournament <>f__this;

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
                    CmdJoinTournament.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_tournamentInfo);
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

