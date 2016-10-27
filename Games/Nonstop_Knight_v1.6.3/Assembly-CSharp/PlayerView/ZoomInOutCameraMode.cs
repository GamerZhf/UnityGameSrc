namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ZoomInOutCameraMode : MonoBehaviour, ICameraMode
    {
        [CompilerGenerated]
        private PlayerView.RoomCamera <RoomCamera>k__BackingField;
        [CompilerGenerated]
        private CharacterInstance <TargetCharacter>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public bool AutomaticUpdate = true;
        private float m_cumulativeMovement;
        private Vector3 m_facingMovingAverage;
        private Vector3 m_velocityMovingAverage;

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
            this.m_cumulativeMovement = 0f;
        }

        public void update(float dt)
        {
            if ((this.TargetCharacter != null) && this.TargetCharacter.PhysicsBody.CharacterController.enabled)
            {
                Vector3 to = this.TargetCharacter.PhysicsBody.Transform.position + this.RoomCamera.Offset;
                this.Tm.position = Vector3.Lerp(this.Tm.position, to, ConfigCamera.CAMERA_FOLLOW_SPEED * dt);
                float num = 0.5f;
                this.m_velocityMovingAverage = (Vector3) ((this.TargetCharacter.Velocity * num) + (this.m_velocityMovingAverage * (1f - num)));
                float magnitude = this.m_velocityMovingAverage.magnitude;
                if (magnitude > 1f)
                {
                    this.m_cumulativeMovement = Mathf.Clamp(this.m_cumulativeMovement + ((magnitude * ConfigCamera.CAMERA_ZOOM_OUT_SENSITIVITY) * dt), 0f, ConfigCamera.CAMERA_ZOOM_MAX_DIST);
                }
                else
                {
                    this.m_cumulativeMovement = Mathf.Clamp(this.m_cumulativeMovement * (1f - (ConfigCamera.CAMERA_ZOOM_IN_SENSITIVITY * dt)), 0f, float.MaxValue);
                }
                Vector3 vector2 = this.Tm.position + new Vector3(0f, this.m_cumulativeMovement, -this.m_cumulativeMovement);
                this.Tm.position = Vector3.Lerp(this.Tm.position, vector2, 0.1f * dt);
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

