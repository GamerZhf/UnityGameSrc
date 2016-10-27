namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class MissionCell : MonoBehaviour
    {
        public Image Bg;
        public GameObject CooldownIconRoot;
        public GameObject CooldownRoot;
        public Text CooldownSubtitle;
        public Text CooldownTime;
        public Text Description;
        public GameObject Divider;
        public Image Icon;
        public Image IconBg;
        public Image IconBgGolden;
        public Text NewText;
        public GameObject Notifier;
        public AnimatedProgressBar ProgressBar;
        public Image ProgressBarFgDefault;
        public Image ProgressBarFgGolden;
        public Text ProgressBarText;
        public Image RewardIconBig;
        public Image RewardIconSmall;
        public GameObject RewardsRoot;
        public Text RewardText;
        public Text Title;

        protected void Awake()
        {
            if (this.NewText != null)
            {
                this.NewText.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.MISSIONS_NEW, null, false));
            }
            if (this.Notifier != null)
            {
                this.Notifier.SetActive(false);
            }
        }

        public void refresh(Player player, MissionInstance mission, bool doShowDivider, [Optional, DefaultParameterValue(null)] string titleOverride, [Optional, DefaultParameterValue(null)] string descriptionOverride)
        {
            double num;
            float v = mission.getMissionProgress(player, out num);
            num = (num <= mission.Requirement) ? num : mission.Requirement;
            ConfigMissions.Mission missionData = ConfigMissions.GetMissionData(mission.MissionId);
            long totalSeconds = mission.getRemainingCooldownSeconds();
            bool flag = ((mission.MissionType == MissionType.PromotionEvent) && (v == 1.0)) && !mission.OnCooldown;
            bool flag2 = (mission.MissionType == MissionType.PromotionEvent) && mission.OnCooldown;
            Image image = (mission.MissionType != MissionType.PromotionEvent) ? this.RewardIconBig : this.RewardIconSmall;
            this.IconBgGolden.gameObject.SetActive(flag);
            this.ProgressBarFgGolden.gameObject.SetActive(flag);
            this.ProgressBarText.gameObject.SetActive(!flag);
            this.CooldownRoot.SetActive(flag2);
            this.Title.gameObject.SetActive(!flag2);
            this.Description.gameObject.SetActive(!flag2);
            this.ProgressBar.gameObject.SetActive(!flag2);
            this.RewardText.gameObject.SetActive(!(flag2 || flag));
            this.RewardIconSmall.gameObject.SetActive(false);
            this.RewardIconBig.gameObject.SetActive(false);
            image.gameObject.SetActive(!(flag2 || flag));
            this.Divider.SetActive(doShowDivider);
            if (mission.MissionType == MissionType.PromotionEvent)
            {
                this.CooldownSubtitle.text = _.L(ConfigLoca.PROMOTION_EVENT_MISSION_AVAILABLE_IN, null, false);
            }
            this.CooldownTime.text = MenuHelpers.SecondsToStringDaysHoursMinutes(totalSeconds, true);
            this.Icon.sprite = (mission.MissionType != MissionType.PromotionEvent) ? PlayerView.Binder.SpriteResources.getSprite(missionData.Icon) : PlayerView.Binder.SpriteResources.getSprite(ConfigMissions.PROMOTION_EVENT_MISSION_ICON);
            this.Icon.material = !flag2 ? null : PlayerView.Binder.DisabledUiMaterial;
            this.IconBg.material = !flag2 ? null : PlayerView.Binder.DisabledUiMaterial;
            this.Notifier.SetActive(!mission.Inspected);
            this.Title.text = StringExtensions.ToUpperLoca((titleOverride == null) ? _.L(missionData.Title, null, false) : titleOverride);
            this.Description.text = !flag ? missionData.getFormattedMissionDescription(mission.Requirement, true, descriptionOverride) : _.L(ConfigLoca.MISSION_COMPLETED, null, false);
            this.ProgressBarText.text = num + " / " + mission.Requirement;
            this.ProgressBar.setNormalizedValue(v);
            image.sprite = ConfigMissions.GetMissionRewardIcon(mission);
            this.RewardText.text = ConfigMissions.GetMissionRewardTitle(mission);
        }
    }
}

