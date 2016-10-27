namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_unity_reference_helper.php"), ExecuteInEditMode]
    public class UnityReferenceHelper : MonoBehaviour
    {
        [HideInInspector, SerializeField]
        private string guid;

        public void Awake()
        {
            this.Reset();
        }

        public string GetGUID()
        {
            return this.guid;
        }

        public void Reset()
        {
            if (string.IsNullOrEmpty(this.guid))
            {
                this.guid = Pathfinding.Util.Guid.NewGuid().ToString();
                Debug.Log("Created new GUID - " + this.guid);
            }
            else
            {
                foreach (UnityReferenceHelper helper in UnityEngine.Object.FindObjectsOfType(typeof(UnityReferenceHelper)) as UnityReferenceHelper[])
                {
                    if ((helper != this) && (this.guid == helper.guid))
                    {
                        this.guid = Pathfinding.Util.Guid.NewGuid().ToString();
                        Debug.Log("Created new GUID - " + this.guid);
                        return;
                    }
                }
            }
        }
    }
}

