namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MarketingBuildController : MonoBehaviour
    {
        [CompilerGenerated]
        private float <CameraRotation>k__BackingField;
        [CompilerGenerated]
        private float <CameraZooming>k__BackingField;
        [CompilerGenerated]
        private float <CheatTimescale>k__BackingField;
        [CompilerGenerated]
        private bool <CombatTextDisabled>k__BackingField;
        [CompilerGenerated]
        private bool <DamageFlashDisabled>k__BackingField;
        [CompilerGenerated]
        private bool <FlyToHudDisabled>k__BackingField;
        [CompilerGenerated]
        private bool <HpIndicatorsDisabled>k__BackingField;
        [CompilerGenerated]
        private bool <HudTopPanelHidden>k__BackingField;
        [CompilerGenerated]
        private bool <UiHidden>k__BackingField;

        private void cycleCheatTimescale()
        {
            float[] array = new float[] { 1f, 0.5f, 0.25f, 0.125f, 0.0625f };
            int index = Array.IndexOf<float>(array, Time.timeScale);
            if (index < 0)
            {
                index = 0;
            }
            float targetTimescale = array[(index + 1) % array.Length];
            GameLogic.Binder.TimeSystem.speedCheat(targetTimescale);
        }

        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                PlayerView.Binder.DungeonHud.onSkillButtonClicked(0);
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                PlayerView.Binder.DungeonHud.onSkillButtonClicked(1);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                PlayerView.Binder.DungeonHud.onSkillButtonClicked(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayerView.Binder.DungeonHud.FloaterText.showOrRefresh(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PlayerView.Binder.DungeonHud.FloaterText.showOrRefresh(4);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                PlayerView.Binder.DungeonHud.FloaterText.showOrRefresh(5);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                PlayerView.Binder.DungeonHud.FloaterText.showOrRefresh(6);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                PlayerView.Binder.DungeonHud.FloaterText.showOrRefresh(7);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                PlayerView.Binder.DungeonHud.FloaterText.showOrRefresh(8);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                PlayerView.Binder.DungeonHud.FloaterText.showOrRefresh(9);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                PlayerView.Binder.DungeonHud.FloaterText.showOrRefresh(10);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                this.UiHidden = !this.UiHidden;
                GameObjectExtensions.SetLayerRecursively(PlayerView.Binder.DungeonHud.gameObject, !this.UiHidden ? Layers.UI : 0);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                this.FlyToHudDisabled = !this.FlyToHudDisabled;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                this.HpIndicatorsDisabled = !this.HpIndicatorsDisabled;
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                this.CombatTextDisabled = !this.CombatTextDisabled;
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                this.HudTopPanelHidden = !this.HudTopPanelHidden;
                GameObjectExtensions.SetLayerRecursively(PlayerView.Binder.DungeonHud.TopPanel.gameObject, (!this.UiHidden && !this.HudTopPanelHidden) ? Layers.UI : 0);
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                this.DamageFlashDisabled = !this.DamageFlashDisabled;
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                this.cycleCheatTimescale();
            }
            else if (Input.GetKey(KeyCode.W))
            {
                this.CameraZooming += Time.unscaledDeltaTime * 12f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                this.CameraZooming -= Time.unscaledDeltaTime * 12f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                this.CameraRotation -= Time.unscaledDeltaTime * 40f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                this.CameraRotation += Time.unscaledDeltaTime * 40f;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                this.UiHidden = false;
                this.FlyToHudDisabled = false;
                this.HpIndicatorsDisabled = false;
                this.CombatTextDisabled = false;
                this.HudTopPanelHidden = false;
                this.DamageFlashDisabled = false;
                GameObjectExtensions.SetLayerRecursively(PlayerView.Binder.DungeonHud.gameObject, Layers.UI);
                GameLogic.Binder.TimeSystem.speedCheat(1f);
                this.CameraZooming = 0f;
                this.CameraRotation = 0f;
            }
        }

        public float CameraRotation
        {
            [CompilerGenerated]
            get
            {
                return this.<CameraRotation>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<CameraRotation>k__BackingField = value;
            }
        }

        public float CameraZooming
        {
            [CompilerGenerated]
            get
            {
                return this.<CameraZooming>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<CameraZooming>k__BackingField = value;
            }
        }

        public float CheatTimescale
        {
            [CompilerGenerated]
            get
            {
                return this.<CheatTimescale>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<CheatTimescale>k__BackingField = value;
            }
        }

        public bool CombatTextDisabled
        {
            [CompilerGenerated]
            get
            {
                return this.<CombatTextDisabled>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<CombatTextDisabled>k__BackingField = value;
            }
        }

        public bool DamageFlashDisabled
        {
            [CompilerGenerated]
            get
            {
                return this.<DamageFlashDisabled>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<DamageFlashDisabled>k__BackingField = value;
            }
        }

        public bool FlyToHudDisabled
        {
            [CompilerGenerated]
            get
            {
                return this.<FlyToHudDisabled>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<FlyToHudDisabled>k__BackingField = value;
            }
        }

        public bool HpIndicatorsDisabled
        {
            [CompilerGenerated]
            get
            {
                return this.<HpIndicatorsDisabled>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<HpIndicatorsDisabled>k__BackingField = value;
            }
        }

        public bool HudTopPanelHidden
        {
            [CompilerGenerated]
            get
            {
                return this.<HudTopPanelHidden>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<HudTopPanelHidden>k__BackingField = value;
            }
        }

        public bool UiHidden
        {
            [CompilerGenerated]
            get
            {
                return this.<UiHidden>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<UiHidden>k__BackingField = value;
            }
        }
    }
}

