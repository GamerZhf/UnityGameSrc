namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PredictiveCameraMode : MonoBehaviour, ICameraMode
    {
        [CompilerGenerated]
        private PlayerView.RoomCamera <RoomCamera>k__BackingField;
        [CompilerGenerated]
        private CharacterInstance <TargetCharacter>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public bool AutomaticUpdate = true;
        private Vector3 m_facingMovingAverage;

        protected void Awake()
        {
            this.Tm = base.transform;
        }

        public CharacterInstance getTarget()
        {
            return this.TargetCharacter;
        }

        public void initialize(PlayerView.RoomCamera roomCamera)
        {
            this.RoomCamera = roomCamera;
        }

        protected void LateUpdate()
        {
            if (this.AutomaticUpdate)
            {
                this.update(Time.deltaTime);
            }
        }

        public void setTarget(CharacterInstance targetCharacter)
        {
            this.TargetCharacter = targetCharacter;
        }

        public void update(float dt)
        {
            if ((this.TargetCharacter != null) && this.TargetCharacter.PhysicsBody.CharacterController.enabled)
            {
                Vector3 vector = this.TargetCharacter.PhysicsBody.Transform.position + this.RoomCamera.Offset;
                if (!this.TargetCharacter.isExecutingSkill(SkillType.Whirlwind))
                {
                    this.m_facingMovingAverage = (Vector3) ((this.TargetCharacter.Facing * ConfigCamera.CAMERA_PREDICTIVE_SENSITIVITY) + (this.m_facingMovingAverage * (1f - ConfigCamera.CAMERA_PREDICTIVE_SENSITIVITY)));
                }
                Vector3 to = vector + ((Vector3) (this.m_facingMovingAverage.normalized * ConfigCamera.CAMERA_PREDICTIVE_DISTANCE_FACTOR));
                float num = 0f;
                if (this.TargetCharacter.Velocity.magnitude > 2f)
                {
                    num = ConfigCamera.CAMERA_PREDICTIVE_RUN_SMOOTHING;
                }
                else
                {
                    num = ConfigCamera.CAMERA_PREDICTIVE_STATIONARY_SMOOTHING;
                }
                this.Tm.position = Vector3.Lerp(this.Tm.position, to, num * dt);
            }
        }

        public PlayerView.RoomCamera RoomCamera
        {
            [CompilerGenerated]
            get
            {
                return this.<RoomCamera>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RoomCamera>k__BackingField = value;
            }
        }

        public CharacterInstance TargetCharacter
        {
            [CompilerGenerated]
            get
            {
                return this.<TargetCharacter>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TargetCharacter>k__BackingField = value;
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
    }
}

