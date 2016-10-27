namespace Pathfinding
{
    using Pathfinding.Util;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class GraphUpdateUtilities
    {
        public static bool UpdateGraphsNoBlock(GraphUpdateObject guo, List<GraphNode> nodes, [Optional, DefaultParameterValue(false)] bool alwaysRevert)
        {
            <UpdateGraphsNoBlock>c__AnonStorey25E storeye = new <UpdateGraphsNoBlock>c__AnonStorey25E();
            storeye.guo = guo;
            storeye.nodes = nodes;
            storeye.alwaysRevert = alwaysRevert;
            for (int i = 0; i < storeye.nodes.Count; i++)
            {
                if (!storeye.nodes[i].Walkable)
                {
                    return false;
                }
            }
            storeye.guo.trackChangedNodes = true;
            storeye.worked = true;
            AstarPath.RegisterSafeUpdate(new Action(storeye.<>m__37));
            AstarPath.active.FlushThreadSafeCallbacks();
            storeye.guo.trackChangedNodes = false;
            return storeye.worked;
        }

        public static bool UpdateGraphsNoBlock(GraphUpdateObject guo, GraphNode node1, GraphNode node2, [Optional, DefaultParameterValue(false)] bool alwaysRevert)
        {
            List<GraphNode> nodes = ListPool<GraphNode>.Claim();
            nodes.Add(node1);
            nodes.Add(node2);
            bool flag = UpdateGraphsNoBlock(guo, nodes, alwaysRevert);
            ListPool<GraphNode>.Release(nodes);
            return flag;
        }

        [CompilerGenerated]
        private sealed class <UpdateGraphsNoBlock>c__AnonStorey25E
        {
            internal bool alwaysRevert;
            internal GraphUpdateObject guo;
            internal List<GraphNode> nodes;
            internal bool worked;

            internal void <>m__37()
            {
                AstarPath.active.UpdateGraphs(this.guo);
                AstarPath.active.FlushGraphUpdates();
                this.worked = this.worked && PathUtilities.IsPathPossible(this.nodes);
                if (!this.worked || this.alwaysRevert)
                {
                    this.guo.RevertFromBackup();
                    AstarPath.active.FloodFill();
                }
            }
        }
    }
}

