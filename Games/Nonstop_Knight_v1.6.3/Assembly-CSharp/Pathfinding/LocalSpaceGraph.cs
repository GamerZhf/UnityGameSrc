namespace Pathfinding
{
    using System;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_local_space_graph.php")]
    public class LocalSpaceGraph : MonoBehaviour
    {
        protected Matrix4x4 originalMatrix;

        public Matrix4x4 GetMatrix()
        {
            return (base.transform.worldToLocalMatrix * this.originalMatrix);
        }

        private void Start()
        {
            this.originalMatrix = base.transform.localToWorldMatrix;
        }
    }
}

