namespace GameLogic
{
    using App;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Projectile : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private int <KillCounter>k__BackingField;
        [CompilerGenerated]
        private CharacterInstance <OwningCharacter>k__BackingField;
        [CompilerGenerated]
        private ProjectileProperties <Properties>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Rigidbody <Rigidbody>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.SphereCollider <SphereCollider>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        private Vector3 m_curveControlPoint;
        private float m_curveTravelDuration;
        private float m_lifetimeTimer;
        private Vector3 m_spawnWorldPt;
        private Vector3 m_targetWorldPt;
        private bool m_usesCurve;

        protected void Awake()
        {
            this.Transform = base.transform;
            this.SphereCollider = base.gameObject.AddComponent<UnityEngine.SphereCollider>();
            this.SphereCollider.isTrigger = true;
            this.Rigidbody = base.gameObject.AddComponent<UnityEngine.Rigidbody>();
            this.Rigidbody.useGravity = false;
        }

        public void cleanUpForReuse()
        {
            this.OwningCharacter = null;
            this.SphereCollider.enabled = false;
            this.KillCounter = 0;
            this.Rigidbody.isKinematic = false;
            this.m_curveControlPoint = Vector3.zero;
            this.m_curveTravelDuration = 0f;
            this.m_spawnWorldPt = Vector3.zero;
            this.m_targetWorldPt = Vector3.zero;
            this.m_usesCurve = false;
        }

        public void destroy()
        {
            GameLogic.Binder.EventBus.ProjectilePreDestroy(this);
            GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveProjectiles.Remove(this);
            GameLogic.Binder.ProjectilePool.returnObject(this);
        }

        protected void FixedUpdate()
        {
            this.m_lifetimeTimer -= Time.deltaTime * Time.timeScale;
            if (this.m_lifetimeTimer <= 0f)
            {
                this.destroy();
            }
            else if (this.Properties.RequireGroundTouch && !PhysicsUtil.IsOnSurface(this.Transform, this.SphereCollider, float.MaxValue, Layers.GroundLayerMask))
            {
                this.destroy();
            }
            else if (this.SphereCollider.enabled)
            {
                if (this.Properties.UsesGravity)
                {
                    this.Rigidbody.velocity += (Vector3) (((Physics.gravity * this.Properties.GravityFactor) * Time.deltaTime) * Time.timeScale);
                }
                if (this.m_usesCurve)
                {
                    float num = this.Properties.MaxLifetime - this.m_lifetimeTimer;
                    this.Rigidbody.MovePosition(MathUtil.QuadraticBezier(num / this.m_curveTravelDuration, this.m_spawnWorldPt, this.m_curveControlPoint, this.m_targetWorldPt));
                }
            }
        }

        private void initialize(CharacterInstance owner, Vector3 spawnWorldPt, ProjectileProperties props)
        {
            this.OwningCharacter = owner;
            this.Transform.position = spawnWorldPt;
            this.Properties = props;
            this.SphereCollider.enabled = true;
            this.SphereCollider.radius = props.Radius;
            this.m_lifetimeTimer = props.MaxLifetime;
            GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveProjectiles.Add(this);
            GameLogic.Binder.EventBus.ProjectileSpawned(this);
        }

