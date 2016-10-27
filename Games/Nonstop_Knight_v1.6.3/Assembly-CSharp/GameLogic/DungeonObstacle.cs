namespace GameLogic
{
    using App;
    using System;
    using UnityEngine;

    public class DungeonObstacle : MonoBehaviour
    {
        public UnityEngine.Collider Collider;
        private float m_nextRefresh;
        private Bounds m_prevBounds;
        private float m_refreshRate = 5f;
        private float m_updateError = 1f;

        private float boundsVolume(Bounds b)
        {
            return Math.Abs((float) ((b.size.x * b.size.y) * b.size.z));
        }

        protected void OnEnable()
        {
            this.m_prevBounds = new Bounds();
        }

        protected void Start()
        {
            if (this.Collider == null)
            {
                this.Collider = base.GetComponent<UnityEngine.Collider>();
            }
            if (this.Collider == null)
            {
                this.Collider = base.GetComponentInChildren<UnityEngine.Collider>();
            }
            if (this.Collider != null)
            {
                this.Collider.gameObject.layer = Layers.OBSTACLES;
            }
            else
            {
                base.enabled = false;
            }
        }

        protected void Update()
        {
            if (((AstarPath.active != null) && (AstarPath.active.lastScanTime > 0f)) && (Time.time >= this.m_nextRefresh))
            {
                if (this.m_nextRefresh == 0f)
                {
                    this.m_nextRefresh = Time.time + UnityEngine.Random.Range((float) 0f, (float) 1.5f);
                }
                else
                {
                    Bounds bounds = this.Collider.bounds;
                    Bounds bounds2 = bounds;
                    bounds2.Encapsulate(this.m_prevBounds);
                    Vector3 vector = bounds2.min - bounds.min;
                    Vector3 vector2 = bounds2.max - bounds.max;
                    if ((((Mathf.Abs(vector.x) > this.m_updateError) || (Mathf.Abs(vector.y) > this.m_updateError)) || ((Mathf.Abs(vector.z) > this.m_updateError) || (Mathf.Abs(vector2.x) > this.m_updateError))) || ((Mathf.Abs(vector2.y) > this.m_updateError) || (Mathf.Abs(vector2.z) > this.m_updateError)))
                    {
                        this.updateGraphs();
                    }
                    this.m_nextRefresh = Time.time + (this.m_refreshRate * UnityEngine.Random.Range((float) 0.75f, (float) 1.25f));
                }
            }
        }

        public void updateGraphs()
        {
            Bounds b = this.Collider.bounds;
            Bounds bounds2 = b;
            bounds2.Encapsulate(this.m_prevBounds);
            if (this.boundsVolume(bounds2) < (this.boundsVolume(b) + this.boundsVolume(this.m_prevBounds)))
            {
                if (AstarPath.active != null)
                {
                    AstarPath.active.UpdateGraphs(bounds2);
                }
            }
            else if (AstarPath.active != null)
            {
                AstarPath.active.UpdateGraphs(this.m_prevBounds);
                AstarPath.active.UpdateGraphs(b);
            }
            this.m_prevBounds = b;
        }
    }
}

