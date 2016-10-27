namespace GameLogic
{
    using Pathfinding;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class PathfindingSystem : MonoBehaviour, IPathfindingSystem
    {
        private NNConstraint m_defaultAstarConstraint = new NNConstraint();
        private List<GraphNode> m_tempNodeConnections = new List<GraphNode>();
        private List<GraphNode> m_temporarilyOccupiedNodes = new List<GraphNode>(0x40);
        public const int TAG_ENEMY = 0x1d;
        public const int TAG_FREE = 0;
        public const int TAG_PLAYER_HERO = 30;
        public const int TAG_PLAYER_SUPPORT = 0x1f;

        private void addNodeConnection(GraphNode gn)
        {
            this.m_tempNodeConnections.Add(gn);
        }

        protected void Awake()
        {
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = Binder.GameState.ActiveDungeon;
            if ((activeDungeon != null) && (activeDungeon.ActiveRoom != null))
            {
                this.freeOccupiedNodes();
                for (int i = 0; i < activeDungeon.ActiveRoom.ActiveCharacters.Count; i++)
                {
                    CharacterInstance instance = activeDungeon.ActiveRoom.ActiveCharacters[i];
                    if (!instance.IsDead)
                    {
                        uint tag = 0;
                        if (instance.IsSupport)
                        {
                            tag = 0x1f;
                        }
                        else if (instance.IsPlayerCharacter)
                        {
                            tag = 30;
                        }
                        else
                        {
                            tag = 0x1d;
                        }
                        NNInfo nearest = activeDungeon.ActiveRoom.AstarPath.GetNearest(instance.PhysicsBody.Transform.position, this.m_defaultAstarConstraint);
                        if (nearest.node != null)
                        {
                            this.occupyNode(nearest.node, tag);
                            if (!instance.IsPlayerCharacter)
                            {
                                List<GraphNode> list = this.getNodeConnections(nearest);
                                for (int j = 0; j < list.Count; j++)
                                {
                                    this.occupyNode(list[j], tag);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void freeOccupiedNodes()
        {
            for (int i = 0; i < this.m_temporarilyOccupiedNodes.Count; i++)
            {
                this.m_temporarilyOccupiedNodes[i].Tag = 0;
            }
            this.m_temporarilyOccupiedNodes.Clear();
        }

        private List<GraphNode> getNodeConnections(NNInfo info)
        {
            this.m_tempNodeConnections.Clear();
            info.node.GetConnections(new GraphNodeDelegate(this.addNodeConnection));
            return this.m_tempNodeConnections;
        }

        private bool nodeTemporarilyOccupied(GraphNode gn)
        {
            for (int i = 0; i < this.m_temporarilyOccupiedNodes.Count; i++)
            {
                if (this.m_temporarilyOccupiedNodes[i] == gn)
                {
                    return true;
                }
            }
            return false;
        }

        private void occupyNode(GraphNode gn, uint tag)
        {
            if ((gn.Tag == 0) && !this.nodeTemporarilyOccupied(gn))
            {
                gn.Tag = tag;
                this.m_temporarilyOccupiedNodes.Add(gn);
            }
        }

        protected void OnDisable()
        {
        }

        protected void OnEnable()
        {
        }
    }
}

