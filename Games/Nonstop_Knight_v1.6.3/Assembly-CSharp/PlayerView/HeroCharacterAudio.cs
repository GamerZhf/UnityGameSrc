namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using UnityEngine;

    public class HeroCharacterAudio : AbstractCharacterAudio
    {
        private const float m_armorLayerTimeScale = 6f;
        private float m_armorLayerVolume;
        private AudioGroupType m_footStepTypeL = AudioGroupType.SfxGrpGameplay_HeroStepL;
        private AudioGroupType m_footStepTypeR = AudioGroupType.SfxGrpGameplay_HeroStepR;

        private void onAnimationActionTriggered(AbstractCharacterAnimator.Action action)
        {
            if (((base.CharacterView.Character != null) && base.CharacterView.Character.IsPlayerCharacter) && !base.CharacterView.Character.IsSupport)
            {
                PlayerView.Binder.AudioSystem.stopAudioGroup(this.m_footStepTypeL);
                PlayerView.Binder.AudioSystem.stopAudioGroup(this.m_footStepTypeR);
            }
        }

        private void onAnimationStateChanged(AbstractCharacterAnimator.State newState)
        {
            if (((base.CharacterView.Character != null) && base.CharacterView.Character.IsPlayerCharacter) && !base.CharacterView.Character.IsSupport)
            {
                PlayerView.Binder.AudioSystem.stopAudioGroup(this.m_footStepTypeL);
                PlayerView.Binder.AudioSystem.stopAudioGroup(this.m_footStepTypeR);
            }
        }

        protected override void onCleanup()
        {
            base.CharacterView.Animator.OnAnimationActionTriggered -= new AbstractCharacterAnimator.AnimationActionTriggered(this.onAnimationActionTriggered);
            base.CharacterView.Animator.OnAnimationStateChanged -= new AbstractCharacterAnimator.AnimationStateChanged(this.onAnimationStateChanged);
            if (PlayerView.Binder.AudioSystem != null)
            {
                PlayerView.Binder.AudioSystem.stopAudioSources(AudioSourceType.SfxGameplay_HeroArmorLayer);
            }
        }

        protected override void onInitialize()
        {
            base.CharacterView.Animator.OnAnimationActionTriggered += new AbstractCharacterAnimator.AnimationActionTriggered(this.onAnimationActionTriggered);
            base.CharacterView.Animator.OnAnimationStateChanged += new AbstractCharacterAnimator.AnimationStateChanged(this.onAnimationStateChanged);
            this.m_armorLayerVolume = 0f;
        }

        protected override void onUpdate(float dt)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if ((activeDungeon != null) && (((base.CharacterView.Character != null) && base.CharacterView.Character.IsPrimaryPlayerCharacter) && !base.CharacterView.Character.IsSupport))
            {
                if (((Time.timeScale == 1f) && (base.CharacterView.Animator.CurrentAction == AbstractCharacterAnimator.Action.NONE)) && ((base.CharacterView.Animator.TargetState == AbstractCharacterAnimator.State.RUN) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION)))
                {
                    if (activeDungeon.Dungeon.Theme == DungeonThemeType.Theme002)
                    {
                        this.m_footStepTypeL = AudioGroupType.SfxGrpGameplay_HeroStepGrassL;
                        this.m_footStepTypeR = AudioGroupType.SfxGrpGameplay_HeroStepGrassR;
                    }
                    else if (activeDungeon.Dungeon.Theme == DungeonThemeType.Theme003)
                    {
                        this.m_footStepTypeL = AudioGroupType.SfxGrpGameplay_HeroStepIceL;
                        this.m_footStepTypeR = AudioGroupType.SfxGrpGameplay_HeroStepIceR;
                    }
                    else
                    {
                        this.m_footStepTypeL = AudioGroupType.SfxGrpGameplay_HeroStepL;
                        this.m_footStepTypeR = AudioGroupType.SfxGrpGameplay_HeroStepR;
                    }
                    if ((PlayerView.Binder.AudioSystem.numberOfSfxAudioGroupPlaying(this.m_footStepTypeL) == 0) && (PlayerView.Binder.AudioSystem.numberOfSfxAudioGroupPlaying(this.m_footStepTypeR) == 0))
                    {
                        AudioSystem.PlaybackParameters pp = new AudioSystem.PlaybackParameters();
                        pp.PitchMin = 1f;
                        pp.PitchMax = 1f;
                        PlayerView.Binder.AudioSystem.playSfxGrp(this.m_footStepTypeL, pp);
                        AudioSystem.PlaybackParameters parameters2 = new AudioSystem.PlaybackParameters();
                        parameters2.DelayMin = 0.28f;
                        parameters2.DelayMax = 0.28f;
                        parameters2.PitchMin = 1f;
                        parameters2.PitchMax = 1f;
                        PlayerView.Binder.AudioSystem.playSfxGrp(this.m_footStepTypeR, parameters2);
                    }
                    this.updateArmorLayer(true);
                }
                else
                {
                    this.updateArmorLayer(false);
                }
            }
        }

        private void updateArmorLayer(bool isMoving)
        {
            Player player = GameLogic.Binder.GameState.Player;
            PoolableAudioSource source = PlayerView.Binder.AudioSystem.getFirstPlayingAudioSource(AudioSourceType.SfxGameplay_HeroArmorLayer);
            if (((source == null) && isMoving) && (!ConfigApp.CHEAT_EDITOR_AUDIO_MUTE && player.SoundEnabled))
            {
                PlayerView.Binder.AudioSystem.playSfx(AudioSourceType.SfxGameplay_HeroArmorLayer, (float) 0f);
                source = PlayerView.Binder.AudioSystem.getFirstPlayingAudioSource(AudioSourceType.SfxGameplay_HeroArmorLayer);
                if (source == null)
                {
                    Debug.LogWarning("Could not start armor sound layer");
                }
                else
                {
                    this.m_armorLayerVolume = 0f;
                    source.AudioSource.volume = 0f;
                }
            }
            if (source != null)
            {
                this.m_armorLayerVolume = Mathf.Lerp(this.m_armorLayerVolume, !isMoving ? 0f : 1f, Time.unscaledDeltaTime * 6f);
                source.AudioSource.volume = source.OrigVolume * this.m_armorLayerVolume;
                if (source.AudioSource.volume <= 0.001f)
                {
                    PlayerView.Binder.AudioSystem.stopAudioSources(AudioSourceType.SfxGameplay_HeroArmorLayer);
                }
            }
        }
    }
}

