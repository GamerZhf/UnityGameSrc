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
    using UnityEngine.Audio;

    public class AudioSystem : MonoBehaviour
    {
        [CompilerGenerated]
        private AudioMixer <Mixer>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public const float DESTRUCTIBLE_MIN_DELAY = 0.1f;
        public const float FADE_DURATION_AGGRESSIVE = 0.3f;
        public const float FADE_DURATION_INSTANT = 0f;
        public const float FADE_DURATION_NORMAL = 0.6f;
        private bool hasAudioFocus;
        public const float HERO_PAIN_HP_THRESHOLD = 0.5f;
        public const float HERO_PAIN_MIN_DELAY = 10f;
        private AudioSourceType m_activeAmbience;
        private PoolableAudioSource m_activeAmbienceSource;
        private AudioSourceType m_activeMusic;
        private List<PoolableAudioSource> m_activePoolableAudioSources = new List<PoolableAudioSource>(0x20);
        private AudioMixerSnapshotType m_activeSnapshot;
        private AudioMixerSnapshotType m_activeSnapshotBefore;
        private Dictionary<AudioGroupType, AudioGroup> m_audioGroups = new Dictionary<AudioGroupType, AudioGroup>(new AudioGroupTypeBoxAvoidanceComparer());
        private Dictionary<AudioMixerSnapshotType, AudioMixerSnapshot> m_audioMixerSnapshotMap = new Dictionary<AudioMixerSnapshotType, AudioMixerSnapshot>(new AudioMixerSnapshotTypeBoxAvoidanceComparer());
        private AudioMixerSnapshot[] m_audioMixerSnapshots;
        private float[] m_audioMixerSnapshotWeights;
        private Dictionary<AudioSourceType, AudioCategoryType> m_audioSourceTypeToCategoryMap = new Dictionary<AudioSourceType, AudioCategoryType>(new AudioSourceTypeBoxAvoidanceComparer());
        private ManualTimer m_crossFadeTimerAmbience = new ManualTimer();
        private ManualTimer m_crossFadeTimerMusic = new ManualTimer();
        private ITypedObjectPool<PoolableAudioSource, AudioSourceType> m_dynamicAudioSourcePool;
        private bool m_isMenuOpen;
        private float m_lastDestructibleTime;
        private float m_lastPlayerPainTime;
        private Dictionary<AudioSourceType, PoolableAudioSource> m_musicAudioSources = new Dictionary<AudioSourceType, PoolableAudioSource>(new AudioSourceTypeBoxAvoidanceComparer());
        private float m_nextDungeonAmbienceSfxTime;
        private Dictionary<AudioSourceType, bool> m_persistentAudioSourceQuickLookup = new Dictionary<AudioSourceType, bool>(new AudioSourceTypeBoxAvoidanceComparer());
        private AudioMixerSnapshotType m_snapshotPriorToPause;
        private AudioSourceType m_targetAmbience;
        private AudioSourceType m_targetMusic;
        public const int MAX_SAME_AUDIO_SOURCE_PLAYING_SIMULTANEOUSLY = 0x18;
        private float? originalVolume;

        [DebuggerHidden]
        private IEnumerator ambienceRoutine()
        {
            <ambienceRoutine>c__IteratorE1 re = new <ambienceRoutine>c__IteratorE1();
            re.<>f__this = this;
            return re;
        }

        protected void Awake()
        {
            this.Tm = base.transform;
            IEnumerator enumerator = Enum.GetValues(typeof(AudioSourceType)).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    AudioSourceType current = (AudioSourceType) ((int) enumerator.Current);
                    if (ConfigObjectPools.PERSISTENT_AUDIO_SOURCES.ContainsKey(current))
                    {
                        this.m_persistentAudioSourceQuickLookup.Add(current, true);
                    }
                    else
                    {
                        this.m_persistentAudioSourceQuickLookup.Add(current, false);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
            for (int i = 0; i < ConfigAudio.MUSIC_AUDIO_SOURCES.Count; i++)
            {
                this.m_audioSourceTypeToCategoryMap.Add(ConfigAudio.MUSIC_AUDIO_SOURCES[i], AudioCategoryType.Music);
            }
            for (int j = 0; j < ConfigAudio.AMBIENCE_AUDIO_SOURCES.Count; j++)
            {
                this.m_audioSourceTypeToCategoryMap.Add(ConfigAudio.AMBIENCE_AUDIO_SOURCES[j], AudioCategoryType.Ambience);
            }
            for (int k = 0; k < ConfigAudio.UI_SFX_AUDIO_SOURCES.Count; k++)
            {
                this.m_audioSourceTypeToCategoryMap.Add(ConfigAudio.UI_SFX_AUDIO_SOURCES[k], AudioCategoryType.SfxUi);
            }
            for (int m = 0; m < ConfigAudio.GAMEPLAY_SFX_AUDIO_SOURCES.Count; m++)
            {
                this.m_audioSourceTypeToCategoryMap.Add(ConfigAudio.GAMEPLAY_SFX_AUDIO_SOURCES[m], AudioCategoryType.SfxGameplay);
            }
            List<AudioSourceType> enumValuesWithException = LangUtil.GetEnumValuesWithException<AudioSourceType>(AudioSourceType.UNSPECIFIED);
            for (int n = 0; n < enumValuesWithException.Count; n++)
            {
            }
            for (int num6 = 0; num6 < ConfigAudio.MUSIC_AUDIO_SOURCES.Count; num6++)
            {
                AudioSourceType type = ConfigAudio.MUSIC_AUDIO_SOURCES[num6];
                PoolableAudioSource source = PlayerView.Binder.PersistentAudioSourcePool.getObject(type);
                source.gameObject.SetActive(true);
                this.m_musicAudioSources.Add(type, source);
            }
            this.Mixer = this.m_musicAudioSources[AudioSourceType.Music_Gameplay].AudioSource.outputAudioMixerGroup.audioMixer;
            List<AudioMixerSnapshotType> list2 = LangUtil.GetEnumValuesWithException<AudioMixerSnapshotType>(AudioMixerSnapshotType.UNSPECIFIED);
            this.m_audioMixerSnapshots = new AudioMixerSnapshot[list2.Count];
            for (int num7 = 0; num7 < list2.Count; num7++)
            {
                AudioMixerSnapshotType key = list2[num7];
                AudioMixerSnapshot snapshot = this.Mixer.FindSnapshot(key.ToString());
                this.m_audioMixerSnapshots[num7] = snapshot;
                this.m_audioMixerSnapshotMap.Add(key, snapshot);
            }
            this.m_audioMixerSnapshotWeights = new float[list2.Count];
            for (int num8 = 0; num8 < this.m_audioMixerSnapshotWeights.Length; num8++)
            {
                this.m_audioMixerSnapshotWeights[num8] = 0f;
            }
            foreach (KeyValuePair<AudioGroupType, List<AudioSourceType>> pair in ConfigAudio.SFX_AUDIO_GROUPS)
            {
                this.m_audioGroups.Add(pair.Key, new AudioGroup(pair.Value));
            }
        }

        [DebuggerHidden]
        private IEnumerator crossFade(PoolableAudioSource from, PoolableAudioSource to, float duration, ManualTimer timer)
        {
            <crossFade>c__IteratorE2 re = new <crossFade>c__IteratorE2();
            re.timer = timer;
            re.to = to;
            re.duration = duration;
            re.from = from;
            re.<$>timer = timer;
            re.<$>to = to;
            re.<$>duration = duration;
            re.<$>from = from;
            return re;
        }

        private void DisableAudioFocusChangeListener()
        {
            using (AndroidJavaClass class2 = new AndroidJavaClass("com.koplagames.unityextensions.AudioFocusChangeListener"))
            {
                class2.CallStatic("disable", new object[0]);
                this.hasAudioFocus = false;
            }
        }

        private void EnableAudioFocusChangeListener()
        {
            if (!this.originalVolume.HasValue)
            {
                this.originalVolume = new float?(AudioListener.volume);
            }
            using (AndroidJavaClass class2 = new AndroidJavaClass("com.koplagames.unityextensions.AudioFocusChangeListener"))
            {
                bool flag = !this.hasAudioFocus;
                object[] args = new object[] { typeof(AudioSystem).ToString() };
                this.hasAudioFocus = class2.CallStatic<int>("enable", args) == 1;
                if (!this.hasAudioFocus)
                {
                    AudioListener.volume = 0f;
                }
                else if (flag)
                {
                    AudioListener.volume = this.originalVolume.Value;
                }
            }
        }

        public PoolableAudioSource getFirstPlayingAudioSource(AudioSourceType ast)
        {
            for (int i = 0; i < this.m_activePoolableAudioSources.Count; i++)
            {
                if (this.m_activePoolableAudioSources[i].AudioSourceType == ast)
                {
                    return this.m_activePoolableAudioSources[i];
                }
            }
            return null;
        }

        public AudioSourceType GetTargetMusic()
        {
            return this.m_targetMusic;
        }

        private bool isPersistentAudioSource(AudioSourceType audioSourceType)
        {
            return this.m_persistentAudioSourceQuickLookup[audioSourceType];
        }

        [DebuggerHidden]
        private IEnumerator musicRoutine()
        {
            <musicRoutine>c__IteratorE0 re = new <musicRoutine>c__IteratorE0();
            re.<>f__this = this;
            return re;
        }

        public int numberOfSfxAudioGroupPlaying(AudioGroupType agt)
        {
            int num = 0;
            List<AudioSourceType> astList = this.m_audioGroups[agt].GetAstList();
            for (int i = 0; i < astList.Count; i++)
            {
                AudioSourceType type = astList[i];
                for (int j = 0; j < this.m_activePoolableAudioSources.Count; j++)
                {
                    if (this.m_activePoolableAudioSources[j].AudioSourceType == type)
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int numberOfSfxAudioSourcePlaying(AudioSourceType ast)
        {
            int num = 0;
            for (int i = 0; i < this.m_activePoolableAudioSources.Count; i++)
            {
                if (this.m_activePoolableAudioSources[i].AudioSourceType == ast)
                {
                    num++;
                }
            }
            return num;
        }

        private void onAchievementClaimed(Player player, string achievementId, int tier)
        {
            this.playSfx(AudioSourceType.SfxUi_AchievementClaim, (float) 0f);
        }

        protected void OnApplicationPause(bool paused)
        {
            if ((App.Binder.AppContext != null) && App.Binder.AppContext.systemsShouldReactToApplicationPause())
            {
                if (paused)
                {
                    this.m_snapshotPriorToPause = this.m_activeSnapshot;
                    this.transitionToSnapshot(AudioMixerSnapshotType.Muted, 0f);
                }
                else if (this.m_snapshotPriorToPause != AudioMixerSnapshotType.UNSPECIFIED)
                {
                    this.transitionToSnapshot(this.m_snapshotPriorToPause, 0.3f);
                }
            }
        }

        public void onAudioFocusChange(string focusChange)
        {
            if (!this.originalVolume.HasValue)
            {
                this.originalVolume = new float?(AudioListener.volume);
            }
            switch ((int.Parse(focusChange) + 3))
            {
                case 0:
                case 1:
                case 2:
                    if (this.hasAudioFocus)
                    {
                        this.hasAudioFocus = false;
                        AudioListener.volume = 0f;
                    }
                    break;

                case 4:
                case 5:
                case 6:
                case 7:
                    if (!this.hasAudioFocus)
                    {
                        this.hasAudioFocus = true;
                        AudioListener.volume = this.originalVolume.Value;
                    }
                    break;
            }
        }

        private void onBuffEnded(CharacterInstance c, Buff buff)
        {
            if (((buff.FromPerk == PerkType.SkillUpgradeWhirlwind2) && !c.IsPlayerCharacter) && c.IsDead)
            {
                this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_HeroHitIce, 2);
            }
        }

        private void onBuffStarted(CharacterInstance c, Buff buff)
        {
            if ((buff.FromPerk == PerkType.SkillUpgradeWhirlwind2) && !c.IsPlayerCharacter)
            {
                this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_SkillWhirlwindFreeze, 1);
            }
        }

        private void onCharacterKilled(CharacterInstance character, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            if (character.IsPlayerCharacter)
            {
                if (character.IsPrimaryPlayerCharacter)
                {
                    this.playSfx(AudioSourceType.SfxGameplay_HeroDeath, (float) 0f);
                }
                else if (character.Prefab == CharacterPrefab.Critter)
                {
                    this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_CritterDeath, 3);
                }
                else
                {
                    this.playSfxIfNotAlreadyPlaying(AudioSourceType.SfxGameplay_SkillCloneDeath);
                }
            }
            else if (killer != null)
            {
                float num = 1f;
                float num2 = 1f;
                if (character.IsBoss)
                {
                    if (character.Type == GameLogic.CharacterType.Yeti)
                    {
                        num2 = 0.9f;
                        num = 1.55f;
                    }
                    else
                    {
                        num2 = 0.6f;
                        num = 1.33f;
                    }
                }
                PlaybackParameters parameters2 = new PlaybackParameters();
                parameters2.PitchMin = num2;
                parameters2.PitchMax = num2;
                parameters2.VolumeMultiplier = num;
                PlaybackParameters pp = parameters2;
                switch (character.Type)
                {
                    case GameLogic.CharacterType.Skeleton:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_SkeletonDeath, pp, 2);
                        break;

                    case GameLogic.CharacterType.Jelly:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_JellyDeath, pp, 2);
                        break;

                    case GameLogic.CharacterType.Pygmy:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_PygmyDeath, pp, 2);
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_PygmyRattle, pp, 1);
                        break;

                    case GameLogic.CharacterType.Worg:
                        this.playSfxIfNotAlreadyPlaying(AudioSourceType.SfxGameplay_WorgDeath, pp);
                        break;

                    case GameLogic.CharacterType.IceGoblin:
                    case GameLogic.CharacterType.Goblin:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_GoblinDeath, pp, 2);
                        break;

                    case GameLogic.CharacterType.Yeti:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_YetiDeath, pp, 2);
                        break;

                    case GameLogic.CharacterType.Crocodile:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_CrocDeath, pp, 2);
                        break;

                    case GameLogic.CharacterType.Rat:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_RatDeath, pp, 2);
                        break;

                    case GameLogic.CharacterType.Mummy:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_MummyDeath, pp, 2);
                        break;

                    case GameLogic.CharacterType.Anubis:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_AnubisDeath, pp, 2);
                        break;

                    case GameLogic.CharacterType.Shroom:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_ShroomDeath, pp, 2);
                        break;
                }
            }
        }

        private void onCharacterMeleeAttackContact(CharacterInstance sourceCharacter, Vector3 contactWorldPt, bool critted)
        {
            if (!sourceCharacter.IsPlayerCharacter)
            {
                this.onPlayerReceiveDamage(sourceCharacter);
            }
            else
            {
                PlaybackParameters pp = new PlaybackParameters();
                pp.PitchMin = 0.92f;
                pp.PitchMax = 1.08f;
                PlaybackParameters parameters = pp;
                pp = new PlaybackParameters();
                pp.PitchMin = 1.22f;
                pp.PitchMax = 1.33f;
                pp.VolumeMultiplier = 0.5f;
                PlaybackParameters parameters2 = pp;
                if (sourceCharacter.Type == GameLogic.CharacterType.Pet)
                {
                    switch (sourceCharacter.Prefab)
                    {
                        case CharacterPrefab.PetMoose1:
                            this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_PetAttackHeadbutt, parameters, 2);
                            break;

                        case CharacterPrefab.PetPanda1:
                            this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_PetAttackPunch, parameters, 2);
                            break;
                    }
                }
                else if (critted)
                {
                    this.playSfx(AudioSourceType.SfxGameplay_HeroHitCrit, parameters);
                }
                else
                {
                    ItemInstance instance = sourceCharacter.getEquippedItemOfType(ItemType.Weapon);
                    if (instance == null)
                    {
                        if (sourceCharacter.IsSupport)
                        {
                            this.playSfxGrp(AudioGroupType.SfxGrpGameplay_HeroHitSword, parameters2);
                        }
                    }
                    else
                    {
                        switch (instance.Item.Accessories.Keys[0])
                        {
                            case AccessoryType.Weapon_SwordSmall:
                            case AccessoryType.Weapon_SwordBig:
                            case AccessoryType.Weapon_Axe:
                            case AccessoryType.Weapon_CanOpener:
                            case AccessoryType.Weapon_Scissor:
                            case AccessoryType.Weapon_Surgeon:
                            case AccessoryType.Weapon_Katana:
                            case AccessoryType.Weapon_Valiant:
                            case AccessoryType.Weapon_SnakeBiter:
                            case AccessoryType.Weapon_CrystalHammer:
                            case AccessoryType.Weapon_Avenger:
                            case AccessoryType.Weapon_Tribal3star:
                            case AccessoryType.Weapon_Naginata1star:
                                this.playSfxGrp(AudioGroupType.SfxGrpGameplay_HeroHitSword, parameters);
                                break;

                            case AccessoryType.Weapon_Stick:
                            case AccessoryType.Weapon_Basher:
                            case AccessoryType.Weapon_Caveman2star:
                            case AccessoryType.Weapon_Caveman3star:
                            case AccessoryType.Weapon_Tribal2star:
                                this.playSfxGrp(AudioGroupType.SfxGrpGameplay_HeroHitWood, parameters);
                                break;

                            case AccessoryType.Weapon_Shovel:
                            case AccessoryType.Weapon_Doom:
                            case AccessoryType.Weapon_GooShovel:
                            case AccessoryType.Weapon_FrozenHammer:
                            case AccessoryType.Weapon_SnowShovel:
                            case AccessoryType.Weapon_Pitchfork1star:
                                this.playSfxGrp(AudioGroupType.SfxGrpGameplay_HeroHitSword, parameters);
                                break;

                            case AccessoryType.Weapon_Fish:
                            case AccessoryType.Weapon_Goldfish:
                            case AccessoryType.Weapon_Ham:
                                this.playSfxGrp(AudioGroupType.SfxGrpGameplay_HeroHitWood, parameters);
                                break;

                            default:
                                this.playSfxGrp(AudioGroupType.SfxGrpGameplay_HeroHitSword, parameters);
                                break;
                        }
                        if (((sourceCharacter.TargetCharacter != null) && (sourceCharacter.TargetCharacter.Character.Type == GameLogic.CharacterType.Pygmy)) && ((UnityEngine.Random.Range(0, 10) == 0) && (this.numberOfSfxAudioGroupPlaying(AudioGroupType.SfxGrpGameplay_WoodenShieldLayer) == 0)))
                        {
                            pp = new PlaybackParameters();
                            pp.PitchMin = 0.9f;
                            pp.PitchMax = 1.1f;
                            this.playSfxGrp(AudioGroupType.SfxGrpGameplay_WoodenShieldLayer, pp);
                        }
                    }
                }
            }
        }

        private void onCharacterMeleeAttackStarted(CharacterInstance sourceCharacter, CharacterInstance targetCharacter)
        {
            if (sourceCharacter.IsPrimaryPlayerCharacter)
            {
                PlaybackParameters parameters2 = new PlaybackParameters();
                parameters2.PitchMin = 0.95f;
                parameters2.PitchMax = 1.05f;
                PlaybackParameters pp = parameters2;
                this.playSfxGrp(AudioGroupType.SfxGrpGameplay_HeroSwing, pp);
            }
        }

        private void onCharacterPreBlink(CharacterInstance c)
        {
            if (c.IsPrimaryPlayerCharacter)
            {
                this.playSfxGrp(AudioGroupType.SfxGrpGameplay_FrenzyTeleport, (float) 0f);
            }
        }

        private void onCharacterRangedAttackEnded(CharacterInstance sourceCharacter, Vector3 targetWorldPt, Projectile projectile)
        {
            if (!sourceCharacter.IsPlayerCharacter)
            {
                this.onPlayerReceiveDamage(sourceCharacter);
            }
        }

        private void onCharacterRangedAttackStarted(CharacterInstance sourceCharacter, Vector3 targetWorldPt)
        {
            AudioGroupType type2;
            CharacterPrefab prefab;
            PlaybackParameters parameters2;
            if (sourceCharacter.IsPlayerCharacter)
            {
                if (sourceCharacter.Type != GameLogic.CharacterType.Pet)
                {
                    if (sourceCharacter.Type == GameLogic.CharacterType.Dragon)
                    {
                        PlaybackParameters pp = new PlaybackParameters();
                        pp.PitchMin = 0.9f;
                        pp.PitchMax = 1.2f;
                        pp.FixedWorldPt = sourceCharacter.PhysicsBody.transform.position;
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_DragonAttack, pp, 2);
                    }
                    else if (sourceCharacter.Prefab == CharacterPrefab.Critter)
                    {
                        PlaybackParameters parameters5 = new PlaybackParameters();
                        parameters5.PitchMin = 0.9f;
                        parameters5.PitchMax = 1.2f;
                        parameters5.FixedWorldPt = sourceCharacter.PhysicsBody.transform.position;
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_CritterAttack, parameters5, 3);
                    }
                    return;
                }
                type2 = AudioGroupType.SfxGrpGameplay_DragonAttack;
                prefab = sourceCharacter.Prefab;
                switch (prefab)
                {
                    case CharacterPrefab.PetCrab1:
                    case CharacterPrefab.PetCrab2:
                        type2 = AudioGroupType.SfxGrpGameplay_PetAttackGunshot;
                        goto Label_0114;

                    case CharacterPrefab.PetSquid1:
                    case CharacterPrefab.PetSquid2:
                    case CharacterPrefab.PetWalrus1:
                        type2 = AudioGroupType.SfxGrpGameplay_PetAttackSquirt;
                        goto Label_0114;

                    case CharacterPrefab.PetYeti1:
                    case CharacterPrefab.PetStumpy1:
                    case CharacterPrefab.PetStumpy2:
                        type2 = AudioGroupType.SfxGrpGameplay_PetAttackThrow;
                        goto Label_0114;

                    case CharacterPrefab.PetShark1:
                    case CharacterPrefab.PetShark2:
                    case CharacterPrefab.PetDog1:
                        goto Label_010C;

                    case CharacterPrefab.PetMoose1:
                        type2 = AudioGroupType.SfxGrpGameplay_PetAttackHeadbutt;
                        goto Label_0114;

                    case CharacterPrefab.PetChest1:
                        type2 = AudioGroupType.SfxGrpGameplay_PetAttackCoin;
                        goto Label_0114;
                }
            }
            else
            {
                AudioGroupType uNSPECIFIED = AudioGroupType.UNSPECIFIED;
                switch (sourceCharacter.Character.RangedProjectileType)
                {
                    case ProjectileType.Fireball:
                        uNSPECIFIED = AudioGroupType.SfxGrpGameplay_EnemyAttackFireball;
                        break;

                    case ProjectileType.Rock:
                        uNSPECIFIED = AudioGroupType.SfxGrpGameplay_PetAttackThrow;
                        break;
                }
                if (uNSPECIFIED != AudioGroupType.UNSPECIFIED)
                {
                    PlaybackParameters parameters = new PlaybackParameters();
                    parameters.FixedWorldPt = sourceCharacter.PhysicsBody.transform.position;
                    this.playSfxGrpLimitPolyphony(uNSPECIFIED, parameters, 2);
                }
                return;
            }
            if ((prefab == CharacterPrefab.PetDragon1) || (prefab == CharacterPrefab.PetDragon2))
            {
            }
        Label_010C:
            type2 = AudioGroupType.SfxGrpGameplay_DragonAttack;
        Label_0114:
            parameters2 = new PlaybackParameters();
            parameters2.PitchMin = 0.9f;
            parameters2.PitchMax = 1.2f;
            parameters2.FixedWorldPt = sourceCharacter.PhysicsBody.transform.position;
            this.playSfxGrpLimitPolyphony(type2, parameters2, 2);
            if (sourceCharacter.Prefab == CharacterPrefab.PetShark1)
            {
                PlaybackParameters parameters3 = new PlaybackParameters();
                parameters3.PitchMin = 0.9f;
                parameters3.PitchMax = 1.2f;
                parameters3.FixedWorldPt = sourceCharacter.PhysicsBody.transform.position;
                this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_PetSharkBite, parameters3, 2);
            }
        }

        private void onCharacterRevived(CharacterInstance character)
        {
            if (character.IsPrimaryPlayerCharacter)
            {
                this.playSfx(AudioSourceType.SfxUi_Revive, (float) 0f);
            }
        }

        private void onCharacterSkillActivated(CharacterInstance character, SkillType skillType, float buildupTime, SkillExecutionStats executionStats)
        {
            if (skillType == SkillType.Heal)
            {
                this.playSfx(AudioSourceType.SfxGameplay_SkillHeal, (float) 0f);
            }
        }

        private void onCharacterSkillBuildupCompleted(CharacterInstance c, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (c.IsPet)
            {
                SkillType type = skillType;
                switch (type)
                {
                    case SkillType.Leap:
                    {
                        PlaybackParameters pp = new PlaybackParameters();
                        pp.PitchMin = 0.9f;
                        pp.PitchMax = 1.1f;
                        pp.FixedWorldPt = c.PhysicsBody.transform.position;
                        this.playSfx(AudioSourceType.SfxGameplay_PetSkillLeapTrigger, pp);
                        return;
                    }
                    case SkillType.Whirlwind:
                    {
                        PlaybackParameters parameters4 = new PlaybackParameters();
                        parameters4.PitchMin = 0.9f;
                        parameters4.PitchMax = 1.1f;
                        parameters4.FixedWorldPt = c.PhysicsBody.transform.position;
                        this.playSfxGrp(AudioGroupType.SfxGrpGameplay_PetSkillWhirl, parameters4);
                        return;
                    }
                    case SkillType.Omnislash:
                    {
                        PlaybackParameters parameters2 = new PlaybackParameters();
                        parameters2.PitchMin = 0.9f;
                        parameters2.PitchMax = 1.1f;
                        parameters2.FixedWorldPt = c.PhysicsBody.transform.position;
                        this.playSfxGrp(AudioGroupType.SfxGrpGameplay_PetSkillDash, parameters2);
                        return;
                    }
                }
                if (type == SkillType.Charge)
                {
                    PlaybackParameters parameters = new PlaybackParameters();
                    parameters.PitchMin = 0.9f;
                    parameters.PitchMax = 1.1f;
                    parameters.FixedWorldPt = c.PhysicsBody.transform.position;
                    this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_PetAttackHeadbutt, parameters, 1);
                }
            }
            else
            {
                switch (skillType)
                {
                    case SkillType.Leap:
                        this.playSfx(AudioSourceType.SfxGameplay_SkillLeapTrigger, (float) 0f);
                        break;

                    case SkillType.Whirlwind:
                        if (c.getPerkInstanceCount(PerkType.SkillUpgradeWhirlwind4) <= 0)
                        {
                            if (c.getPerkInstanceCount(PerkType.SkillUpgradeWhirlwind1) > 0)
                            {
                                this.playSfx(AudioSourceType.SfxGameplay_SkillWhirlwindKnockback, (float) 0f);
                            }
                            else
                            {
                                this.playSfxGrp(AudioGroupType.SfxGrpGameplay_SkillWhirlwind, (float) 0f);
                            }
                            break;
                        }
                        this.playSfxGrp(AudioGroupType.SfxGrpGameplay_SkillWhirlwind6x, (float) 0f);
                        break;

                    case SkillType.Omnislash:
                        if (c.getPerkInstanceCount(PerkType.SkillUpgradeOmnislash2) <= 0)
                        {
                            if (executionStats.EnemiesAround > 0)
                            {
                                this.playSfx(AudioSourceType.SfxGameplay_SkillOmnislash, (float) 0f);
                            }
                            break;
                        }
                        this.playSfxGrp(AudioGroupType.SfxGrpGameplay_SkillOmnislashDash, (float) 0f);
                        break;

                    case SkillType.Slam:
                        if ((c.getPerkInstanceCount(PerkType.SkillUpgradeSlam4) <= 0) && (c.getPerkInstanceCount(PerkType.SkillUpgradeSlam2) <= 0))
                        {
                            this.playSfx(AudioSourceType.SfxGameplay_SkillSlam, (float) 0f);
                            break;
                        }
                        this.playSfxGrp(AudioGroupType.SfxGrpGameplay_SkillSlamRocky, (float) 0f);
                        break;

                    case SkillType.Implosion:
                        this.playSfx(AudioSourceType.SfxGameplay_SkillImplosionCast, (float) 0f);
                        break;

                    case SkillType.Clone:
                        this.playSfx(AudioSourceType.SfxGameplay_SkillClone, (float) 0f);
                        break;

                    case SkillType.BossSummoner:
                    case SkillType.BossLeader:
                    case SkillType.BossBreeder:
                    case SkillType.BossEscaper:
                    case SkillType.BossBreederEscaper:
                        this.playSfx(AudioSourceType.SfxGameplay_SkillSummonMinions, (float) 0f);
                        break;

                    case SkillType.BossDefender:
                        this.playSfx(AudioSourceType.SfxGameplay_SkillShield, (float) 0f);
                        break;

                    case SkillType.BossSplitter:
                        this.playSfx(AudioSourceType.SfxGameplay_SkillSplit, (float) 0f);
                        break;

                    case SkillType.PoisonPuff:
                    {
                        PlaybackParameters parameters5 = new PlaybackParameters();
                        parameters5.FixedWorldPt = c.PhysicsBody.transform.position;
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_ShroomPuff, parameters5, 2);
                        break;
                    }
                }
            }
        }

        private void onCharacterSkillExecuted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (skillType == SkillType.Implosion)
            {
                this.playSfx(AudioSourceType.SfxGameplay_SkillImplosion, (float) 0f);
            }
        }

        private void onCharacterSkillExecutionMidpoint(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (character.IsPet)
            {
                if (skillType == SkillType.Leap)
                {
                    this.playSfx(AudioSourceType.SfxGameplay_PetSkillLeapSlam, (float) 0f);
                }
            }
            else
            {
                switch (skillType)
                {
                    case SkillType.Omnislash:
                        if ((executionStats.EnemiesAround > 0) && (character.getPerkInstanceCount(PerkType.SkillUpgradeOmnislash2) == 0))
                        {
                            this.playSfxGrp(AudioGroupType.SfxGrpGameplay_SkillOmnislashHit, (float) 0f);
                        }
                        return;

                    case SkillType.Implosion:
                        return;

                    case SkillType.Leap:
                        this.playSfx(AudioSourceType.SfxGameplay_SkillLeapSlam, (float) 0f);
                        break;
                }
            }
        }

        protected void OnDisable()
        {
            this.DisableAudioFocusChangeListener();
            GameLogic.Binder.EventBus.OnGameStateInitialized -= new GameLogic.Events.GameStateInitialized(this.onGamestateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            GameLogic.Binder.EventBus.OnGameplayStateChanged -= new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            GameLogic.Binder.EventBus.OnItemRankUpped -= new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnItemUnlocked -= new GameLogic.Events.ItemUnlocked(this.onItemUnlocked);
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackStarted -= new GameLogic.Events.CharacterMeleeAttackStarted(this.onCharacterMeleeAttackStarted);
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackContact -= new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
            GameLogic.Binder.EventBus.OnCharacterRangedAttackStarted -= new GameLogic.Events.CharacterRangedAttackStarted(this.onCharacterRangedAttackStarted);
            GameLogic.Binder.EventBus.OnCharacterRangedAttackEnded -= new GameLogic.Events.CharacterRangedAttackEnded(this.onCharacterRangedAttackEnded);
            GameLogic.Binder.EventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnCharacterRevived -= new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            GameLogic.Binder.EventBus.OnAchievementClaimed -= new GameLogic.Events.AchievementClaimed(this.onAchievementClaimed);
            GameLogic.Binder.EventBus.OnCharacterSkillBuildupCompleted -= new GameLogic.Events.CharacterSkillBuildupCompleted(this.onCharacterSkillBuildupCompleted);
            GameLogic.Binder.EventBus.OnCharacterSkillExecutionMidpoint -= new GameLogic.Events.CharacterSkillExecutionMidpoint(this.onCharacterSkillExecutionMidpoint);
            GameLogic.Binder.EventBus.OnCharacterSkillExecuted -= new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            GameLogic.Binder.EventBus.OnCharacterSkillActivated -= new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            GameLogic.Binder.EventBus.OnBuffStarted -= new GameLogic.Events.BuffStarted(this.onBuffStarted);
            GameLogic.Binder.EventBus.OnBuffEnded -= new GameLogic.Events.BuffEnded(this.onBuffEnded);
            GameLogic.Binder.EventBus.OnProjectileSpawned -= new GameLogic.Events.ProjectileSpawned(this.onProjectileSpawned);
            GameLogic.Binder.EventBus.OnCharacterPreBlink -= new GameLogic.Events.CharacterPreBlink(this.onCharacterPreBlink);
            GameLogic.Binder.EventBus.OnFrenzyActivated -= new GameLogic.Events.FrenzyActivated(this.onFrenzyActivated);
            GameLogic.Binder.EventBus.OnFrenzyDeactivated -= new GameLogic.Events.FrenzyDeactivated(this.onFrenzyDeactivated);
            GameLogic.Binder.EventBus.OnProjectileCollided -= new GameLogic.Events.ProjectileCollided(this.onProjectileCollided);
            GameLogic.Binder.EventBus.OnDungeonBoostActivated -= new GameLogic.Events.DungeonBoostActivated(this.onDungeonBoostActivated);
            PlayerView.Binder.EventBus.OnMenuChangeStarted -= new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
        }

        private void onDungeonBoostActivated(DungeonBoost dungeonBoost, SkillType fromSkill)
        {
            if ((Time.time - this.m_lastDestructibleTime) >= 0.1f)
            {
                PlaybackParameters parameters2 = new PlaybackParameters();
                parameters2.PitchMin = 0.9f;
                parameters2.PitchMax = 1.1f;
                parameters2.FixedWorldPt = dungeonBoost.gameObject.transform.position;
                PlaybackParameters pp = parameters2;
                switch (dungeonBoost.PrefabType)
                {
                    case DungeonBoostPrefabType.Barrel:
                    case DungeonBoostPrefabType.Cache:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_SmashCrate, pp, 3);
                        break;

                    case DungeonBoostPrefabType.Urn:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_SmashPot, pp, 3);
                        break;

                    case DungeonBoostPrefabType.Pumpkin:
                        this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_SmashPumpkin, pp, 3);
                        break;
                }
                if ((dungeonBoost.Properties.ActivationType == DungeonBoostActivationType.DestructibleHit) && (dungeonBoost.Properties.BuffPerkType == PerkType.DungeonBoostPoison))
                {
                    this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_SmashPoison, pp, 2);
                }
                this.m_lastDestructibleTime = Time.time;
            }
        }

        protected void OnEnable()
        {
            GameLogic.Binder.EventBus.OnGameStateInitialized += new GameLogic.Events.GameStateInitialized(this.onGamestateInitialized);
            GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            GameLogic.Binder.EventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            GameLogic.Binder.EventBus.OnGameplayStateChanged += new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            GameLogic.Binder.EventBus.OnItemRankUpped += new GameLogic.Events.ItemRankUpped(this.onItemRankUpped);
            GameLogic.Binder.EventBus.OnItemUnlocked += new GameLogic.Events.ItemUnlocked(this.onItemUnlocked);
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackStarted += new GameLogic.Events.CharacterMeleeAttackStarted(this.onCharacterMeleeAttackStarted);
            GameLogic.Binder.EventBus.OnCharacterMeleeAttackContact += new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
            GameLogic.Binder.EventBus.OnCharacterRangedAttackStarted += new GameLogic.Events.CharacterRangedAttackStarted(this.onCharacterRangedAttackStarted);
            GameLogic.Binder.EventBus.OnCharacterRangedAttackEnded += new GameLogic.Events.CharacterRangedAttackEnded(this.onCharacterRangedAttackEnded);
            GameLogic.Binder.EventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            GameLogic.Binder.EventBus.OnCharacterRevived += new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            GameLogic.Binder.EventBus.OnAchievementClaimed += new GameLogic.Events.AchievementClaimed(this.onAchievementClaimed);
            GameLogic.Binder.EventBus.OnCharacterSkillBuildupCompleted += new GameLogic.Events.CharacterSkillBuildupCompleted(this.onCharacterSkillBuildupCompleted);
            GameLogic.Binder.EventBus.OnCharacterSkillExecutionMidpoint += new GameLogic.Events.CharacterSkillExecutionMidpoint(this.onCharacterSkillExecutionMidpoint);
            GameLogic.Binder.EventBus.OnCharacterSkillExecuted += new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            GameLogic.Binder.EventBus.OnCharacterSkillActivated += new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            GameLogic.Binder.EventBus.OnBuffStarted += new GameLogic.Events.BuffStarted(this.onBuffStarted);
            GameLogic.Binder.EventBus.OnBuffEnded += new GameLogic.Events.BuffEnded(this.onBuffEnded);
            GameLogic.Binder.EventBus.OnProjectileSpawned += new GameLogic.Events.ProjectileSpawned(this.onProjectileSpawned);
            GameLogic.Binder.EventBus.OnCharacterPreBlink += new GameLogic.Events.CharacterPreBlink(this.onCharacterPreBlink);
            GameLogic.Binder.EventBus.OnFrenzyActivated += new GameLogic.Events.FrenzyActivated(this.onFrenzyActivated);
            GameLogic.Binder.EventBus.OnFrenzyDeactivated += new GameLogic.Events.FrenzyDeactivated(this.onFrenzyDeactivated);
            GameLogic.Binder.EventBus.OnProjectileCollided += new GameLogic.Events.ProjectileCollided(this.onProjectileCollided);
            GameLogic.Binder.EventBus.OnDungeonBoostActivated += new GameLogic.Events.DungeonBoostActivated(this.onDungeonBoostActivated);
            PlayerView.Binder.EventBus.OnMenuChangeStarted += new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
            this.m_lastPlayerPainTime = 0f;
            base.StartCoroutine(this.musicRoutine());
            base.StartCoroutine(this.ambienceRoutine());
            this.EnableAudioFocusChangeListener();
        }

        private void onFrenzyActivated()
        {
            this.playMusic(AudioSourceType.Music_Frenzy);
        }

        private void onFrenzyDeactivated()
        {
            this.playSfx(AudioSourceType.Music_JingleFrenzyEnd, (float) 0f);
            this.playMusic(AudioSourceType.Music_Gameplay);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            if (!activeDungeon.SeamlessTransition)
            {
                for (int i = this.m_activePoolableAudioSources.Count - 1; i >= 0; i--)
                {
                    PoolableAudioSource source = this.m_activePoolableAudioSources[i];
                    if (!this.isPersistentAudioSource(source.AudioSourceType))
                    {
                        this.m_dynamicAudioSourcePool.returnObject(source, source.AudioSourceType);
                        this.m_activePoolableAudioSources.Remove(source);
                    }
                }
                this.m_dynamicAudioSourcePool.destroy();
                this.m_dynamicAudioSourcePool = null;
            }
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            if (!activeDungeon.SeamlessTransition)
            {
                Dictionary<AudioSourceType, int> initialCapacityPerType = ConfigObjectPools.PER_THEME_AUDIO_SOURCES[activeDungeon.Dungeon.Theme];
                this.m_dynamicAudioSourcePool = new TypedObjectPool<PoolableAudioSource, AudioSourceType>(new PoolableAudioSourceProvider(Layers.DEFAULT, App.Binder.DynamicObjectRootTm), 4, initialCapacityPerType, ObjectPoolExpansionMethod.DOUBLE, true);
            }
        }

        private void onGameplayStateChanged(GameplayState previousState, GameplayState currentState)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if ((currentState != GameplayState.ENDED) && (currentState != GameplayState.UNDETERMINED))
            {
                if (activeDungeon.Dungeon.Theme == DungeonThemeType.Theme002)
                {
                    this.playAmbience(AudioSourceType.Ambience_Cloudy);
                }
                else if (activeDungeon.Dungeon.Theme == DungeonThemeType.Theme003)
                {
                    this.playAmbience(AudioSourceType.Ambience_Winter);
                }
                else
                {
                    this.playAmbience(AudioSourceType.Ambience_Dungeon);
                }
            }
            else
            {
                this.stopAmbience();
            }
            if (currentState == GameplayState.START_CEREMONY_STEP1)
            {
                if (this.m_activeSnapshot != AudioMixerSnapshotType.Menu)
                {
                    this.transitionToSnapshot(AudioMixerSnapshotType.Gameplay, 0.6f);
                }
                this.playMusic(!GameLogic.Binder.FrenzySystem.isFrenzyActive() ? AudioSourceType.Music_Gameplay : AudioSourceType.Music_Frenzy);
            }
            else if (currentState == GameplayState.BOSS_START)
            {
                this.stopMusic();
                this.playSfx(AudioSourceType.Music_JingleBoss, (float) 0f);
            }
            else if (currentState == GameplayState.BOSS_FIGHT)
            {
                if (!this.m_isMenuOpen)
                {
                    this.transitionToSnapshot(AudioMixerSnapshotType.Gameplay, 0.3f);
                }
                this.playMusic(AudioSourceType.Music_Boss);
            }
            else if (currentState == GameplayState.ROOM_COMPLETION)
            {
                Room activeRoom = activeDungeon.ActiveRoom;
                if ((!activeDungeon.isTutorialDungeon() && activeRoom.MainBossSummoned) && (activeRoom.EndCondition == RoomEndCondition.NORMAL_COMPLETION))
                {
                    this.playSfx(AudioSourceType.Music_JingleVictory, (float) 0f);
                    this.stopMusic();
                }
                else if (activeRoom.EndCondition == RoomEndCondition.FAIL)
                {
                    this.transitionToSnapshot(AudioMixerSnapshotType.KnockedDown, 0.3f);
                }
            }
            else if (currentState == GameplayState.END_CEREMONY)
            {
                if (this.m_activeMusic == AudioSourceType.Music_Boss)
                {
                    this.stopMusic();
                }
                if (this.m_activeSnapshot == AudioMixerSnapshotType.KnockedDown)
                {
                }
            }
            else if (currentState == GameplayState.ACTION)
            {
                if (!this.m_isMenuOpen)
                {
                    this.transitionToSnapshot(AudioMixerSnapshotType.Gameplay, 0.3f);
                }
            }
            else if (currentState == GameplayState.RETIREMENT)
            {
                this.stopMusic();
                this.playSfx(AudioSourceType.SfxGameplay_HeroAscend, (float) 0f);
            }
        }

        private void onGamestateInitialized()
        {
            this.transitionToSnapshot(AudioMixerSnapshotType.Startup, 0f);
        }

        private void onItemRankUpped(CharacterInstance character, ItemInstance itemInstance, int rankUpCount, bool free)
        {
            this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpUi_ItemRankUp, 8);
        }

        private void onItemUnlocked(CharacterInstance character, ItemInstance itemInstance)
        {
            this.playSfx(AudioSourceType.SfxUi_ItemUnlock, (float) 0f);
        }

        private void onMenuChangeStarted(MenuType sourceMenuType, MenuType targetMenuType)
        {
            this.m_isMenuOpen = targetMenuType != MenuType.NONE;
            if (this.m_activeSnapshot != AudioMixerSnapshotType.KnockedDown)
            {
                if (targetMenuType != MenuType.NONE)
                {
                    if (targetMenuType == MenuType.FullscreenAdMenu)
                    {
                        UnityEngine.Debug.Log("--------Stop Music for Ad");
                        this.m_activeSnapshotBefore = this.m_activeSnapshot;
                        this.transitionToSnapshot(AudioMixerSnapshotType.Muted, 0.3f);
                    }
                    else if (sourceMenuType == MenuType.FullscreenAdMenu)
                    {
                        UnityEngine.Debug.Log("--------Start Music after Ad");
                        this.transitionToSnapshot(this.m_activeSnapshotBefore, 0.3f);
                    }
                    else if (this.numberOfSfxAudioSourcePlaying(AudioSourceType.Music_JingleVictory) == 0)
                    {
                        this.transitionToSnapshot(AudioMixerSnapshotType.Menu, 0.3f);
                    }
                }
                else
                {
                    this.transitionToSnapshot(AudioMixerSnapshotType.Gameplay, 0.3f);
                }
            }
        }

        private void onPlayerReceiveDamage(CharacterInstance sourceCharacter)
        {
            if ((sourceCharacter.TargetCharacter != null) && ((((Time.time - this.m_lastPlayerPainTime) > 10f) && sourceCharacter.TargetCharacter.IsPrimaryPlayerCharacter) && (sourceCharacter.TargetCharacter.CurrentHpNormalized <= 0.5f)))
            {
                this.playSfxGrp(AudioGroupType.SfxGrpGameplay_HeroPain, (float) 0f);
                this.m_lastPlayerPainTime = Time.time;
            }
        }

        private void onProjectileCollided(Projectile projectile, Collider collider)
        {
            if ((projectile.Properties.Type == ProjectileType.Rock) && projectile.OwningCharacter.IsPet)
            {
                PlaybackParameters pp = new PlaybackParameters();
                pp.FixedWorldPt = collider.transform.position;
                this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_PetAttackHitBoulder, pp, 4);
            }
            if (((projectile.Properties.Type == ProjectileType.Splinters1) || (projectile.Properties.Type == ProjectileType.Splinters2)) && projectile.OwningCharacter.IsPet)
            {
                PlaybackParameters parameters2 = new PlaybackParameters();
                parameters2.FixedWorldPt = collider.transform.position;
                this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_PetAttackHitFoliage, parameters2, 2);
            }
        }

        private void onProjectileSpawned(Projectile projectile)
        {
            if (projectile.Properties.Type == ProjectileType.Slam)
            {
                Player player = GameLogic.Binder.GameState.Player;
                float num = Mathf.Max((float) 1f, (float) (Vector3Extensions.XzDistanceTo(projectile.Transform.position, player.ActiveCharacter.PhysicsBody.transform.position) * 0.5f));
                PlaybackParameters parameters2 = new PlaybackParameters();
                parameters2.PitchMin = 0.9f;
                parameters2.PitchMax = 1.1f;
                parameters2.VolumeMultiplier = 1f / num;
                PlaybackParameters pp = parameters2;
                this.playSfxGrpLimitPolyphony(AudioGroupType.SfxGrpGameplay_SkillSlamAftershock, pp, 4);
            }
        }

        public void playAmbience(AudioSourceType ast)
        {
            if (!Application.isEditor || !ConfigApp.CHEAT_EDITOR_AUDIO_MUTE)
            {
                this.m_targetAmbience = ast;
            }
        }

        public void playItemEquipSfx(ItemType type)
        {
            switch (type)
            {
                case ItemType.Weapon:
                    this.playSfx(AudioSourceType.SfxUi_ItemEquip_Weapon_Metal, (float) 0f);
                    break;

                case ItemType.Armor:
                    this.playSfx(AudioSourceType.SfxUi_ItemEquip_Armor_Metal, (float) 0f);
                    break;

                case ItemType.Cloak:
                    this.playSfx(AudioSourceType.SfxUi_ItemEquip_Helmet_Leather, (float) 0f);
                    break;
            }
        }

        public void playMusic(AudioSourceType ast)
        {
            if (!Application.isEditor || !ConfigApp.CHEAT_EDITOR_AUDIO_MUTE)
            {
                this.m_targetMusic = ast;
            }
        }

        public void playSfx(AudioSourceType ast, PlaybackParameters pp)
        {
            Player player = GameLogic.Binder.GameState.Player;
            if ((((((player.SoundEnabled || (((AudioCategoryType) this.m_audioSourceTypeToCategoryMap[ast]) != AudioCategoryType.SfxUi)) && (player.SoundEnabled || (((AudioCategoryType) this.m_audioSourceTypeToCategoryMap[ast]) != AudioCategoryType.SfxGameplay))) && (player.MusicEnabled || (((AudioCategoryType) this.m_audioSourceTypeToCategoryMap[ast]) != AudioCategoryType.Music))) && (this.numberOfSfxAudioSourcePlaying(ast) < 0x18)) && (!Application.isEditor || !ConfigApp.CHEAT_EDITOR_AUDIO_MUTE)) && (this.isPersistentAudioSource(ast) || (this.m_dynamicAudioSourcePool != null)))
            {
                PoolableAudioSource source;
                if (this.isPersistentAudioSource(ast))
                {
                    source = PlayerView.Binder.PersistentAudioSourcePool.getObject(ast);
                }
                else
                {
                    source = this.m_dynamicAudioSourcePool.getObject(ast);
                }
                source.gameObject.SetActive(true);
                if (pp.FixedWorldPt != Vector3.zero)
                {
                    source.Tm.position = pp.FixedWorldPt;
                }
                else
                {
                    source.Tm.localPosition = Vector3.zero;
                }
                if (((pp.PitchMin > 0f) && (pp.PitchMax > 0f)) && ((pp.PitchMin != 1f) || (pp.PitchMax != 1f)))
                {
                    source.AudioSource.pitch = UnityEngine.Random.Range(pp.PitchMin, pp.PitchMax);
                }
                if (pp.VolumeMultiplier > 0f)
                {
                    source.AudioSource.volume = source.OrigVolume * pp.VolumeMultiplier;
                }
                else if (((pp.VolumeMin > 0f) && (pp.VolumeMax > 0f)) && ((pp.VolumeMin != 1f) || (pp.VolumeMax != 1f)))
                {
                    source.AudioSource.volume = UnityEngine.Random.Range(pp.VolumeMin, pp.VolumeMax);
                }
                if ((pp.DelayMin > 0f) || (pp.DelayMax > 0f))
                {
                    source.AudioSource.PlayDelayed(UnityEngine.Random.Range(pp.DelayMin, pp.DelayMax));
                }
                else
                {
                    source.AudioSource.Play();
                }
                this.m_activePoolableAudioSources.Add(source);
            }
        }

        public void playSfx(AudioSourceType ast, [Optional, DefaultParameterValue(0f)] float delay)
        {
            PlaybackParameters parameters2 = new PlaybackParameters();
            parameters2.DelayMin = delay;
            parameters2.DelayMax = delay;
            PlaybackParameters pp = parameters2;
            this.playSfx(ast, pp);
        }

        public void playSfxGrp(AudioGroupType agt, PlaybackParameters pp)
        {
            AudioSourceType nextAst = this.m_audioGroups[agt].GetNextAst();
            this.playSfx(nextAst, pp);
        }

        public void playSfxGrp(AudioGroupType agt, [Optional, DefaultParameterValue(0f)] float delay)
        {
            PlaybackParameters parameters2 = new PlaybackParameters();
            parameters2.DelayMin = delay;
            parameters2.DelayMax = delay;
            PlaybackParameters pp = parameters2;
            this.playSfxGrp(agt, pp);
        }

        public void playSfxGrpLimitPolyphony(AudioGroupType agt, [Optional, DefaultParameterValue(1)] int maxPlaying)
        {
            if (this.numberOfSfxAudioGroupPlaying(agt) < maxPlaying)
            {
                this.playSfxGrp(agt, (float) 0f);
            }
        }

        public void playSfxGrpLimitPolyphony(AudioGroupType agt, PlaybackParameters pp, [Optional, DefaultParameterValue(1)] int maxPlaying)
        {
            if (this.numberOfSfxAudioGroupPlaying(agt) < maxPlaying)
            {
                this.playSfxGrp(agt, pp);
            }
        }

        public void playSfxIfNotAlreadyPlaying(AudioSourceType ast)
        {
            if (this.numberOfSfxAudioSourcePlaying(ast) <= 0)
            {
                this.playSfx(ast, (float) 0f);
            }
        }

        public void playSfxIfNotAlreadyPlaying(AudioSourceType ast, PlaybackParameters pp)
        {
            if (this.numberOfSfxAudioSourcePlaying(ast) <= 0)
            {
                this.playSfx(ast, pp);
            }
        }

        public void stopAmbience()
        {
            this.m_targetAmbience = AudioSourceType.UNSPECIFIED;
        }

        public void stopAudioGroup(AudioGroupType agt)
        {
            for (int i = 0; i < this.m_activePoolableAudioSources.Count; i++)
            {
                if (this.m_audioGroups[agt].GetAstList().Contains(this.m_activePoolableAudioSources[i].AudioSourceType))
                {
                    this.m_activePoolableAudioSources[i].AudioSource.Stop();
                }
            }
        }

        public void stopAudioSources(AudioSourceType ast)
        {
            for (int i = 0; i < this.m_activePoolableAudioSources.Count; i++)
            {
                if (this.m_activePoolableAudioSources[i].AudioSourceType == ast)
                {
                    this.m_activePoolableAudioSources[i].AudioSource.Stop();
                }
            }
        }

        public void stopMusic()
        {
            this.m_targetMusic = AudioSourceType.UNSPECIFIED;
        }

        private void transitionToSnapshot(AudioMixerSnapshotType targetSnapshot, float duration)
        {
            if ((targetSnapshot != AudioMixerSnapshotType.UNSPECIFIED) && (this.m_activeSnapshot != targetSnapshot))
            {
                for (int i = 0; i < this.m_audioMixerSnapshots.Length; i++)
                {
                    if (this.m_audioMixerSnapshots[i] == this.m_audioMixerSnapshotMap[targetSnapshot])
                    {
                        this.m_audioMixerSnapshotWeights[i] = 1f;
                    }
                    else
                    {
                        this.m_audioMixerSnapshotWeights[i] = 0f;
                    }
                }
                this.Mixer.TransitionToSnapshots(this.m_audioMixerSnapshots, this.m_audioMixerSnapshotWeights, duration * Time.timeScale);
                this.m_activeSnapshot = targetSnapshot;
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                if (this.m_activeSnapshot != AudioMixerSnapshotType.Menu)
                {
                    if ((activeDungeon != null) && (activeDungeon.Dungeon.Theme == DungeonThemeType.Theme002))
                    {
                        this.Mixer.SetFloat("ReverbRoom", -2500f);
                        this.Mixer.SetFloat("ReverbDecay", 6f);
                    }
                    else if ((activeDungeon != null) && (activeDungeon.Dungeon.Theme == DungeonThemeType.Theme003))
                    {
                        this.Mixer.SetFloat("ReverbRoom", -2500f);
                        this.Mixer.SetFloat("ReverbDecay", 6f);
                    }
                    else
                    {
                        this.Mixer.SetFloat("ReverbRoom", -1800f);
                        this.Mixer.SetFloat("ReverbDecay", 1.8f);
                    }
                }
                else
                {
                    this.Mixer.SetFloat("ReverbRoom", -1014f);
                }
            }
        }

        protected void Update()
        {
            for (int i = this.m_activePoolableAudioSources.Count - 1; i >= 0; i--)
            {
                PoolableAudioSource source = this.m_activePoolableAudioSources[i];
                if (!source.AudioSource.isPlaying)
                {
                    if (this.isPersistentAudioSource(source.AudioSourceType))
                    {
                        PlayerView.Binder.PersistentAudioSourcePool.returnObject(source, source.AudioSourceType);
                    }
                    else
                    {
                        this.m_dynamicAudioSourcePool.returnObject(source, source.AudioSourceType);
                    }
                    this.m_activePoolableAudioSources.Remove(source);
                }
            }
        }

        public AudioMixer Mixer
        {
            [CompilerGenerated]
            get
            {
                return this.<Mixer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Mixer>k__BackingField = value;
            }
        }

        public Transform Tm
        {
            [CompilerGenerated]
            get
            {
                return this.<Tm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Tm>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <ambienceRoutine>c__IteratorE1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AudioSystem <>f__this;
            internal float <duration>__4;
            internal PoolableAudioSource <from>__2;
            internal IEnumerator <ie>__5;
            internal Player <player>__0;
            internal AudioSourceType <targetAmbience>__1;
            internal PoolableAudioSource <to>__3;

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
                        break;

                    case 1:
                        goto Label_0030;

                    case 2:
                        goto Label_01CF;

                    case 3:
                        break;
                        this.$PC = -1;
                        goto Label_0220;

                    default:
                        goto Label_0220;
                }
                this.<player>__0 = null;
            Label_0030:
                this.<player>__0 = GameLogic.Binder.GameState.Player;
                if (this.<player>__0 == null)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_0222;
                }
                this.<targetAmbience>__1 = (!this.<player>__0.MusicEnabled && !this.<player>__0.SoundEnabled) ? AudioSourceType.UNSPECIFIED : this.<>f__this.m_targetAmbience;
                if (this.<>f__this.m_activeAmbience == this.<targetAmbience>__1)
                {
                    goto Label_0201;
                }
                this.<from>__2 = null;
                this.<to>__3 = null;
                if (this.<>f__this.m_activeAmbience != AudioSourceType.UNSPECIFIED)
                {
                    this.<from>__2 = this.<>f__this.m_activeAmbienceSource;
                }
                if (this.<targetAmbience>__1 != AudioSourceType.UNSPECIFIED)
                {
                    if (this.<>f__this.isPersistentAudioSource(this.<targetAmbience>__1))
                    {
                        this.<to>__3 = PlayerView.Binder.PersistentAudioSourcePool.getObject(this.<targetAmbience>__1);
                    }
                    else
                    {
                        this.<to>__3 = this.<>f__this.m_dynamicAudioSourcePool.getObject(this.<targetAmbience>__1);
                    }
                }
                if (this.<to>__3 != null)
                {
                    this.<to>__3.gameObject.SetActive(true);
                    this.<>f__this.m_activePoolableAudioSources.Add(this.<to>__3);
                }
                this.<duration>__4 = 0.3f;
                this.<ie>__5 = this.<>f__this.crossFade(this.<from>__2, this.<to>__3, this.<duration>__4, this.<>f__this.m_crossFadeTimerAmbience);
            Label_01CF:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 2;
                    goto Label_0222;
                }
                this.<>f__this.m_activeAmbience = this.<targetAmbience>__1;
                this.<>f__this.m_activeAmbienceSource = this.<to>__3;
            Label_0201:
                this.$current = null;
                this.$PC = 3;
                goto Label_0222;
            Label_0220:
                return false;
            Label_0222:
                return true;
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
        private sealed class <crossFade>c__IteratorE2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>duration;
            internal PoolableAudioSource <$>from;
            internal ManualTimer <$>timer;
            internal PoolableAudioSource <$>to;
            internal float duration;
            internal PoolableAudioSource from;
            internal ManualTimer timer;
            internal PoolableAudioSource to;

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
                        if (this.to != null)
                        {
                            this.to.AudioSource.Play();
                        }
                        this.timer.set(this.duration);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_018E;
                }
                while (!this.timer.Idle)
                {
                    if (this.from != null)
                    {
                        this.from.AudioSource.volume = this.from.OrigVolume * (1f - this.timer.normalizedProgress());
                    }
                    if (this.to != null)
                    {
                        this.to.AudioSource.volume = this.to.OrigVolume * this.timer.normalizedProgress();
                    }
                    if ((this.from == null) && (this.to == null))
                    {
                        this.timer.end();
                    }
                    this.timer.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                if (this.from != null)
                {
                    this.from.AudioSource.Stop();
                }
                if (this.to != null)
                {
                    this.to.AudioSource.volume = this.to.OrigVolume;
                    goto Label_018E;
                    this.$PC = -1;
                }
            Label_018E:
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
        private sealed class <musicRoutine>c__IteratorE0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AudioSystem <>f__this;
            internal float <duration>__4;
            internal PoolableAudioSource <from>__2;
            internal IEnumerator <ie>__5;
            internal Player <player>__0;
            internal AudioSourceType <targetMusic>__1;
            internal PoolableAudioSource <to>__3;

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
                        break;

                    case 1:
                        goto Label_0030;

                    case 2:
                        goto Label_0181;

                    case 3:
                        break;
                        this.$PC = -1;
                        goto Label_01C1;

                    default:
                        goto Label_01C1;
                }
                this.<player>__0 = null;
            Label_0030:
                this.<player>__0 = GameLogic.Binder.GameState.Player;
                if (this.<player>__0 == null)
                {
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01C3;
                }
                this.<targetMusic>__1 = !this.<player>__0.MusicEnabled ? AudioSourceType.UNSPECIFIED : this.<>f__this.m_targetMusic;
                if (this.<>f__this.m_activeMusic == this.<targetMusic>__1)
                {
                    goto Label_01A2;
                }
                this.<from>__2 = null;
                this.<to>__3 = null;
                if (this.<>f__this.m_activeMusic != AudioSourceType.UNSPECIFIED)
                {
                    this.<from>__2 = this.<>f__this.m_musicAudioSources[this.<>f__this.m_activeMusic];
                }
                if (this.<targetMusic>__1 != AudioSourceType.UNSPECIFIED)
                {
                    this.<to>__3 = this.<>f__this.m_musicAudioSources[this.<targetMusic>__1];
                }
                this.<duration>__4 = (this.<from>__2 != null) ? 0.3f : 0f;
                this.<ie>__5 = this.<>f__this.crossFade(this.<from>__2, this.<to>__3, this.<duration>__4, this.<>f__this.m_crossFadeTimerMusic);
            Label_0181:
                while (this.<ie>__5.MoveNext())
                {
                    this.$current = this.<ie>__5.Current;
                    this.$PC = 2;
                    goto Label_01C3;
                }
                this.<>f__this.m_activeMusic = this.<targetMusic>__1;
            Label_01A2:
                this.$current = null;
                this.$PC = 3;
                goto Label_01C3;
            Label_01C1:
                return false;
            Label_01C3:
                return true;
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
        public struct PlaybackParameters
        {
            public float DelayMin;
            public float DelayMax;
            public float PitchMin;
            public float PitchMax;
            public float VolumeMin;
            public float VolumeMax;
            public float VolumeMultiplier;
            public Vector3 FixedWorldPt;
        }
    }
}

