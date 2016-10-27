namespace PlayerView
{
    using App;
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class Ultimate1CameraMode : MonoBehaviour, ICameraMode
    {
        [CompilerGenerated]
        private PlayerView.RoomCamera <RoomCamera>k__BackingField;
        [CompilerGenerated]
        private CharacterInstance <TargetCharacter>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public bool AutomaticUpdate = true;
        private Vector3 m_facingMovingAverage;
        private CharacterView m_targetCharacterView;
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
            this.m_targetCharacterView = PlayerView.Binder.RoomView.getCharacterViewForCharacter(this.TargetCharacter);
            this.m_velocityMovingAverage = Vector3.zero;
            this.m_facingMovingAverage = Vector3.zero;
        }

        public void update(float dt)
        {
            if (((this.TargetCharacter != null) && this.TargetCharacter.PhysicsBody.CharacterController.enabled) && (this.m_targetCharacterView != null))
            {
                Vector3 point = Vector3Extensions.ToXzVector3(this.m_targetCharacterView.Transform.position) + this.RoomCamera.Offset;
                if (ConfigApp.CHEAT_MARKETING_MODE_ENABLED)
                {
                    Vector3 vector5 = this.m_targetCharacterView.Transform.position - point;
                    Vector3 normalized = vector5.normalized;
                    point += (Vector3) (normalized * PlayerView.Binder.MarketingBuildController.CameraZooming);
                    point = MathUtil.RotatePointAroundPivot(point, this.m_targetCharacterView.Transform.position, new Vector3(0f, PlayerView.Binder.MarketingBuildController.CameraRotation, 0f));
                    Vector3 vector6 = this.m_targetCharacterView.Transform.position - point;
                    Quaternion quaternion = Quaternion.LookRotation(vector6.normalized);
                    this.Tm.rotation = Quaternion.Euler(this.Tm.rotation.eulerAngles.x, quaternion.eulerAngles.y, this.Tm.rotation.eulerAngles.z);
                }
                float num = 0.5f;
                this.m_velocityMovingAverage = (Vector3) ((this.TargetCharacter.Velocity * num) + (this.m_velocityMovingAverage * (1f - num)));
                float magnitude = this.m_velocityMovingAverage.magnitude;
                bool flag = magnitude > 1f;
                if (!this.TargetCharacter.isExecutingSkill(SkillType.Whirlwind))
                {
                    float num3 = 0f;
                    if (flag)
                    {
                        num3 = ConfigCamera.CAMERA_ULTIMATE1_RUNNING_SENSITIVITY;
                    }
                    else
                    {
                        num3 = ConfigCamera.CAMERA_ULTIMATE1_STATIONARY_SENSITIVITY;
                    }
                    this.m_facingMovingAverage = (Vector3) ((this.m_targetCharacterView.Transform.forward * num3) + (this.m_facingMovingAverage * (1f - num3)));
                }
                this.Tm.position = Vector3.Lerp(this.Tm.position, point + ((Vector3) (this.m_facingMovingAverage * ConfigCamera.CAMERA_ULTIMATE1_DISTANCE_FACTOR)), ConfigCamera.CAMERA_ULTIMATE1_FOLLOW_SPEED * dt);
                if (!this.RoomCamera.FovAnimator.IsAnimating)
                {
                    float to = Mathf.Clamp(this.RoomCamera.FovAnimator.DefaultFov + ((magnitude * 10f) * dt), this.RoomCamera.FovAnimator.DefaultFov, this.RoomCamera.FovAnimator.DefaultFov + 3f);
                    float num5 = !flag ? 0.5f : 1f;
                    this.RoomCamera.Camera.fieldOfView = Mathf.Lerp(this.RoomCamera.Camera.fieldOfView, to, num5 * dt);
                }
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

