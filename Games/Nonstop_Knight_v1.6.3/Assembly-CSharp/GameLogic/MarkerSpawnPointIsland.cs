namespace GameLogic
{
    using App;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MarkerSpawnPointIsland : GameLogic.AbstractMarker
    {
        [CompilerGenerated]
        private ObjectLayout <ObjectLayout>k__BackingField;

        protected void Awake()
        {
            this.ObjectLayout = base.GetComponent<ObjectLayout>();
            this.refreshObjectLayout();
            this.setFixedValues();
        }

        private void refreshObjectLayout()
        {
            if (this.ObjectLayout != null)
            {
                this.ObjectLayout.ResourcePath = ConfigDungeons.DUNGEON_ISLAND_RESOURCE_PATH;
            }
        }

        private void setFixedValues()
        {
            base.shape = GameLogic.AbstractMarker.Shape.SPHERE;
            base.color = Color.blue;
            base.customSize = true;
            base.customSphereRadius = 0.6f;
        }

        public ObjectLayout ObjectLayout
        {
            [CompilerGenerated]
            get
            {
                return this.<ObjectLayout>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ObjectLayout>k__BackingField = value;
            }
        }
    }
}

