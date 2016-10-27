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

    public class RoomCamera : MonoBehaviour
    {
        [CompilerGenerated]
        private Vector3 <BoundariesMax>k__BackingField;
        [CompilerGenerated]
        private Vector3 <BoundariesMin>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Camera <Camera>k__BackingField;
        [CompilerGenerated]
        private PlayerView.CustomShaderParameters <CustomShaderParameters>k__BackingField;
        [CompilerGenerated]
        private CameraFieldOfViewAnimator <FovAnimator>k__BackingField;
        [CompilerGenerated]
        private Vector3 <Offset>k__BackingField;
        [CompilerGenerated]
        private float <Orientation>k__BackingField;
        [CompilerGenerated]
        private PlayerView.ParallaxBackground <ParallaxBackground>k__BackingField;
        [CompilerGenerated]
        private PlayerView.ParallaxCamera <ParallaxCamera>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        [CompilerGenerated]
        private TransformAnimation <TransformAnimation>k__BackingField;
        private ICameraMode m_activeCameraMode;
        private int m_cameraCullingMask;
        private CameraShake m_cameraShake;
        private Vector3 m_defaultOffset;
        private Vector3 m_initialOffset;
        private Quaternion m_initialRotation;
        private Coroutine m_moodTransitionRoutine;
        private List<OccludedObject> m_occludedObjects = new List<OccludedObject>(4);
        private List<OccludedObject> m_occludedObjectsNew = new List<OccludedObject>(4);
        private int m_parallaxCameraCullingMask;
        private Coroutine m_teleportFollowRoutine;
        public const float SLIDING_ADVENTURE_PANEL_CAMERA_OFFSET = -4f;
        public const float SLIDING_TASK_PANEL_CAMERA_OFFSET = 2f;
        public UnityEngine.Transform WeatherTransform;

        protected void Awake()
        {
            this.Transform = base.transform;
            this.Camera = base.GetComponent<UnityEngine.Camera>();
            this.TransformAnimation = GameObjectExtensions.AddOrGetComponent<TransformAnimation>(base.gameObject);
            this.FovAnimator = GameObjectExtensions.AddOrGetComponent<CameraFieldOfViewAnimator>(base.gameObject);
            this.ParallaxCamera = base.GetComponentInChildren<PlayerView.ParallaxCamera>();
            this.ParallaxCamera.initialize();
            this.ParallaxBackground = base.GetComponentInChildren<PlayerView.ParallaxBackground>();
            if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
            {
                this.Camera.farClipPlane = 27f;
            }
            else if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Med)
            {
                this.Camera.farClipPlane = 36f;
            }
            else
            {
                this.Camera.farClipPlane = 100f;
            }
            switch (ConfigCamera.CAMERA_MODE)
            {
                case ConfigCamera.CameraMode.Follow:
                {
                    FollowCameraMode mode2 = base.gameObject.AddComponent<FollowCameraMode>();
                    mode2.FollowSpeed = ConfigCamera.CAMERA_FOLLOW_SPEED;
                    mode2.LookAtEnabled = false;
                    mode2.ViewingPointCheckEnabled = false;
                    mode2.AutomaticUpdate = false;
                    this.m_activeCameraMode = mode2;
                    break;
                }
                case ConfigCamera.CameraMode.ZoomInOut:
                {
                    ZoomInOutCameraMode mode3 = base.gameObject.AddComponent<ZoomInOutCameraMode>();
                    mode3.AutomaticUpdate = false;
                    this.m_activeCameraMode = mode3;
                    break;
                }
                case ConfigCamera.CameraMode.TargetEnemy:
                {
                    TargetEnemyCameraMode mode4 = base.gameObject.AddComponent<TargetEnemyCameraMode>();
                    mode4.AutomaticUpdate = false;
                    this.m_activeCameraMode = mode4;
                    break;
                }
                case ConfigCamera.CameraMode.Behind:
                {
                    BehindCameraMode mode5 = base.gameObject.AddComponent<BehindCameraMode>();
                    mode5.AutomaticUpdate = false;
                    this.m_activeCameraMode = mode5;
                    break;
                }
                case ConfigCamera.CameraMode.Predictive:
                {
                    PredictiveCameraMode mode6 = base.gameObject.AddComponent<PredictiveCameraMode>();
                    mode6.AutomaticUpdate = false;
                    this.m_activeCameraMode = mode6;
                    break;
                }
                case ConfigCamera.CameraMode.Ultimate1:
                {
                    Ultimate1CameraMode mode7 = base.gameObject.AddComponent<Ultimate1CameraMode>();
                    mode7.AutomaticUpdate = false;
                    this.m_activeCameraMode = mode7;
                    break;
                }
            }
            this.m_activeCameraMode.initialize(this);
            this.m_cameraShake = base.gameObject.AddComponent<CameraShake>();
            this.CustomShaderParameters = base.gameObject.AddComponent<PlayerView.CustomShaderParameters>();
            this.m_initialOffset = this.Transform.position;
            this.m_initialRotation = this.Transform.rotation;
            this.m_defaultOffset = this.Transform.position;
            this.Offset = this.m_defaultOffset;
            this.m_cameraCullingMask = this.Camera.cullingMask;
            this.m_parallaxCameraCullingMask = this.ParallaxCamera.Camera.cullingMask;
            this.Camera.enabled = false;
            this.FovAnimator.setFov(ConfigCamera.RETREATED_FOV);
        }

        [DebuggerHidden]
        public IEnumerator dimLightsRoutine(bool dim, float duration)
        {
            <dimLightsRoutine>c__IteratorE6 re = new <dimLightsRoutine>c__IteratorE6();
            re.duration = duration;
            re.dim = dim;
            re.<$>duration = duration;
            re.<$>dim = dim;
            re.<>f__this = this;
            return re;
        }

        public void endCameraShake()
        {
            this.m_cameraShake.endShake();
        }

        public ICameraMode getActiveCameraMode()
        {
            return this.m_activeCameraMode;
        }

        protected void LateUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (activeDungeon != null)
            {
                CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
                if (!activeCharacter.IsDead)
                {
                    this.CustomShaderParameters.HeroLightWorldPos = activeCharacter.PhysicsBody.Transform.position + ((Vector3) (Vector3.up * 3f));
                }
                if (ConfigDevice.DeviceQuality() >= DeviceQualityType.High)
                {
                    OccludedObject obj5;
                    Vector3 direction = PlayerView.Binder.RoomView.getCharacterViewForCharacter(activeCharacter).Transform.position - this.Transform.position;
                    RaycastHit[] hitArray = Physics.RaycastAll(this.Transform.position, direction, direction.magnitude, Layers.CameraOcclusionLayerMask);
                    this.m_occludedObjectsNew.Clear();
                    for (int i = 0; i < hitArray.Length; i++)
                    {
                        Collider collider = hitArray[i].collider;
                        if (collider.tag == Tags.CAMERA_OCCLUDER)
                        {
                            CameraOccluder componentInParent = collider.GetComponentInParent<CameraOccluder>();
                            if ((componentInParent != null) && componentInParent.Initialized)
                            {
                                obj5 = new OccludedObject();
                                obj5.Collider = collider;
                                obj5.CameraOccluder = componentInParent;
                                OccludedObject item = obj5;
                                this.m_occludedObjectsNew.Add(item);
                            }
                        }
                    }
                    for (int j = this.m_occludedObjects.Count - 1; j >= 0; j--)
                    {
                        OccludedObject obj3 = this.m_occludedObjects[j];
                        bool flag = true;
                        for (int m = 0; m < this.m_occludedObjectsNew.Count; m++)
                        {
                            obj5 = this.m_occludedObjectsNew[m];
                            if (obj5.Collider == obj3.Collider)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            if (obj3.Collider != null)
                            {
                                obj3.CameraOccluder.animateToOpaque(0.25f, Easing.Function.SMOOTHSTEP, 0f);
                            }
                            this.m_occludedObjects.Remove(obj3);
                        }
                    }
                    for (int k = 0; k < this.m_occludedObjectsNew.Count; k++)
                    {
                        OccludedObject obj4 = this.m_occludedObjectsNew[k];
                        bool flag2 = false;
                        for (int n = 0; n < this.m_occludedObjects.Count; n++)
                        {
                            OccludedObject obj6 = this.m_occludedObjects[n];
                            if (obj6.Collider == obj4.Collider)
                            {
                                flag2 = true;
                                break;
                            }
                        }
                        if (!flag2)
                        {
                            obj4.CameraOccluder.animateToTransparent(0.25f, Easing.Function.SMOOTHSTEP, 0f);
                            this.m_occludedObjects.Add(obj4);
                        }
                    }
                    this.m_occludedObjectsNew.Clear();
                }
                if ((activeDungeon.CurrentGameplayState != GameplayState.END_CEREMONY) && !this.TransformAnimation.HasTasks)
                {
                    this.m_activeCameraMode.update(Time.deltaTime);
                    Vector3 position = this.Transform.position;
                    this.limitToBoundaries(ref position);
                    this.Transform.position = position;
                }
            }
        }

        private void limitToBoundaries(ref Vector3 pos)
        {
            pos.x = Mathf.Clamp(pos.x, this.BoundariesMin.x, this.BoundariesMax.x);
            pos.y = Mathf.Clamp(pos.y, this.BoundariesMin.y, this.BoundariesMax.y);
            pos.z = Mathf.Clamp(pos.z, this.BoundariesMin.z, this.BoundariesMax.z);
        }

        [DebuggerHidden]
        private IEnumerator moodTransitionRoutine(DungeonMood targetMood, float duration)
        {
            <moodTransitionRoutine>c__IteratorE7 re = new <moodTransitionRoutine>c__IteratorE7();
            re.duration = duration;
            re.targetMood = targetMood;
            re.<$>duration = duration;
            re.<$>targetMood = targetMood;
            re.<>f__this = this;
            return re;
        }

        private void onCharacterBlinked(CharacterInstance c)
        {
            if (c.IsPrimaryPlayerCharacter)
            {
                UnityUtils.StopCoroutine(this, ref this.m_teleportFollowRoutine);
                this.m_teleportFollowRoutine = UnityUtils.StartCoroutine(this, this.teleportFollowRoutine(c));
            }
        }

        private void onCharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            if (((sourceCharacter != null) && sourceCharacter.IsPlayerCharacter) && critted)
            {
                this.shakeCamera(ConfigCamera.CRIT_SHAKE_INTENSITY, ConfigCamera.CRIT_SHAKE_DECAY);
            }
        }

        private void onCharacterPreBlink(CharacterInstance c)
        {
            if (c.IsPrimaryPlayerCharacter)
            {
                this.setActiveCameraModeTarget(null);
            }
        }

        private void onCharacterSkillBuildupCompleted(CharacterInstance c, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (c.IsPrimaryPlayerCharacter && (skillType == SkillType.Slam))
            {
                this.shakeCamera(ConfigSkills.Slam.CameraShakeIntensity, ConfigSkills.Slam.CameraShakeDecay);
            }
        }

        private void onCharacterSkillExecuted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (skillType == SkillType.Implosion)
            {
                this.shakeCamera(ConfigSkills.Implosion.CameraShakeIntensity, ConfigSkills.Implosion.CameraShakeDecay);
            }
        }

        private void onCharacterSkillExecutionMidpoint(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if ((character.IsPrimaryPlayerCharacter && (skillType == SkillType.Leap)) && (executionStats.EnemiesAround > 0))
            {
                this.shakeCamera(ConfigCamera.CRIT_SHAKE_INTENSITY, ConfigCamera.CRIT_SHAKE_DECAY);
            }
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStateChanged -= new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            eventBus.OnCharacterDealtDamage -= new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnCharacterSkillBuildupCompleted -= new GameLogic.Events.CharacterSkillBuildupCompleted(this.onCharacterSkillBuildupCompleted);
            eventBus.OnCharacterSkillExecutionMidpoint -= new GameLogic.Events.CharacterSkillExecutionMidpoint(this.onCharacterSkillExecutionMidpoint);
            eventBus.OnCharacterSkillExecuted -= new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            eventBus.OnProjectileCollided -= new GameLogic.Events.ProjectileCollided(this.onProjectileCollided);
            eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterPreBlink -= new GameLogic.Events.CharacterPreBlink(this.onCharacterPreBlink);
            eventBus.OnCharacterBlinked -= new GameLogic.Events.CharacterBlinked(this.onCharacterBlinked);
            eventBus.OnTimescaleChangeStarted -= new GameLogic.Events.TimescaleChangeStarted(this.onTimescaleChangeStarted);
            PlayerView.Binder.EventBus.OnMenuChangeStarted -= new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
            PlayerView.Binder.EventBus.OnMenuChanged -= new PlayerView.Events.MenuChanged(this.onMenuChanged);
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStateChanged += new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            eventBus.OnCharacterDealtDamage += new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnCharacterSkillBuildupCompleted += new GameLogic.Events.CharacterSkillBuildupCompleted(this.onCharacterSkillBuildupCompleted);
            eventBus.OnCharacterSkillExecutionMidpoint += new GameLogic.Events.CharacterSkillExecutionMidpoint(this.onCharacterSkillExecutionMidpoint);
            eventBus.OnCharacterSkillExecuted += new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            eventBus.OnProjectileCollided += new GameLogic.Events.ProjectileCollided(this.onProjectileCollided);
            eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnCharacterPreBlink += new GameLogic.Events.CharacterPreBlink(this.onCharacterPreBlink);
            eventBus.OnCharacterBlinked += new GameLogic.Events.CharacterBlinked(this.onCharacterBlinked);
            eventBus.OnTimescaleChangeStarted += new GameLogic.Events.TimescaleChangeStarted(this.onTimescaleChangeStarted);
            PlayerView.Binder.EventBus.OnMenuChangeStarted += new PlayerView.Events.MenuChangeStarted(this.onMenuChangeStarted);
            PlayerView.Binder.EventBus.OnMenuChanged += new PlayerView.Events.MenuChanged(this.onMenuChanged);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            if (!activeDungeon.SeamlessTransition)
            {
                this.Camera.enabled = false;
            }
            for (int i = 0; i < this.m_occludedObjects.Count; i++)
            {
                OccludedObject obj2 = this.m_occludedObjects[i];
                obj2.CameraOccluder.setTransparent(false);
            }
            this.m_occludedObjects.Clear();
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.Camera.enabled = true;
            if (!activeDungeon.SeamlessTransition)
            {
                this.FovAnimator.setFov(ConfigCamera.RETREATED_FOV);
                this.transitionToMood(activeDungeon.Mood, 0f);
            }
            bool layersVisible = ConfigDungeons.ParallaxCloudsEnabledForTheme(activeDungeon.Dungeon.Theme);
            this.ParallaxBackground.initialize(this.ParallaxCamera, layersVisible);
            this.ParallaxBackground.setMaterialColor(activeDungeon.Mood.BackgroundColor);
        }

        private void onGameplayStateChanged(GameplayState previousState, GameplayState currentState)
        {
            if (currentState == GameplayState.START_CEREMONY_STEP1)
            {
                ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                CharacterInstance primaryPlayerCharacter = GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter;
                if (!activeDungeon.SeamlessTransition)
                {
                    this.refreshDefaultOrientation();
                }
                this.m_activeCameraMode.setTarget(primaryPlayerCharacter);
            }
            else if (currentState == GameplayState.END_CEREMONY)
            {
                CharacterInstance targetCharacter = GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter;
                UnityUtils.StopCoroutine(this, ref this.m_teleportFollowRoutine);
                this.m_activeCameraMode.setTarget(targetCharacter);
            }
        }

        private void onMenuChanged(Menu sourceMenu, Menu targetMenu)
        {
            if (((targetMenu != null) && ((targetMenu.MenuType == MenuType.ThinPopupMenu) || (targetMenu.MenuType == MenuType.StackedPopupMenu))) && ((targetMenu.activeContentObject() == null) || !targetMenu.activeContentObject().ForceShowGameplayLayerBehind))
            {
                this.Camera.cullingMask = 0;
                this.ParallaxCamera.Camera.cullingMask = 0;
            }
        }

        private void onMenuChangeStarted(MenuType sourceMenuType, MenuType targetMenuType)
        {
            this.refreshCameraOffset(targetMenuType);
            if ((targetMenuType != MenuType.ThinPopupMenu) && (targetMenuType != MenuType.StackedPopupMenu))
            {
                this.Camera.cullingMask = this.m_cameraCullingMask;
                this.ParallaxCamera.Camera.cullingMask = this.m_parallaxCameraCullingMask;
            }
        }

        private void onProjectileCollided(Projectile projectile, Collider collider)
        {
            if (projectile.Properties.Type == ProjectileType.Cluster)
            {
                this.shakeCamera(ConfigSkills.Cluster.CameraShakeIntensity, ConfigSkills.Cluster.CameraShakeDecay);
            }
        }

        private void onTimescaleChangeStarted(float targetTimescale)
        {
            if (targetTimescale == 0.001f)
            {
                this.endCameraShake();
            }
        }

        public void refreshCameraOffset(MenuType refMenuType)
        {
            CharacterInstance activeCharacter = GameLogic.Binder.GameState.Player.ActiveCharacter;
            if (refMenuType == MenuType.SlidingInventoryMenu)
            {
                Vector3 normalized = Vector3Extensions.ToXzVector3(activeCharacter.PhysicsBody.Transform.position - this.Transform.position).normalized;
                this.Offset = this.m_defaultOffset - ((Vector3) (normalized * 5f));
            }
            else if ((refMenuType == MenuType.SlidingTaskPanel) && !PlayerView.Binder.SlidingTaskPanelController.PanningActive)
            {
                this.Offset = this.m_defaultOffset - this.Transform.TransformVector(2f, 0f, 0f);
            }
            else if ((refMenuType == MenuType.SlidingAdventurePanel) && !PlayerView.Binder.SlidingAdventurePanelController.PanningActive)
            {
                this.Offset = this.m_defaultOffset - this.Transform.TransformVector(-4f, 0f, 0f);
            }
            else if (refMenuType == MenuType.NONE)
            {
                this.Offset = this.m_defaultOffset;
            }
        }

        public void refreshDefaultOrientation()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            CharacterInstance primaryPlayerCharacter = GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter;
            this.BoundariesMin = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            this.BoundariesMax = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            this.Transform.position = primaryPlayerCharacter.PhysicsBody.transform.position + this.m_initialOffset;
            this.Transform.rotation = this.m_initialRotation;
            this.Orientation = 0f;
            if (!activeDungeon.ActiveRoom.RoomLayout.LockCameraRotation)
            {
                this.Orientation = LangUtil.GetRandomValueFromList<float>(ConfigCamera.RANDOMIZED_STARTING_ROTATION_X);
                this.Transform.RotateAround(primaryPlayerCharacter.PhysicsBody.transform.position, Vector3.up, this.Orientation);
            }
            this.m_defaultOffset = this.Transform.position - primaryPlayerCharacter.PhysicsBody.Transform.position;
            this.Offset = this.m_defaultOffset;
            this.refreshCameraOffset(PlayerView.Binder.MenuSystem.topmostActiveMenuType());
        }

        public void setActiveCameraModeTarget(CharacterInstance targetCharacter)
        {
            if (this.m_activeCameraMode != null)
            {
                this.m_activeCameraMode.setTarget(targetCharacter);
            }
        }

        public void shakeCamera(float intensity, float decay)
        {
            if (PlayerView.Binder.MenuSystem.topmostActiveMenuType() == MenuType.NONE)
            {
                this.m_cameraShake.shake(intensity, decay);
            }
        }

        [DebuggerHidden]
        public IEnumerator teleportFollowRoutine(CharacterInstance targetCharacter)
        {
            <teleportFollowRoutine>c__IteratorE5 re = new <teleportFollowRoutine>c__IteratorE5();
            re.targetCharacter = targetCharacter;
            re.<$>targetCharacter = targetCharacter;
            re.<>f__this = this;
            return re;
        }

        public Coroutine transitionToMood(DungeonMood targetMood, float duration)
        {
            UnityUtils.StopCoroutine(this, ref this.m_moodTransitionRoutine);
            this.m_moodTransitionRoutine = UnityUtils.StartCoroutine(this, this.moodTransitionRoutine(targetMood, duration));
            return this.m_moodTransitionRoutine;
        }

        public Vector3 BoundariesMax
        {
            [CompilerGenerated]
            get
            {
                return this.<BoundariesMax>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<BoundariesMax>k__BackingField = value;
            }
        }

        public Vector3 BoundariesMin
        {
            [CompilerGenerated]
            get
            {
                return this.<BoundariesMin>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<BoundariesMin>k__BackingField = value;
            }
        }

        public UnityEngine.Camera Camera
        {
            [CompilerGenerated]
            get
            {
                return this.<Camera>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Camera>k__BackingField = value;
            }
        }

        public PlayerView.CustomShaderParameters CustomShaderParameters
        {
            [CompilerGenerated]
            get
            {
                return this.<CustomShaderParameters>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CustomShaderParameters>k__BackingField = value;
            }
        }

        public Vector3 DefaultOffset
        {
            get
            {
                return this.m_defaultOffset;
            }
        }

        public CameraFieldOfViewAnimator FovAnimator
        {
            [CompilerGenerated]
            get
            {
                return this.<FovAnimator>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<FovAnimator>k__BackingField = value;
            }
        }

        public Vector3 Offset
        {
            [CompilerGenerated]
            get
            {
                return this.<Offset>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Offset>k__BackingField = value;
            }
        }

        public float Orientation
        {
            [CompilerGenerated]
            get
            {
                return this.<Orientation>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Orientation>k__BackingField = value;
            }
        }

        public PlayerView.ParallaxBackground ParallaxBackground
        {
            [CompilerGenerated]
            get
            {
                return this.<ParallaxBackground>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ParallaxBackground>k__BackingField = value;
            }
        }

        public PlayerView.ParallaxCamera ParallaxCamera
        {
            [CompilerGenerated]
            get
            {
                return this.<ParallaxCamera>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ParallaxCamera>k__BackingField = value;
            }
        }

        public UnityEngine.Transform Transform
        {
            [CompilerGenerated]
            get
            {
                return this.<Transform>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Transform>k__BackingField = value;
            }
        }

        public TransformAnimation TransformAnimation
        {
            [CompilerGenerated]
            get
            {
                return this.<TransformAnimation>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TransformAnimation>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <dimLightsRoutine>c__IteratorE6 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>dim;
            internal float <$>duration;
            internal RoomCamera <>f__this;
            internal ActiveDungeon <ad>__0;
            internal Color <ambientLightTarget>__2;
            internal Color <bgColorTarget>__4;
            internal float <easedV>__6;
            internal Color <fogColorTarget>__3;
            internal ManualTimer <timer>__1;
            internal float <v>__5;
            internal bool dim;
            internal float duration;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<timer>__1 = new ManualTimer(this.duration);
                        this.<ambientLightTarget>__2 = (Color) (this.<ad>__0.Mood.AmbientLightColor * (!this.dim ? 1f : ConfigDungeons.BOSS_TARGET_LIGHTING_INTENSITY));
                        this.<fogColorTarget>__3 = (Color) (this.<ad>__0.Mood.FogColor * (!this.dim ? 1f : ConfigDungeons.BOSS_TARGET_LIGHTING_INTENSITY));
                        this.<bgColorTarget>__4 = (Color) (this.<ad>__0.Mood.BackgroundColor * (!this.dim ? 1f : ConfigDungeons.BOSS_TARGET_LIGHTING_INTENSITY));
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0210;
                }
                if (!this.<timer>__1.Idle)
                {
                    this.<v>__5 = this.<timer>__1.normalizedProgress();
                    this.<easedV>__6 = Easing.Apply(this.<v>__5, Easing.Function.SMOOTHSTEP);
                    RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, this.<fogColorTarget>__3, this.<easedV>__6);
                    RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, this.<ambientLightTarget>__2, this.<easedV>__6);
                    this.<>f__this.ParallaxCamera.Camera.backgroundColor = Color.Lerp(this.<>f__this.ParallaxCamera.Camera.backgroundColor, this.<bgColorTarget>__4, this.<easedV>__6);
                    this.<>f__this.ParallaxBackground.setMaterialColor(this.<>f__this.ParallaxCamera.Camera.backgroundColor);
                    this.<>f__this.CustomShaderParameters.BackgroundColor = Color.Lerp(this.<>f__this.CustomShaderParameters.BackgroundColor, this.<bgColorTarget>__4, this.<easedV>__6);
                    this.<timer>__1.tick(Time.deltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                goto Label_0210;
                this.$PC = -1;
            Label_0210:
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
        private sealed class <moodTransitionRoutine>c__IteratorE7 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>duration;
            internal DungeonMood <$>targetMood;
            internal RoomCamera <>f__this;
            internal ActiveDungeon <ad>__0;
            internal float <easedV>__22;
            internal DropLight <heroDropLight>__2;
            internal int <i>__18;
            internal int <i>__23;
            internal CharacterInstance <pc>__1;
            internal Color <sourceAmbientLightColor>__3;
            internal Color <sourceCameraBackgroundColor>__4;
            internal Dictionary<DungeonDeco, Color> <sourceDungeonLightColors>__16;
            internal Dictionary<DungeonDeco, float> <sourceDungeonLightIntensities>__17;
            internal Color <sourceFogColor>__12;
            internal Color <sourceHeroDropLightColor>__10;
            internal float <sourceHeroDropLightIntensity>__11;
            internal Color <sourcePropColor>__15;
            internal Color <sourceShaderBackgroundColor>__5;
            internal float <sourceShaderFloorAndDecoMoodContribution>__8;
            internal float <sourceShaderFogEndTerm>__14;
            internal float <sourceShaderFogStartTerm>__13;
            internal Color <sourceShaderHeroLightColor>__6;
            internal float <sourceShaderHeroLightIntensity>__7;
            internal float <sourceShaderHeroLightRange>__9;
            internal MarkerSpawnPointDeco <sp>__19;
            internal MarkerSpawnPointDeco <sp>__24;
            internal ManualTimer <timer>__20;
            internal float <v>__21;
            internal float duration;
            internal DungeonMood targetMood;

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
                        this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                        this.<pc>__1 = GameLogic.Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter;
                        this.<heroDropLight>__2 = PlayerView.Binder.RoomView.getCharacterViewForCharacter(this.<pc>__1).DropLight;
                        this.<sourceAmbientLightColor>__3 = RenderSettings.ambientLight;
                        this.<sourceCameraBackgroundColor>__4 = this.<>f__this.Camera.backgroundColor;
                        this.<sourceShaderBackgroundColor>__5 = this.<>f__this.CustomShaderParameters.BackgroundColor;
                        this.<sourceShaderHeroLightColor>__6 = this.<>f__this.CustomShaderParameters.HeroLightColor;
                        this.<sourceShaderHeroLightIntensity>__7 = this.<>f__this.CustomShaderParameters.HeroLightIntensity;
                        this.<sourceShaderFloorAndDecoMoodContribution>__8 = this.<>f__this.CustomShaderParameters.FloorAndDecoMoodContribution;
                        this.<sourceShaderHeroLightRange>__9 = this.<>f__this.CustomShaderParameters.HeroLightRange;
                        this.<sourceHeroDropLightColor>__10 = this.<heroDropLight>__2.Color;
                        this.<sourceHeroDropLightIntensity>__11 = this.<heroDropLight>__2.LightIntensity;
                        this.<sourceFogColor>__12 = RenderSettings.fogColor;
                        this.<sourceShaderFogStartTerm>__13 = this.<>f__this.CustomShaderParameters.HorizontalFogStartTerm;
                        this.<sourceShaderFogEndTerm>__14 = this.<>f__this.CustomShaderParameters.HorizontalFogEndTerm;
                        this.<sourcePropColor>__15 = this.<>f__this.CustomShaderParameters.PropColor;
                        this.<sourceDungeonLightColors>__16 = new Dictionary<DungeonDeco, Color>(this.<ad>__0.ActiveRoom.DecoSpawnpoints.Count);
                        this.<sourceDungeonLightIntensities>__17 = new Dictionary<DungeonDeco, float>(this.<ad>__0.ActiveRoom.DecoSpawnpoints.Count);
                        this.<i>__18 = 0;
                        while (this.<i>__18 < this.<ad>__0.ActiveRoom.DecoSpawnpoints.Count)
                        {
                            this.<sp>__19 = this.<ad>__0.ActiveRoom.DecoSpawnpoints[this.<i>__18];
                            if (this.<sp>__19.ActiveDeco != null)
                            {
                                this.<sourceDungeonLightColors>__16.Add(this.<sp>__19.ActiveDeco, this.<sp>__19.ActiveDeco.getActiveColor());
                                this.<sourceDungeonLightIntensities>__17.Add(this.<sp>__19.ActiveDeco, this.<sp>__19.ActiveDeco.getActiveIntensity());
                            }
                            this.<i>__18++;
                        }
                        this.<timer>__20 = new ManualTimer(this.duration);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0622;
                }
                while (!this.<timer>__20.Idle)
                {
                    this.<v>__21 = this.<timer>__20.normalizedProgress();
                    this.<easedV>__22 = Easing.Apply(this.<v>__21, Easing.Function.SMOOTHSTEP);
                    RenderSettings.ambientLight = Color.Lerp(this.<sourceAmbientLightColor>__3, this.targetMood.AmbientLightColor, this.<easedV>__22);
                    this.<>f__this.ParallaxCamera.Camera.backgroundColor = Color.Lerp(this.<sourceCameraBackgroundColor>__4, this.targetMood.BackgroundColor, this.<easedV>__22);
                    this.<>f__this.ParallaxBackground.setMaterialColor(this.<>f__this.ParallaxCamera.Camera.backgroundColor);
                    this.<>f__this.CustomShaderParameters.BackgroundColor = Color.Lerp(this.<sourceShaderBackgroundColor>__5, this.targetMood.BackgroundColor, this.<easedV>__22);
                    this.<>f__this.CustomShaderParameters.HeroLightColor = Color.Lerp(this.<sourceShaderHeroLightColor>__6, this.targetMood.HeroLightColor, this.<easedV>__22);
                    this.<>f__this.CustomShaderParameters.HeroLightIntensity = Mathf.Lerp(this.<sourceShaderHeroLightIntensity>__7, this.targetMood.HeroLightIntensity, this.<easedV>__22);
                    this.<>f__this.CustomShaderParameters.HeroLightRange = Mathf.Lerp(this.<sourceShaderHeroLightRange>__9, this.targetMood.HeroLightRange, this.<easedV>__22);
                    RenderSettings.fogColor = Color.Lerp(this.<sourceFogColor>__12, this.targetMood.FogColor, this.<easedV>__22);
                    this.<>f__this.CustomShaderParameters.HorizontalFogStartTerm = Mathf.Lerp(this.<sourceShaderFogStartTerm>__13, this.targetMood.HorizontalFogStartTerm, this.<easedV>__22);
                    this.<>f__this.CustomShaderParameters.HorizontalFogEndTerm = Mathf.Lerp(this.<sourceShaderFogEndTerm>__14, this.targetMood.HorizontalFogEndTerm, this.<easedV>__22);
                    this.<>f__this.CustomShaderParameters.PropColor = Color.Lerp(this.<sourcePropColor>__15, this.targetMood.PropColor, this.<easedV>__22);
                    this.<heroDropLight>__2.Color = Color.Lerp(this.<sourceHeroDropLightColor>__10, this.targetMood.HeroLightColor, this.<easedV>__22);
                    this.<heroDropLight>__2.LightIntensity = Mathf.Lerp(this.<sourceHeroDropLightIntensity>__11, this.targetMood.HeroLightIntensity * 0.2f, this.<easedV>__22);
                    if (this.<ad>__0.ActiveRoom != null)
                    {
                        this.<i>__23 = 0;
                        while (this.<i>__23 < this.<ad>__0.ActiveRoom.DecoSpawnpoints.Count)
                        {
                            this.<sp>__24 = this.<ad>__0.ActiveRoom.DecoSpawnpoints[this.<i>__23];
                            if (this.<sp>__24.ActiveDeco != null)
                            {
                                this.<sp>__24.ActiveDeco.refreshLights(Color.Lerp(this.<sourceDungeonLightColors>__16[this.<sp>__24.ActiveDeco], this.targetMood.DungeonLightColors[0], this.<easedV>__22), Mathf.Lerp(this.<sourceDungeonLightIntensities>__17[this.<sp>__24.ActiveDeco], this.targetMood.DungeonLightIntensity, this.<easedV>__22));
                            }
                            this.<i>__23++;
                        }
                    }
                    this.<>f__this.CustomShaderParameters.FloorAndDecoMoodContribution = Mathf.Lerp(this.<sourceShaderFloorAndDecoMoodContribution>__8, this.targetMood.FloorAndDecoMoodContribution, this.<easedV>__22);
                    this.<timer>__20.tick(Time.deltaTime / Time.timeScale);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.m_moodTransitionRoutine = null;
                goto Label_0622;
                this.$PC = -1;
            Label_0622:
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
        private sealed class <teleportFollowRoutine>c__IteratorE5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>targetCharacter;
            internal RoomCamera <>f__this;
            internal CharacterView <characterView>__0;
            internal Vector3 <targetWorldPt>__2;
            internal TransformAnimationTask <tt>__1;
            internal CharacterInstance targetCharacter;

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
                        this.<characterView>__0 = PlayerView.Binder.RoomView.getCharacterViewForCharacter(this.targetCharacter);
                        this.<>f__this.setActiveCameraModeTarget(null);
                        this.<tt>__1 = new TransformAnimationTask(this.<>f__this.Transform, 0.4f, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                        this.<targetWorldPt>__2 = this.targetCharacter.PhysicsBody.Transform.position + this.<>f__this.Offset;
                        this.<tt>__1.translate(this.<targetWorldPt>__2, false, Easing.Function.IN_CUBIC);
                        this.<>f__this.TransformAnimation.stopAll();
                        this.<>f__this.TransformAnimation.addTask(this.<tt>__1);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_012B;
                }
                if (this.<>f__this.TransformAnimation.HasTasks)
                {
                    this.<characterView>__0.setSpurtTrailActive(false);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.setActiveCameraModeTarget(this.targetCharacter);
                this.<>f__this.m_teleportFollowRoutine = null;
                goto Label_012B;
                this.$PC = -1;
            Label_012B:
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
        private struct OccludedObject
        {
            public UnityEngine.Collider Collider;
            public PlayerView.CameraOccluder CameraOccluder;
        }
    }
}

