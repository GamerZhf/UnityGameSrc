namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_animation_link.php")]
    public class AnimationLink : NodeLink2
    {
        public float animSpeed = 1f;
        public string boneRoot = "bn_COG_Root";
        public string clip;
        public GameObject referenceMesh;
        public bool reverseAnim = true;
        public LinkClip[] sequence;

        public void CalculateOffsets(List<Vector3> trace, out Vector3 endPosition)
        {
            endPosition = base.transform.position;
            if (this.referenceMesh != null)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate(this.referenceMesh, base.transform.position, base.transform.rotation) as GameObject;
                obj2.hideFlags = HideFlags.HideAndDontSave;
                Transform transform = SearchRec(obj2.transform, this.boneRoot);
                if (transform == null)
                {
                    throw new Exception("Could not find root transform");
                }
                Animation component = obj2.GetComponent<Animation>();
                if (component == null)
                {
                    component = obj2.AddComponent<Animation>();
                }
                for (int i = 0; i < this.sequence.Length; i++)
                {
                    component.AddClip(this.sequence[i].clip, this.sequence[i].clip.name);
                }
                Vector3 zero = Vector3.zero;
                Vector3 position = base.transform.position;
                Vector3 vector3 = Vector3.zero;
                for (int j = 0; j < this.sequence.Length; j++)
                {
                    LinkClip clip = this.sequence[j];
                    if (clip == null)
                    {
                        endPosition = position;
                        return;
                    }
                    component[clip.clip.name].enabled = true;
                    component[clip.clip.name].weight = 1f;
                    for (int k = 0; k < clip.loopCount; k++)
                    {
                        component[clip.clip.name].normalizedTime = 0f;
                        component.Sample();
                        Vector3 vector4 = transform.position - base.transform.position;
                        if (j > 0)
                        {
                            position += zero - vector4;
                        }
                        else
                        {
                            vector3 = vector4;
                        }
                        for (int m = 0; m <= 20; m++)
                        {
                            float num5 = ((float) m) / 20f;
                            component[clip.clip.name].normalizedTime = num5;
                            component.Sample();
                            Vector3 item = (position + (transform.position - base.transform.position)) + ((Vector3) ((clip.velocity * num5) * clip.clip.length));
                            trace.Add(item);
                        }
                        position += (Vector3) ((clip.velocity * 1f) * clip.clip.length);
                        component[clip.clip.name].normalizedTime = 1f;
                        component.Sample();
                        zero = transform.position - base.transform.position;
                    }
                    component[clip.clip.name].enabled = false;
                    component[clip.clip.name].weight = 0f;
                }
                position += zero - vector3;
                UnityEngine.Object.DestroyImmediate(obj2);
                endPosition = position;
            }
        }

        public override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            List<Vector3> trace = ListPool<Vector3>.Claim();
            Vector3 zero = Vector3.zero;
            this.CalculateOffsets(trace, out zero);
            Gizmos.color = Color.blue;
            for (int i = 0; i < (trace.Count - 1); i++)
            {
                Gizmos.DrawLine(trace[i], trace[i + 1]);
            }
        }

        private static Transform SearchRec(Transform tr, string name)
        {
            int childCount = tr.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform child = tr.GetChild(i);
                if (child.name == name)
                {
                    return child;
                }
                Transform transform2 = SearchRec(child, name);
                if (transform2 != null)
                {
                    return transform2;
                }
            }
            return null;
        }

        [Serializable]
        public class LinkClip
        {
            public AnimationClip clip;
            public int loopCount = 1;
            public Vector3 velocity;

            public string name
            {
                get
                {
                    return ((this.clip == null) ? string.Empty : this.clip.name);
                }
            }
        }
    }
}

