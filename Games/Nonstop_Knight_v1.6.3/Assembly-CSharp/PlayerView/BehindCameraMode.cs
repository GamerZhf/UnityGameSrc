namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BehindCameraMode : MonoBehaviour, ICameraMode
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
                if (!this.TargetCharacter.isExecutingSkill(SkillType.Whirlwind))
                {
                    this.m_facingMovingAverage = (Vector3) ((this.TargetCharacter.PhysicsBody.Transform.forward * ConfigCamera.CAMERA_BEHIND_SENSITIVITY) + (this.m_facingMovingAverage * (1f - ConfigCamera.CAMERA_BEHIND_SENSITIVITY)));
                }
                Vector3 to = (Vector3) ((this.TargetCharacter.PhysicsBody.Transform.position + (Vector3.up * ConfigCamera.CAMERA_BEHIND_OFFSET_Y)) - (this.m_facingMovingAverage.normalized * ConfigCamera.CAMERA_BEHIND_OFFSET_Z));
                this.Tm.position = Vector3.Lerp(this.Tm.position, to, ConfigCamera.CAMERA_BEHIND_FOLLOW_SPEED * dt);
                Vector3 forward = this.TargetCharacter.PhysicsBody.Transform.position - base.transform.position;
                Quaternion quaternion = Quaternion.LookRotation(forward, Vector3.up);
                base.transform.rotation = quaternion;
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

