namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Rendering;
    using Xft;

    [SelectionBase]
    public class CharacterView : MonoBehaviour, IPoolable, IFlashable
    {
        [CompilerGenerated]
        private AbstractCharacterAnimator <Animator>k__BackingField;
        [CompilerGenerated]
        private List<UnityEngine.Transform> <ArmorAccessoryRoots>k__BackingField;
        [CompilerGenerated]
        private AbstractCharacterAudio <Audio>k__BackingField;
        [CompilerGenerated]
        private CharacterInstance <Character>k__BackingField;
        [CompilerGenerated]
        private GameLogic.CharacterPrefab <CharacterPrefab>k__BackingField;
        [CompilerGenerated]
        private XWeaponTrail <CharacterTrail>k__BackingField;
        [CompilerGenerated]
        private Cloth <CloakCloth>k__BackingField;
        [CompilerGenerated]
        private PlayerView.DropLight <DropLight>k__BackingField;
        [CompilerGenerated]
        private Material <FaceOriginalMaterial>k__BackingField;
        [CompilerGenerated]
        private Renderer <FaceRenderer>k__BackingField;
        [CompilerGenerated]
        private bool <IsMenuView>k__BackingField;
        [CompilerGenerated]
        private Dictionary<Renderer, Mesh> <Meshes>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <ProjectileSpawnpoint>k__BackingField;
        [CompilerGenerated]
        private List<Rigidbody> <RagdollRigidbodies>k__BackingField;
        [CompilerGenerated]
        private Renderer[] <Renderers>k__BackingField;
        [CompilerGenerated]
        private Renderer <ShadowRenderer>k__BackingField;
        [CompilerGenerated]
        private XWeaponTrail <SpurtTrail>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        [CompilerGenerated]
        private TransformAnimation <TransformAnimation>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <WeaponAccessoryRoot>k__BackingField;
        private List<Accessory> m_activeArmorAccessories;
        private string m_activeArmorItemId;
        private Accessory m_activeWeaponAccessory;
        private List<BodyMaterial> m_bodyMaterials;
        private Coroutine m_damageFlashRoutine;
        private Coroutine m_eyeBlinkRoutine;
        private Color m_flashMaterialColor;
        private List<FlashMaterial> m_flashMaterials;
        private bool m_flashMaterialsEnabled;
        private Coroutine m_leapRoutine;
        private Dictionary<Renderer, Material> m_origMaterials;
        private Vector3 m_origShadowLocalPos;
        private Color m_spurtTrailOrigColor;
        private ManualTimer m_timer = new ManualTimer();

        private void applyFlashColor()
        {
            bool enabled = false;
            if (this.Character.Stunned)
            {
                enabled = true;
                this.setFlashMaterialColor(ConfigGameplay.STUN_MATERIAL_COLOR);
            }
            else if ((this.Character.Prefab == GameLogic.CharacterPrefab.KnightClone) && (this.Character.getPerkInstanceCount(PerkType.SkillUpgradeClone4) > 0))
            {
                enabled = true;
                this.setFlashMaterialColor(ConfigGameplay.CLONE_COOLDOWN_MATERIAL_COLOR);
            }
            else if ((this.Character.Prefab == GameLogic.CharacterPrefab.KnightClone) && (this.Character.getPerkInstanceCount(PerkType.SkillUpgradeClone3) > 0))
            {
                enabled = true;
                this.setFlashMaterialColor(ConfigGameplay.CLONE_DECOY_MATERIAL_COLOR);
            }
            else if ((this.Character.Prefab == GameLogic.CharacterPrefab.KnightClone) && (this.Character.getPerkInstanceCount(PerkType.SkillUpgradeClone1) > 0))
            {
                enabled = true;
                this.setFlashMaterialColor(ConfigGameplay.CLONE_HEAL_MATERIAL_COLOR);
            }
            else if (PlayerView.Binder.EffectSystem.hasFollowParticleSystem(this.Character, ConfigPerks.GlobalPoisonEffect.EffectType))
            {
                enabled = true;
                this.setFlashMaterialColor(ConfigGameplay.POISON_MATERIAL_COLOR);
            }
            else if (PlayerView.Binder.EffectSystem.hasFollowParticleSystem(this.Character, ConfigPerks.GlobalFrostEffect.EffectType))
            {
                enabled = true;
                this.setFlashMaterialColor(ConfigGameplay.SLOW_MATERIAL_COLOR);
            }
            else if (UnityUtils.CoroutineRunning(ref this.m_damageFlashRoutine))
            {
                enabled = true;
            }
            this.setFlashMaterialsEnabled(enabled);
            if (this.m_flashMaterialsEnabled)
            {
                for (int i = 0; i < this.m_flashMaterials.Count; i++)
                {
                    for (int j = 0; j < this.m_flashMaterials[i].FlashMaterials.Length; j++)
                    {
                        this.m_flashMaterials[i].FlashMaterials[j].color = this.m_flashMaterialColor;
                    }
                }
            }
        }

        protected void Awake()
        {
            this.Transform = base.transform;
            this.TransformAnimation = base.gameObject.AddComponent<TransformAnimation>();
            this.Renderers = base.GetComponentsInChildren<Renderer>(false);
            this.ProjectileSpawnpoint = TransformExtensions.FindChildRecursively(this.Transform, "ProjectileSpawnpoint");
            UnityEngine.Transform transform = TransformExtensions.FindChildRecursively(this.Transform, "Shadow");
            if (transform != null)
            {
                this.ShadowRenderer = transform.GetComponent<Renderer>();
                if (ConfigDevice.DeviceQuality() >= DeviceQualityType.Med)
                {
                    this.m_origShadowLocalPos = this.ShadowRenderer.transform.localPosition;
                }
                else
                {
                    this.ShadowRenderer.gameObject.SetActive(false);
                    this.ShadowRenderer = null;
                }
            }
            UnityEngine.Transform transform2 = TransformExtensions.FindChildRecursively(this.Transform, "DropLight");
            if (transform2 != null)
            {
                this.DropLight = transform2.GetComponent<PlayerView.DropLight>();
            }
            UnityEngine.Transform transform3 = this.Transform.FindChild("Trail");
            if (transform3 != null)
            {
                this.CharacterTrail = transform3.GetComponent<XWeaponTrail>();
                if (ConfigDevice.DeviceQuality() >= DeviceQualityType.Med)
                {
                    this.CharacterTrail.PersistentObjectRoot = App.Binder.PersistentObjectRootTm;
                    this.CharacterTrail.Init();
                    this.CharacterTrail.Deactivate();
                }
                else
                {
                    this.CharacterTrail.gameObject.SetActive(false);
                    this.CharacterTrail = null;
                }
            }
            UnityEngine.Transform transform4 = this.Transform.FindChild("SpurtTrail");
            if (transform4 != null)
            {
                this.SpurtTrail = transform4.GetComponent<XWeaponTrail>();
                if (ConfigDevice.DeviceQuality() >= DeviceQualityType.Med)
                {
                    this.SpurtTrail.PersistentObjectRoot = App.Binder.PersistentObjectRootTm;
                    this.SpurtTrail.Init();
                    this.SpurtTrail.Deactivate();
                    this.m_spurtTrailOrigColor = this.SpurtTrail.MyColor;
                }
                else
                {
                    this.SpurtTrail.gameObject.SetActive(false);
                    this.SpurtTrail = null;
                }
            }
            this.initializeFlashMaterials();
            UnityEngine.Transform self = this.Transform.FindChild("HeroRoot");
            if (self != null)
            {
                this.WeaponAccessoryRoot = self.FindChild("Armature").FindChild("Root").FindChild("Weapon");
                this.ArmorAccessoryRoots = new List<UnityEngine.Transform>();
                UnityEngine.Transform item = TransformExtensions.FindChildRecursively(self, "Helmet");
                this.ArmorAccessoryRoots.Add(item);
                item = TransformExtensions.FindChildRecursively(self, "ShoulderPadLeft");
                if (item != null)
                {
                    this.ArmorAccessoryRoots.Add(item);
                }
                item = TransformExtensions.FindChildRecursively(self, "ShoulderPadRight");
                if (item != null)
                {
                    this.ArmorAccessoryRoots.Add(item);
                }
            }
            if (self != null)
            {
                this.m_bodyMaterials = new List<BodyMaterial>(6);
                SkinnedMeshRenderer component = self.FindChild("Hero").GetComponent<SkinnedMeshRenderer>();
                BodyMaterial material = new BodyMaterial();
                material.ChangedWithItemType = ItemType.Armor;
                material.Renderer = component;
                material.OriginalMat = component.sharedMaterials[0];
                this.m_bodyMaterials.Add(material);
                SkinnedMeshRenderer renderer2 = self.FindChild("Hero").GetComponent<SkinnedMeshRenderer>();
                material = new BodyMaterial();
                material.ChangedWithItemType = ItemType.Armor;
                material.Renderer = renderer2;
                material.RendererMaterialIndex = 1;
                material.OriginalMat = renderer2.sharedMaterials[1];
                this.m_bodyMaterials.Add(material);
                UnityEngine.Transform transform7 = TransformExtensions.FindChildRecursively(self.FindChild("Armature"), "Belt");
                if (transform7 != null)
                {
                    MeshRenderer renderer3 = transform7.GetComponent<MeshRenderer>();
                    material = new BodyMaterial();
                    material.ChangedWithItemType = ItemType.Armor;
                    material.Renderer = renderer3;
                    material.OriginalMat = renderer3.sharedMaterial;
                    this.m_bodyMaterials.Add(material);
                }
                UnityEngine.Transform transform8 = TransformExtensions.FindChildRecursively(self.FindChild("Armature"), "Badge");
                if (transform8 != null)
                {
                    MeshRenderer renderer4 = transform8.GetComponent<MeshRenderer>();
                    material = new BodyMaterial();
                    material.ChangedWithItemType = ItemType.Cloak;
                    material.Renderer = renderer4;
                    material.OriginalMat = renderer4.sharedMaterial;
                    this.m_bodyMaterials.Add(material);
                }
                UnityEngine.Transform transform9 = self.FindChild("Cloak");
                if (transform9 != null)
                {
                    SkinnedMeshRenderer renderer5 = transform9.GetComponent<SkinnedMeshRenderer>();
                    material = new BodyMaterial();
                    material.ChangedWithItemType = ItemType.Cloak;
                    material.Renderer = renderer5;
                    material.OriginalMat = renderer5.sharedMaterials[0];
                    this.m_bodyMaterials.Add(material);
                }
            }
            this.m_origMaterials = new Dictionary<Renderer, Material>();
            this.Meshes = new Dictionary<Renderer, Mesh>(this.Renderers.Length);
            for (int i = 0; i < this.Renderers.Length; i++)
            {
                Renderer key = this.Renderers[i];
                this.m_origMaterials.Add(key, key.sharedMaterial);
                if (key is SkinnedMeshRenderer)
                {
                    this.Meshes.Add(key, ((SkinnedMeshRenderer) key).sharedMesh);
                }
                else
                {
                    MeshFilter filter = key.GetComponent<MeshFilter>();
                    if (filter != null)
                    {
                        this.Meshes.Add(key, filter.sharedMesh);
                    }
                }
            }
            this.CloakCloth = this.Transform.parent.GetComponentInChildren<Cloth>();
            if (this.CloakCloth != null)
            {
                if (ConfigDevice.DeviceQuality() <= DeviceQualityType.Low)
                {
                    UnityEngine.Object.Destroy(this.CloakCloth);
                    this.CloakCloth = null;
                }
                else
                {
                    this.CloakCloth.enabled = false;
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator characterLeapRoutine(float duration, float targetHeight)
        {
            <characterLeapRoutine>c__IteratorE8 re = new <characterLeapRoutine>c__IteratorE8();
            re.duration = duration;
            re.targetHeight = targetHeight;
            re.<$>duration = duration;
            re.<$>targetHeight = targetHeight;
            re.<>f__this = this;
            return re;
        }

        public void cleanUpForReuse()
        {
            this.Character = null;
            this.IsMenuView = false;
            if (this.Animator != null)
            {
                this.Animator.enabled = true;
            }
            this.setKnockedOutFaceActive(false);
            this.setCharacterTrailActive(false);
            this.setSpurtTrailActive(false);
            if (this.ShadowRenderer != null)
            {
                this.ShadowRenderer.enabled = true;
            }
            UnityUtils.StopCoroutine(this, ref this.m_eyeBlinkRoutine);
            UnityUtils.StopCoroutine(this, ref this.m_leapRoutine);
            UnityUtils.StopCoroutine(this, ref this.m_damageFlashRoutine);
            for (int i = 0; i < this.Renderers.Length; i++)
            {
                this.Renderers[i].sharedMaterial = this.m_origMaterials[this.Renderers[i]];
            }
            if (this.m_activeWeaponAccessory != null)
            {
                PlayerView.Binder.AccessoryStorage.releaseAccessory(this.m_activeWeaponAccessory);
                this.m_activeWeaponAccessory = null;
            }
            if (this.m_activeArmorAccessories != null)
            {
                for (int j = 0; j < this.m_activeArmorAccessories.Count; j++)
                {
                    PlayerView.Binder.AccessoryStorage.releaseAccessory(this.m_activeArmorAccessories[j]);
                }
                this.m_activeArmorAccessories.Clear();
            }
            this.m_activeArmorItemId = null;
        }

        [DebuggerHidden]
        private IEnumerator damageFlashRoutine()
        {
            <damageFlashRoutine>c__IteratorE9 re = new <damageFlashRoutine>c__IteratorE9();
            re.<>f__this = this;
            return re;
        }

        [DebuggerHidden]
        private IEnumerator eyeBlinkRoutine()
        {
            <eyeBlinkRoutine>c__IteratorEA rea = new <eyeBlinkRoutine>c__IteratorEA();
            rea.<>f__this = this;
            return rea;
        }

        public void initialize(CharacterInstance attachedToCharacter, bool isMenuView)
        {
            this.Character = attachedToCharacter;
            this.IsMenuView = isMenuView;
            if (this.IsMenuView)
            {
                for (int k = 0; k < this.Renderers.Length; k++)
                {
                    Renderer renderer = this.Renderers[k];
                    Material[] sharedMaterials = renderer.sharedMaterials;
                    for (int m = 0; m < sharedMaterials.Length; m++)
                    {
                        sharedMaterials[m] = PlayerView.Binder.MaterialStorage.getSharedMenuMaterial(sharedMaterials[m]);
                    }
                    renderer.sharedMaterials = sharedMaterials;
                }
            }
            if (this.CloakCloth != null)
            {
                if (this.IsMenuView)
                {
                    this.CloakCloth.useGravity = true;
                    this.CloakCloth.damping = 0.3f;
                    this.CloakCloth.externalAcceleration = new Vector3(1.5f, 0f, 5f);
                    this.CloakCloth.worldAccelerationScale = 0.5f;
                    this.CloakCloth.worldVelocityScale = 0.17f;
                    this.CloakCloth.randomAcceleration = new Vector3(1.3f, 1.3f, 1.3f);
                    this.CloakCloth.enabled = true;
                }
                else
                {
                    this.CloakCloth.useGravity = false;
                    this.CloakCloth.damping = 0f;
                    this.CloakCloth.externalAcceleration = Vector3.zero;
                    this.CloakCloth.worldAccelerationScale = 0.02f;
                    this.CloakCloth.worldVelocityScale = 0.01f;
                    this.CloakCloth.randomAcceleration = Vector3.zero;
                    this.CloakCloth.enabled = true;
                }
            }
            for (int i = 0; i < this.Renderers.Length; i++)
            {
                if (this.Renderers[i] is SkinnedMeshRenderer)
                {
                    SkinnedMeshRenderer renderer2 = (SkinnedMeshRenderer) this.Renderers[i];
                    renderer2.shadowCastingMode = ShadowCastingMode.Off;
                    renderer2.receiveShadows = false;
                    renderer2.updateWhenOffscreen = false;
                    if ((this.Character == null) || this.Character.IsPlayerCharacter)
                    {
                        renderer2.quality = SkinQuality.Bone4;
                    }
                    else
                    {
                        renderer2.quality = SkinQuality.Bone1;
                    }
                }
            }
            for (int j = 0; j < this.Renderers.Length; j++)
            {
                Renderer renderer3 = this.Renderers[j];
                renderer3.shadowCastingMode = ShadowCastingMode.Off;
                renderer3.reflectionProbeUsage = ReflectionProbeUsage.Off;
                renderer3.useLightProbes = false;
                renderer3.receiveShadows = false;
            }
            if (!this.IsMenuView)
            {
                if (this.ShadowRenderer != null)
                {
                    this.ShadowRenderer.transform.localPosition = this.m_origShadowLocalPos;
                }
                if (this.DropLight != null)
                {
                    this.DropLight.setVisible(true);
                }
                goto Label_03BA;
            }
            if (this.ShadowRenderer == null)
            {
                goto Label_0354;
            }
            this.ShadowRenderer.enabled = true;
            this.ShadowRenderer.gameObject.SetActive(true);
            Vector3 zero = Vector3.zero;
            GameLogic.CharacterPrefab characterPrefab = this.CharacterPrefab;
            switch (characterPrefab)
            {
                case GameLogic.CharacterPrefab.PetSquid1:
                case GameLogic.CharacterPrefab.PetSquid2:
                    break;

                case GameLogic.CharacterPrefab.PetShark1:
                case GameLogic.CharacterPrefab.PetShark2:
                    zero = new Vector3(this.m_origShadowLocalPos.x, 0.5f, this.m_origShadowLocalPos.z);
                    goto Label_0342;

                default:
                    if ((characterPrefab != GameLogic.CharacterPrefab.PetDragon1) && (characterPrefab != GameLogic.CharacterPrefab.PetDragon2))
                    {
                        zero = new Vector3(this.m_origShadowLocalPos.x, -0.034f, this.m_origShadowLocalPos.z);
                        goto Label_0342;
                    }
                    break;
            }
            zero = new Vector3(this.m_origShadowLocalPos.x, 1f, this.m_origShadowLocalPos.z);
        Label_0342:
            this.ShadowRenderer.transform.localPosition = zero;
        Label_0354:
            if (this.DropLight != null)
            {
                this.DropLight.setVisible(false);
            }
        Label_03BA:
            if ((this.CharacterPrefab == GameLogic.CharacterPrefab.KnightMale) || (this.CharacterPrefab == GameLogic.CharacterPrefab.KnightFemale))
            {
                this.FaceRenderer = this.Transform.FindChild("HeroRoot").FindChild("Hero").GetComponent<SkinnedMeshRenderer>();
                this.FaceOriginalMaterial = this.FaceRenderer.sharedMaterials[2];
            }
        }

        public void initializeFlashMaterials()
        {
            this.m_flashMaterials = new List<FlashMaterial>();
            for (int i = 0; i < this.Renderers.Length; i++)
            {
                Renderer renderer = this.Renderers[i];
                if (((renderer.name != "Shadow") && (renderer.name != "DropLight")) && (renderer.name != "DungeonLight"))
                {
                    FlashMaterial item = new FlashMaterial();
                    item.Renderer = renderer;
                    item.FlashMaterials = new Material[renderer.sharedMaterials.Length];
                    for (int j = 0; j < renderer.sharedMaterials.Length; j++)
                    {
                        Material material2 = UnityEngine.Object.Instantiate<Material>(renderer.sharedMaterials[j]);
                        material2.name = material2.name + "-INSTANCE";
                        material2.shader = Shader.Find("CUSTOM/Opaque_Monochrome");
                        item.FlashMaterials[j] = material2;
                    }
                    this.m_flashMaterials.Add(item);
                }
            }
        }

        protected void LateUpdate()
        {
            if (this.Character != null)
            {
                if (this.IsMenuView)
                {
                    if (!UnityUtils.CoroutineRunning(ref this.m_eyeBlinkRoutine))
                    {
                        this.m_eyeBlinkRoutine = UnityUtils.StartCoroutine(this, this.eyeBlinkRoutine());
                    }
                }
                else if (GameLogic.Binder.GameState.ActiveDungeon != null)
                {
                    this.applyFlashColor();
                    if (!this.Character.IsDead && !UnityUtils.CoroutineRunning(ref this.m_leapRoutine))
                    {
                        this.Transform.position = this.Character.PhysicsBody.Transform.position;
                        this.Transform.rotation = this.Character.PhysicsBody.Transform.rotation;
                    }
                }
            }
        }

        public void leap(float duration, float height)
        {
            if (!UnityUtils.CoroutineRunning(ref this.m_leapRoutine))
            {
                this.m_leapRoutine = UnityUtils.StartCoroutine(this, this.characterLeapRoutine(duration, height));
            }
        }

        private void onBuffEnded(CharacterInstance character, Buff buff)
        {
            if ((character == this.Character) && !this.IsMenuView)
            {
                this.refreshBuffScale();
                this.refreshSpurtTrail();
            }
        }

        private void onBuffStarted(CharacterInstance character, Buff buff)
        {
            if ((character == this.Character) && !this.IsMenuView)
            {
                this.refreshBuffScale();
                this.refreshSpurtTrail();
            }
        }

        private void onCharacterAttackStopped(CharacterInstance character)
        {
            if (((character == this.Character) && !this.IsMenuView) && ((this.m_activeWeaponAccessory != null) && (this.m_activeWeaponAccessory.Trail != null)))
            {
                this.m_activeWeaponAccessory.Trail.Deactivate();
            }
        }

        private void onCharacterBlinked(CharacterInstance character)
        {
            if ((character == this.Character) && !this.IsMenuView)
            {
                this.setCharacterTrailActive(false);
            }
        }

        private void onCharacterDealtDamage(CharacterInstance sourceCharacter, CharacterInstance targetCharacter, Vector3 worldPos, double amount, bool critted, bool damageReduced, DamageType damageType, SkillType fromSkill)
        {
            if (((!this.IsMenuView && ((PlayerView.Binder.RoomView.getCharacterViewForCharacter(targetCharacter) == this) && base.gameObject.activeSelf)) && (amount > 0.0)) && (!this.Character.IsPlayerCharacter && ((!this.Character.Stunned && !this.Character.Charmed) && !this.Character.Confused)))
            {
                UnityUtils.StopCoroutine(this, ref this.m_damageFlashRoutine);
                this.m_damageFlashRoutine = UnityUtils.StartCoroutine(this, this.damageFlashRoutine());
            }
        }

        private void onCharacterInterrupted(CharacterInstance character, bool stopSkills)
        {
            if (((character == this.Character) && !this.IsMenuView) && ((this.m_activeWeaponAccessory != null) && (this.m_activeWeaponAccessory.Trail != null)))
            {
                this.m_activeWeaponAccessory.Trail.Deactivate();
            }
        }

        private void onCharacterKilled(CharacterInstance character, CharacterInstance killer, bool critted, SkillType fromSkill)
        {
            if (character == this.Character)
            {
                this.setFlashMaterialsEnabled(false);
                this.setKnockedOutFaceActive(true);
            }
        }

        private void onCharacterMeleeAttackEnded(CharacterInstance character, CharacterInstance targetCharacter, Vector3 contactWorldPt, int killCount)
        {
            if (((character == this.Character) && !this.IsMenuView) && ((this.m_activeWeaponAccessory != null) && (this.m_activeWeaponAccessory.Trail != null)))
            {
                this.m_activeWeaponAccessory.Trail.StopSmoothly(0.2f);
            }
        }

        private void onCharacterMeleeAttackStarted(CharacterInstance sourceCharacter, CharacterInstance targetCharacter)
        {
            if (((sourceCharacter == this.Character) && !this.IsMenuView) && ((this.m_activeWeaponAccessory != null) && (this.m_activeWeaponAccessory.Trail != null)))
            {
                this.m_activeWeaponAccessory.Trail.Activate();
            }
        }

        private void onCharacterPreBlink(CharacterInstance character)
        {
            if ((character == this.Character) && !this.IsMenuView)
            {
                this.setCharacterTrailActive(false);
                this.setSpurtTrailActive(false);
            }
        }

        private void onCharacterRevived(CharacterInstance character)
        {
            if (character == this.Character)
            {
                this.setKnockedOutFaceActive(false);
            }
        }

        private void onCharacterSkillActivated(CharacterInstance character, SkillType skillType, float buildupTime, SkillExecutionStats executionStats)
        {
            if (((character == this.Character) && !this.IsMenuView) && (((skillType == SkillType.Slam) && (this.m_activeWeaponAccessory != null)) && (this.m_activeWeaponAccessory.Trail != null)))
            {
                this.m_activeWeaponAccessory.Trail.Activate();
            }
        }

        private void onCharacterSkillBuildupCompleted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if ((character == this.Character) && !this.IsMenuView)
            {
                if (skillType == SkillType.Leap)
                {
                    float movementDurationDynamic;
                    float leapTargetHeight = ConfigSkills.Leap.LeapTargetHeight;
                    if (character.IsPet)
                    {
                        leapTargetHeight = ConfigSkills.Leap.LeapTargetHeight;
                    }
                    if (executionStats.MovementDurationDynamic > 0f)
                    {
                        movementDurationDynamic = executionStats.MovementDurationDynamic;
                        if (executionStats.MovementDurationDynamic < ConfigSkills.Leap.LeapDuration)
                        {
                            leapTargetHeight *= Mathf.Clamp01(executionStats.MovementDurationDynamic / ConfigSkills.Leap.LeapDuration);
                        }
                    }
                    else
                    {
                        movementDurationDynamic = ConfigSkills.Leap.LeapDuration;
                    }
                    this.leap(movementDurationDynamic, leapTargetHeight);
                    if ((this.m_activeWeaponAccessory != null) && (this.m_activeWeaponAccessory.Trail != null))
                    {
                        this.m_activeWeaponAccessory.Trail.Activate();
                    }
                }
                else if (skillType == SkillType.Decoy)
                {
                    this.leap(ConfigSkills.Decoy.LeapDuration, ConfigSkills.Decoy.LeapTargetHeight);
                    if ((this.m_activeWeaponAccessory != null) && (this.m_activeWeaponAccessory.Trail != null))
                    {
                        this.m_activeWeaponAccessory.Trail.Activate();
                    }
                }
                else if (skillType == SkillType.Omnislash)
                {
                    if (executionStats.EnemiesAround > 0)
                    {
                        this.setCharacterTrailActive(true);
                        this.setSpurtTrailActive(false);
                    }
                }
                else if (skillType == SkillType.Implosion)
                {
                    this.leap(ConfigSkills.Implosion.LeapDuration, ConfigSkills.Implosion.LeapTargetHeight);
                }
                else if (skillType == SkillType.BossSlam)
                {
                    this.leap(ConfigSkills.BossSlam.LeapDuration, ConfigSkills.BossSlam.LeapTargetHeight);
                }
            }
        }

        private void onCharacterSkillExecuted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if ((character == this.Character) && !this.IsMenuView)
            {
                if ((this.m_activeWeaponAccessory != null) && (this.m_activeWeaponAccessory.Trail != null))
                {
                    this.m_activeWeaponAccessory.Trail.StopSmoothly(0.2f);
                }
                if (skillType == SkillType.Omnislash)
                {
                    this.setCharacterTrailActive(false);
                }
            }
        }

        private void onCharacterSkillStopped(CharacterInstance character, SkillType skillType)
        {
            if (!this.IsMenuView && (character == this.Character))
            {
                this.setCharacterTrailActive(false);
            }
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnCharacterDealtDamage -= new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnGameplayStateChanged -= new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            eventBus.OnCharacterSkillActivated -= new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            eventBus.OnCharacterSkillBuildupCompleted -= new GameLogic.Events.CharacterSkillBuildupCompleted(this.onCharacterSkillBuildupCompleted);
            eventBus.OnCharacterSkillExecuted -= new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            eventBus.OnCharacterSkillStopped -= new GameLogic.Events.CharacterSkillStopped(this.onCharacterSkillStopped);
            eventBus.OnItemEquipped -= new GameLogic.Events.ItemEquipped(this.onItemEquipped);
            eventBus.OnCharacterMeleeAttackStarted -= new GameLogic.Events.CharacterMeleeAttackStarted(this.onCharacterMeleeAttackStarted);
            eventBus.OnCharacterMeleeAttackEnded -= new GameLogic.Events.CharacterMeleeAttackEnded(this.onCharacterMeleeAttackEnded);
            eventBus.OnCharacterInterrupted -= new GameLogic.Events.CharacterInterrupted(this.onCharacterInterrupted);
            eventBus.OnCharacterAttackStopped -= new GameLogic.Events.CharacterAttackStopped(this.onCharacterAttackStopped);
            eventBus.OnTimescaleChangeStarted -= new GameLogic.Events.TimescaleChangeStarted(this.onTimescaleChangeStarted);
            eventBus.OnTimescaleChanged -= new GameLogic.Events.TimescaleChanged(this.onTimescaleChanged);
            eventBus.OnCharacterPreBlink -= new GameLogic.Events.CharacterPreBlink(this.onCharacterPreBlink);
            eventBus.OnCharacterBlinked -= new GameLogic.Events.CharacterBlinked(this.onCharacterBlinked);
            eventBus.OnBuffStarted -= new GameLogic.Events.BuffStarted(this.onBuffStarted);
            eventBus.OnBuffEnded -= new GameLogic.Events.BuffEnded(this.onBuffEnded);
            eventBus.OnCharacterKilled -= new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterRevived -= new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            this.m_eyeBlinkRoutine = null;
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnCharacterDealtDamage += new GameLogic.Events.CharacterDealtDamage(this.onCharacterDealtDamage);
            eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            eventBus.OnGameplayStateChanged += new GameLogic.Events.GameplayStateChanged(this.onGameplayStateChanged);
            eventBus.OnCharacterSkillActivated += new GameLogic.Events.CharacterSkillActivated(this.onCharacterSkillActivated);
            eventBus.OnCharacterSkillBuildupCompleted += new GameLogic.Events.CharacterSkillBuildupCompleted(this.onCharacterSkillBuildupCompleted);
            eventBus.OnCharacterSkillExecuted += new GameLogic.Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            eventBus.OnCharacterSkillStopped += new GameLogic.Events.CharacterSkillStopped(this.onCharacterSkillStopped);
            eventBus.OnItemEquipped += new GameLogic.Events.ItemEquipped(this.onItemEquipped);
            eventBus.OnCharacterMeleeAttackStarted += new GameLogic.Events.CharacterMeleeAttackStarted(this.onCharacterMeleeAttackStarted);
            eventBus.OnCharacterMeleeAttackEnded += new GameLogic.Events.CharacterMeleeAttackEnded(this.onCharacterMeleeAttackEnded);
            eventBus.OnCharacterInterrupted += new GameLogic.Events.CharacterInterrupted(this.onCharacterInterrupted);
            eventBus.OnCharacterAttackStopped += new GameLogic.Events.CharacterAttackStopped(this.onCharacterAttackStopped);
            eventBus.OnTimescaleChangeStarted += new GameLogic.Events.TimescaleChangeStarted(this.onTimescaleChangeStarted);
            eventBus.OnTimescaleChanged += new GameLogic.Events.TimescaleChanged(this.onTimescaleChanged);
            eventBus.OnCharacterPreBlink += new GameLogic.Events.CharacterPreBlink(this.onCharacterPreBlink);
            eventBus.OnCharacterBlinked += new GameLogic.Events.CharacterBlinked(this.onCharacterBlinked);
            eventBus.OnBuffStarted += new GameLogic.Events.BuffStarted(this.onBuffStarted);
            eventBus.OnBuffEnded += new GameLogic.Events.BuffEnded(this.onBuffEnded);
            eventBus.OnCharacterKilled += new GameLogic.Events.CharacterKilled(this.onCharacterKilled);
            eventBus.OnCharacterRevived += new GameLogic.Events.CharacterRevived(this.onCharacterRevived);
            if ((Time.timeScale == 0.001f) && (this.CloakCloth != null))
            {
                this.CloakCloth.enabled = false;
            }
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            this.setFlashMaterialsEnabled(false);
            this.setCharacterTrailActive(false);
            this.setSpurtTrailActive(false);
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.Transform.localRotation = Quaternion.identity;
            for (int i = 0; i < this.Renderers.Length; i++)
            {
                this.Renderers[i].useLightProbes = false;
            }
            this.setFlashMaterialsEnabled(false);
            this.setKnockedOutFaceActive(false);
            this.refreshActiveArmorAccessories();
            this.refreshActiveWeaponAccessory();
            this.refreshBodyMaterials();
        }

        private void onGameplayStateChanged(GameplayState previousState, GameplayState currentState)
        {
            if (currentState == GameplayState.ACTION)
            {
                this.refreshSpurtTrail();
            }
        }

        private void onItemEquipped(CharacterInstance character, ItemInstance itemInstance, ItemInstance replacedItemInstance)
        {
            if ((character == this.Character) && !this.IsMenuView)
            {
                this.setFlashMaterialsEnabled(false);
                if (itemInstance.Item.Type == ItemType.Weapon)
                {
                    this.refreshActiveWeaponAccessory();
                }
                else if (itemInstance.Item.Type == ItemType.Armor)
                {
                    this.refreshActiveArmorAccessories();
                }
                this.refreshBodyMaterials();
            }
        }

        private void onTimescaleChanged(float targetTimescale)
        {
            if ((this.CloakCloth != null) && ((targetTimescale != 0.001f) && (ConfigDevice.DeviceQuality() >= DeviceQualityType.Med)))
            {
                this.CloakCloth.enabled = true;
            }
        }

        private void onTimescaleChangeStarted(float targetTimescale)
        {
            if ((this.CloakCloth != null) && (targetTimescale == 0.001f))
            {
                this.CloakCloth.enabled = false;
            }
        }

        public void postAwake(GameLogic.CharacterPrefab characterPrefab)
        {
            this.CharacterPrefab = characterPrefab;
        }

        private void refreshActiveArmorAccessories()
        {
            if (this.ArmorAccessoryRoots != null)
            {
                ItemInstance instance = this.Character.getEquippedItemOfType(ItemType.Armor);
                if ((instance == null) || (instance.ItemId != this.m_activeArmorItemId))
                {
                    if (this.m_activeArmorAccessories == null)
                    {
                        this.m_activeArmorAccessories = new List<Accessory>();
                    }
                    for (int i = 0; i < this.m_activeArmorAccessories.Count; i++)
                    {
                        PlayerView.Binder.AccessoryStorage.releaseAccessory(this.m_activeArmorAccessories[i]);
                    }
                    this.m_activeArmorAccessories.Clear();
                    this.m_activeArmorItemId = null;
                    if (instance != null)
                    {
                        OrderedDict<AccessoryType, string> accessories = instance.Item.Accessories;
                        Accessory item = PlayerView.Binder.AccessoryStorage.getAccessory(accessories.Keys[0], accessories.Values[0]);
                        item.Tm.SetParent(this.ArmorAccessoryRoots[0], false);
                        if (this.IsMenuView)
                        {
                            item.Renderer.sharedMaterial = PlayerView.Binder.MaterialStorage.getSharedMenuMaterial(item.Renderer.sharedMaterial);
                        }
                        this.m_activeArmorAccessories.Add(item);
                        if (this.ArmorAccessoryRoots.Count > 1)
                        {
                            item = PlayerView.Binder.AccessoryStorage.getAccessory(accessories.Keys[1], accessories.Values[1]);
                            item.Tm.SetParent(this.ArmorAccessoryRoots[1], false);
                            if (this.IsMenuView)
                            {
                                item.Renderer.sharedMaterial = PlayerView.Binder.MaterialStorage.getSharedMenuMaterial(item.Renderer.sharedMaterial);
                            }
                            this.m_activeArmorAccessories.Add(item);
                        }
                        if (this.ArmorAccessoryRoots.Count > 2)
                        {
                            item = PlayerView.Binder.AccessoryStorage.getAccessory(accessories.Keys[1], accessories.Values[1]);
                            item.Tm.SetParent(this.ArmorAccessoryRoots[2], false);
                            if (this.IsMenuView)
                            {
                                item.Renderer.sharedMaterial = PlayerView.Binder.MaterialStorage.getSharedMenuMaterial(item.Renderer.sharedMaterial);
                            }
                            this.m_activeArmorAccessories.Add(item);
                        }
                        this.m_activeArmorItemId = instance.ItemId;
                    }
                    else
                    {
                        this.m_activeArmorItemId = null;
                    }
                }
            }
        }

        private void refreshActiveWeaponAccessory()
        {
            if (this.WeaponAccessoryRoot != null)
            {
                AccessoryType nONE = AccessoryType.NONE;
                ItemInstance instance = this.Character.getEquippedItemOfType(ItemType.Weapon);
                if (instance != null)
                {
                    nONE = instance.Item.Accessories.Keys[0];
                }
                if ((this.m_activeWeaponAccessory == null) || (this.m_activeWeaponAccessory.Type != nONE))
                {
                    if (this.m_activeWeaponAccessory != null)
                    {
                        PlayerView.Binder.AccessoryStorage.releaseAccessory(this.m_activeWeaponAccessory);
                        this.m_activeWeaponAccessory = null;
                    }
                    if (nONE != AccessoryType.NONE)
                    {
                        this.m_activeWeaponAccessory = PlayerView.Binder.AccessoryStorage.getAccessory(nONE, null);
                        this.m_activeWeaponAccessory.Tm.SetParent(this.WeaponAccessoryRoot, false);
                        if (this.IsMenuView)
                        {
                            this.m_activeWeaponAccessory.Renderer.sharedMaterial = PlayerView.Binder.MaterialStorage.getSharedMenuMaterial(this.m_activeWeaponAccessory.Renderer.sharedMaterial);
                        }
                        if (this.m_activeWeaponAccessory.Trail != null)
                        {
                            this.m_activeWeaponAccessory.Trail.Deactivate();
                        }
                    }
                }
            }
        }

        private void refreshBodyMaterials()
        {
            if ((this.m_bodyMaterials != null) && (this.Character != null))
            {
                for (int i = 0; i < ConfigMeta.ACTIVE_ITEM_TYPES.Count; i++)
                {
                    ItemType itemType = ConfigMeta.ACTIVE_ITEM_TYPES[i];
                    ItemInstance instance = this.Character.getEquippedItemOfType(itemType);
                    for (int j = 0; j < this.m_bodyMaterials.Count; j++)
                    {
                        BodyMaterial material = this.m_bodyMaterials[j];
                        if ((material.ChangedWithItemType == itemType) && material.Renderer.gameObject.activeInHierarchy)
                        {
                            Material[] sharedMaterials = material.Renderer.sharedMaterials;
                            if ((instance != null) && (instance.Item.BodyMaterials.Count > 0))
                            {
                                int rendererMaterialIndex = material.RendererMaterialIndex;
                                if ((sharedMaterials[rendererMaterialIndex] != null) && (sharedMaterials[rendererMaterialIndex] != material.OriginalMat))
                                {
                                    GameLogic.Binder.ItemResources.releaseItemBodyMaterial(sharedMaterials[rendererMaterialIndex]);
                                    sharedMaterials[rendererMaterialIndex] = null;
                                }
                                Material gameplayMaterial = GameLogic.Binder.ItemResources.getItemBodyMaterial(instance.Item, material.ResourceMaterialIndex);
                                if (this.IsMenuView)
                                {
                                    sharedMaterials[rendererMaterialIndex] = PlayerView.Binder.MaterialStorage.getSharedMenuMaterial(gameplayMaterial);
                                }
                                else
                                {
                                    sharedMaterials[rendererMaterialIndex] = gameplayMaterial;
                                }
                            }
                            else
                            {
                                sharedMaterials[material.RendererMaterialIndex] = material.OriginalMat;
                            }
                            material.Renderer.sharedMaterials = sharedMaterials;
                        }
                    }
                }
            }
        }

        private void refreshBuffScale()
        {
            if (!this.IsMenuView)
            {
                float num = GameLogic.Binder.BuffSystem.getTargetViewScaleForCharacter(this.Character);
                float characterVisualScale = ConfigGameplay.GetCharacterVisualScale(this.Character);
                this.scale(num * characterVisualScale, ConfigGameplay.BUFF_VIEW_SCALE_MODIFIER_ENTRY_DURATION);
            }
        }

        private void refreshSpurtTrail()
        {
            if (!this.IsMenuView && (this.SpurtTrail != null))
            {
                if (GameLogic.Binder.BuffSystem.getNumberOfBuffsWithId(this.Character, ConfigGameplay.SPURT_BUFF_ID) > 0)
                {
                    this.setSpurtTrailActive(true);
                    float a = Easing.Apply(this.Character.getSpurtBuffStrength(), Easing.Function.IN_QUAD);
                    this.SpurtTrail.MyColor = new Color(this.m_spurtTrailOrigColor.r, this.m_spurtTrailOrigColor.g, this.m_spurtTrailOrigColor.b, a);
                }
                else
                {
                    this.setSpurtTrailActive(false);
                }
            }
        }

        public void scale(float targetScale, float duration)
        {
            Vector3 target = (Vector3) (Vector3.one * targetScale);
            if (this.Transform.localScale != target)
            {
                if (duration > 0f)
                {
                    TransformAnimationTask animationTask = new TransformAnimationTask(this.Transform, duration, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    animationTask.scale(target, true, Easing.Function.OUT_BACK);
                    this.TransformAnimation.addTask(animationTask);
                }
                else
                {
                    this.Transform.localScale = target;
                }
            }
        }

        private void setCharacterTrailActive(bool active)
        {
            if (this.CharacterTrail != null)
            {
                if (active && !this.CharacterTrail.gameObject.activeSelf)
                {
                    this.CharacterTrail.Activate();
                }
                else if (!active)
                {
                    this.CharacterTrail.Deactivate();
                }
            }
        }

        public void setFlashMaterialColor(Color color)
        {
            this.m_flashMaterialColor = color;
            if (this.m_activeWeaponAccessory != null)
            {
                this.m_activeWeaponAccessory.setFlashMaterialColor(color);
            }
            if (this.m_activeArmorAccessories != null)
            {
                for (int i = 0; i < this.m_activeArmorAccessories.Count; i++)
                {
                    this.m_activeArmorAccessories[i].setFlashMaterialColor(color);
                }
            }
        }

        public void setFlashMaterialsEnabled(bool enabled)
        {
            if (this.m_flashMaterialsEnabled != enabled)
            {
                for (int i = 0; i < this.m_flashMaterials.Count; i++)
                {
                    FlashMaterial material = this.m_flashMaterials[i];
                    if (enabled)
                    {
                        material.MaterialsBeforeFlash = material.Renderer.sharedMaterials;
                        material.Renderer.sharedMaterials = material.FlashMaterials;
                    }
                    else
                    {
                        material.Renderer.sharedMaterials = material.MaterialsBeforeFlash;
                    }
                }
                if (this.m_activeWeaponAccessory != null)
                {
                    this.m_activeWeaponAccessory.setFlashMaterialsEnabled(enabled);
                }
                if (this.m_activeArmorAccessories != null)
                {
                    for (int j = 0; j < this.m_activeArmorAccessories.Count; j++)
                    {
                        this.m_activeArmorAccessories[j].setFlashMaterialsEnabled(enabled);
                    }
                }
                this.m_flashMaterialsEnabled = enabled;
            }
        }

        public void setKnockedOutFaceActive(bool active)
        {
            if (this.FaceRenderer != null)
            {
                Material[] sharedMaterials = this.FaceRenderer.sharedMaterials;
                if (active)
                {
                    sharedMaterials[2] = PlayerView.Binder.MaterialStorage.getSharedKnockedOutMaterialForCharacterPrefab(this.CharacterPrefab, this.IsMenuView);
                }
                else
                {
                    sharedMaterials[2] = this.FaceOriginalMaterial;
                }
                this.FaceRenderer.sharedMaterials = sharedMaterials;
            }
        }

        public void setSpurtTrailActive(bool active)
        {
            if (this.SpurtTrail != null)
            {
                if (active && !this.SpurtTrail.gameObject.activeSelf)
                {
                    this.SpurtTrail.Activate();
                }
                else if (!active)
                {
                    this.SpurtTrail.Deactivate();
                }
            }
        }

        public void setVisibility(bool visible)
        {
            for (int i = 0; i < this.Renderers.Length; i++)
            {
                this.Renderers[i].enabled = visible;
            }
            if (this.CloakCloth != null)
            {
                if (this.IsMenuView)
                {
                    this.CloakCloth.enabled = visible;
                }
                else if (visible)
                {
                    this.CloakCloth.enabled = ConfigDevice.DeviceQuality() >= DeviceQualityType.Med;
                }
                else
                {
                    this.CloakCloth.enabled = false;
                }
            }
            this.setKnockedOutFaceActive(false);
            this.setFlashMaterialsEnabled(false);
            this.refreshActiveArmorAccessories();
            this.refreshActiveWeaponAccessory();
            this.refreshBodyMaterials();
            if (this.Character != null)
            {
                PlayerView.Binder.EffectSystem.setFollowParticleSystemsVisibility(this.Character, visible);
            }
            if (this.m_activeWeaponAccessory != null)
            {
                this.m_activeWeaponAccessory.setVisibility(visible);
            }
            if (this.m_activeArmorAccessories != null)
            {
                for (int j = 0; j < this.m_activeArmorAccessories.Count; j++)
                {
                    this.m_activeArmorAccessories[j].setVisibility(visible);
                }
            }
            if (!visible)
            {
                this.setSpurtTrailActive(false);
                this.setCharacterTrailActive(false);
            }
        }

        public AbstractCharacterAnimator Animator
        {
            [CompilerGenerated]
            get
            {
                return this.<Animator>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Animator>k__BackingField = value;
            }
        }

        public List<UnityEngine.Transform> ArmorAccessoryRoots
        {
            [CompilerGenerated]
            get
            {
                return this.<ArmorAccessoryRoots>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ArmorAccessoryRoots>k__BackingField = value;
            }
        }

        public AbstractCharacterAudio Audio
        {
            [CompilerGenerated]
            get
            {
                return this.<Audio>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Audio>k__BackingField = value;
            }
        }

        public CharacterInstance Character
        {
            [CompilerGenerated]
            get
            {
                return this.<Character>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Character>k__BackingField = value;
            }
        }

        public GameLogic.CharacterPrefab CharacterPrefab
        {
            [CompilerGenerated]
            get
            {
                return this.<CharacterPrefab>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CharacterPrefab>k__BackingField = value;
            }
        }

        public XWeaponTrail CharacterTrail
        {
            [CompilerGenerated]
            get
            {
                return this.<CharacterTrail>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CharacterTrail>k__BackingField = value;
            }
        }

        public Cloth CloakCloth
        {
            [CompilerGenerated]
            get
            {
                return this.<CloakCloth>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<CloakCloth>k__BackingField = value;
            }
        }

        public PlayerView.DropLight DropLight
        {
            [CompilerGenerated]
            get
            {
                return this.<DropLight>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DropLight>k__BackingField = value;
            }
        }

        public Material FaceOriginalMaterial
        {
            [CompilerGenerated]
            get
            {
                return this.<FaceOriginalMaterial>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<FaceOriginalMaterial>k__BackingField = value;
            }
        }

        public Renderer FaceRenderer
        {
            [CompilerGenerated]
            get
            {
                return this.<FaceRenderer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<FaceRenderer>k__BackingField = value;
            }
        }

        public bool IsMenuView
        {
            [CompilerGenerated]
            get
            {
                return this.<IsMenuView>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<IsMenuView>k__BackingField = value;
            }
        }

        public Dictionary<Renderer, Mesh> Meshes
        {
            [CompilerGenerated]
            get
            {
                return this.<Meshes>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Meshes>k__BackingField = value;
            }
        }

        public UnityEngine.Transform ProjectileSpawnpoint
        {
            [CompilerGenerated]
            get
            {
                return this.<ProjectileSpawnpoint>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ProjectileSpawnpoint>k__BackingField = value;
            }
        }

        public List<Rigidbody> RagdollRigidbodies
        {
            [CompilerGenerated]
            get
            {
                return this.<RagdollRigidbodies>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RagdollRigidbodies>k__BackingField = value;
            }
        }

        public Renderer[] Renderers
        {
            [CompilerGenerated]
            get
            {
                return this.<Renderers>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Renderers>k__BackingField = value;
            }
        }

        public Renderer ShadowRenderer
        {
            [CompilerGenerated]
            get
            {
                return this.<ShadowRenderer>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ShadowRenderer>k__BackingField = value;
            }
        }

        public XWeaponTrail SpurtTrail
        {
            [CompilerGenerated]
            get
            {
                return this.<SpurtTrail>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SpurtTrail>k__BackingField = value;
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

        public UnityEngine.Transform WeaponAccessoryRoot
        {
            [CompilerGenerated]
            get
            {
                return this.<WeaponAccessoryRoot>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<WeaponAccessoryRoot>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <characterLeapRoutine>c__IteratorE8 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>duration;
            internal float <$>targetHeight;
            internal CharacterView <>f__this;
            internal float <easedV>__4;
            internal Vector3 <pos>__2;
            internal float <targetY>__1;
            internal ManualTimer <timer>__0;
            internal float <v>__3;
            internal float duration;
            internal float targetHeight;

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
                        this.<timer>__0 = new ManualTimer(this.duration);
                        this.<targetY>__1 = this.targetHeight;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_012B;
                }
                if (!this.<timer>__0.Idle)
                {
                    this.<pos>__2 = this.<>f__this.Character.PhysicsBody.Transform.position;
                    this.<v>__3 = this.<timer>__0.normalizedProgress();
                    this.<easedV>__4 = Mathf.Clamp01((this.<v>__3 <= 0.75f) ? this.<v>__3 : (1.2f * this.<v>__3));
                    this.<pos>__2.y = Mathf.Sin(this.<easedV>__4 * 3.141593f) * this.<targetY>__1;
                    this.<>f__this.Transform.position = this.<pos>__2;
                    this.<timer>__0.tick(Time.deltaTime);
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.m_leapRoutine = null;
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

        [CompilerGenerated]
        private sealed class <damageFlashRoutine>c__IteratorE9 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterView <>f__this;
            internal Color <damageFlashColor>__3;
            internal float <duration>__0;
            internal float <entryDuration>__2;
            internal float <exitDuration>__4;
            internal float <flashMaxAlpha>__1;

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
                        if (!ConfigApp.CHEAT_MARKETING_MODE_ENABLED || !PlayerView.Binder.MarketingBuildController.DamageFlashDisabled)
                        {
                            this.<duration>__0 = 0.15f;
                            this.<flashMaxAlpha>__1 = 0.6f;
                            this.<entryDuration>__2 = this.<duration>__0 * 0.5f;
                            this.<damageFlashColor>__3 = Color.red;
                            this.<>f__this.m_timer.set(this.<entryDuration>__2);
                            break;
                        }
                        this.<>f__this.m_damageFlashRoutine = null;
                        goto Label_01D6;

                    case 1:
                        break;

                    case 2:
                        goto Label_01A9;

                    default:
                        goto Label_01D6;
                }
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<damageFlashColor>__3.a = this.<flashMaxAlpha>__1 * this.<>f__this.m_timer.normalizedProgress();
                    this.<>f__this.setFlashMaterialColor(this.<damageFlashColor>__3);
                    this.<>f__this.m_timer.tick(Time.deltaTime * Time.timeScale);
                    this.$current = null;
                    this.$PC = 1;
                    goto Label_01D8;
                }
                this.<exitDuration>__4 = this.<duration>__0 * 0.5f;
                this.<>f__this.m_timer.set(this.<exitDuration>__4);
            Label_01A9:
                while (!this.<>f__this.m_timer.Idle)
                {
                    this.<damageFlashColor>__3.a = this.<flashMaxAlpha>__1 * (1f - this.<>f__this.m_timer.normalizedProgress());
                    this.<>f__this.setFlashMaterialColor(this.<damageFlashColor>__3);
                    this.<>f__this.m_timer.tick(Time.deltaTime * Time.timeScale);
                    this.$current = null;
                    this.$PC = 2;
                    goto Label_01D8;
                }
                this.<>f__this.m_damageFlashRoutine = null;
                goto Label_01D6;
                this.$PC = -1;
            Label_01D6:
                return false;
            Label_01D8:
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
        private sealed class <eyeBlinkRoutine>c__IteratorEA : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterView <>f__this;

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
                        this.<>f__this.setKnockedOutFaceActive(true);
                        this.$current = new WaitForSeconds(0.08f);
                        this.$PC = 2;
                        goto Label_008D;

                    case 2:
                        this.<>f__this.setKnockedOutFaceActive(false);
                        break;

                    default:
                        goto Label_008B;
                }
                this.$current = new WaitForSeconds(UnityEngine.Random.Range((float) 2f, (float) 3.5f));
                this.$PC = 1;
                goto Label_008D;
                this.$PC = -1;
            Label_008B:
                return false;
            Label_008D:
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

        private class BodyMaterial
        {
            public ItemType ChangedWithItemType;
            public Material OriginalMat;
            public UnityEngine.Renderer Renderer;
            public int RendererMaterialIndex;
            public int ResourceMaterialIndex;
        }

        private class FlashMaterial
        {
            public Material[] FlashMaterials;
            public Material[] MaterialsBeforeFlash;
            public UnityEngine.Renderer Renderer;
        }
    }
}

