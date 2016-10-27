namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class LeagueTitleCell : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private GameLogic.LeagueData <LeagueData>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public Text CrownRequirement;
        public Image RewardIcon;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(GameLogic.LeagueData leagueData, bool stripedRow)
        {
            this.LeagueData = leagueData;
            this.CrownRequirement.text = this.LeagueData.Title;
            this.RewardIcon.sprite = PlayerView.Binder.SpriteResources.getSprite(ConfigUi.CHEST_BLUEPRINTS[leagueData.RewardChestType].Icon);
            this.CrownRequirement.text = MenuHelpers.BigValueToString(this.LeagueData.CrownRequirement);
            bool flag = ConfigLeagues.HasPlayerReachedLeague(GameLogic.Binder.GameState.Player.getResourceAmount(ResourceType.Crown), this.LeagueData);
            this.RewardIcon.material = !flag ? PlayerView.Binder.DisabledUiMaterial : null;
        }

        public void onClick()
        {
            if (PlayerView.Binder.MenuSystem.InTransition)
            {
            }
        }

        public void refresh()
        {
        }

        public GameLogic.LeagueData LeagueData
        {
            [CompilerGenerated]
            get
            {
                return this.<LeagueData>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LeagueData>k__BackingField = value;
            }
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }
    }
}

