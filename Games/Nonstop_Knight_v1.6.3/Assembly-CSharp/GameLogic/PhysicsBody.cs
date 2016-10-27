namespace GameLogic
{
    using App;
    using Pathfinding;
    using PlayerView;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class PhysicsBody : MonoBehaviour
    {
        [CompilerGenerated]
        private CharacterInstance <AttachedCharacter>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.CharacterController <CharacterController>k__BackingField;
        [CompilerGenerated]
        private GameLogic.PathPlanner <PathPlanner>k__BackingField;
        [CompilerGenerated]
        private Seeker <Seeker>k__BackingField;
        [CompilerGenerated]
        private SimpleSmoothModifier <Ssm>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        private float m_nextOutOfBoundsCheck;
        private const float m_outOfBoundsCheckFrequency = 0.5f;

        protected void Awake()
        {
            this.Transform = base.transform;
            this.CharacterController = base.gameObject.AddComponent<UnityEngine.CharacterController>();
            this.Seeker = base.gameObject.AddComponent<Seeker>();
            this.Ssm = base.gameObject.AddComponent<SimpleSmoothModifier>();
            this.Ssm.smoothType = SimpleSmoothModifier.SmoothType.Simple;
            this.Ssm.uniformLength = false;
            this.Ssm.subdivisions = 1;
            this.Ssm.iterations = 8;
            this.Ssm.strength = 0.5f;
            this.PathPlanner = base.gameObject.AddComponent<GameLogic.PathPlanner>();
            this.PathPlanner.initialize(this);
        }

        protected void FixedUpdate()
        {
            if (this.CharacterController.enabled)
            {
                if (this.AttachedCharacter.Velocity == Vector3.zero)
                {
                    this.AttachedCharacter.RunAccelerationTimer.reset();
                }
                this.AttachedCharacter.RunAccelerationTimer.tick(Time.fixedDeltaTime * Time.timeScale);
                if (ConfigGameplay.OUT_OF_BOUNDS_CHECKING_ENABLED && (Time.fixedTime >= this.m_nextOutOfBoundsCheck))
                {
                    ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
                    if (((activeDungeon != null) && (activeDungeon.ActiveRoom != null)) && (activeDungeon.CurrentGameplayState == GameplayState.ACTION))
                    {
                        Room activeRoom = activeDungeon.ActiveRoom;
                        if (!PhysicsUtil.IsOnSurface(this.Transform.position + ((Vector3) (Vector3.up * 0.5f)), float.MaxValue, Layers.GroundLayerMask))
                        {
                            Vector3 worldPt = activeRoom.adjustToNearestGridPoint(this.Transform.position);
                            Vector3 vector2 = activeRoom.calculateNearestEmptySpot(worldPt, Vector3.zero, 1f, 1f, 6f, null);
                            this.Transform.position = vector2;
                        }
                        this.m_nextOutOfBoundsCheck = Time.fixedTime + (0.5f * UnityEngine.Random.Range((float) 1f, (float) 2f));
                    }
                }
            }
        }

        [DebuggerHidden]
        public IEnumerator leapBackRoutine([Optional, DefaultParameterValue(0.1f)] float postLeapWaitTime)
        {
            <leapBackRoutine>c__Iterator75 iterator = new <leapBackRoutine>c__Iterator75();
            iterator.postLeapWaitTime = postLeapWaitTime;
            iterator.<$>postLeapWaitTime = postLeapWaitTime;
            iterator.<>f__this = this;
            return iterator;
        }

        public void onInstantiate(CharacterInstance attachedToCharacter)
        {
            this.AttachedCharacter = attachedToCharacter;
        }

        public bool rotatedEnoughForMovement()
        {
            Quaternion b = Quaternion.LookRotation(this.AttachedCharacter.Facing, Vector3.up);
            return (Mathf.Abs(Quaternion.Dot(this.Transform.rotation, b)) >= ConfigGameplay.ROTATION_STEERING_THRESHOLD);
        }

        [DebuggerHidden]
        public IEnumerator spinAroundRoutine(int totalSpinCount, float totalDuration, Easing.Function easing)
        {
            <spinAroundRoutine>c__Iterator76 iterator = new <spinAroundRoutine>c__Iterator76();
            iterator.totalSpinCount = totalSpinCount;
            iterator.totalDuration = totalDuration;
            iterator.easing = easing;
            iterator.<$>totalSpinCount = totalSpinCount;
            iterator.<$>totalDuration = totalDuration;
            iterator.<$>easing = easing;
            iterator.<>f__this = this;
            return iterator;
        }

        protected void Update()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            if (this.CharacterController.enabled)
            {
                if (this.AttachedCharacter.ExternallyControlled)
                {
                    this.Transform.rotation = QuaternionExtensions.RotateTowardsPoint(this.Transform.rotation, this.Transform.position, this.Transform.position + this.AttachedCharacter.Facing, 1f);
                }
                else
                {
                    this.Transform.rotation = QuaternionExtensions.RotateTowardsPoint(this.Transform.rotation, this.Transform.position, this.Transform.position + this.AttachedCharacter.Facing, Time.deltaTime * this.AttachedCharacter.Character.RotationSpeed);
                }
                if ((this.AttachedCharacter.Velocity != Vector3.zero) && this.rotatedEnoughForMovement())
                {
                    Vector3 motion = (Vector3) (this.AttachedCharacter.Velocity * Time.deltaTime);
                    if ((!this.AttachedCharacter.ExternallyControlled && this.AttachedCharacter.IsPlayerCharacter) || PhysicsUtil.IsOnSurface((this.Transform.position + ((Vector3) (Vector3.up * 0.5f))) + motion, float.MaxValue, Layers.GroundLayerMask))
                    {
                        this.CharacterController.Move(motion);
                        if ((activeDungeon != null) && (activeDungeon.ActiveRoom != null))
                        {
                            this.Transform.position = new Vector3(this.Transform.position.x, activeDungeon.ActiveRoom.WorldGroundPosY, this.Transform.position.z);
                        }
                    }
                }
            }
        }

        public CharacterInstance AttachedCharacter
        {
            [CompilerGenerated]
            get
            {
                return this.<AttachedCharacter>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<AttachedCharacter>k__BackingField = value;
            }
        }

        public UnityEngine.CharacterController CharacterController
        {
            [CompilerGenerated]
            get
            {
                return this.<CharacterController>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CharacterController>k__BackingField = value;
            }
        }

        public GameLogic.PathPlanner PathPlanner
        {
            [CompilerGenerated]
            get
            {
                return this.<PathPlanner>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<PathPlanner>k__BackingField = value;
            }
        }

        public Seeker Seeker
        {
            [CompilerGenerated]
            get
            {
                return this.<Seeker>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Seeker>k__BackingField = value;
            }
        }

        public SimpleSmoothModifier Ssm
        {
            [CompilerGenerated]
            get
            {
                return this.<Ssm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Ssm>k__BackingField = value;
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

        [CompilerGenerated]
        private sealed class <leapBackRoutine>c__Iterator75 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <$>postLeapWaitTime;
            internal PhysicsBody <>f__this;
            internal CharacterInstance <c>__0;
            internal CharacterView <cv>__1;
            internal Vector3 <dashDir>__2;
            internal double <hpBeforeWait>__6;
            internal IEnumerator <ie>__7;
            internal int <originalPhysicsLayer>__3;
            internal Vector3 <vel>__4;
            internal float <velSqrMag>__5;
            internal float postLeapWaitTime;

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
                        this.<c>__0 = this.<>f__this.AttachedCharacter;
                        this.<cv>__1 = PlayerView.Binder.RoomView.getCharacterViewForCharacter(this.<c>__0);
                        this.<cv>__1.leap(0.25f, 1.4f);
                        this.<dashDir>__2 = -this.<c>__0.PhysicsBody.Transform.forward;
                        this.<originalPhysicsLayer>__3 = this.<c>__0.PhysicsBody.gameObject.layer;
                        this.<vel>__4 = (Vector3) (this.<dashDir>__2 * 35f);
                        this.<velSqrMag>__5 = this.<vel>__4.sqrMagnitude;
                        break;

                    case 1:
                        break;

                    case 2:
                        goto Label_01EF;

                    default:
                        goto Label_020B;
                }
                if (this.<velSqrMag>__5 > 25f)
                {
                    this.<c>__0.PhysicsBody.gameObject.layer = Layers.IGNORE_CHARACTERS;
                    this.<velSqrMag>__5 = this.<vel>__4.sqrMagnitude;
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.<c>__0, this.<vel>__4, this.<c>__0.Facing);
                    PhysicsUtil.ApplyDrag(ref this.<vel>__4, 5f, Time.fixedDeltaTime);
                    this.$current = new WaitForFixedUpdate();
                    this.$PC = 1;
                    goto Label_020D;
                }
                this.<c>__0.PhysicsBody.gameObject.layer = this.<originalPhysicsLayer>__3;
                CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.<c>__0, Vector3.zero, this.<c>__0.Facing);
                if (this.postLeapWaitTime <= 0f)
                {
                    goto Label_020B;
                }
                this.<hpBeforeWait>__6 = this.<c>__0.CurrentHp;
                this.<ie>__7 = TimeUtil.WaitForFixedSeconds(0.1f);
            Label_01EF:
                while (this.<ie>__7.MoveNext())
                {
                    if (this.<c>__0.CurrentHp < this.<hpBeforeWait>__6)
                    {
                        break;
                    }
                    this.$current = this.<ie>__7.Current;
                    this.$PC = 2;
                    goto Label_020D;
                }
                goto Label_020B;
                this.$PC = -1;
            Label_020B:
                return false;
            Label_020D:
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
        private sealed class <spinAroundRoutine>c__Iterator76 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Easing.Function <$>easing;
            internal float <$>totalDuration;
            internal int <$>totalSpinCount;
            internal PhysicsBody <>f__this;
            internal float <angle>__7;
            internal CharacterInstance <c>__0;
            internal float <easedV>__6;
            internal int <loopCount>__5;
            internal ManualTimer <spinTimer>__3;
            internal Vector3 <startForward>__4;
            internal float <targetTotalRotation>__1;
            internal ManualTimer <totalTimer>__2;
            internal Easing.Function easing;
            internal float totalDuration;
            internal int totalSpinCount;

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
                        this.<c>__0 = this.<>f__this.AttachedCharacter;
                        this.<c>__0.SpinningAround = true;
                        this.<targetTotalRotation>__1 = 360f * this.totalSpinCount;
                        this.<totalTimer>__2 = new ManualTimer(this.totalDuration);
                        this.<spinTimer>__3 = new ManualTimer(this.totalDuration / ((float) this.totalSpinCount));
                        this.<startForward>__4 = this.<c>__0.PhysicsBody.Transform.forward;
                        this.<loopCount>__5 = 0;
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_019E;
                }
                if (!this.<totalTimer>__2.Idle)
                {
                    this.<easedV>__6 = Easing.Apply(this.<totalTimer>__2.normalizedProgress(), this.easing);
                    this.<angle>__7 = this.<targetTotalRotation>__1 * this.<easedV>__6;
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(this.<c>__0, this.<c>__0.Velocity, (Vector3) (Quaternion.Euler(0f, this.<angle>__7, 0f) * this.<startForward>__4));
                    this.<totalTimer>__2.tick(Time.deltaTime);
                    this.<spinTimer>__3.tick(Time.deltaTime);
                    if (this.<spinTimer>__3.Idle && (this.<loopCount>__5 < this.totalSpinCount))
                    {
                        this.<loopCount>__5++;
                        this.<spinTimer>__3.reset();
                    }
                    this.$current = null;
                    this.$PC = 1;
                    return true;
                }
                this.<c>__0.SpinningAround = false;
                goto Label_019E;
                this.$PC = -1;
            Label_019E:
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

        public static class LeapBackParameters
        {
            public const float DragPerSecond = 5f;
            public const float LeapDuration = 0.25f;
            public const float LeapTargetHeight = 1.4f;
            public const float MovementForce = 35f;
            public const float PostLeapWaitTime = 0.1f;
        }
    }
}

