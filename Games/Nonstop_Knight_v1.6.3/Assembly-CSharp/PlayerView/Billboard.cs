namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ExecuteInEditMode]
    public class Billboard : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        public UnityEngine.Camera Camera;
        public bool ManualUpdate;
        public bool Reversed;

        protected void Awake()
        {
            this.Transform = base.transform;
        }

        protected void LateUpdate()
        {
            if (!this.ManualUpdate)
            {
                this.update(Time.deltaTime);
            }
        }

        public void update(float dt)
        {
            if (this.Camera == null)
            {
                this.Camera = UnityEngine.Camera.main;
            }
            if (this.Camera != null)
            {
                UnityEngine.Transform transform = this.Camera.transform;
                if (this.Reversed)
                {
                    Vector3 vector = transform.position - this.Transform.position;
                    this.Transform.LookAt(this.Transform.position - vector, transform.up);
                }
                else
                {
                    this.Transform.LookAt(transform.position);
                }
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
    }
}

