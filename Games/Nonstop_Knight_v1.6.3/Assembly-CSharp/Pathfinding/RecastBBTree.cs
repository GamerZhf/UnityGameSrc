namespace Pathfinding
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class RecastBBTree
    {
        private RecastBBTreeBox root;

        private static Rect ExpandToContain(Rect r, Rect r2)
        {
            float left = Mathf.Min(r.xMin, r2.xMin);
            float right = Mathf.Max(r.xMax, r2.xMax);
            float top = Mathf.Min(r.yMin, r2.yMin);
            float bottom = Mathf.Max(r.yMax, r2.yMax);
            return Rect.MinMaxRect(left, top, right, bottom);
        }

        private static float ExpansionRequired(Rect r, Rect r2)
        {
            float num = Mathf.Min(r.xMin, r2.xMin);
            float num2 = Mathf.Max(r.xMax, r2.xMax);
            float num3 = Mathf.Min(r.yMin, r2.yMin);
            float num4 = Mathf.Max(r.yMax, r2.yMax);
            return (((num2 - num) * (num4 - num3)) - RectArea(r));
        }

        public void Insert(RecastMeshObj mesh)
        {
            RecastBBTreeBox box = new RecastBBTreeBox(mesh);
            if (this.root == null)
            {
                this.root = box;
                return;
            }
            RecastBBTreeBox root = this.root;
        Label_0021:
            root.rect = ExpandToContain(root.rect, box.rect);
            if (root.mesh != null)
            {
                root.c1 = box;
                RecastBBTreeBox box3 = new RecastBBTreeBox(root.mesh);
                root.c2 = box3;
                root.mesh = null;
            }
            else
            {
                float num = ExpansionRequired(root.c1.rect, box.rect);
                float num2 = ExpansionRequired(root.c2.rect, box.rect);
                if (num < num2)
                {
                    root = root.c1;
                }
                else if (num2 < num)
                {
                    root = root.c2;
                }
                else
                {
                    root = (RectArea(root.c1.rect) >= RectArea(root.c2.rect)) ? root.c2 : root.c1;
                }
                goto Label_0021;
            }
        }

        private void QueryBoxInBounds(RecastBBTreeBox box, Rect bounds, List<RecastMeshObj> boxes)
        {
            if (box.mesh != null)
            {
                if (RectIntersectsRect(box.rect, bounds))
                {
                    boxes.Add(box.mesh);
                }
            }
            else
            {
                if (RectIntersectsRect(box.c1.rect, bounds))
                {
                    this.QueryBoxInBounds(box.c1, bounds, boxes);
                }
                if (RectIntersectsRect(box.c2.rect, bounds))
                {
                    this.QueryBoxInBounds(box.c2, bounds, boxes);
                }
            }
        }

        public void QueryInBounds(Rect bounds, List<RecastMeshObj> buffer)
        {
            if (this.root != null)
            {
                this.QueryBoxInBounds(this.root, bounds, buffer);
            }
        }

        private static float RectArea(Rect r)
        {
            return (r.width * r.height);
        }

        private static bool RectIntersectsRect(Rect r, Rect r2)
        {
            return ((((r.xMax > r2.xMin) && (r.yMax > r2.yMin)) && (r2.xMax > r.xMin)) && (r2.yMax > r.yMin));
        }

        public bool Remove(RecastMeshObj mesh)
        {
            if (mesh == null)
            {
                throw new ArgumentNullException("mesh");
            }
            if (this.root == null)
            {
                return false;
            }
            bool found = false;
            Bounds bounds = mesh.GetBounds();
            Rect rect = Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
            this.root = this.RemoveBox(this.root, mesh, rect, ref found);
            return found;
        }

        private RecastBBTreeBox RemoveBox(RecastBBTreeBox c, RecastMeshObj mesh, Rect bounds, ref bool found)
        {
            if (RectIntersectsRect(c.rect, bounds))
            {
                if (c.mesh == mesh)
                {
                    found = true;
                    return null;
                }
                if ((c.mesh != null) || found)
                {
                    return c;
                }
                c.c1 = this.RemoveBox(c.c1, mesh, bounds, ref found);
                if (c.c1 == null)
                {
                    return c.c2;
                }
                if (!found)
                {
                    c.c2 = this.RemoveBox(c.c2, mesh, bounds, ref found);
                    if (c.c2 == null)
                    {
                        return c.c1;
                    }
                }
                if (found)
                {
                    c.rect = ExpandToContain(c.c1.rect, c.c2.rect);
                }
            }
            return c;
        }
    }
}

