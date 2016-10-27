namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class MultiHeroMenu : Menu
    {
        [CompilerGenerated]
        private bool <PreDungeonModeActive>k__BackingField;
        public AnimatedProgressBar AttackProgressBar;
        public MenuDragHandler AvatarDragHandler;
        public RawImage AvatarImage;
        public GameObject BackButton;
        public Text CharacterLevel;
        public Text CharacterName;
        public AnimatedProgressBar DefenseProgressBar;
        public Text DungeonEnergyCost;
        public static List<string> HERO_CHARACTER_IDS;
        public GameObject LevelRoot;
        public Button LevelUpButton;
        public List<MaterialCell> LevelUpCostCells;
        public GameObject LevelUpRoot;
        public Image LockIcon;
        public AnimatedProgressBar LuckProgressBar;
        private Coroutine m_characterUnlockRoutine;
        private InputParams m_params;
        private Character m_visibleCharacter;
        private CharacterInstance m_visibleCharacterInstance;
        private CharacterView m_visibleCharacterView;
        public MenuOverlay Overlay;
        public CanvasGroup PanelRoot;
        public GameObject PlayButton;
        public List<Image> SkillIcons;
        public AnimatedProgressBar SpeedProgressBar;
        public GameObject StatsRoot;
        public MaterialCell UnlockMaterialCell;
        public AnimatedProgressBar UnlockProgressBar;
        public Text UnlockProgressBarText;
        public GameObject UnlockRoot;

        static MultiHeroMenu()
        {
            List<string> list = new List<string>();
            list.Add("Hero001");
            list.Add("Hero002");
            list.Add("Hero003");
            HERO_CHARACTER_IDS = list;
        }

        [DebuggerHidden]
        private IEnumerator characterUnlockRoutine()
        {
            <characterUnlockRoutine>c__Iterator156 iterator = new <characterUnlockRoutine>c__Iterator156();
            iterator.<>f__this = this;
            return iterator;
        }

        private string getVisibleCharacterId()
        {
            if (this.m_visibleCharacter != null)
            {
                return this.m_visibleCharacter.Id;
            }
            return this.m_visibleCharacterInstance.CharacterId;
        }

        [DebuggerHidden]
        public override IEnumerator hideRoutine(bool instant)
        {
            <hideRoutine>c__Iterator155 iterator = new <hideRoutine>c__Iterator155();
            iterator.instant = instant;
            iterator.<$>instant = instant;
            iterator.<>f__this = this;
            return iterator;
        }

        private void onArrowClicked(bool left)
        {
            int num;
            Player player = GameLogic.Binder.GameState.Player;
            if (left)
            {
                num = HERO_CHARACTER_IDS.IndexOf(this.getVisibleCharacterId()) - 1;
                if (num < 0)
                {
                    num = HERO_CHARACTER_IDS.Count - 1;
                }
            }
            else
            {
                num = (HERO_CHARACTER_IDS.IndexOf(this.getVisibleCharacterId()) + 1) % HERO_CHARACTER_IDS.Count;
            }
            string characterId = HERO_CHARACTER_IDS[num];
            if (player.hasUnlockedCharacter(characterId))
            {
                this.setVisibleCharacter(null, player.getCharacterInstance(characterId));
                GameLogic.Binder.CommandProcessor.execute(new CmdSetActiveCharacter(player, characterId), 0f);
            }
            else
            {
                this.setVisibleCharacter(GameLogic.Binder.CharacterResources.getResource(characterId), null);
            }
        }

        protected override void onAwake()
        {
        }

        public void onBackButtonClicked()
        {
            PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.MapMenu, MenuContentType.NONE, null, 0f, false, true);
        }

        private void onCharacterRankUpped(CharacterInstance character)
        {
            this.onRefresh();
        }

        public void onLeftArrowClicked()
        {
            this.onArrowClicked(true);
        }

        public void onLevelUpButtonClicked()
        {
        }

        public void onPlayButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                Dungeon dungeon = GameLogic.Binder.DungeonResources.getResource(this.m_params.DungeonId);
                if (GameLogic.Binder.GameState.Player.getResourceAmount(ResourceType.Energy) < dungeon.EnergyCost)
                {
                    OutOfEnergyMiniPopupContent.InputParams parameter = new OutOfEnergyMiniPopupContent.InputParams();
                    PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.MiniPopupMenu, MenuContentType.OutOfEnergyMiniPopup, parameter, 0f, false, true);
                }
            }
        }

        protected override void onRefresh()
        {
            Character character = (this.m_visibleCharacterInstance == null) ? this.m_visibleCharacter : this.m_visibleCharacterInstance.Character;
            this.CharacterName.text = StringExtensions.ToUpperLoca(character.Name);
            this.LevelRoot.SetActive(this.m_visibleCharacterInstance != null);
            if (this.m_visibleCharacterInstance != null)
            {
                this.CharacterLevel.text = this.m_visibleCharacterInstance.Rank.ToString();
            }
            this.AvatarImage.texture = PlayerView.Binder.MenuSystem.CharacterMenuCamera.RenderTexture;
            this.AvatarImage.enabled = true;
            if (this.m_visibleCharacterInstance != null)
            {
                this.StatsRoot.SetActive(true);
            }
            else
            {
                this.StatsRoot.SetActive(false);
            }
            this.LockIcon.gameObject.SetActive(false);
        }

        public void onRightArrowClicked()
        {
            this.onArrowClicked(false);
        }

        public void onUnlockButtonClicked()
        {
            if (!UnityUtils.CoroutineRunning(ref this.m_characterUnlockRoutine))
            {
                string key = string.Empty;
                int num = 0;
                if (num > 0)
                {
                    OutOfMaterialsMiniPopupContent.InputParams params2 = new OutOfMaterialsMiniPopupContent.InputParams();
                    Dictionary<string, int> dictionary = new Dictionary<string, int>();
                    dictionary.Add(key, num);
                    params2.MissingItems = dictionary;
                    params2.SuccessCallback = delegate {
                        PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                        this.m_characterUnlockRoutine = UnityUtils.StartCoroutine(this, this.characterUnlockRoutine());
                    };
                    OutOfMaterialsMiniPopupContent.InputParams parameter = params2;
                    PlayerView.Binder.MenuSystem.transitionToMenu(PlayerView.MenuType.MiniPopupMenu, MenuContentType.OutOfMaterialsMiniPopup, parameter, 0f, false, true);
                }
                else
                {
                    this.m_characterUnlockRoutine = UnityUtils.StartCoroutine(this, this.characterUnlockRoutine());
                }
            }
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator153 iterator = new <preShowRoutine>c__Iterator153();
            iterator.parameter = parameter;
            iterator.<$>parameter = parameter;
            iterator.<>f__this = this;
            return iterator;
        }

        private void setVisibleCharacter(Character character, CharacterInstance characterInstance)
        {
            if (this.m_visibleCharacterView != null)
            {
                PlayerView.Binder.PersistentCharacterViewPool.returnObject(this.m_visibleCharacterView, this.m_visibleCharacter.Prefab);
                this.m_visibleCharacterView = null;
            }
            if (PlayerView.Binder.MenuSystem.MenuHero.CharacterView != null)
            {
                PlayerView.Binder.MenuSystem.MenuHero.CharacterView.setVisibility(false);
            }
            if (character != null)
            {
                this.m_visibleCharacter = character;
                this.m_visibleCharacterInstance = null;
                this.m_visibleCharacterView = PlayerView.Binder.PersistentCharacterViewPool.getObject(character.Prefab);
                this.m_visibleCharacterView.transform.SetParent(base.transform, true);
                this.m_visibleCharacterView.transform.localScale = Vector3.one;
                this.m_visibleCharacterView.gameObject.SetActive(true);
                this.m_visibleCharacterView.initialize(null, false);
                PlayerView.Binder.MenuSystem.MenuHero.setTarget(this.m_visibleCharacterView);
            }
            else
            {
                this.m_visibleCharacter = null;
                this.m_visibleCharacterInstance = characterInstance;
                PlayerView.Binder.MenuSystem.MenuHero.setTarget(PlayerView.Binder.RoomView.getCharacterViewForCharacter(characterInstance));
            }
            PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target = PlayerView.Binder.MenuSystem.MenuHero;
            PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target.CharacterView.setVisibility(true);
            PlayerView.Binder.MenuSystem.CharacterMenuCamera.Camera.enabled = true;
            this.onRefresh();
        }

        [DebuggerHidden]
        public override IEnumerator showRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <showRoutine>c__Iterator154 iterator = new <showRoutine>c__Iterator154();
            iterator.<>f__this = this;
            return iterator;
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.MultiHeroMenu;
            }
        }

        public bool PreDungeonModeActive
        {
            [CompilerGenerated]
            get
            {
                return this.<PreDungeonModeActive>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PreDungeonModeActive>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <characterUnlockRoutine>c__Iterator156 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MultiHeroMenu <>f__this;
            internal Player <player>__0;

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
                    case 1:
                        if (PlayerView.Binder.MenuSystem.InTransition)
                        {
                            this.$current = null;
                            this.$PC = 1;
                        }
                        else
                        {
                            this.<player>__0 = GameLogic.Binder.GameState.Player;
                            this.$current = GameLogic.Binder.CommandProcessor.execute(new CmdUnlockCharacter(this.<player>__0, this.<>f__this.m_visibleCharacter), 0f);
                            this.$PC = 2;
                        }
                        return true;

                    case 2:
                        this.<>f__this.setVisibleCharacter(null, GameLogic.Binder.GameState.Player.ActiveCharacter);
                        PlayerView.Binder.MenuSystem.transitionToMenu(MenuType.CharacterUnlockCeremonyMenu, MenuContentType.NONE, this.<>f__this.m_visibleCharacterInstance, 0f, false, true);
                        this.<>f__this.m_characterUnlockRoutine = null;
                        break;

                    default:
                        break;
                        this.$PC = -1;
                        break;
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

        [CompilerGenerated]
        private sealed class <hideRoutine>c__Iterator155 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>instant;
            internal MultiHeroMenu <>f__this;
            internal bool instant;

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
                        if (ConfigUi.MENU_TO_MENU_FADE_TO_BLACK_TOTAL_DURATION <= 0f)
                        {
                            goto Label_00B6;
                        }
                        this.<>f__this.Overlay.setTransparent(true);
                        this.<>f__this.Overlay.fadeToBlack(!this.instant ? (ConfigUi.MENU_TO_MENU_FADE_TO_BLACK_TOTAL_DURATION * 0.5f) : 0f, 1f, Easing.Function.LINEAR);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0164;
                }
                if (this.<>f__this.Overlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.Overlay.setTransparent(false);
            Label_00B6:
                PlayerView.Binder.MenuSystem.CharacterMenuCamera.Camera.enabled = false;
                MenuCharacterView target = PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target;
                this.<>f__this.AvatarDragHandler.OnDragged -= new MenuDragHandler.Dragged(target.OnDrag);
                GameLogic.Binder.EventBus.OnCharacterRankUpped -= new GameLogic.Events.CharacterRankUpped(this.<>f__this.onCharacterRankUpped);
                if (this.<>f__this.m_visibleCharacterView != null)
                {
                    PlayerView.Binder.PersistentCharacterViewPool.returnObject(this.<>f__this.m_visibleCharacterView, this.<>f__this.m_visibleCharacter.Prefab);
                    this.<>f__this.m_visibleCharacterView = null;
                    goto Label_0164;
                    this.$PC = -1;
                }
            Label_0164:
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

        [CompilerGenerated]
        private sealed class <preShowRoutine>c__Iterator153 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal object <$>parameter;
            internal MultiHeroMenu <>f__this;
            internal Dungeon <dungeon>__0;
            internal object parameter;

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
                    if (this.parameter != null)
                    {
                        this.<>f__this.m_params = (MultiHeroMenu.InputParams) this.parameter;
                        this.<>f__this.PreDungeonModeActive = this.<>f__this.m_params.PreDungeonModeActive;
                        this.<dungeon>__0 = GameLogic.Binder.DungeonResources.getResource(this.<>f__this.m_params.DungeonId);
                        this.<>f__this.DungeonEnergyCost.text = this.<dungeon>__0.EnergyCost.ToString();
                    }
                    else
                    {
                        this.<>f__this.PreDungeonModeActive = false;
                    }
                    this.<>f__this.BackButton.SetActive(this.<>f__this.PreDungeonModeActive);
                    this.<>f__this.PlayButton.SetActive(this.<>f__this.PreDungeonModeActive);
                    this.<>f__this.setVisibleCharacter(null, GameLogic.Binder.GameState.Player.ActiveCharacter);
                    MenuCharacterView target = PlayerView.Binder.MenuSystem.CharacterMenuCamera.Target;
                    this.<>f__this.AvatarDragHandler.OnDragged += new MenuDragHandler.Dragged(target.OnDrag);
                    GameLogic.Binder.EventBus.OnCharacterRankUpped += new GameLogic.Events.CharacterRankUpped(this.<>f__this.onCharacterRankUpped);
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

        [CompilerGenerated]
        private sealed class <showRoutine>c__Iterator154 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MultiHeroMenu <>f__this;

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
                        this.<>f__this.onRefresh();
                        if (ConfigUi.MENU_TO_MENU_FADE_TO_BLACK_TOTAL_DURATION <= 0f)
                        {
                            goto Label_0095;
                        }
                        this.<>f__this.Overlay.setTransparent(false);
                        this.<>f__this.Overlay.fadeToTransparent(ConfigUi.MENU_TO_MENU_FADE_TO_BLACK_TOTAL_DURATION * 0.5f, Easing.Function.LINEAR);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00B2;
                }
                if (this.<>f__this.Overlay.IsAnimating)
                {
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
            Label_0095:
                this.<>f__this.Overlay.setTransparent(true);
                goto Label_00B2;
                this.$PC = -1;
            Label_00B2:
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

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParams
        {
            public bool PreDungeonModeActive;
            public string DungeonId;
        }
    }
}

