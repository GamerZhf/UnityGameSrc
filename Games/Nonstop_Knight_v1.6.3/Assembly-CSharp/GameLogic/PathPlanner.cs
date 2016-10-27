namespace GameLogic
{
    using Pathfinding;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PathPlanner : MonoBehaviour
    {
        [CompilerGenerated]
        private GameLogic.PhysicsBody <PhysicsBody>k__BackingField;
        [NonSerialized]
        public Path ActivePath;
        [NonSerialized]
        public List<Vector3> ActiveVectorPath;
        [NonSerialized]
        public int ActiveWaypoint;
        private float m_nextRepath;
        [NonSerialized]
        public float MoveNextDist = 1.2f;

        private void assignNextRepathTime()
        {
            if ((this.PhysicsBody.AttachedCharacter != null) && this.PhysicsBody.AttachedCharacter.IsPlayerCharacter)
            {
                this.m_nextRepath = Time.fixedTime + UnityEngine.Random.Range((float) 0.1f, (float) 0.3f);
            }
            else
            {
                this.m_nextRepath = Time.fixedTime + UnityEngine.Random.Range((float) 1.25f, (float) 1.5f);
            }
        }

        protected void FixedUpdate()
        {
            if ((this.PhysicsBody.AttachedCharacter != null) && (Time.fixedTime >= this.m_nextRepath))
            {
                this.recalculatePath();
            }
        }

        public void initialize(GameLogic.PhysicsBody physicsBody)
        {
            this.PhysicsBody = physicsBody;
        }

        private void onCharacterSkillExecuted(CharacterInstance character, SkillType skillType, SkillExecutionStats executionStats)
        {
            if (character.IsBoss || (character == this.PhysicsBody.AttachedCharacter))
            {
                this.recalculatePath();
            }
        }

        private void onCharacterTargetUpdated(CharacterInstance character, CharacterInstance oldTarget)
        {
            if (character == this.PhysicsBody.AttachedCharacter)
            {
                this.recalculatePath();
            }
        }

        protected void OnDisable()
        {
            Binder.EventBus.OnCharacterTargetUpdated -= new Events.CharacterTargetUpdated(this.onCharacterTargetUpdated);
            Binder.EventBus.OnCharacterSkillExecuted -= new Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            Binder.EventBus.OnDungeonDecosRefreshed -= new Events.DungeonDecosRefreshed(this.onDungeonDecosRefreshed);
            Binder.EventBus.OnGameplayStarted -= new Events.GameplayStarted(this.onGameplayStarted);
            Binder.EventBus.OnGameplayEnded -= new Events.GameplayEnded(this.onGameplayEnded);
            this.releaseActivePath();
        }

        private void onDungeonDecosRefreshed()
        {
            if (this.PhysicsBody.AttachedCharacter.IsPlayerCharacter)
            {
                this.recalculatePath();
            }
        }

        protected void OnEnable()
        {
            Binder.EventBus.OnCharacterTargetUpdated += new Events.CharacterTargetUpdated(this.onCharacterTargetUpdated);
            Binder.EventBus.OnCharacterSkillExecuted += new Events.CharacterSkillExecuted(this.onCharacterSkillExecuted);
            Binder.EventBus.OnDungeonDecosRefreshed += new Events.DungeonDecosRefreshed(this.onDungeonDecosRefreshed);
            Binder.EventBus.OnGameplayStarted += new Events.GameplayStarted(this.onGameplayStarted);
            Binder.EventBus.OnGameplayEnded += new Events.GameplayEnded(this.onGameplayEnded);
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            this.releaseActivePath();
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            if (this.PhysicsBody.AttachedCharacter.IsPlayerCharacter)
            {
                this.releaseActivePath();
                this.recalculatePath();
            }
        }

        public void onPathComplete(Path _p)
        {
            ABPath path = _p as ABPath;
            this.releaseActivePath();
            if (!path.error)
            {
                this.ActivePath = path;
                path.Claim(this);
                Vector3 originalStartPoint = path.originalStartPoint;
                Vector3 position = this.PhysicsBody.Transform.position;
                originalStartPoint.y = position.y;
                Vector3 vector5 = position - originalStartPoint;
                float magnitude = vector5.magnitude;
                this.ActiveWaypoint = 0;
                this.ActiveVectorPath = path.vectorPath;
                for (float i = 0f; i <= magnitude; i += this.MoveNextDist * 0.6f)
                {
                    Vector3 vector6;
                    this.ActiveWaypoint--;
                    Vector3 vector4 = originalStartPoint + ((Vector3) ((position - originalStartPoint) * i));
                    do
                    {
                        this.ActiveWaypoint++;
                        Vector3 vector3 = this.ActiveVectorPath[this.ActiveWaypoint];
                        vector3.y = vector4.y;
                        vector6 = vector4 - vector3;
                    }
                    while ((vector6.sqrMagnitude < (this.MoveNextDist * this.MoveNextDist)) && (this.ActiveWaypoint != (this.ActiveVectorPath.Count - 1)));
                }
            }
        }

        private void recalculatePath()
        {
            if (this.PhysicsBody.AttachedCharacter.TargetCharacter != null)
            {
                this.recalculatePath(this.PhysicsBody.AttachedCharacter.TargetCharacter.PhysicsBody.Transform.position);
            }
            else if (this.PhysicsBody.AttachedCharacter.ManualTargetPos != Vector3.zero)
            {
                this.recalculatePath(this.PhysicsBody.AttachedCharacter.ManualTargetPos);
            }
        }

        public void recalculatePath(Vector3 targetWorldPt)
        {
            this.assignNextRepathTime();
            this.PhysicsBody.Seeker.StartPath(this.PhysicsBody.Transform.position, targetWorldPt, new OnPathDelegate(this.onPathComplete));
        }

        private void releaseActivePath()
        {
            if (this.ActivePath != null)
            {
                this.ActivePath.Release(this, false);
                this.ActivePath = null;
                this.ActiveWaypoint = 0;
                this.ActiveVectorPath = null;
            }
        }

        public GameLogic.PhysicsBody PhysicsBody
        {
            [CompilerGenerated]
            get
            {
                return this.<PhysicsBody>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PhysicsBody>k__BackingField = value;
            }
        }
    }
}

