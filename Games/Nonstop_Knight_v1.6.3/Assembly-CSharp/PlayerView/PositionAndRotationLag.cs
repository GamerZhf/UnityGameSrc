namespace PlayerView
{
    using System;
    using UnityEngine;

    public class PositionAndRotationLag : MonoBehaviour
    {
        private Vector3 m_localOffset;
        private Transform m_tm;
        public float Smooth = 5f;
        public Transform TargetTm;

        protected void Awake()
        {
            this.m_tm = base.transform;
            this.m_localOffset = base.transform.localPosition;
        }

        protected void Update()
        {
            this.m_tm.localRotation = Quaternion.Slerp(this.m_tm.localRotation, this.TargetTm.localRotation, this.Smooth * Time.deltaTime);
            this.m_tm.localPosition = Vector3.Slerp(this.m_tm.localPosition, this.TargetTm.localPosition + this.m_localOffset, this.Smooth * Time.deltaTime);
        }
    }
}

