﻿namespace Pathfinding
{
    using System;
    using UnityEngine;

    [AddComponentMenu("Pathfinding/Navmesh/RecastTileUpdateHandler"), HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_recast_tile_update_handler.php")]
    public class RecastTileUpdateHandler : MonoBehaviour
    {
        private bool anyDirtyTiles;
        private bool[] dirtyTiles;
        private float earliestDirty = float.NegativeInfinity;
        private RecastGraph graph;
        public float maxThrottlingDelay = 0.5f;

        private void OnDisable()
        {
            RecastTileUpdate.OnNeedUpdates -= new Action<Bounds>(this.ScheduleUpdate);
        }

        private void OnEnable()
        {
            RecastTileUpdate.OnNeedUpdates += new Action<Bounds>(this.ScheduleUpdate);
        }

        public void ScheduleUpdate(Bounds bounds)
        {
            if (this.graph == null)
            {
                if (AstarPath.active != null)
                {
                    this.SetGraph(AstarPath.active.astarData.recastGraph);
                }
                if (this.graph == null)
                {
                    Debug.LogError("Received tile update request (from RecastTileUpdate), but no RecastGraph could be found to handle it");
                    return;
                }
            }
            int num2 = Mathf.CeilToInt(this.graph.characterRadius / this.graph.cellSize) + 3;
            bounds.Expand((Vector3) ((new Vector3((float) num2, 0f, (float) num2) * this.graph.cellSize) * 2f));
            IntRect touchingTiles = this.graph.GetTouchingTiles(bounds);
            if ((touchingTiles.Width * touchingTiles.Height) > 0)
            {
                if (!this.anyDirtyTiles)
                {
                    this.earliestDirty = Time.time;
                    this.anyDirtyTiles = true;
                }
                for (int i = touchingTiles.ymin; i <= touchingTiles.ymax; i++)
                {
                    for (int j = touchingTiles.xmin; j <= touchingTiles.xmax; j++)
                    {
                        this.dirtyTiles[(i * this.graph.tileXCount) + j] = true;
                    }
                }
            }
        }

        public void SetGraph(RecastGraph graph)
        {
            this.graph = graph;
            if (graph != null)
            {
                this.dirtyTiles = new bool[graph.tileXCount * graph.tileZCount];
                this.anyDirtyTiles = false;
            }
        }

        private void Update()
        {
            if ((this.anyDirtyTiles && ((Time.time - this.earliestDirty) >= this.maxThrottlingDelay)) && (this.graph != null))
            {
                this.UpdateDirtyTiles();
            }
        }

        public void UpdateDirtyTiles()
        {
            if (this.graph == null)
            {
                new InvalidOperationException("No graph is set on this object");
            }
            if ((this.graph.tileXCount * this.graph.tileZCount) != this.dirtyTiles.Length)
            {
                Debug.LogError("Graph has changed dimensions. Clearing queued graph updates and resetting.");
                this.SetGraph(this.graph);
            }
            else
            {
                for (int i = 0; i < this.graph.tileZCount; i++)
                {
                    for (int j = 0; j < this.graph.tileXCount; j++)
                    {
                        if (this.dirtyTiles[(i * this.graph.tileXCount) + j])
                        {
                            this.dirtyTiles[(i * this.graph.tileXCount) + j] = false;
                            Bounds b = this.graph.GetTileBounds(j, i, 1, 1);
                            b.extents = (Vector3) (b.extents * 0.5f);
                            GraphUpdateObject ob = new GraphUpdateObject(b);
                            ob.nnConstraint.graphMask = ((int) 1) << this.graph.graphIndex;
                            AstarPath.active.UpdateGraphs(ob);
                        }
                    }
                }
                this.anyDirtyTiles = false;
            }
        }
    }
}

