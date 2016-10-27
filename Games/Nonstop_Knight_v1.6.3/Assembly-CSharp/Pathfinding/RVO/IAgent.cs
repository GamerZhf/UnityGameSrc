namespace Pathfinding.RVO
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IAgent
    {
        void SetYPosition(float yCoordinate);
        void Teleport(Vector3 pos);

        float AgentTimeHorizon { get; set; }

        RVOLayer CollidesWith { get; set; }

        bool DebugDraw { get; set; }

        Vector3 DesiredVelocity { get; set; }

        float Height { get; set; }

        Vector3 InterpolatedPosition { get; }

        RVOLayer Layer { get; set; }

        bool Locked { get; set; }

        int MaxNeighbours { get; set; }

        float MaxSpeed { get; set; }

        float NeighbourDist { get; set; }

        List<ObstacleVertex> NeighbourObstacles { get; }

        float ObstacleTimeHorizon { get; set; }

        Vector3 Position { get; }

        float Radius { get; set; }

        Vector3 Velocity { get; set; }
    }
}

