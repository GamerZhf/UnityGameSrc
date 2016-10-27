namespace GameLogic
{
    using App;
    using PlayerView;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class CmdActivateDungeonBoost : ICommand
    {
        private DungeonBoost m_dungeonBoost;
        private SkillType m_fromSkill;

        public CmdActivateDungeonBoost(DungeonBoost dungeonBoost, [Optional, DefaultParameterValue(0)] SkillType fromSkill)
        {
            this.m_dungeonBoost = dungeonBoost;
            this.m_fromSkill = fromSkill;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator48 iterator = new <executeRoutine>c__Iterator48();
            iterator.<>f__this = this;
            return iterator;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator48 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdActivateDungeonBoost <>f__this;
            internal ActiveDungeon <ad>__0;
            internal CharacterInstance <character>__6;
            internal float <durationSeconds>__7;
            internal int <i>__5;
            internal IEnumerator <ie>__10;
            internal double <modifier>__8;
            internal ConfigPerks.SharedData <perkData>__4;
            internal PerkType <perkType>__3;
            internal Player <player>__1;
            internal CharacterInstance <playerCharacter>__2;
            internal Vector3 <position>__9;
            internal Reward <reward>__11;
            internal ShopEntry <shopEntry>__13;
            internal string <shopEntryId>__12;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<player>__1 = GameLogic.Binder.GameState.Player;
                        this.<playerCharacter>__2 = this.<player>__1.ActiveCharacter;
                        this.<perkType>__3 = this.<>f__this.m_dungeonBoost.Properties.BuffPerkType;
                        this.<perkData>__4 = (this.<perkType>__3 == PerkType.NONE) ? null : ConfigPerks.SHARED_DATA[this.<perkType>__3];
                        switch (this.<>f__this.m_dungeonBoost.Properties.Type)
                        {
                            case DungeonBoostType.BuffBox:
                                this.<i>__5 = 0;
                                while (this.<i>__5 < this.<ad>__0.ActiveRoom.ActiveCharacters.Count)
                                {
                                    this.<character>__6 = this.<ad>__0.ActiveRoom.ActiveCharacters[this.<i>__5];
                                    if ((!this.<character>__6.IsDead && !this.<character>__6.IsSupport) && (this.<perkData>__4.TargetsEnemies == !this.<character>__6.isFriendlyTowards(this.<playerCharacter>__2)))
                                    {
                                        this.<durationSeconds>__7 = this.<perkData>__4.DurationSeconds;
                                        this.<modifier>__8 = ConfigPerks.GetBestModifier(this.<perkType>__3);
                                        switch (this.<perkType>__3)
                                        {
                                            case PerkType.DungeonBoostEnrageEnemies:
                                                this.<durationSeconds>__7 = 1000f;
                                                break;

                                            case PerkType.DungeonBoostPoison:
                                                this.<modifier>__8 *= this.<playerCharacter>__2.SkillDamage(true);
                                                break;
                                        }
                                        BuffSource source = new BuffSource();
                                        source.Object = this.<>f__this.m_dungeonBoost;
                                        GameLogic.Binder.BuffSystem.startBuffFromPerk(this.<character>__6, this.<perkType>__3, this.<durationSeconds>__7, this.<modifier>__8, source, this.<playerCharacter>__2);
                                    }
                                    this.<i>__5++;
                                }
                                goto Label_0285;

                            case DungeonBoostType.ExplosiveBox:
                                this.<position>__9 = this.<>f__this.m_dungeonBoost.Transform.position;
                                if (this.<perkType>__3 == PerkType.DungeonBoostExplosion)
                                {
                                    ExplosionSkill.ExecuteStatic(this.<playerCharacter>__2, this.<position>__9, null, 0.25f);
                                }
                                goto Label_0285;
                        }
                        break;

                    case 1:
                        goto Label_0472;

                    default:
                        goto Label_048E;
                }
            Label_0285:
                this.<ie>__10 = null;
                if (!string.IsNullOrEmpty(this.<>f__this.m_dungeonBoost.Properties.ShopEntryId))
                {
                    this.<reward>__11 = new Reward();
                    this.<shopEntryId>__12 = this.<>f__this.m_dungeonBoost.Properties.ShopEntryId;
                    this.<shopEntry>__13 = ConfigShops.GetVendorShopEntry(this.<shopEntryId>__12);
                    if (this.<shopEntry>__13 != null)
                    {
                        switch (this.<shopEntry>__13.Type)
                        {
                            case ShopEntryType.CoinBundle:
                                this.<reward>__11.CoinDrops.Add(App.Binder.ConfigMeta.CoinBundleSize(this.<player>__1, this.<shopEntryId>__12, 0.0));
                                break;

                            case ShopEntryType.TokenBundle:
                                this.<reward>__11.TokenDrops.Add(App.Binder.ConfigMeta.TokenBundleSize(this.<player>__1, this.<shopEntryId>__12));
                                break;
                        }
                    }
                    CmdConsumeReward.ExecuteStatic(this.<player>__1, this.<reward>__11, false, "TRACKING_ID_GAMEPLAY_LOOT_GAIN");
                    if (App.Binder.ConfigMeta.COMBAT_STATS_ENABLED)
                    {
                        GameLogic.Binder.HeroStatRecordingSystem.RealtimeCombatStats.addDatapoint(this.<reward>__11.getTotalCoinAmount(), RealtimeCombatStats.DatapointType.Coins);
                    }
                    this.<ie>__10 = PlayerView.Binder.DungeonHud.flyToHud(this.<reward>__11, RectTransformUtility.WorldToScreenPoint(PlayerView.Binder.RoomView.RoomCamera.Camera, this.<>f__this.m_dungeonBoost.Transform.position), false, true);
                }
                GameLogic.Binder.EventBus.DungeonBoostActivated(this.<>f__this.m_dungeonBoost, this.<>f__this.m_fromSkill);
                if (this.<>f__this.m_dungeonBoost.Properties.DoDestroyOnActivation)
                {
                    this.<>f__this.m_dungeonBoost.destroy();
                }
                if (this.<ie>__10 == null)
                {
                    goto Label_048E;
                }
            Label_0472:
                while (this.<ie>__10.MoveNext())
                {
                    this.$current = this.<ie>__10.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_048E;
                this.$PC = -1;
            Label_048E:
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

