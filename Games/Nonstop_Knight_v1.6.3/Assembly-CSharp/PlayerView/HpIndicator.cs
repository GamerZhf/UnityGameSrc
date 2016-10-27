namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class HpIndicator : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private CharacterInstance <CharacterAttached>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public GameObject BossBarRoot;
        public AnimatedProgressBar BossHpSlider;
        public GameObject EnemyBarRoot;
        public AnimatedProgressBar EnemyHpSlider;
        private AnimatedProgressBar m_activeProgressBar;
        private Camera m_canvasCamera;
        private CharacterView m_targetCharacterView;
        public GameObject PlayerBarRoot;
        public AnimatedProgressBar PlayerHpSlider;

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
        }

        public void cleanUpForReuse()
        {
            this.CharacterAttached = null;
            this.m_targetCharacterView = null;
            base.StopAllCoroutines();
        }

        public void initialize(Camera canvasCamera, CharacterInstance attachToCharacter)
        {
            this.m_canvasCamera = canvasCamera;
            base.transform.localScale = Vector3.one;
            this.CharacterAttached = attachToCharacter;
            this.m_targetCharacterView = PlayerView.Binder.RoomView.getCharacterViewForCharacter(this.CharacterAttached);
            if (this.CharacterAttached.IsPlayerCharacter)
            {
                this.BossBarRoot.SetActive(false);
                this.PlayerBarRoot.SetActive(true);
                this.EnemyBarRoot.SetActive(false);
                this.m_activeProgressBar = this.PlayerHpSlider;
            }
            else if (this.CharacterAttached.IsBoss)
            {
                this.BossBarRoot.SetActive(true);
                this.PlayerBarRoot.SetActive(false);
                this.EnemyBarRoot.SetActive(false);
                this.m_activeProgressBar = this.BossHpSlider;
            }
            else
            {
                this.BossBarRoot.SetActive(false);
                this.PlayerBarRoot.SetActive(false);
                this.EnemyBarRoot.SetActive(true);
                this.m_activeProgressBar = this.EnemyHpSlider;
            }
            float v = MathUtil.Clamp01(attachToCharacter.CurrentHp / attachToCharacter.MaxLife(true));
            this.m_activeProgressBar.setNormalizedValue(v);
        }

        protected void LateUpdate()
        {
            if (this.CharacterAttached != null)
            {
                Vector3 vector2;
                Vector3 screenPoint = PlayerView.Binder.RoomView.RoomCamera.Camera.WorldToScreenPoint(this.m_targetCharacterView.Transform.position + ConfigUi.HP_INDICATOR_WORLD_OFFSET);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(this.RectTm, screenPoint, this.m_canvasCamera, out vector2);
                this.RectTm.position = vector2;
            }
        }

        public void refreshCurrentHp(bool instant)
        {
            float v = MathUtil.Clamp01(this.CharacterAttached.CurrentHp / this.CharacterAttached.MaxLife(true));
            if (instant)
            {
                this.m_activeProgressBar.setNormalizedValue(v);
            }
            else if (v < this.m_activeProgressBar.getNormalizedValue())
            {
                this.m_activeProgressBar.animateToNormalizedValue(this.m_activeProgressBar.getNormalizedValue(), v, ConfigUi.HP_LOSS_INDICATOR_ANIMATION_SPEED, 5, null, 0f);
            }
            else
            {
                this.m_activeProgressBar.animateToNormalizedValue(this.m_activeProgressBar.getNormalizedValue(), v, ConfigUi.HP_GAIN_INDICATOR_ANIMATION_SPEED, 5, null, 0f);
            }
        }

        public CharacterInstance CharacterAttached
        {
            [CompilerGenerated]
            get
            {
                return this.<CharacterAttached>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CharacterAttached>k__BackingField = value;
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

