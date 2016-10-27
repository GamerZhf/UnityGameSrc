namespace App
{
    using Service;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ServerSelectPopup : MonoBehaviour
    {
        [CompilerGenerated]
        private List<ServerSelectCell> <Cells>k__BackingField;
        [CompilerGenerated]
        private string <SelectedServerId>k__BackingField;
        public GameObject Cell;
        private const int CELL_POOL_SIZE = 10;
        public static ServerSelectPopup Root;
        public RectTransform VerticalGroup;

        protected void Awake()
        {
            Root = this;
            this.Cells = new List<ServerSelectCell>();
            this.Cells.Add(this.Cell.GetComponent<ServerSelectCell>());
            for (int i = 1; i < 10; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.Cell);
                obj2.transform.SetParent(this.VerticalGroup, false);
                this.Cells.Add(obj2.GetComponent<ServerSelectCell>());
            }
            for (int j = 0; j < 10; j++)
            {
                this.Cells[j].gameObject.SetActive(false);
            }
        }

        public void initialize(ServerEntry[] collection)
        {
            for (int i = 0; i < collection.Length; i++)
            {
                this.Cells[i].gameObject.SetActive(true);
                this.Cells[i].initialize(collection[i], (i % 2) != 0);
            }
            for (int j = collection.Length; j < 10; j++)
            {
                this.Cells[j].gameObject.SetActive(false);
            }
            this.SelectedServerId = null;
        }

        public void selectServer(string id)
        {
            this.SelectedServerId = id;
        }

        public List<ServerSelectCell> Cells
        {
            [CompilerGenerated]
            get
            {
                return this.<Cells>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Cells>k__BackingField = value;
            }
        }

        public string SelectedServerId
        {
            [CompilerGenerated]
            get
            {
                return this.<SelectedServerId>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SelectedServerId>k__BackingField = value;
            }
        }
    }
}

