namespace Pathfinding
{
    using Pathfinding.Serialization;
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public class LayerGridGraph : GridGraph, IUpdatableGraph
    {
        [JsonMember]
        public float characterHeight = 0.4f;
        internal int lastScannedDepth;
        internal int lastScannedWidth;
        [JsonMember]
        public int layerCount;
        [JsonMember]
        public float mergeSpanRange = 0.5f;
        public int[] nodeCellIndices;
        public LevelGridNode[] nodes;

        public void AddLayers(int count)
        {
            int num = this.layerCount + count;
            if (num > 15)
            {
                UnityEngine.Debug.LogError("Too many layers, a maximum of LevelGridNode.MaxLayerCount are allowed (required " + num + ")");
            }
            else
            {
                LevelGridNode[] nodes = this.nodes;
                this.nodes = new LevelGridNode[(base.width * base.depth) * num];
                for (int i = 0; i < nodes.Length; i++)
                {
                    this.nodes[i] = nodes[i];
                }
                this.layerCount = num;
            }
        }

        public void CalculateConnections(GraphNode[] nodes, GraphNode node, int x, int z, int layerIndex)
        {
            if (node != null)
            {
                LevelGridNode node2 = (LevelGridNode) node;
                node2.ResetAllGridConnections();
                if (node.Walkable)
                {
                    float positiveInfinity;
                    if ((layerIndex == (this.layerCount - 1)) || (nodes[node2.NodeInGridIndex + ((base.width * base.depth) * (layerIndex + 1))] == null))
                    {
                        positiveInfinity = float.PositiveInfinity;
                    }
                    else
                    {
                        positiveInfinity = Math.Abs((int) (node2.position.y - nodes[node2.NodeInGridIndex + ((base.width * base.depth) * (layerIndex + 1))].position.y)) * 0.001f;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        int num3 = x + base.neighbourXOffsets[i];
                        int num4 = z + base.neighbourZOffsets[i];
                        if ((((num3 >= 0) && (num4 >= 0)) && (num3 < base.width)) && (num4 < base.depth))
                        {
                            int num5 = (num4 * base.width) + num3;
                            int num6 = 15;
                            for (int j = 0; j < this.layerCount; j++)
                            {
                                GraphNode node3 = nodes[num5 + ((base.width * base.depth) * j)];
                                if ((node3 != null) && node3.Walkable)
                                {
                                    float num8;
                                    if ((j == (this.layerCount - 1)) || (nodes[num5 + ((base.width * base.depth) * (j + 1))] == null))
                                    {
                                        num8 = float.PositiveInfinity;
                                    }
                                    else
                                    {
                                        num8 = Math.Abs((int) (node3.position.y - nodes[num5 + ((base.width * base.depth) * (j + 1))].position.y)) * 0.001f;
                                    }
                                    float num9 = Mathf.Max((float) (node3.position.y * 0.001f), (float) (node2.position.y * 0.001f));
                                    float num11 = Mathf.Min((float) ((node3.position.y * 0.001f) + num8), (float) ((node2.position.y * 0.001f) + positiveInfinity)) - num9;
                                    if ((num11 >= this.characterHeight) && ((Mathf.Abs((int) (node3.position.y - node2.position.y)) * 0.001f) <= base.maxClimb))
                                    {
                                        num6 = j;
                                    }
                                }
                            }
                            node2.SetConnectionValue(i, num6);
                        }
                    }
                }
            }
        }

        public static bool CheckConnection(LevelGridNode node, int dir)
        {
            return node.GetConnection(dir);
        }

        public override int CountNodes()
        {
            if (this.nodes == null)
            {
                return 0;
            }
            int num = 0;
            for (int i = 0; i < this.nodes.Length; i++)
            {
                if (this.nodes[i] != null)
                {
                    num++;
                }
            }
            return num;
        }

        public override void DeserializeExtraInfo(GraphSerializationContext ctx)
        {
            int num = ctx.reader.ReadInt32();
            if (num == -1)
            {
                this.nodes = null;
            }
            else
            {
                this.nodes = new LevelGridNode[num];
                for (int i = 0; i < this.nodes.Length; i++)
                {
                    if (ctx.reader.ReadInt32() != -1)
                    {
                        this.nodes[i] = new LevelGridNode(base.active);
                        this.nodes[i].DeserializeNode(ctx);
                    }
                    else
                    {
                        this.nodes[i] = null;
                    }
                }
            }
        }

        public override void ErodeWalkableArea(int xmin, int zmin, int xmax, int zmax)
        {
            xmin = Mathf.Clamp(xmin, 0, base.Width);
            xmax = Mathf.Clamp(xmax, 0, base.Width);
            zmin = Mathf.Clamp(zmin, 0, base.Depth);
            zmax = Mathf.Clamp(zmax, 0, base.Depth);
            if (base.erosionUseTags)
            {
                UnityEngine.Debug.LogError("Erosion Uses Tags is not supported for LayerGridGraphs yet");
            }
            for (int i = 0; i < base.erodeIterations; i++)
            {
                for (int j = 0; j < this.layerCount; j++)
                {
                    for (int m = zmin; m < zmax; m++)
                    {
                        for (int n = xmin; n < xmax; n++)
                        {
                            LevelGridNode node = this.nodes[((m * base.width) + n) + ((base.width * base.depth) * j)];
                            if ((node != null) && node.Walkable)
                            {
                                bool flag = false;
                                for (int num5 = 0; num5 < 4; num5++)
                                {
                                    if (!node.GetConnection(num5))
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (flag)
                                {
                                    node.Walkable = false;
                                }
                            }
                        }
                    }
                }
                for (int k = 0; k < this.layerCount; k++)
                {
                    for (int num7 = zmin; num7 < zmax; num7++)
                    {
                        for (int num8 = xmin; num8 < xmax; num8++)
                        {
                            LevelGridNode node2 = this.nodes[((num7 * base.width) + num8) + ((base.width * base.depth) * k)];
                            if (node2 != null)
                            {
                                this.CalculateConnections(this.nodes, node2, num8, num7, k);
                            }
                        }
                    }
                }
            }
        }

        public override NNInfoInternal GetNearest(Vector3 position, NNConstraint constraint, GraphNode hint)
        {
            if ((this.nodes == null) || (((base.depth * base.width) * this.layerCount) != this.nodes.Length))
            {
                return new NNInfoInternal();
            }
            Vector3 vector = this.inverseMatrix.MultiplyPoint3x4(position);
            int x = Mathf.Clamp(Mathf.RoundToInt(vector.x - 0.5f), 0, base.width - 1);
            int z = Mathf.Clamp(Mathf.RoundToInt(vector.z - 0.5f), 0, base.depth - 1);
            return new NNInfoInternal(this.GetNearestNode(position, x, z, null));
        }

        public override NNInfoInternal GetNearestForce(Vector3 position, NNConstraint constraint)
        {
            if (((this.nodes == null) || (((base.depth * base.width) * this.layerCount) != this.nodes.Length)) || (this.layerCount == 0))
            {
                return new NNInfoInternal();
            }
            Vector3 vector = position;
            position = this.inverseMatrix.MultiplyPoint3x4(position);
            int x = Mathf.Clamp(Mathf.RoundToInt(position.x - 0.5f), 0, base.width - 1);
            int z = Mathf.Clamp(Mathf.RoundToInt(position.z - 0.5f), 0, base.depth - 1);
            float positiveInfinity = float.PositiveInfinity;
            int num4 = 2;
            LevelGridNode node = this.GetNearestNode(vector, x, z, constraint);
            if (node != null)
            {
                Vector3 vector2 = ((Vector3) node.position) - vector;
                positiveInfinity = vector2.sqrMagnitude;
            }
            if (node != null)
            {
                if (num4 == 0)
                {
                    return new NNInfoInternal(node);
                }
                num4--;
            }
            float num5 = !constraint.constrainDistance ? float.PositiveInfinity : AstarPath.active.maxNearestNodeDistance;
            float num6 = num5 * num5;
            int num7 = 1;
            while (true)
            {
                int num8;
                int num9 = z + num7;
                if ((base.nodeSize * num7) > num5)
                {
                    return new NNInfoInternal(node);
                }
                for (num8 = x - num7; num8 <= (x + num7); num8++)
                {
                    if ((((num8 >= 0) && (num9 >= 0)) && (num8 < base.width)) && (num9 < base.depth))
                    {
                        LevelGridNode node2 = this.GetNearestNode(vector, num8, num9, constraint);
                        if (node2 != null)
                        {
                            Vector3 vector3 = ((Vector3) node2.position) - vector;
                            float sqrMagnitude = vector3.sqrMagnitude;
                            if ((sqrMagnitude < positiveInfinity) && (sqrMagnitude < num6))
                            {
                                positiveInfinity = sqrMagnitude;
                                node = node2;
                            }
                        }
                    }
                }
                num9 = z - num7;
                for (num8 = x - num7; num8 <= (x + num7); num8++)
                {
                    if ((((num8 >= 0) && (num9 >= 0)) && (num8 < base.width)) && (num9 < base.depth))
                    {
                        LevelGridNode node3 = this.GetNearestNode(vector, num8, num9, constraint);
                        if (node3 != null)
                        {
                            Vector3 vector4 = ((Vector3) node3.position) - vector;
                            float num11 = vector4.sqrMagnitude;
                            if ((num11 < positiveInfinity) && (num11 < num6))
                            {
                                positiveInfinity = num11;
                                node = node3;
                            }
                        }
                    }
                }
                num8 = x - num7;
                for (num9 = (z - num7) + 1; num9 <= ((z + num7) - 1); num9++)
                {
                    if ((((num8 >= 0) && (num9 >= 0)) && (num8 < base.width)) && (num9 < base.depth))
                    {
                        LevelGridNode node4 = this.GetNearestNode(vector, num8, num9, constraint);
                        if (node4 != null)
                        {
                            Vector3 vector5 = ((Vector3) node4.position) - vector;
                            float num12 = vector5.sqrMagnitude;
                            if ((num12 < positiveInfinity) && (num12 < num6))
                            {
                                positiveInfinity = num12;
                                node = node4;
                            }
                        }
                    }
                }
                num8 = x + num7;
                for (num9 = (z - num7) + 1; num9 <= ((z + num7) - 1); num9++)
                {
                    if ((((num8 >= 0) && (num9 >= 0)) && (num8 < base.width)) && (num9 < base.depth))
                    {
                        LevelGridNode node5 = this.GetNearestNode(vector, num8, num9, constraint);
                        if (node5 != null)
                        {
                            Vector3 vector6 = ((Vector3) node5.position) - vector;
                            float num13 = vector6.sqrMagnitude;
                            if ((num13 < positiveInfinity) && (num13 < num6))
                            {
                                positiveInfinity = num13;
                                node = node5;
                            }
                        }
                    }
                }
                if (node != null)
                {
                    if (num4 == 0)
                    {
                        return new NNInfoInternal(node);
                    }
                    num4--;
                }
                num7++;
            }
        }

        private LevelGridNode GetNearestNode(Vector3 position, int x, int z, NNConstraint constraint)
        {
            int num = (base.width * z) + x;
            float positiveInfinity = float.PositiveInfinity;
            LevelGridNode node = null;
            for (int i = 0; i < this.layerCount; i++)
            {
                LevelGridNode node2 = this.nodes[num + ((base.width * base.depth) * i)];
                if (node2 != null)
                {
                    Vector3 vector = ((Vector3) node2.position) - position;
                    float sqrMagnitude = vector.sqrMagnitude;
                    if ((sqrMagnitude < positiveInfinity) && ((constraint == null) || constraint.Suitable(node2)))
                    {
                        positiveInfinity = sqrMagnitude;
                        node = node2;
                    }
                }
            }
            return node;
        }

        protected override GridNodeBase GetNeighbourAlongDirection(GridNodeBase node, int direction)
        {
            LevelGridNode node2 = node as LevelGridNode;
            if (node2.GetConnection(direction))
            {
                return this.nodes[(node2.NodeInGridIndex + base.neighbourOffsets[direction]) + ((base.width * base.depth) * node2.GetConnectionValue(direction))];
            }
            return null;
        }

        public override void GetNodes(GraphNodeDelegateCancelable del)
        {
            if (this.nodes != null)
            {
                for (int i = 0; i < this.nodes.Length; i++)
                {
                    if ((this.nodes[i] != null) && !del(this.nodes[i]))
                    {
                        break;
                    }
                }
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            this.RemoveGridGraphFromStatic();
        }

        public override void OnDrawGizmos(bool drawNodes)
        {
            if (drawNodes)
            {
                base.OnDrawGizmos(false);
                if (this.nodes != null)
                {
                    PathHandler debugPathData = AstarPath.active.debugPathData;
                    for (int i = 0; i < this.nodes.Length; i++)
                    {
                        LevelGridNode node = this.nodes[i];
                        if ((node != null) && node.Walkable)
                        {
                            Gizmos.color = this.NodeColor(node, AstarPath.active.debugPathData);
                            if (AstarPath.active.showSearchTree && (AstarPath.active.debugPathData != null))
                            {
                                if (NavGraph.InSearchTree(node, AstarPath.active.debugPath))
                                {
                                    PathNode pathNode = debugPathData.GetPathNode(node);
                                    if ((pathNode != null) && (pathNode.parent != null))
                                    {
                                        Gizmos.DrawLine((Vector3) node.position, (Vector3) pathNode.parent.node.position);
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    int connectionValue = node.GetConnectionValue(j);
                                    if (connectionValue != 15)
                                    {
                                        int index = (node.NodeInGridIndex + base.neighbourOffsets[j]) + ((base.width * base.depth) * connectionValue);
                                        if ((index >= 0) && (index < this.nodes.Length))
                                        {
                                            GraphNode node3 = this.nodes[index];
                                            if (node3 != null)
                                            {
                                                Gizmos.DrawLine((Vector3) node.position, (Vector3) node3.position);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void PostDeserialization()
        {
            base.GenerateMatrix();
            this.lastScannedWidth = base.width;
            this.lastScannedDepth = base.depth;
            this.SetUpOffsetsAndCosts();
            if ((this.nodes != null) && (this.nodes.Length != 0))
            {
                LevelGridNode.SetGridGraph(AstarPath.active.astarData.GetGraphIndex(this), this);
                for (int i = 0; i < base.depth; i++)
                {
                    for (int j = 0; j < base.width; j++)
                    {
                        for (int k = 0; k < this.layerCount; k++)
                        {
                            LevelGridNode node = this.nodes[((i * base.width) + j) + ((base.width * base.depth) * k)];
                            if (node != null)
                            {
                                node.NodeInGridIndex = (i * base.width) + j;
                            }
                        }
                    }
                }
            }
        }

        public bool RecalculateCell(int x, int z, bool preserveExistingNodes)
        {
            LinkedLevelCell cell = new LinkedLevelCell();
            Vector3 position = this.matrix.MultiplyPoint3x4(new Vector3(x + 0.5f, 0f, z + 0.5f));
            RaycastHit[] hitArray = base.collision.CheckHeightAll(position);
            for (int i = 0; i < (hitArray.Length / 2); i++)
            {
                RaycastHit hit = hitArray[i];
                hitArray[i] = hitArray[(hitArray.Length - 1) - i];
                hitArray[(hitArray.Length - 1) - i] = hit;
            }
            bool flag = false;
            if (hitArray.Length > 0)
            {
                LinkedLevelNode next = null;
                for (int j = 0; j < hitArray.Length; j++)
                {
                    LinkedLevelNode node2 = new LinkedLevelNode();
                    node2.position = hitArray[j].point;
                    if ((next != null) && ((node2.position.y - next.position.y) <= this.mergeSpanRange))
                    {
                        next.position = node2.position;
                        next.hit = hitArray[j];
                        next.walkable = base.collision.Check(node2.position);
                    }
                    else
                    {
                        node2.walkable = base.collision.Check(node2.position);
                        node2.hit = hitArray[j];
                        node2.height = float.PositiveInfinity;
                        if (cell.first == null)
                        {
                            cell.first = node2;
                            next = node2;
                        }
                        else
                        {
                            next.next = node2;
                            next.height = node2.position.y - next.position.y;
                            next = next.next;
                        }
                    }
                }
            }
            else
            {
                LinkedLevelNode node3 = new LinkedLevelNode();
                node3.position = position;
                node3.height = float.PositiveInfinity;
                node3.walkable = !base.collision.unwalkableWhenNoGround;
                cell.first = node3;
            }
            uint graphIndex = (uint) base.active.astarData.GetGraphIndex(this);
            LinkedLevelNode first = cell.first;
            int num4 = 0;
            int num5 = 0;
            do
            {
                if (num5 >= this.layerCount)
                {
                    if ((num5 + 1) > 15)
                    {
                        UnityEngine.Debug.LogError("Too many layers, a maximum of LevelGridNode.MaxLayerCount are allowed (required " + (num5 + 1) + ")");
                        return flag;
                    }
                    this.AddLayers(1);
                    flag = true;
                }
                LevelGridNode node5 = this.nodes[((z * base.width) + x) + ((base.width * base.depth) * num5)];
                if ((node5 == null) || !preserveExistingNodes)
                {
                    this.nodes[((z * base.width) + x) + ((base.width * base.depth) * num5)] = new LevelGridNode(base.active);
                    node5 = this.nodes[((z * base.width) + x) + ((base.width * base.depth) * num5)];
                    node5.Penalty = base.initialPenalty;
                    node5.GraphIndex = graphIndex;
                    flag = true;
                }
                node5.SetPosition((Int3) first.position);
                node5.Walkable = first.walkable;
                node5.WalkableErosion = node5.Walkable;
                if (first.hit.normal != Vector3.zero)
                {
                    float num6 = Vector3.Dot(first.hit.normal.normalized, base.collision.up);
                    if (base.penaltyAngle)
                    {
                        node5.Penalty += (uint) Mathf.RoundToInt((1f - num6) * base.penaltyAngleFactor);
                    }
                    float num7 = Mathf.Cos(base.maxSlope * 0.01745329f);
                    if (num6 < num7)
                    {
                        node5.Walkable = false;
                    }
                }
                node5.NodeInGridIndex = (z * base.width) + x;
                if (first.height < this.characterHeight)
                {
                    node5.Walkable = false;
                }
                num4++;
                first = first.next;
                num5++;
            }
            while (first != null);
            while (num5 < this.layerCount)
            {
                this.nodes[((z * base.width) + x) + ((base.width * base.depth) * num5)] = null;
                num5++;
            }
            cell.count = num4;
            return flag;
        }

        private void RemoveGridGraphFromStatic()
        {
            LevelGridNode.SetGridGraph(base.active.astarData.GetGraphIndex(this), null);
        }

        [DebuggerHidden]
        public override IEnumerable<Progress> ScanInternal()
        {
            <ScanInternal>c__Iterator15 iterator = new <ScanInternal>c__Iterator15();
            iterator.<>f__this = this;
            iterator.$PC = -2;
            return iterator;
        }

        public void ScanInternal(OnScanStatus status)
        {
            if (base.nodeSize > 0f)
            {
                base.GenerateMatrix();
                if ((base.width > 0x400) || (base.depth > 0x400))
                {
                    UnityEngine.Debug.LogError("One of the grid's sides is longer than 1024 nodes");
                }
                else
                {
                    this.lastScannedWidth = base.width;
                    this.lastScannedDepth = base.depth;
                    this.SetUpOffsetsAndCosts();
                    LevelGridNode.SetGridGraph(base.active.astarData.GetGraphIndex(this), this);
                    base.maxClimb = Mathf.Clamp(base.maxClimb, 0f, this.characterHeight);
                    LinkedLevelCell[] cellArray = new LinkedLevelCell[base.width * base.depth];
                    if (base.collision == null)
                    {
                    }
                    base.collision = new GraphCollision();
                    base.collision.Initialize(base.matrix, base.nodeSize);
                    for (int i = 0; i < base.depth; i++)
                    {
                        for (int k = 0; k < base.width; k++)
                        {
                            cellArray[(i * base.width) + k] = new LinkedLevelCell();
                            LinkedLevelCell cell = cellArray[(i * base.width) + k];
                            Vector3 position = this.matrix.MultiplyPoint3x4(new Vector3(k + 0.5f, 0f, i + 0.5f));
                            RaycastHit[] hitArray = base.collision.CheckHeightAll(position);
                            for (int m = 0; m < (hitArray.Length / 2); m++)
                            {
                                RaycastHit hit = hitArray[m];
                                hitArray[m] = hitArray[(hitArray.Length - 1) - m];
                                hitArray[(hitArray.Length - 1) - m] = hit;
                            }
                            if (hitArray.Length > 0)
                            {
                                LinkedLevelNode next = null;
                                for (int n = 0; n < hitArray.Length; n++)
                                {
                                    LinkedLevelNode node2 = new LinkedLevelNode();
                                    node2.position = hitArray[n].point;
                                    if ((next != null) && ((node2.position.y - next.position.y) <= this.mergeSpanRange))
                                    {
                                        next.position = node2.position;
                                        next.hit = hitArray[n];
                                        next.walkable = base.collision.Check(node2.position);
                                    }
                                    else
                                    {
                                        node2.walkable = base.collision.Check(node2.position);
                                        node2.hit = hitArray[n];
                                        node2.height = float.PositiveInfinity;
                                        if (cell.first == null)
                                        {
                                            cell.first = node2;
                                            next = node2;
                                        }
                                        else
                                        {
                                            next.next = node2;
                                            next.height = node2.position.y - next.position.y;
                                            next = next.next;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                LinkedLevelNode node3 = new LinkedLevelNode();
                                node3.position = position;
                                node3.height = float.PositiveInfinity;
                                node3.walkable = !base.collision.unwalkableWhenNoGround;
                                cell.first = node3;
                            }
                        }
                    }
                    int num5 = 0;
                    this.layerCount = 0;
                    for (int j = 0; j < base.depth; j++)
                    {
                        for (int num7 = 0; num7 < base.width; num7++)
                        {
                            LinkedLevelCell cell2 = cellArray[(j * base.width) + num7];
                            LinkedLevelNode first = cell2.first;
                            int num8 = 0;
                            do
                            {
                                num8++;
                                num5++;
                                first = first.next;
                            }
                            while (first != null);
                            this.layerCount = (num8 <= this.layerCount) ? this.layerCount : num8;
                        }
                    }
                    if (this.layerCount > 15)
                    {
                        UnityEngine.Debug.LogError("Too many layers, a maximum of LevelGridNode.MaxLayerCount are allowed (found " + this.layerCount + ")");
                    }
                    else
                    {
                        this.nodes = new LevelGridNode[(base.width * base.depth) * this.layerCount];
                        for (int num9 = 0; num9 < this.nodes.Length; num9++)
                        {
                            this.nodes[num9] = new LevelGridNode(base.active);
                            this.nodes[num9].Penalty = base.initialPenalty;
                        }
                        int num10 = 0;
                        float num11 = Mathf.Cos(base.maxSlope * 0.01745329f);
                        for (int num12 = 0; num12 < base.depth; num12++)
                        {
                            for (int num13 = 0; num13 < base.width; num13++)
                            {
                                LinkedLevelCell cell3 = cellArray[(num12 * base.width) + num13];
                                LinkedLevelNode node5 = cell3.first;
                                cell3.index = num10;
                                int num14 = 0;
                                int num15 = 0;
                                do
                                {
                                    LevelGridNode node6 = this.nodes[((num12 * base.width) + num13) + ((base.width * base.depth) * num15)];
                                    node6.SetPosition((Int3) node5.position);
                                    node6.Walkable = node5.walkable;
                                    if ((node5.hit.normal != Vector3.zero) && (base.penaltyAngle || (num11 < 1f)))
                                    {
                                        float num16 = Vector3.Dot(node5.hit.normal.normalized, base.collision.up);
                                        if (base.penaltyAngle)
                                        {
                                            node6.Penalty += (uint) Mathf.RoundToInt((1f - num16) * base.penaltyAngleFactor);
                                        }
                                        if (num16 < num11)
                                        {
                                            node6.Walkable = false;
                                        }
                                    }
                                    node6.NodeInGridIndex = (num12 * base.width) + num13;
                                    if (node5.height < this.characterHeight)
                                    {
                                        node6.Walkable = false;
                                    }
                                    node6.WalkableErosion = node6.Walkable;
                                    num10++;
                                    num14++;
                                    node5 = node5.next;
                                    num15++;
                                }
                                while (node5 != null);
                                while (num15 < this.layerCount)
                                {
                                    this.nodes[((num12 * base.width) + num13) + ((base.width * base.depth) * num15)] = null;
                                    num15++;
                                }
                                cell3.count = num14;
                            }
                        }
                        num10 = 0;
                        this.nodeCellIndices = new int[cellArray.Length];
                        for (int num17 = 0; num17 < base.depth; num17++)
                        {
                            for (int num18 = 0; num18 < base.width; num18++)
                            {
                                for (int num19 = 0; num19 < this.layerCount; num19++)
                                {
                                    GraphNode node = this.nodes[((num17 * base.width) + num18) + ((base.width * base.depth) * num19)];
                                    this.CalculateConnections(this.nodes, node, num18, num17, num19);
                                }
                            }
                        }
                        uint graphIndex = (uint) base.active.astarData.GetGraphIndex(this);
                        for (int num21 = 0; num21 < this.nodes.Length; num21++)
                        {
                            LevelGridNode node8 = this.nodes[num21];
                            if (node8 != null)
                            {
                                this.UpdatePenalty(node8);
                                node8.GraphIndex = graphIndex;
                                if (!node8.HasAnyGridConnections())
                                {
                                    node8.Walkable = false;
                                    node8.WalkableErosion = node8.Walkable;
                                }
                            }
                        }
                        this.ErodeWalkableArea();
                    }
                }
            }
        }

        public override void SerializeExtraInfo(GraphSerializationContext ctx)
        {
            if (this.nodes == null)
            {
                ctx.writer.Write(-1);
            }
            else
            {
                ctx.writer.Write(this.nodes.Length);
                for (int i = 0; i < this.nodes.Length; i++)
                {
                    if (this.nodes[i] == null)
                    {
                        ctx.writer.Write(-1);
                    }
                    else
                    {
                        ctx.writer.Write(0);
                        this.nodes[i].SerializeNode(ctx);
                    }
                }
            }
        }

        public void UpdateArea(GraphUpdateObject o)
        {
            if ((this.nodes == null) || (this.nodes.Length != ((base.width * base.depth) * this.layerCount)))
            {
                UnityEngine.Debug.LogWarning("The Grid Graph is not scanned, cannot update area ");
            }
            else
            {
                Vector3 vector;
                Vector3 vector2;
                GridGraph.GetBoundsMinMax(o.bounds, base.inverseMatrix, out vector, out vector2);
                int xmin = Mathf.RoundToInt(vector.x - 0.5f);
                int xmax = Mathf.RoundToInt(vector2.x - 0.5f);
                int ymin = Mathf.RoundToInt(vector.z - 0.5f);
                int ymax = Mathf.RoundToInt(vector2.z - 0.5f);
                IntRect a = new IntRect(xmin, ymin, xmax, ymax);
                IntRect b = a;
                IntRect rect3 = new IntRect(0, 0, base.width - 1, base.depth - 1);
                IntRect rect4 = a;
                bool flag = o.updatePhysics || o.modifyWalkability;
                bool flag2 = (o is LayerGridGraphUpdate) && ((LayerGridGraphUpdate) o).recalculateNodes;
                bool preserveExistingNodes = !(o is LayerGridGraphUpdate) || ((LayerGridGraphUpdate) o).preserveExistingNodes;
                int range = !o.updateErosion ? 0 : base.erodeIterations;
                if (o.trackChangedNodes && flag2)
                {
                    UnityEngine.Debug.LogError("Cannot track changed nodes when creating or deleting nodes.\nWill not update LayerGridGraph");
                }
                else
                {
                    if ((o.updatePhysics && !o.modifyWalkability) && base.collision.collisionCheck)
                    {
                        Vector3 vector3 = (Vector3) (new Vector3(base.collision.diameter, 0f, base.collision.diameter) * 0.5f);
                        vector -= (Vector3) (vector3 * 1.02f);
                        vector2 += (Vector3) (vector3 * 1.02f);
                        int introduced61 = Mathf.RoundToInt(vector.x - 0.5f);
                        int introduced62 = Mathf.RoundToInt(vector2.x - 0.5f);
                        rect4 = new IntRect(introduced61, Mathf.RoundToInt(vector.z - 0.5f), introduced62, Mathf.RoundToInt(vector2.z - 0.5f));
                        b = IntRect.Union(rect4, b);
                    }
                    if (flag || (range > 0))
                    {
                        b = b.Expand(range + 1);
                    }
                    IntRect rect5 = IntRect.Intersection(b, rect3);
                    if (!flag2)
                    {
                        for (int j = rect5.xmin; j <= rect5.xmax; j++)
                        {
                            for (int k = rect5.ymin; k <= rect5.ymax; k++)
                            {
                                for (int m = 0; m < this.layerCount; m++)
                                {
                                    o.WillUpdateNode(this.nodes[(((m * base.width) * base.depth) + (k * base.width)) + j]);
                                }
                            }
                        }
                    }
                    if (o.updatePhysics && !o.modifyWalkability)
                    {
                        base.collision.Initialize(base.matrix, base.nodeSize);
                        rect5 = IntRect.Intersection(rect4, rect3);
                        bool flag4 = false;
                        for (int n = rect5.xmin; n <= rect5.xmax; n++)
                        {
                            for (int num10 = rect5.ymin; num10 <= rect5.ymax; num10++)
                            {
                                flag4 |= this.RecalculateCell(n, num10, preserveExistingNodes);
                            }
                        }
                        for (int num11 = rect5.xmin; num11 <= rect5.xmax; num11++)
                        {
                            for (int num12 = rect5.ymin; num12 <= rect5.ymax; num12++)
                            {
                                for (int num13 = 0; num13 < this.layerCount; num13++)
                                {
                                    int index = (((num13 * base.width) * base.depth) + (num12 * base.width)) + num11;
                                    LevelGridNode node = this.nodes[index];
                                    if (node != null)
                                    {
                                        this.CalculateConnections(this.nodes, node, num11, num12, num13);
                                    }
                                }
                            }
                        }
                    }
                    rect5 = IntRect.Intersection(a, rect3);
                    for (int i = rect5.xmin; i <= rect5.xmax; i++)
                    {
                        for (int num16 = rect5.ymin; num16 <= rect5.ymax; num16++)
                        {
                            for (int num17 = 0; num17 < this.layerCount; num17++)
                            {
                                int num18 = (((num17 * base.width) * base.depth) + (num16 * base.width)) + i;
                                LevelGridNode node2 = this.nodes[num18];
                                if (node2 != null)
                                {
                                    if (flag)
                                    {
                                        node2.Walkable = node2.WalkableErosion;
                                        if (o.bounds.Contains((Vector3) node2.position))
                                        {
                                            o.Apply(node2);
                                        }
                                        node2.WalkableErosion = node2.Walkable;
                                    }
                                    else if (o.bounds.Contains((Vector3) node2.position))
                                    {
                                        o.Apply(node2);
                                    }
                                }
                            }
                        }
                    }
                    if (flag && (range == 0))
                    {
                        rect5 = IntRect.Intersection(b, rect3);
                        for (int num19 = rect5.xmin; num19 <= rect5.xmax; num19++)
                        {
                            for (int num20 = rect5.ymin; num20 <= rect5.ymax; num20++)
                            {
                                for (int num21 = 0; num21 < this.layerCount; num21++)
                                {
                                    int num22 = (((num21 * base.width) * base.depth) + (num20 * base.width)) + num19;
                                    LevelGridNode node3 = this.nodes[num22];
                                    if (node3 != null)
                                    {
                                        this.CalculateConnections(this.nodes, node3, num19, num20, num21);
                                    }
                                }
                            }
                        }
                    }
                    else if (flag && (range > 0))
                    {
                        IntRect rect6 = IntRect.Union(a, rect4).Expand(range);
                        IntRect rect7 = rect6.Expand(range);
                        rect6 = IntRect.Intersection(rect6, rect3);
                        rect7 = IntRect.Intersection(rect7, rect3);
                        for (int num23 = rect7.xmin; num23 <= rect7.xmax; num23++)
                        {
                            for (int num24 = rect7.ymin; num24 <= rect7.ymax; num24++)
                            {
                                for (int num25 = 0; num25 < this.layerCount; num25++)
                                {
                                    int num26 = (((num25 * base.width) * base.depth) + (num24 * base.width)) + num23;
                                    LevelGridNode node4 = this.nodes[num26];
                                    if (node4 != null)
                                    {
                                        bool walkable = node4.Walkable;
                                        node4.Walkable = node4.WalkableErosion;
                                        if (!rect6.Contains(num23, num24))
                                        {
                                            node4.TmpWalkable = walkable;
                                        }
                                    }
                                }
                            }
                        }
                        for (int num27 = rect7.xmin; num27 <= rect7.xmax; num27++)
                        {
                            for (int num28 = rect7.ymin; num28 <= rect7.ymax; num28++)
                            {
                                for (int num29 = 0; num29 < this.layerCount; num29++)
                                {
                                    int num30 = (((num29 * base.width) * base.depth) + (num28 * base.width)) + num27;
                                    LevelGridNode node5 = this.nodes[num30];
                                    if (node5 != null)
                                    {
                                        this.CalculateConnections(this.nodes, node5, num27, num28, num29);
                                    }
                                }
                            }
                        }
                        this.ErodeWalkableArea(rect7.xmin, rect7.ymin, rect7.xmax + 1, rect7.ymax + 1);
                        for (int num31 = rect7.xmin; num31 <= rect7.xmax; num31++)
                        {
                            for (int num32 = rect7.ymin; num32 <= rect7.ymax; num32++)
                            {
                                if (!rect6.Contains(num31, num32))
                                {
                                    for (int num33 = 0; num33 < this.layerCount; num33++)
                                    {
                                        int num34 = (((num33 * base.width) * base.depth) + (num32 * base.width)) + num31;
                                        LevelGridNode node6 = this.nodes[num34];
                                        if (node6 != null)
                                        {
                                            node6.Walkable = node6.TmpWalkable;
                                        }
                                    }
                                }
                            }
                        }
                        for (int num35 = rect7.xmin; num35 <= rect7.xmax; num35++)
                        {
                            for (int num36 = rect7.ymin; num36 <= rect7.ymax; num36++)
                            {
                                for (int num37 = 0; num37 < this.layerCount; num37++)
                                {
                                    int num38 = (((num37 * base.width) * base.depth) + (num36 * base.width)) + num35;
                                    LevelGridNode node7 = this.nodes[num38];
                                    if (node7 != null)
                                    {
                                        this.CalculateConnections(this.nodes, node7, num35, num36, num37);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual void UpdatePenalty(LevelGridNode node)
        {
            node.Penalty = 0;
            node.Penalty = base.initialPenalty;
            if (base.penaltyPosition)
            {
                node.Penalty += (uint) Mathf.RoundToInt((node.position.y - base.penaltyPositionOffset) * base.penaltyPositionFactor);
            }
        }

        public override bool uniformWidthDepthGrid
        {
            get
            {
                return false;
            }
        }

        [CompilerGenerated]
        private sealed class <ScanInternal>c__Iterator15 : IEnumerator, IEnumerable<Progress>, IEnumerator<Progress>, IDisposable, IEnumerable
        {
            internal Progress $current;
            internal int $PC;
            private static OnScanStatus <>f__am$cache3;
            internal LayerGridGraph <>f__this;

            private static void <>m__24(Progress p)
            {
            }

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    if (<>f__am$cache3 == null)
                    {
                        <>f__am$cache3 = new OnScanStatus(LayerGridGraph.<ScanInternal>c__Iterator15.<>m__24);
                    }
                    this.<>f__this.ScanInternal(<>f__am$cache3);
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<Progress> IEnumerable<Progress>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                LayerGridGraph.<ScanInternal>c__Iterator15 iterator = new LayerGridGraph.<ScanInternal>c__Iterator15();
                iterator.<>f__this = this.<>f__this;
                return iterator;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<Pathfinding.Progress>.GetEnumerator();
            }

            Progress IEnumerator<Progress>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

