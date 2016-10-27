namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class WinterSea : MonoBehaviour
    {
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        public UnityEngine.Transform ShaderTransform;

        protected void Awake()
        {
            this.Transform = base.transform;
        }

        protected void Update()
        {
            RoomView roomView = Binder.RoomView;
            if (roomView != null)
            {
                RoomCamera roomCamera = roomView.RoomCamera;
                this.Transform.position = new Vector3(roomCamera.Transform.position.x, this.Transform.position.y, roomCamera.Transform.position.z);
                this.Transform.localEulerAngles = new Vector3(this.Transform.localEulerAngles.x, roomCamera.Orientation, this.Transform.localEulerAngles.z);
                this.ShaderTransform.localEulerAngles = new Vector3(this.ShaderTransform.localEulerAngles.x, -roomCamera.Orientation, this.ShaderTransform.localEulerAngles.z);
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

