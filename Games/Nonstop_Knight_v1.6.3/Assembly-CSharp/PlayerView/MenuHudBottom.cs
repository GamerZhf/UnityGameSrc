namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class MenuHudBottom : MonoBehaviour
    {
        public RectTransform BottomPanelTm;
        public UnityEngine.Canvas Canvas;
        public IconWithText ChallengeDungeon;
        public GameObject FpsCounter;
        public GridLayoutGroup HeroCellGrid;
        private List<HeroCell> m_heroCells = new List<HeroCell>();

        private HeroCell addHeroCellToGrid(CharacterInstance characterInstance, Character character)
        {
            HeroCell item = PlayerView.Binder.HeroCellPool.getObject();
            item.transform.SetParent(this.HeroCellGrid.transform, false);
            item.initialize(characterInstance, character);
            this.m_heroCells.Add(item);
            item.gameObject.SetActive(true);
            return item;
        }

        protected void Awake()
        {
        }

        private void cleanupHeroCells()
        {
            for (int i = this.m_heroCells.Count - 1; i >= 0; i--)
            {
                HeroCell item = this.m_heroCells[i];
                this.m_heroCells.Remove(item);
                PlayerView.Binder.HeroCellPool.returnObject(item);
            }
        }

        public void onChallengeDungeonClicked()
        {
        }

        private void onCharacterRankUpped(CharacterInstance characterInstance)
        {
            this.refreshBottomPanel();
        }

        private void onCharacterUnlocked(CharacterInstance characterInstance)
        {
            this.refreshBottomPanel();
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnCharacterUnlocked -= new GameLogic.Events.CharacterUnlocked(this.onCharacterUnlocked);
            GameLogic.Binder.EventBus.OnCharacterRankUpped -= new GameLogic.Events.CharacterRankUpped(this.onCharacterRankUpped);
            PlayerView.Binder.EventBus.OnMenuChangeStarted -= new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
            PlayerView.Binder.EventBus.OnMenuChanged -= new PlayerView.Events.MenuChanged(this.onMenuChanged);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGameStateInitialized);
            GameLogic.Binder.EventBus.OnCharacterUnlocked += new GameLogic.Events.CharacterUnlocked(this.onCharacterUnlocked);
            GameLogic.Binder.EventBus.OnCharacterRankUpped += new GameLogic.Events.CharacterRankUpped(this.onCharacterRankUpped);
            PlayerView.Binder.EventBus.OnMenuChangeStarted += new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
            PlayerView.Binder.EventBus.OnMenuChanged += new PlayerView.Events.MenuChanged(this.onMenuChanged);
            this.BottomPanelTm.gameObject.SetActive(true);
            this.ChallengeDungeon.gameObject.SetActive(false);
        }

        private void onGameStateInitialized()
        {
            this.refreshBottomPanel();
        }

        private void onMenuChanged(Menu sourceMenu, Menu targetMenu)
        {
            if (targetMenu != null)
            {
                bool flag = true;
                if (PlayerView.Binder.MenuSystem.menuTypeInActiveStack(MenuType.MultiHeroMenu))
                {
                    flag = !((MultiHeroMenu) PlayerView.Binder.MenuSystem.getSharedMenuObject(MenuType.MultiHeroMenu)).PreDungeonModeActive;
                }
                this.BottomPanelTm.gameObject.SetActive(flag);
                if (flag)
                {
                    this.refreshChallengeDungeons(targetMenu.MenuType);
                }
            }
            else
            {
                this.BottomPanelTm.gameObject.SetActive(false);
            }
        }

        private void onMenuChangeStarted(MenuType sourceMenuType, MenuType targetMenuType)
        {
            if (targetMenuType == MenuType.MapMenu)
            {
                this.BottomPanelTm.gameObject.SetActive(true);
            }
            this.refreshChallengeDungeons(targetMenuType);
        }

        private void refreshBottomPanel()
        {
            this.cleanupHeroCells();
            Player player = GameLogic.Binder.GameState.Player;
            for (int i = 0; i < player.CharacterInstances.Count; i++)
            {
                this.addHeroCellToGrid(player.CharacterInstances[i], null);
            }
            for (int j = 0; j < MultiHeroMenu.HERO_CHARACTER_IDS.Count; j++)
            {
                string characterId = MultiHeroMenu.HERO_CHARACTER_IDS[j];
                if (!player.hasUnlockedCharacter(characterId))
                {
                    this.addHeroCellToGrid(null, GameLogic.Binder.CharacterResources.getResource(characterId));
                }
            }
            this.addHeroCellToGrid(null, null);
            this.addHeroCellToGrid(null, null);
            this.addHeroCellToGrid(null, null);
        }

        private void refreshChallengeDungeons(MenuType targetMenuType)
        {
            bool flag = false;
            if (targetMenuType == MenuType.MultiHeroMenu)
            {
                flag = false;
            }
            else if (targetMenuType == MenuType.MapMenu)
            {
                flag = true;
            }
            else
            {
                flag = PlayerView.Binder.MenuSystem.menuTypeInActiveStack(MenuType.MapMenu);
            }
            this.ChallengeDungeon.gameObject.SetActive(flag);
        }

        protected void Update()
        {
            this.refreshChallengeDungeons(MenuType.NONE);
        }
    }
}

