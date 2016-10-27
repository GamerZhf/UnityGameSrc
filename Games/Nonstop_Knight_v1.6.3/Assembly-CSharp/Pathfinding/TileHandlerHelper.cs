namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_tile_handler_helper.php")]
    public class TileHandlerHelper : MonoBehaviour
    {
        private readonly List<Bounds> forcedReloadBounds = new List<Bounds>();
        private TileHandler handler;
        private float lastUpdateTime = -999f;
        public float updateInterval;

        public void DiscardPending()
        {
            List<NavmeshCut> all = NavmeshCut.GetAll();
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].RequiresUpdate())
                {
                    all[i].NotifyUpdated();
                }
            }
        }

        public void ForceUpdate()
        {
            if (this.handler == null)
            {
                throw new Exception("Cannot update graphs. No TileHandler. Do not call this method in Awake.");
            }
            this.lastUpdateTime = Time.realtimeSinceStartup;
            List<NavmeshCut> all = NavmeshCut.GetAll();
            if (!this.handler.isValid)
            {
                Debug.Log("TileHandler no longer matched the underlaying RecastGraph (possibly because of a graph scan). Recreating TileHandler...");
                this.handler = new TileHandler(this.handler.graph);
                this.handler.CreateTileTypesFromGraph();
                this.forcedReloadBounds.Add(new Bounds(Vector3.zero, new Vector3(1E+07f, 1E+07f, 1E+07f)));
            }
            if (this.forcedReloadBounds.Count == 0)
            {
                int num = 0;
                for (int m = 0; m < all.Count; m++)
                {
                    if (all[m].RequiresUpdate())
                    {
                        num++;
                        break;
                    }
                }
                if (num == 0)
                {
                    return;
                }
            }
            bool flag = this.handler.StartBatchLoad();
            for (int i = 0; i < this.forcedReloadBounds.Count; i++)
            {
                this.handler.ReloadInBounds(this.forcedReloadBounds[i]);
            }
            this.forcedReloadBounds.Clear();
            for (int j = 0; j < all.Count; j++)
            {
                if (all[j].enabled)
                {
                    if (all[j].RequiresUpdate())
                    {
                        this.handler.ReloadInBounds(all[j].LastBounds);
                        this.handler.ReloadInBounds(all[j].GetBounds());
                    }
                }
                else if (all[j].RequiresUpdate())
                {
                    this.handler.ReloadInBounds(all[j].LastBounds);
                }
            }
            for (int k = 0; k < all.Count; k++)
            {
                if (all[k].RequiresUpdate())
                {
                    all[k].NotifyUpdated();
                }
            }
            if (flag)
            {
                this.handler.EndBatchLoad();
            }
        }

        private void HandleOnDestroyCallback(NavmeshCut obj)
        {
            this.forcedReloadBounds.Add(obj.LastBounds);
            this.lastUpdateTime = -999f;
        }

        private void OnDisable()
        {
            NavmeshCut.OnDestroyCallback -= new Action<NavmeshCut>(this.HandleOnDestroyCallback);
        }

        private void OnEnable()
        {
            NavmeshCut.OnDestroyCallback += new Action<NavmeshCut>(this.HandleOnDestroyCallback);
        }

        private void Start()
        {
            if (UnityEngine.Object.FindObjectsOfType(typeof(TileHandlerHelper)).Length > 1)
            {
                Debug.LogError("There should only be one TileHandlerHelper per scene. Destroying.");
                UnityEngine.Object.Destroy(this);
            }
            else if (this.handler == null)
            {
                if ((AstarPath.active == null) || (AstarPath.active.astarData.recastGraph == null))
                {
                    Debug.LogWarning("No AstarPath object in the scene or no RecastGraph on that AstarPath object");
                }
                this.handler = new TileHandler(AstarPath.active.astarData.recastGraph);
                this.handler.CreateTileTypesFromGraph();
            }
        }

        private void Update()
        {
            if (((this.handler != null) && (((this.updateInterval != -1f) && ((Time.realtimeSinceStartup - this.lastUpdateTime) >= this.updateInterval)) || !this.handler.isValid)) && !AstarPath.active.isScanning)
            {
                this.ForceUpdate();
            }
        }

        public void UseSpecifiedHandler(TileHandler handler)
        {
            this.handler = handler;
        }
    }
}

