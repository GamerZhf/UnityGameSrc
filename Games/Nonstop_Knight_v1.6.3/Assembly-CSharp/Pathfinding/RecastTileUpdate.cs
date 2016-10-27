namespace Pathfinding
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_recast_tile_update.php"), AddComponentMenu("Pathfinding/Navmesh/RecastTileUpdate")]
    public class RecastTileUpdate : MonoBehaviour
    {
        public static  event Action<Bounds> OnNeedUpdates;

        private void OnDestroy()
        {
            this.ScheduleUpdate();
        }

        public void ScheduleUpdate()
        {
            Collider component = base.GetComponent<Collider>();
            if (component != null)
            {
                if (OnNeedUpdates != null)
                {
                    OnNeedUpdates(component.bounds);
                }
            }
            else if (OnNeedUpdates != null)
            {
                OnNeedUpdates(new Bounds(base.transform.position, Vector3.zero));
            }
        }

        private void Start()
        {
            this.ScheduleUpdate();
        }
    }
}

