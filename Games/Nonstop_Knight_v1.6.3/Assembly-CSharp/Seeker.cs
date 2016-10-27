using Pathfinding;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

[HelpURL("http://arongranberg.com/astar/docs/class_seeker.php"), AddComponentMenu("Pathfinding/Seeker")]
public class Seeker : MonoBehaviour, ISerializationCallbackReceiver
{
    [CompilerGenerated]
    private static Comparison<IPathModifier> <>f__am$cache12;
    public bool detailedGizmos;
    public bool drawGizmos = true;
    [NonSerialized]
    private List<GraphNode> lastCompletedNodePath;
    [NonSerialized]
    private List<Vector3> lastCompletedVectorPath;
    protected uint lastPathID;
    private readonly List<IPathModifier> modifiers = new List<IPathModifier>();
    private readonly OnPathDelegate onPartialPathDelegate;
    private readonly OnPathDelegate onPathDelegate;
    [NonSerialized]
    protected Path path;
    public OnPathDelegate pathCallback;
    public OnPathDelegate postProcessPath;
    public OnPathDelegate preProcessPath;
    [NonSerialized]
    private Path prevPath;
    public StartEndModifier startEndModifier = new StartEndModifier();
    [HideInInspector]
    public int[] tagPenalties = new int[0x20];
    private OnPathDelegate tmpPathCallback;
    [HideInInspector]
    public int traversableTags = -1;
    [HideInInspector, SerializeField, FormerlySerializedAs("traversableTags")]
    protected TagMask traversableTagsCompatibility = new TagMask(-1, -1);

    public Seeker()
    {
        this.onPathDelegate = new OnPathDelegate(this.OnPathComplete);
        this.onPartialPathDelegate = new OnPathDelegate(this.OnPartialPathComplete);
    }

    private void Awake()
    {
        this.startEndModifier.Awake(this);
    }

    public void DeregisterModifier(IPathModifier mod)
    {
        this.modifiers.Remove(mod);
    }

    public Path GetCurrentPath()
    {
        return this.path;
    }

    public ABPath GetNewPath(Vector3 start, Vector3 end)
    {
        return ABPath.Construct(start, end, null);
    }

    public bool IsDone()
    {
        return ((this.path == null) || (this.path.GetState() >= PathState.Returned));
    }

    public void OnDestroy()
    {
        this.ReleaseClaimedPath();
        this.startEndModifier.OnDestroy(this);
    }

    public void OnDrawGizmos()
    {
        if ((this.lastCompletedNodePath != null) && this.drawGizmos)
        {
            if (this.detailedGizmos)
            {
                Gizmos.color = new Color(0.7f, 0.5f, 0.1f, 0.5f);
                if (this.lastCompletedNodePath != null)
                {
                    for (int i = 0; i < (this.lastCompletedNodePath.Count - 1); i++)
                    {
                        Gizmos.DrawLine((Vector3) this.lastCompletedNodePath[i].position, (Vector3) this.lastCompletedNodePath[i + 1].position);
                    }
                }
            }
            Gizmos.color = new Color(0f, 1f, 0f, 1f);
            if (this.lastCompletedVectorPath != null)
            {
                for (int j = 0; j < (this.lastCompletedVectorPath.Count - 1); j++)
                {
                    Gizmos.DrawLine(this.lastCompletedVectorPath[j], this.lastCompletedVectorPath[j + 1]);
                }
            }
        }
    }

    private void OnMultiPathComplete(Path p)
    {
        this.OnPathComplete(p, false, true);
    }

    private void OnPartialPathComplete(Path p)
    {
        this.OnPathComplete(p, true, false);
    }

    private void OnPathComplete(Path p)
    {
        this.OnPathComplete(p, true, true);
    }

    private void OnPathComplete(Path p, bool runModifiers, bool sendCallbacks)
    {
        if ((((p == null) || (p == this.path)) || !sendCallbacks) && (((this != null) && (p != null)) && (p == this.path)))
        {
            if (!this.path.error && runModifiers)
            {
                this.RunModifiers(ModifierPass.PostProcess, this.path);
            }
            if (sendCallbacks)
            {
                p.Claim(this);
                this.lastCompletedNodePath = p.path;
                this.lastCompletedVectorPath = p.vectorPath;
                if (this.tmpPathCallback != null)
                {
                    this.tmpPathCallback(p);
                }
                if (this.pathCallback != null)
                {
                    this.pathCallback(p);
                }
                if (this.prevPath != null)
                {
                    this.prevPath.Release(this, true);
                }
                this.prevPath = p;
                if (!this.drawGizmos)
                {
                    this.ReleaseClaimedPath();
                }
            }
        }
    }

    public void PostProcess(Path p)
    {
        this.RunModifiers(ModifierPass.PostProcess, p);
    }

