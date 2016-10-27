namespace GameLogic
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [ExecuteInEditMode]
    public class RoomLayout : MonoBehaviour
    {
        [CompilerGenerated]
        private ObjectLayout <ObjectLayout>k__BackingField;
        public string Id;
        public bool LockCameraRotation;
        public bool RandomHeroSpawnpoint = true;

        protected void Awake()
        {
            this.OnValidate();
        }

        public void clear()
        {
            this.ObjectLayout.clear();
        }

        public void load()
        {
            this.ObjectLayout.Id = this.Id;
            RoomLayoutData data = JsonUtils.Deserialize<RoomLayoutData>(ResourceUtil.LoadSafe<TextAsset>("RoomLayouts/" + this.Id, false).text, true);
            this.ObjectLayout.load(data.ObjectLayout);
            this.LockCameraRotation = data.LockCameraRotation;
            this.RandomHeroSpawnpoint = data.RandomHeroSpawnpoint;
        }

        protected void OnValidate()
        {
            this.ObjectLayout = base.gameObject.GetComponent<ObjectLayout>();
            if (this.ObjectLayout == null)
            {
                this.ObjectLayout = base.gameObject.AddComponent<ObjectLayout>();
            }
            this.ObjectLayout.hideFlags = HideFlags.HideInInspector;
            this.ObjectLayout.Id = this.Id;
        }

        public ObjectLayout ObjectLayout
        {
            [CompilerGenerated]
            get
            {
                return this.<ObjectLayout>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ObjectLayout>k__BackingField = value;
            }
        }

        private class RoomLayoutData
        {
            public bool LockCameraRotation;
            public ObjectLayout.Layout ObjectLayout = new ObjectLayout.Layout();
            public bool RandomHeroSpawnpoint = true;
        }
    }
}

