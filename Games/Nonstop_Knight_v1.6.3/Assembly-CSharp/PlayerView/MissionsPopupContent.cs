namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class MissionsPopupContent : MenuContent
    {
        private List<MissionCell> m_missionCells = new List<MissionCell>(ConfigMissions.NUM_ACTIVE_MISSIONS);
        private List<MissionInstance> m_sortedMissionInstanceList = new List<MissionInstance>(ConfigMissions.NUM_ACTIVE_MISSIONS);
        public RectTransform MasterCooldownRootTm;
        public Text MasterCooldownSubtitle;
        public Text MasterCooldownText;
        public Text MasterDescription;
        public AnimatedProgressBar MasterProgressBar;
        public Text MasterProgressBarText;
        public PrettyButton MissionClaimButton;
        public GameObject MissionClaimButtonNotifier;
        public RectTransform MissionRootTm;
        public Text MissionSubtitle;

        protected override void onAwake()
        {
            this.MissionSubtitle.text = StringExtensions.ToUpperLoca(_.L(ConfigLoca.MISSIONS_SUBTITLE, null, false));
            this.MasterCooldownSubtitle.text = _.L(ConfigLoca.MISSIONS_NEW_AVAILABLE_IN, null, false);
        }

        public void onCancelButtonClicked()
        {
            if (!PlayerView.Binder.MenuSystem.InTransition)
            {
                PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
            }
        }

        public void onClaimButtonClicked()
        {
        }

        protected override void onCleanup()
        {
            for (int i = 0; i < this.m_missionCells.Count; i++)
            {
                PlayerView.Binder.MissionCellPool.returnObject(this.m_missionCells[i]);
            }
            this.m_missionCells.Clear();
        }

        protected void OnDisable()
        {
            GameLogic.Binder.EventBus.OnMissionStarted -= new GameLogic.Events.MissionStarted(this.onMissionStarted);
            GameLogic.Binder.EventBus.OnMissionEnded -= new GameLogic.Events.MissionEnded(this.onMissionEnded);
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnMissionStarted += new GameLogic.Events.MissionStarted(this.onMissionStarted);
            GameLogic.Binder.EventBus.OnMissionEnded += new GameLogic.Events.MissionEnded(this.onMissionEnded);
        }

        private void onMissionEnded(Player player, MissionInstance mission, bool success)
        {
            if (mission.MissionType == MissionType.Adventure)
            {
                base.refresh();
            }
        }

        private void onMissionStarted(Player player, MissionInstance mission)
        {
            if (mission.MissionType == MissionType.Adventure)
            {
                base.refresh();
            }
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            for (int i = 0; i < ConfigMissions.NUM_ACTIVE_MISSIONS; i++)
            {
                MissionCell item = PlayerView.Binder.MissionCellPool.getObject();
                item.transform.SetParent(this.MissionRootTm, false);
                item.transform.SetSiblingIndex(this.MissionRootTm.GetSiblingIndex() + 1);
                item.gameObject.SetActive(true);
                this.m_missionCells.Add(item);
            }
            this.reconstructContent();
        }

        protected override void onRefresh()
        {
            Player player = GameLogic.Binder.GameState.Player;
            base.m_contentMenu.refreshTitle(StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_BUTTON_MISSIONS, null, false)), string.Empty, string.Empty);
            bool flag = player.Missions.hasMissionOnCooldown();
            this.m_sortedMissionInstanceList.Clear();
            for (int i = 0; i < player.Missions.Instances.Count; i++)
            {
                this.m_sortedMissionInstanceList.Add(player.Missions.Instances[i]);
            }
            this.m_sortedMissionInstanceList.Sort(new Comparison<MissionInstance>(MissionInstance.CompareByCooldown));
            for (int j = 0; j < this.m_missionCells.Count; j++)
            {
                MissionCell cell = this.m_missionCells[j];
                if (j < this.m_sortedMissionInstanceList.Count)
                {
                    MissionInstance mission = this.m_sortedMissionInstanceList[j];
                    bool flag2 = mission.getRemainingCooldownSeconds() > 0L;
                    cell.gameObject.SetActive(!flag2);
                    if (!flag2)
                    {
                        if (App.Binder.ConfigMeta.NOTIFY_NEW_MISSIONS_DURING_WELCOME_BACK)
                        {
                            CmdInspectMission.ExecuteStatic(player, mission);
                        }
                        cell.refresh(player, mission, (j < player.Missions.Instances.Count) || flag, null, null);
                        if (!App.Binder.ConfigMeta.NOTIFY_NEW_MISSIONS_DURING_WELCOME_BACK)
                        {
                            CmdInspectMission.ExecuteStatic(player, mission);
                        }
                    }
                }
                else
                {
                    cell.gameObject.SetActive(false);
                }
            }
            if (flag)
            {
                this.MasterCooldownRootTm.gameObject.SetActive(true);
                this.MasterCooldownText.text = MenuHelpers.SecondsToStringHoursMinutes(player.Missions.getMinRemainingCooldownSeconds());
                this.MasterCooldownRootTm.SetAsLastSibling();
            }
            else
            {
                this.MasterCooldownRootTm.gameObject.SetActive(false);
            }
            int amount = player.Missions.getNumCompletedMissionsRequiredForBigPrize();
            int num4 = amount - Mathf.Max(amount - player.Missions.NumUnclaimedMissionCompletions, 0);
            this.MasterDescription.text = _.L(ConfigLoca.MISSIONS_BIG_PRIZE, new <>__AnonType9<int>(amount), false);
            this.MasterProgressBar.setNormalizedValue(Mathf.Clamp01(((float) num4) / ((float) amount)));
            this.MasterProgressBarText.text = num4 + " / " + amount;
        }

        private void reconstructContent()
        {
            this.onRefresh();
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.MissionsPopupContent;
            }
        }

        public override string TabTitle
        {
            get
            {
                return StringExtensions.ToUpperLoca(_.L(ConfigLoca.DHUD_BUTTON_MISSIONS, null, false));
            }
        }
    }
}

