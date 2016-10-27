namespace PlayerView
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ExecuteInEditMode]
    public class ParallaxLayer : MonoBehaviour
    {
        [CompilerGenerated]
        private Renderer[] <Renderers>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public float ParallaxFactorX;
        public float ParallaxFactorY;
        public bool RotationFromGrandParent = true;

        public void initialize()
        {
            this.Tm = base.transform;
            this.Tm.localPosition = new Vector3(0f, 0f, this.Tm.localPosition.z);
            this.Renderers = base.GetComponentsInChildren<Renderer>(false);
        }

        public void move(Vector3 delta)
        {
            Vector3 localPosition = this.Tm.localPosition;
            if (this.RotationFromGrandParent)
            {
                Vector3 vector2 = (Vector3) (Quaternion.Euler(0f, -this.Tm.parent.parent.localEulerAngles.y, 0f) * delta.normalized);
                delta = (Vector3) (vector2 * delta.magnitude);
            }
            localPosition.x += delta.x * this.ParallaxFactorX;
            localPosition.y += delta.z * this.ParallaxFactorY;
            this.Tm.localPosition = localPosition;
        }

        public void setMaterialColor(Color materialColor)
        {
            if (this.Renderers != null)
            {
                for (int i = 0; i < this.Renderers.Length; i++)
                {
                    Material material = Binder.MaterialStorage.getSharedGenericMaterial(this.Renderers[i].sharedMaterial);
                    material.color = Color.Lerp(Color.white, (Color) (materialColor * (1f + ((this.Tm.GetSiblingIndex() + 1) * 0.5f))), 0.8f);
                    this.Renderers[i].sharedMaterial = material;
                }
            }
        }

        public Renderer[] Renderers
        {
            [CompilerGenerated]
            get
            {
                return this.<Renderers>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Renderers>k__BackingField = value;
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