        private void onContact(Collider targetCollider)
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            bool flag = activeDungeon.CurrentGameplayState == GameplayState.ACTION;
            if (flag)
            {
                DestructiblePhysicsBody component = targetCollider.GetComponent<DestructiblePhysicsBody>();
                if (component != null)
                {
                    component.Hit(this.Properties.FromSkill);
                }
                PhysicsBody body2 = targetCollider.GetComponent<PhysicsBody>();
                if ((body2 != null) && !body2.AttachedCharacter.IsDead)
                {
                    Buff buff5;
                    CharacterInstance attachedCharacter = body2.AttachedCharacter;
                    if (body2.AttachedCharacter.IsPlayerCharacter)
                    {
                        if (body2.AttachedCharacter.IsSupport)
                        {
                            if (!body2.AttachedCharacter.isDecoyClone())
                            {
                                return;
                            }
                        }
                        else if (body2.AttachedCharacter.IsPrimaryPlayerCharacter && (activeDungeon.ActiveRoom.numberOfDecoyClonesAlive() > 0))
                        {
                            return;
                        }
                    }
                    if (this.Properties.StunDurationSeconds > 0f)
                    {
                        buff5 = new Buff();
                        buff5.Stuns = true;
                        buff5.DurationSeconds = this.Properties.StunDurationSeconds;
                        buff5.SourceCharacter = this.OwningCharacter;
                        Buff buff = buff5;
                        GameLogic.Binder.BuffSystem.startBuff(attachedCharacter, buff);
                    }
                    if ((this.Properties.CharmDurationSeconds > 0f) && !attachedCharacter.IsBoss)
                    {
                        buff5 = new Buff();
                        buff5.Charms = true;
                        buff5.DurationSeconds = this.Properties.CharmDurationSeconds;
                        Buff buff2 = buff5;
                        GameLogic.Binder.BuffSystem.startBuff(attachedCharacter, buff2);
                    }
                    if (this.Properties.ConfusionDurationSeconds > 0f)
                    {
                        buff5 = new Buff();
                        buff5.Confuses = true;
                        buff5.DurationSeconds = this.Properties.ConfusionDurationSeconds;
                        Buff buff3 = buff5;
                        GameLogic.Binder.BuffSystem.startBuff(attachedCharacter, buff3);
                    }
                    if (this.Properties.KnockbackAndSlowDurationSeconds > 0f)
                    {
                        float num = UnityEngine.Random.Range(ConfigPerks.CritKnockbackSlow.KnockbackForceMin, ConfigPerks.CritKnockbackSlow.KnockbackForceMax);
                        Vector3 normalized = Vector3Extensions.ToXzVector3(attachedCharacter.PhysicsBody.Transform.position - this.Transform.position).normalized;
                        GameLogic.Binder.CommandProcessor.executeCharacterSpecific(attachedCharacter, new CmdPushCharacter(attachedCharacter, (Vector3) (normalized * num)), 0f);
                        buff5 = new Buff();
                        buff5.BaseStat1 = BaseStatProperty.AttacksPerSecond;
                        buff5.BaseStat2 = BaseStatProperty.MovementSpeed;
                        buff5.Modifier = ConfigPerks.GlobalFrostEffect.SpeedModifier;
                        buff5.DurationSeconds = this.Properties.KnockbackAndSlowDurationSeconds;
                        Buff buff4 = buff5;
                        GameLogic.Binder.BuffSystem.startBuff(attachedCharacter, buff4);
                    }
                    CmdDealDamageToCharacter.ExecuteStatic(this.OwningCharacter, attachedCharacter, this.Properties.DamagePerHit, this.Properties.IsCriticalHit, this.Properties.DamageType, this.Properties.FromSkill);
                    if (attachedCharacter.IsDead)
                    {
                        this.KillCounter++;
                    }
                }
                GameLogic.Binder.EventBus.ProjectileCollided(this, targetCollider);
            }
            if (this.Properties.DestroyAfterContact || !flag)
            {
                this.destroy();
            }
        }

        protected void OnTriggerEnter(Collider targetCollider)
        {
            if (this.OwningCharacter != null)
            {
                if (this.Properties.CollideWithGround && (targetCollider.gameObject.layer == Layers.GROUND))
                {
                    this.onContact(targetCollider);
                }
                else if (targetCollider.tag == Tags.MONSTER_CHARACTER)
                {
                    if ((this.OwningCharacter.IsPlayerCharacter || this.OwningCharacter.Charmed) || this.OwningCharacter.Confused)
                    {
                        this.onContact(targetCollider);
                    }
                }
                else if ((targetCollider.tag == Tags.HERO_CHARACTER) || (targetCollider.tag == Tags.SUPPORT_CHARACTER))
                {
                    if (!this.OwningCharacter.IsPlayerCharacter && !this.OwningCharacter.Charmed)
                    {
                        this.onContact(targetCollider);
                    }
                }
                else if ((targetCollider.tag == Tags.DUNGEON_BOOST) && this.OwningCharacter.IsPlayerCharacter)
                {
                    this.onContact(targetCollider);
                }
            }
        }

        public void spawn(CharacterInstance owner, Vector3 spawnWorldPt, Vector3 direction, ProjectileProperties props)
        {
            this.initialize(owner, spawnWorldPt, props);
            this.Rigidbody.velocity = (Vector3) (direction * this.Properties.Speed);
        }

        public void spawnUsingCurve(CharacterInstance owner, Vector3 spawnWorldPt, Vector3 targetWorldPt, float curveHeight, ProjectileProperties props)
        {
            if (curveHeight == 0f)
            {
                Vector3 vector = targetWorldPt - spawnWorldPt;
                this.spawn(owner, spawnWorldPt, vector.normalized, props);
            }
            else
            {
                this.initialize(owner, spawnWorldPt, props);
                this.Rigidbody.isKinematic = true;
                this.m_curveControlPoint = Vector3.Lerp(spawnWorldPt, targetWorldPt, 0.5f);
                this.m_curveControlPoint.y = Mathf.Max(spawnWorldPt.y, targetWorldPt.y) + curveHeight;
                this.m_curveTravelDuration = Vector3.Distance(spawnWorldPt, targetWorldPt) / props.Speed;
                this.m_spawnWorldPt = spawnWorldPt;
                this.m_targetWorldPt = targetWorldPt;
                this.m_usesCurve = true;
            }
        }

        public int KillCounter
        {
            [CompilerGenerated]
            get
            {
                return this.<KillCounter>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<KillCounter>k__BackingField = value;
            }
        }

        public CharacterInstance OwningCharacter
        {
            [CompilerGenerated]
            get
            {
                return this.<OwningCharacter>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<OwningCharacter>k__BackingField = value;
            }
        }

        public ProjectileProperties Properties
        {
            [CompilerGenerated]
            get
            {
                return this.<Properties>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Properties>k__BackingField = value;
            }
        }

        public UnityEngine.Rigidbody Rigidbody
        {
            [CompilerGenerated]
            get
            {
                return this.<Rigidbody>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Rigidbody>k__BackingField = value;
            }
        }

        public UnityEngine.SphereCollider SphereCollider
        {
            [CompilerGenerated]
            get
            {
                return this.<SphereCollider>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SphereCollider>k__BackingField = value;
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

        [StructLayout(LayoutKind.Sequential)]
        public struct ProjectileProperties
        {
            public ProjectileType Type;
            public float Radius;
            public float Speed;
            public double DamagePerHit;
            public bool IsCriticalHit;
            public float StunDurationSeconds;
            public float KnockbackAndSlowDurationSeconds;
            public float MaxLifetime;
            public bool DestroyAfterContact;
            public bool RequireGroundTouch;
            public bool UsesGravity;
            public float GravityFactor;
            public bool CollideWithGround;
            public GameLogic.DamageType DamageType;
            public SkillType FromSkill;
            public float CharmDurationSeconds;
            public float ConfusionDurationSeconds;
            public float StartSizeMultiplier;
            public ProjectileProperties(Projectile.ProjectileProperties another)
            {
                this.Type = another.Type;
                this.Radius = another.Radius;
                this.Speed = another.Speed;
                this.DamagePerHit = another.DamagePerHit;
                this.IsCriticalHit = another.IsCriticalHit;
                this.StunDurationSeconds = another.StunDurationSeconds;
                this.KnockbackAndSlowDurationSeconds = another.KnockbackAndSlowDurationSeconds;
                this.MaxLifetime = another.MaxLifetime;
                this.DestroyAfterContact = another.DestroyAfterContact;
                this.RequireGroundTouch = another.RequireGroundTouch;
                this.UsesGravity = another.UsesGravity;
                this.GravityFactor = another.GravityFactor;
                this.CollideWithGround = another.CollideWithGround;
                this.DamageType = another.DamageType;
                this.FromSkill = another.FromSkill;
                this.CharmDurationSeconds = another.CharmDurationSeconds;
                this.ConfusionDurationSeconds = another.ConfusionDurationSeconds;
                this.StartSizeMultiplier = another.StartSizeMultiplier;
            }
        }
    }
}