    public void RegisterModifier(IPathModifier mod)
    {
        this.modifiers.Add(mod);
        if (<>f__am$cache12 == null)
        {
            <>f__am$cache12 = delegate (IPathModifier a, IPathModifier b) {
                return a.Order.CompareTo(b.Order);
            };
        }
        this.modifiers.Sort(<>f__am$cache12);
    }

    public void ReleaseClaimedPath()
    {
        if (this.prevPath != null)
        {
            this.prevPath.Release(this, true);
            this.prevPath = null;
        }
    }

    public void RunModifiers(ModifierPass pass, Path p)
    {
        if ((pass == ModifierPass.PreProcess) && (this.preProcessPath != null))
        {
            this.preProcessPath(p);
        }
        else if ((pass == ModifierPass.PostProcess) && (this.postProcessPath != null))
        {
            this.postProcessPath(p);
        }
        for (int i = 0; i < this.modifiers.Count; i++)
        {
            MonoModifier modifier = this.modifiers[i] as MonoModifier;
            if ((modifier == null) || modifier.enabled)
            {
                if (pass == ModifierPass.PreProcess)
                {
                    this.modifiers[i].PreProcess(p);
                }
                else if (pass == ModifierPass.PostProcess)
                {
                    this.modifiers[i].Apply(p);
                }
            }
        }
    }

    [Obsolete("You can use StartPath instead of this method now. It will behave identically.")]
    public MultiTargetPath StartMultiTargetPath(MultiTargetPath p, [Optional, DefaultParameterValue(null)] OnPathDelegate callback, [Optional, DefaultParameterValue(-1)] int graphMask)
    {
        this.StartPath(p, callback, graphMask);
        return p;
    }

    public MultiTargetPath StartMultiTargetPath(Vector3 start, Vector3[] endPoints, bool pathsForAll, [Optional, DefaultParameterValue(null)] OnPathDelegate callback, [Optional, DefaultParameterValue(-1)] int graphMask)
    {
        MultiTargetPath p = MultiTargetPath.Construct(start, endPoints, null, null);
        p.pathsForAll = pathsForAll;
        this.StartPath(p, callback, graphMask);
        return p;
    }

    public MultiTargetPath StartMultiTargetPath(Vector3[] startPoints, Vector3 end, bool pathsForAll, [Optional, DefaultParameterValue(null)] OnPathDelegate callback, [Optional, DefaultParameterValue(-1)] int graphMask)
    {
        MultiTargetPath p = MultiTargetPath.Construct(startPoints, end, null, null);
        p.pathsForAll = pathsForAll;
        this.StartPath(p, callback, graphMask);
        return p;
    }

    public Path StartPath(Vector3 start, Vector3 end)
    {
        return this.StartPath(start, end, null, -1);
    }

    public Path StartPath(Path p, [Optional, DefaultParameterValue(null)] OnPathDelegate callback, [Optional, DefaultParameterValue(-1)] int graphMask)
    {
        MultiTargetPath path = p as MultiTargetPath;
        if (path != null)
        {
            OnPathDelegate[] delegateArray = new OnPathDelegate[path.targetPoints.Length];
            for (int i = 0; i < delegateArray.Length; i++)
            {
                delegateArray[i] = this.onPartialPathDelegate;
            }
            path.callbacks = delegateArray;
            p.callback = (OnPathDelegate) Delegate.Combine(p.callback, new OnPathDelegate(this.OnMultiPathComplete));
        }
        else
        {
            p.callback = (OnPathDelegate) Delegate.Combine(p.callback, this.onPathDelegate);
        }
        p.enabledTags = this.traversableTags;
        p.tagPenalties = this.tagPenalties;
        p.nnConstraint.graphMask = graphMask;
        this.StartPathInternal(p, callback);
        return p;
    }

    public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback)
    {
        return this.StartPath(start, end, callback, -1);
    }

    public Path StartPath(Vector3 start, Vector3 end, OnPathDelegate callback, int graphMask)
    {
        return this.StartPath(this.GetNewPath(start, end), callback, graphMask);
    }

    private void StartPathInternal(Path p, OnPathDelegate callback)
    {
        if (((this.path != null) && (this.path.GetState() <= PathState.Processing)) && (this.lastPathID == this.path.pathID))
        {
            this.path.Error();
        }
        this.path = p;
        this.tmpPathCallback = callback;
        this.lastPathID = this.path.pathID;
        this.RunModifiers(ModifierPass.PreProcess, this.path);
        AstarPath.StartPath(this.path, false);
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if ((this.traversableTagsCompatibility != null) && (this.traversableTagsCompatibility.tagsChange != -1))
        {
            this.traversableTags = this.traversableTagsCompatibility.tagsChange;
            this.traversableTagsCompatibility = new TagMask(-1, -1);
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
    }

    public enum ModifierPass
    {
        PostProcess = 2,
        PreProcess = 0
    }
}

