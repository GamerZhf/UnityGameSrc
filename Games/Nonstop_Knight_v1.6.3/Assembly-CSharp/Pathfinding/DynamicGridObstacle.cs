namespace Pathfinding
{
    using System;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_dynamic_grid_obstacle.php"), RequireComponent(typeof(Collider))]
    public class DynamicGridObstacle : MonoBehaviour
    {
        public float checkTime = 0.2f;
        private Collider col;
        private float lastCheckTime = -9999f;
        private Bounds prevBounds;
        private bool prevEnabled;
        private Quaternion prevRotation;
        private Transform tr;
        public float updateError = 1f;

        private static float BoundsVolume(Bounds b)
        {
            return Math.Abs((float) ((b.size.x * b.size.y) * b.size.z));
        }

        public void DoUpdateGraphs()
        {
            if (this.col != null)
            {
                if (!this.col.enabled)
                {
                    AstarPath.active.UpdateGraphs(this.prevBounds);
                }
                else
                {
                    Bounds b = this.col.bounds;
                    Bounds bounds2 = b;
                    bounds2.Encapsulate(this.prevBounds);
                    if (BoundsVolume(bounds2) < (BoundsVolume(b) + BoundsVolume(this.prevBounds)))
                    {
                        AstarPath.active.UpdateGraphs(bounds2);
                    }
                    else
                    {
                        AstarPath.active.UpdateGraphs(this.prevBounds);
                        AstarPath.active.UpdateGraphs(b);
                    }
                    this.prevBounds = b;
                }
                this.prevEnabled = this.col.enabled;
                this.prevRotation = this.tr.rotation;
                this.lastCheckTime = Time.realtimeSinceStartup;
            }
        }

        private void OnDestroy()
        {
            if (AstarPath.active != null)
            {
                GraphUpdateObject ob = new GraphUpdateObject(this.prevBounds);
                AstarPath.active.UpdateGraphs(ob);
            }
        }

        private void Start()
        {
            this.col = base.GetComponent<Collider>();
            this.tr = base.transform;
            if (this.col == null)
            {
                throw new Exception("A collider must be attached to the GameObject for the DynamicGridObstacle to work");
            }
            this.prevBounds = this.col.bounds;
            this.prevEnabled = this.col.enabled;
            this.prevRotation = this.tr.rotation;
        }

        private void Update()
        {
            if (this.col == null)
            {
                Debug.LogError("Removed collider from DynamicGridObstacle", this);
                base.enabled = false;
            }
            else
            {
                while ((AstarPath.active == null) || AstarPath.active.isScanning)
                {
                    this.lastCheckTime = Time.realtimeSinceStartup;
                }
                if ((Time.realtimeSinceStartup - this.lastCheckTime) >= this.checkTime)
                {
                    if (this.col.enabled)
                    {
                        Bounds bounds = this.col.bounds;
                        Quaternion rotation = this.tr.rotation;
                        Vector3 vector = this.prevBounds.min - bounds.min;
                        Vector3 vector2 = this.prevBounds.max - bounds.max;
                        float num2 = (bounds.extents.magnitude * Quaternion.Angle(this.prevRotation, rotation)) * 0.01745329f;
                        if (((vector.sqrMagnitude > (this.updateError * this.updateError)) || (vector2.sqrMagnitude > (this.updateError * this.updateError))) || ((num2 > this.updateError) || !this.prevEnabled))
                        {
                            this.DoUpdateGraphs();
                        }
                    }
                    else if (this.prevEnabled)
                    {
                        this.DoUpdateGraphs();
                    }
                }
            }
        }
    }
}

