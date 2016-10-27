namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DungeonBoostView : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private Renderer[] <Renderers>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        [CompilerGenerated]
        private TransformAnimation <TransformAnimation>k__BackingField;
        [CompilerGenerated]
        private DungeonBoostType <Type>k__BackingField;
        private Quaternion m_originalLocalRotation;
        public DungeonBoostPrefabType PrefabType;

        protected void Awake()
        {
            this.Transform = base.transform;
            this.TransformAnimation = base.gameObject.AddComponent<TransformAnimation>();
            this.Renderers = base.GetComponentsInChildren<Renderer>(false);
            this.m_originalLocalRotation = this.Transform.localRotation;
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(DungeonBoost dungeonBoost)
        {
            object[] objArray1 = new object[] { "DungeonBoost_", this.Type, "_", this.PrefabType };
            base.name = string.Concat(objArray1);
            this.Transform.position = dungeonBoost.Transform.position;
            this.Transform.localRotation = Quaternion.Euler(0f, UnityEngine.Random.Range((float) 0f, (float) 360f), 0f) * this.m_originalLocalRotation;
        }

        public bool isOnscreen()
        {
            for (int i = 0; i < this.Renderers.Length; i++)
            {
                if (this.Renderers[i].isVisible)
                {
                    return true;
                }
            }
            return false;
        }

        public void scale(float targetScale, float duration)
        {
            Vector3 target = (Vector3) (Vector3.one * targetScale);
            if (this.Transform.localScale != target)
            {
                if (duration > 0f)
                {
                    TransformAnimationTask animationTask = new TransformAnimationTask(this.Transform, duration, 0f, TimeUtil.DeltaTimeType.UNSCALED_DELTA_TIME);
                    animationTask.scale(target, true, Easing.Function.OUT_BACK);
                    this.TransformAnimation.addTask(animationTask);
                }
                else
                {
                    this.Transform.localScale = target;
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

        public TransformAnimation TransformAnimation
        {
            [CompilerGenerated]
            get
            {
                return this.<TransformAnimation>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<TransformAnimation>k__BackingField = value;
            }
        }

        public DungeonBoostType Type
        {
            [CompilerGenerated]
            get
            {
                return this.<Type>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Type>k__BackingField = value;
            }
        }
    }
}

