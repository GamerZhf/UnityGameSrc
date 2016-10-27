namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class ActiveDungeon
    {
        public DungeonRuleset ActiveDungeonRuleset;
        public Room ActiveRoom;
        public TournamentInstance ActiveTournament;
        public int BossesKilled;
        public string BossId = string.Empty;
        public GameplayState CurrentGameplayState;
        public GameLogic.Dungeon Dungeon;
        public GameLogic.DungeonEvent DungeonEvent;
        public GameLogic.DungeonEventType DungeonEventType;
        public int Floor;
        public Vector3 LastBossKillWorldPt;
        private Dictionary<DungeonModifierType, bool> m_dungeonModifiers = new Dictionary<DungeonModifierType, bool>(new DungeonModifierTypeBoxAvoidanceComparer());
        public DungeonMood Mood;
        public int NumberOfPaidRevivesUsed;
        public GameplayState PreviousGameplayState;
        public bool SeamlessTransition;
        public bool SecondChanceUsed;
        public bool Simulated;
        public BossRewards VisualizableBossRewards;
        public bool WildBossEscapeTriggered;
        public bool WildBossMode;

        public ActiveDungeon()
        {
            for (int i = 0; i < ConfigDungeonModifiers.ALL_MODIFIERS.Count; i++)
            {
                this.m_dungeonModifiers.Add(ConfigDungeonModifiers.ALL_MODIFIERS[i], false);
            }
        }

        public void clearDungeonModifiers()
        {
            for (int i = 0; i < ConfigDungeonModifiers.ALL_MODIFIERS.Count; i++)
            {
                this.m_dungeonModifiers[ConfigDungeonModifiers.ALL_MODIFIERS[i]] = false;
            }
        }

        public void enableDungeonModifier(DungeonModifierType modifier)
        {
            this.m_dungeonModifiers[modifier] = true;
        }

        public void enableDungeonModifier(List<DungeonModifierType> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                this.enableDungeonModifier(modifiers[i]);
            }
        }

        public double getCurrentRevivePrice()
        {
            return (double) (App.Binder.ConfigMeta.REVIVE_PRICE_BASE + (App.Binder.ConfigMeta.REVIVE_PRICE_INCREMENT * this.NumberOfPaidRevivesUsed));
        }

        public double getProgressDifficultyExponent()
        {
            if (this.ActiveTournament != null)
            {
                return this.ActiveTournament.DifficultyModifier;
            }
            return App.Binder.ConfigMeta.PROGRESS_DIFFICULTY_EXPONENT;
        }

        public bool hasDungeonModifier(DungeonModifierType modifier)
        {
            return this.m_dungeonModifiers[modifier];
        }

        public bool isBossFloor()
        {
            return !string.IsNullOrEmpty(this.BossId);
        }

        public bool isEliteBossFloor()
        {
            if (this.ActiveTournament != null)
            {
                return this.Dungeon.hasBoss();
            }
            return this.Dungeon.hasEliteTag();
        }

        public bool isTutorialDungeon()
        {
            return (this.Dungeon.Id == "T1");
        }

        public bool roomCompletionConditionSatisfied()
        {
            Player owningPlayer = this.PrimaryPlayerCharacter.OwningPlayer;
            if (!this.ActiveRoom.CompletionTriggered)
            {
                if (this.CurrentGameplayState != GameplayState.ACTION)
                {
                    return false;
                }
                if (this.PrimaryPlayerCharacter.IsDead)
                {
                    return true;
                }
                if (this.WildBossMode)
                {
                    return false;
                }
                if (this.isBossFloor())
                {
                    if ((this.ActiveRoom.MainBossSummoned && (this.BossesKilled > 0)) && (this.ActiveRoom.numberOfBossesAlive() == 0))
                    {
                        return true;
                    }
                }
                else if (owningPlayer.floorCompletionGoalSatisfied(this))
                {
                    return true;
                }
            }
            return false;
        }

        public CharacterInstance PrimaryPlayerCharacter
        {
            get
            {
                return GameLogic.Binder.GameState.Player.ActiveCharacter;
            }
        }

        public class BossRewards
        {
            public Reward AdditionalDrop;
            public int CoinDropCount;
            public double CoinsPerDrop;
            public int FrenzyPotions;
            public List<Reward> MainDrops = new List<Reward>();
            public List<Reward> RiggedRewards = new List<Reward>();
            public double Tokens;
            public double XpPerDrop;
        }
    }
}

