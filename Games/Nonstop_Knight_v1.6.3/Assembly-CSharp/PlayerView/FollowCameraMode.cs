namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class FollowCameraMode : MonoBehaviour, ICameraMode
    {
        [CompilerGenerated]
        private PlayerView.RoomCamera <RoomCamera>k__BackingField;
        [CompilerGenerated]
        private CharacterInstance <TargetCharacter>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public bool AutomaticUpdate = true;
        public float FollowSpeed = 2f;
        public bool LookAtEnabled = true;
        private Vector3 m_newPos;
        private float m_relCameraPosMag;
        public bool ViewingPointCheckEnabled = true;

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
            this.m_relCameraPosMag = Mathf.Clamp(this.RoomCamera.Offset.magnitude - 0.5f, 0f, float.MaxValue);
        }

        private void smoothLookAt(float dt)
        {
            Vector3 forward = this.TargetCharacter.PhysicsBody.Transform.position - this.Tm.position;
            Quaternion to = Quaternion.LookRotation(forward, Vector3.up);
            this.Tm.rotation = Quaternion.Lerp(this.Tm.rotation, to, this.FollowSpeed * dt);
        }

        public void update(float dt)
        {
            if ((this.TargetCharacter != null) && this.TargetCharacter.PhysicsBody.CharacterController.enabled)
            {
                Vector3 from = this.TargetCharacter.PhysicsBody.Transform.position + this.RoomCamera.Offset;
                Vector3 to = this.TargetCharacter.PhysicsBody.Transform.position + ((Vector3) (Vector3.up * this.m_relCameraPosMag));
                if (this.ViewingPointCheckEnabled)
                {
                    Vector3[] vectorArray = new Vector3[] { from, Vector3.Lerp(from, to, 0.25f), Vector3.Lerp(from, to, 0.5f), Vector3.Lerp(from, to, 0.75f), to };
                    for (int i = 0; i < vectorArray.Length; i++)
                    {
                        if (this.viewingPosCheck(vectorArray[i]))
                        {
                            break;
                        }
                    }
                }
                else
                {
                    this.m_newPos = from;
                }
                this.Tm.position = Vector3.Lerp(this.Tm.position, this.m_newPos, this.FollowSpeed * dt);
                if (this.LookAtEnabled)
                {
                    this.smoothLookAt(dt);
                }
            }
        }

        private bool viewingPosCheck(Vector3 checkPos)
        {
            RaycastHit hit;
            if (Physics.Raycast(checkPos, this.TargetCharacter.PhysicsBody.Transform.position - checkPos, out hit, this.m_relCameraPosMag) && (hit.transform != this.TargetCharacter.PhysicsBody.Transform))
            {
                return false;
            }
            this.m_newPos = checkPos;
            return true;
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

