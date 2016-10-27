namespace GameLogic
{
    using App;
    using PlayerView;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class DungeonBoost : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private GameLogic.DestructiblePhysicsBody <DestructiblePhysicsBody>k__BackingField;
        [CompilerGenerated]
        private DungeonBoostPrefabType <PrefabType>k__BackingField;
        [CompilerGenerated]
        private DungeonBoostProperties <Properties>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.SphereCollider <SphereCollider>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        private bool m_destroyWhenOffscreen;

        private void activate([Optional, DefaultParameterValue(0)] SkillType fromSkill)
        {
            GameLogic.Binder.CommandProcessor.execute(new CmdActivateDungeonBoost(this, fromSkill), 0f);
        }

        protected void Awake()
        {
            this.Transform = base.transform;
            this.SphereCollider = base.gameObject.AddComponent<UnityEngine.SphereCollider>();
            this.SphereCollider.isTrigger = true;
            this.DestructiblePhysicsBody = base.gameObject.AddComponent<GameLogic.DestructiblePhysicsBody>();
            this.DestructiblePhysicsBody.Hit = new GameLogic.DestructiblePhysicsBody.OnHit(this.onDestructibleHit);
        }

        public void cleanUpForReuse()
        {
            this.PrefabType = DungeonBoostPrefabType.None;
            this.SphereCollider.enabled = false;
            this.m_destroyWhenOffscreen = false;
        }

        public void destroy()
        {
            GameLogic.Binder.EventBus.DungeonBoostPreDestroy(this);
            GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveDungeonBoosts.Remove(this);
            GameLogic.Binder.DungeonBoostPool.returnObject(this);
        }

        public void flagForOffscreenDestroy()
        {
            this.m_destroyWhenOffscreen = true;
        }

        public void initialize(Vector3 spawnWorldPt, DungeonBoostProperties props)
        {
            base.tag = Tags.DUNGEON_BOOST;
            this.Transform.position = spawnWorldPt;
            this.Properties = props;
            this.SphereCollider.enabled = true;
            this.SphereCollider.radius = this.Properties.Radius;
            GameLogic.Binder.GameState.ActiveDungeon.ActiveRoom.ActiveDungeonBoosts.Add(this);
        }

        protected void onDestructibleHit(SkillType fromSkill)
        {
            if (this.Properties.ActivationType == DungeonBoostActivationType.DestructibleHit)
            {
                this.activate(fromSkill);
            }
        }

        protected void Update()
        {
            if (this.m_destroyWhenOffscreen && !PlayerView.Binder.RoomView.isDungeonBoostOnscreen(this))
            {
                this.destroy();
            }
        }

        public GameLogic.DestructiblePhysicsBody DestructiblePhysicsBody
        {
            [CompilerGenerated]
            get
            {
                return this.<DestructiblePhysicsBody>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<DestructiblePhysicsBody>k__BackingField = value;
            }
        }

        public DungeonBoostPrefabType PrefabType
        {
            [CompilerGenerated]
            get
            {
                return this.<PrefabType>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<PrefabType>k__BackingField = value;
            }
        }

        public DungeonBoostProperties Properties
        {
            [CompilerGenerated]
            get
            {
                return this.<Properties>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Properties>k__BackingField = value;
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
        public struct DungeonBoostProperties
        {
            public DungeonBoostActivationType ActivationType;
            public PerkType BuffPerkType;
            public bool DoDestroyOnActivation;
            public float Radius;
            public string ShopEntryId;
            public DungeonBoostType Type;
            public DungeonBoostProperties(DungeonBoost.DungeonBoostProperties another)
            {
                this.ActivationType = another.ActivationType;
                this.BuffPerkType = another.BuffPerkType;
                this.DoDestroyOnActivation = another.DoDestroyOnActivation;
                this.Radius = another.Radius;
                this.ShopEntryId = another.ShopEntryId;
                this.Type = another.Type;
            }
        }
    }
}

