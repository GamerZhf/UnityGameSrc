namespace Pathfinding.RVO.Sampled
{
    using Pathfinding;
    using Pathfinding.RVO;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class Agent : IAgent
    {
        [CompilerGenerated]
        private float <AgentTimeHorizon>k__BackingField;
        [CompilerGenerated]
        private RVOLayer <CollidesWith>k__BackingField;
        [CompilerGenerated]
        private bool <DebugDraw>k__BackingField;
        [CompilerGenerated]
        private Vector3 <DesiredVelocity>k__BackingField;
        [CompilerGenerated]
        private float <Height>k__BackingField;
        [CompilerGenerated]
        private RVOLayer <Layer>k__BackingField;
        [CompilerGenerated]
        private bool <Locked>k__BackingField;
        [CompilerGenerated]
        private int <MaxNeighbours>k__BackingField;
        [CompilerGenerated]
        private float <MaxSpeed>k__BackingField;
        [CompilerGenerated]
        private float <NeighbourDist>k__BackingField;
        [CompilerGenerated]
        private float <ObstacleTimeHorizon>k__BackingField;
        [CompilerGenerated]
        private Vector3 <Position>k__BackingField;
        [CompilerGenerated]
        private float <Radius>k__BackingField;
        [CompilerGenerated]
        private Vector3 <Velocity>k__BackingField;
        public float agentTimeHorizon;
        private RVOLayer collidesWith;
        public Vector3 desiredVelocity;
        public static float DesiredVelocityScale = 0.1f;
        public static float DesiredVelocityWeight = 0.02f;
        public static float GlobalIncompressibility = 30f;
        public float height;
        private RVOLayer layer;
        public bool locked;
        public int maxNeighbours;
        public float maxSpeed;
        public float neighbourDist;
        public List<float> neighbourDists = new List<float>();
        public List<Agent> neighbours = new List<Agent>();
        internal Vector3 newVelocity;
        internal Agent next;
        private List<float> obstacleDists = new List<float>();
        private List<ObstacleVertex> obstacles = new List<ObstacleVertex>();
        private List<ObstacleVertex> obstaclesBuffered = new List<ObstacleVertex>();
        public float obstacleTimeHorizon;
        public Vector3 position;
        public Vector3 prevSmoothPos;
        public float radius;
        public Simulator simulator;
        private Vector3 smoothPos;
        private Vector3 velocity;
        private const float WallWeight = 5f;
        public static Stopwatch watch1 = new Stopwatch();
        public static Stopwatch watch2 = new Stopwatch();
        public float weight;

        public Agent(Vector3 pos)
        {
            this.MaxSpeed = 2f;
            this.NeighbourDist = 15f;
            this.AgentTimeHorizon = 2f;
            this.ObstacleTimeHorizon = 2f;
            this.Height = 5f;
            this.Radius = 5f;
            this.MaxNeighbours = 10;
            this.Locked = false;
            this.position = pos;
            this.Position = this.position;
            this.prevSmoothPos = this.position;
            this.smoothPos = this.position;
            this.Layer = RVOLayer.DefaultAgent;
            this.CollidesWith = -1;
        }

        public void BufferSwitch()
        {
            this.radius = this.Radius;
            this.height = this.Height;
            this.maxSpeed = this.MaxSpeed;
            this.neighbourDist = this.NeighbourDist;
            this.agentTimeHorizon = this.AgentTimeHorizon;
            this.obstacleTimeHorizon = this.ObstacleTimeHorizon;
            this.maxNeighbours = this.MaxNeighbours;
            this.desiredVelocity = this.DesiredVelocity;
            this.locked = this.Locked;
            this.collidesWith = this.CollidesWith;
            this.layer = this.Layer;
            this.Velocity = this.velocity;
            List<ObstacleVertex> obstaclesBuffered = this.obstaclesBuffered;
            this.obstaclesBuffered = this.obstacles;
            this.obstacles = obstaclesBuffered;
        }

        public void CalculateNeighbours()
        {
            this.neighbours.Clear();
            this.neighbourDists.Clear();
            if (!this.locked)
            {
                float num;
                if (this.MaxNeighbours > 0)
                {
                    num = this.neighbourDist * this.neighbourDist;
                    this.simulator.Quadtree.Query(new Vector2(this.position.x, this.position.z), this.neighbourDist, this);
                }
                this.obstacles.Clear();
                this.obstacleDists.Clear();
                num = (this.obstacleTimeHorizon * this.maxSpeed) + this.radius;
                num *= num;
            }
        }

        internal void CalculateVelocity(Simulator.WorkerContext context)
        {
            if (this.locked)
            {
                this.newVelocity = (Vector3) Vector2.zero;
            }
            else
            {
                if (context.vos.Length < (this.neighbours.Count + this.simulator.obstacles.Count))
                {
                    context.vos = new VO[Mathf.Max((int) (context.vos.Length * 2), (int) (this.neighbours.Count + this.simulator.obstacles.Count))];
                }
                Vector2 p = new Vector2(this.position.x, this.position.z);
                VO[] vos = context.vos;
                int index = 0;
                Vector2 vector2 = new Vector2(this.velocity.x, this.velocity.z);
                float inverseDt = 1f / this.agentTimeHorizon;
                float wallThickness = this.simulator.WallThickness;
                float weightFactor = (this.simulator.algorithm != Simulator.SamplingAlgorithm.GradientDescent) ? 5f : 1f;
                for (int i = 0; i < this.simulator.obstacles.Count; i++)
                {
                    ObstacleVertex vertex = this.simulator.obstacles[i];
                    ObstacleVertex next = vertex;
                    do
                    {
                        if ((next.ignore || (this.position.y > (next.position.y + next.height))) || (((this.position.y + this.height) < next.position.y) || ((next.layer & this.collidesWith) == 0)))
                        {
                            next = next.next;
                        }
                        else
                        {
                            float f = VO.Det(new Vector2(next.position.x, next.position.z), next.dir, p);
                            float num8 = Vector2.Dot(next.dir, p - new Vector2(next.position.x, next.position.z));
                            bool flag = (num8 <= (wallThickness * 0.05f)) || (num8 >= ((new Vector2(next.position.x, next.position.z) - new Vector2(next.next.position.x, next.next.position.z)).magnitude - (wallThickness * 0.05f)));
                            if (Mathf.Abs(f) < this.neighbourDist)
                            {
                                if (((f <= 0f) && !flag) && (f > -wallThickness))
                                {
                                    vos[index] = new VO(p, new Vector2(next.position.x, next.position.z) - p, next.dir, weightFactor * 2f);
                                    index++;
                                }
                                else if (f > 0f)
                                {
                                    Vector2 vector3 = new Vector2(next.position.x, next.position.z) - p;
                                    Vector2 vector4 = new Vector2(next.next.position.x, next.next.position.z) - p;
                                    Vector2 normalized = vector3.normalized;
                                    Vector2 vector6 = vector4.normalized;
                                    vos[index] = new VO(p, vector3, vector4, normalized, vector6, weightFactor);
                                    index++;
                                }
                            }
                            next = next.next;
                        }
                    }
                    while (next != vertex);
                }
                for (int j = 0; j < this.neighbours.Count; j++)
                {
                    Agent agent = this.neighbours[j];
                    if (agent != this)
                    {
                        float num10 = Math.Min((float) (this.position.y + this.height), (float) (agent.position.y + agent.height));
                        float num11 = Math.Max(this.position.y, agent.position.y);
                        if ((num10 - num11) >= 0f)
                        {
                            Vector2 vector10;
                            Vector2 vector7 = new Vector2(agent.Velocity.x, agent.velocity.z);
                            float radius = this.radius + agent.radius;
                            Vector2 center = new Vector2(agent.position.x, agent.position.z) - p;
                            Vector2 sideChooser = vector2 - vector7;
                            if (agent.locked)
                            {
                                vector10 = vector7;
                            }
                            else
                            {
                                vector10 = (Vector2) ((vector2 + vector7) * 0.5f);
                            }
                            vos[index] = new VO(center, vector10, radius, sideChooser, inverseDt, 1f);
                            index++;
                            if (this.DebugDraw)
                            {
                                DrawVO((p + ((Vector2) (center * inverseDt))) + vector10, radius * inverseDt, p + vector10);
                            }
                        }
                    }
                }
                Vector2 zero = Vector2.zero;
                if (this.simulator.algorithm == Simulator.SamplingAlgorithm.GradientDescent)
                {
                    float num23;
                    if (this.DebugDraw)
                    {
                        for (int k = 0; k < 40; k++)
                        {
                            for (int m = 0; m < 40; m++)
                            {
                                Vector2 vector12 = new Vector2((k * 15f) / 40f, (m * 15f) / 40f);
                                Vector2 vector13 = Vector2.zero;
                                float num17 = 0f;
                                for (int n = 0; n < index; n++)
                                {
                                    float num19;
                                    vector13 += vos[n].Sample(vector12 - p, out num19);
                                    if (num19 > num17)
                                    {
                                        num17 = num19;
                                    }
                                }
                                Vector2 vector14 = new Vector2(this.desiredVelocity.x, this.desiredVelocity.z) - (vector12 - p);
                                vector13 += (Vector2) (vector14 * DesiredVelocityScale);
                                if ((vector14.magnitude * DesiredVelocityWeight) > num17)
                                {
                                    num17 = vector14.magnitude * DesiredVelocityWeight;
                                }
                                if (num17 > 0f)
                                {
                                    vector13 = (Vector2) (vector13 / num17);
                                }
                                UnityEngine.Debug.DrawRay(To3D(vector12), To3D((Vector2) (vector14 * 0f)), Color.blue);
                                float score = 0f;
                                Vector2 vector15 = vector12 - ((Vector2) ((Vector2.one * 15f) * 0.5f));
                                Vector2 vector16 = this.Trace(vos, index, vector15, 0.01f, out score);
                                Vector2 vector26 = vector15 - vector16;
                                if (vector26.sqrMagnitude < (this.Sqr(0.375f) * 2.6f))
                                {
                                    UnityEngine.Debug.DrawRay(To3D(vector16 + p), (Vector3) (Vector3.up * 1f), Color.red);
                                }
                            }
                        }
                    }
                    float positiveInfinity = float.PositiveInfinity;
                    Vector2 vector27 = new Vector2(this.velocity.x, this.velocity.z);
                    float cutoff = vector27.magnitude * this.simulator.qualityCutoff;
                    zero = this.Trace(vos, index, new Vector2(this.desiredVelocity.x, this.desiredVelocity.z), cutoff, out positiveInfinity);
                    if (this.DebugDraw)
                    {
                        DrawCross(zero + p, Color.yellow, 0.5f);
                    }
                    Vector2 velocity = this.Velocity;
                    Vector2 vector18 = this.Trace(vos, index, velocity, cutoff, out num23);
                    if (num23 < positiveInfinity)
                    {
                        zero = vector18;
                        positiveInfinity = num23;
                    }
                    if (this.DebugDraw)
                    {
                        DrawCross(vector18 + p, Color.magenta, 0.5f);
                    }
                }
                else
                {
                    Vector2[] samplePos = context.samplePos;
                    float[] sampleSize = context.sampleSize;
                    int num24 = 0;
                    Vector2 vector19 = new Vector2(this.desiredVelocity.x, this.desiredVelocity.z);
                    float num25 = Mathf.Max(this.radius, Mathf.Max(vector19.magnitude, this.Velocity.magnitude));
                    samplePos[num24] = vector19;
                    sampleSize[num24] = num25 * 0.3f;
                    num24++;
                    samplePos[num24] = vector2;
                    sampleSize[num24] = num25 * 0.3f;
                    num24++;
                    Vector2 vector20 = (Vector2) (vector2 * 0.5f);
                    Vector2 vector21 = new Vector2(vector20.y, -vector20.x);
                    for (int num28 = 0; num28 < 8; num28++)
                    {
                        samplePos[num24] = (Vector2) ((vector21 * Mathf.Sin(((num28 * 3.141593f) * 2f) / 8f)) + (vector20 * (1f + Mathf.Cos(((num28 * 3.141593f) * 2f) / 8f))));
                        sampleSize[num24] = ((1f - (Mathf.Abs((float) (num28 - 4f)) / 8f)) * num25) * 0.5f;
                        num24++;
                    }
                    vector20 = (Vector2) (vector20 * 0.6f);
                    vector21 = (Vector2) (vector21 * 0.6f);
                    for (int num31 = 0; num31 < 6; num31++)
                    {
                        samplePos[num24] = (Vector2) ((vector21 * Mathf.Cos((((num31 + 0.5f) * 3.141593f) * 2f) / 6f)) + (vector20 * (1.666667f + Mathf.Sin((((num31 + 0.5f) * 3.141593f) * 2f) / 6f))));
                        sampleSize[num24] = num25 * 0.3f;
                        num24++;
                    }
                    for (int num34 = 0; num34 < 6; num34++)
                    {
                        samplePos[num24] = vector2 + new Vector2((num25 * 0.2f) * Mathf.Cos((((num34 + 0.5f) * 3.141593f) * 2f) / 6f), (num25 * 0.2f) * Mathf.Sin((((num34 + 0.5f) * 3.141593f) * 2f) / 6f));
                        sampleSize[num24] = (num25 * 0.2f) * 2f;
                        num24++;
                    }
                    samplePos[num24] = (Vector2) (vector2 * 0.5f);
                    sampleSize[num24] = num25 * 0.4f;
                    num24++;
                    Vector2[] bestPos = context.bestPos;
                    float[] bestSizes = context.bestSizes;
                    float[] bestScores = context.bestScores;
                    for (int num36 = 0; num36 < 3; num36++)
                    {
                        bestScores[num36] = float.PositiveInfinity;
                    }
                    bestScores[3] = float.NegativeInfinity;
                    Vector2 vector22 = vector2;
                    float num37 = float.PositiveInfinity;
                    for (int num38 = 0; num38 < 3; num38++)
                    {
                        for (int num39 = 0; num39 < num24; num39++)
                        {
                            float num40 = 0f;
                            for (int num41 = 0; num41 < index; num41++)
                            {
                                num40 = Math.Max(num40, vos[num41].ScalarSample(samplePos[num39]));
                            }
                            Vector2 vector29 = samplePos[num39] - vector19;
                            float magnitude = vector29.magnitude;
                            float num43 = num40 + (magnitude * DesiredVelocityWeight);
                            num40 += magnitude * 0.001f;
                            if (this.DebugDraw)
                            {
                                DrawCross(p + samplePos[num39], Rainbow(Mathf.Log(num40 + 1f) * 5f), sampleSize[num39] * 0.5f);
                            }
                            if (num43 < bestScores[0])
                            {
                                for (int num44 = 0; num44 < 3; num44++)
                                {
                                    if (num43 >= bestScores[num44 + 1])
                                    {
                                        bestScores[num44] = num43;
                                        bestSizes[num44] = sampleSize[num39];
                                        bestPos[num44] = samplePos[num39];
                                        break;
                                    }
                                }
                            }
                            if (num40 < num37)
                            {
                                vector22 = samplePos[num39];
                                num37 = num40;
                                if (num40 == 0f)
                                {
                                    num38 = 100;
                                    break;
                                }
                            }
                        }
                        num24 = 0;
                        for (int num45 = 0; num45 < 3; num45++)
                        {
                            Vector2 vector23 = bestPos[num45];
                            float num46 = bestSizes[num45];
                            bestScores[num45] = float.PositiveInfinity;
                            float x = (num46 * 0.6f) * 0.5f;
                            samplePos[num24] = vector23 + new Vector2(x, x);
                            samplePos[num24 + 1] = vector23 + new Vector2(-x, x);
                            samplePos[num24 + 2] = vector23 + new Vector2(-x, -x);
                            samplePos[num24 + 3] = vector23 + new Vector2(x, -x);
                            num46 *= num46 * 0.6f;
                            sampleSize[num24] = num46;
                            sampleSize[num24 + 1] = num46;
                            sampleSize[num24 + 2] = num46;
                            sampleSize[num24 + 3] = num46;
                            num24 += 4;
                        }
                    }
                    zero = vector22;
                }
                if (this.DebugDraw)
                {
                    DrawCross(zero + p, 1f);
                }
                this.newVelocity = To3D(Vector2.ClampMagnitude(zero, this.maxSpeed));
            }
        }

        private static void DrawCircle(Vector2 _p, float radius, Color col)
        {
            DrawCircle(_p, radius, 0f, 6.283185f, col);
        }

        private static void DrawCircle(Vector2 _p, float radius, float a0, float a1, Color col)
        {
            Vector3 vector = To3D(_p);
            while (a0 > a1)
            {
                a0 -= 6.283185f;
            }
            Vector3 vector2 = new Vector3(Mathf.Cos(a0) * radius, 0f, Mathf.Sin(a0) * radius);
            for (int i = 0; i <= 40f; i++)
            {
                Vector3 vector3 = new Vector3(Mathf.Cos(Mathf.Lerp(a0, a1, ((float) i) / 40f)) * radius, 0f, Mathf.Sin(Mathf.Lerp(a0, a1, ((float) i) / 40f)) * radius);
                UnityEngine.Debug.DrawLine(vector + vector2, vector + vector3, col);
                vector2 = vector3;
            }
        }

        private static void DrawCross(Vector2 p, [Optional, DefaultParameterValue(1)] float size)
        {
            DrawCross(p, Color.white, size);
        }

        private static void DrawCross(Vector2 p, Color col, [Optional, DefaultParameterValue(1)] float size)
        {
            size *= 0.5f;
            UnityEngine.Debug.DrawLine(new Vector3(p.x, 0f, p.y) - ((Vector3) (Vector3.right * size)), new Vector3(p.x, 0f, p.y) + ((Vector3) (Vector3.right * size)), col);
            UnityEngine.Debug.DrawLine(new Vector3(p.x, 0f, p.y) - ((Vector3) (Vector3.forward * size)), new Vector3(p.x, 0f, p.y) + ((Vector3) (Vector3.forward * size)), col);
        }

        private static void DrawVO(Vector2 circleCenter, float radius, Vector2 origin)
        {
            Vector2 vector5 = origin - circleCenter;
            Vector2 vector6 = origin - circleCenter;
            float num = Mathf.Atan2(vector5.y, vector6.x);
            Vector2 vector7 = origin - circleCenter;
            float f = radius / vector7.magnitude;
            float num3 = (f > 1f) ? 0f : Mathf.Abs(Mathf.Acos(f));
            DrawCircle(circleCenter, radius, num - num3, num + num3, Color.black);
            Vector2 p = (Vector2) (new Vector2(Mathf.Cos(num - num3), Mathf.Sin(num - num3)) * radius);
            Vector2 vector2 = (Vector2) (new Vector2(Mathf.Cos(num + num3), Mathf.Sin(num + num3)) * radius);
            Vector2 vector3 = -new Vector2(-p.y, p.x);
            Vector2 vector4 = new Vector2(-vector2.y, vector2.x);
            p += circleCenter;
            vector2 += circleCenter;
            UnityEngine.Debug.DrawRay(To3D(p), (Vector3) (To3D(vector3).normalized * 100f), Color.black);
            UnityEngine.Debug.DrawRay(To3D(vector2), (Vector3) (To3D(vector4).normalized * 100f), Color.black);
        }

        public float InsertAgentNeighbour(Agent agent, float rangeSq)
        {
            if (this != agent)
            {
                if ((agent.layer & this.collidesWith) == 0)
                {
                    return rangeSq;
                }
                float item = this.Sqr(agent.position.x - this.position.x) + this.Sqr(agent.position.z - this.position.z);
                if (item >= rangeSq)
                {
                    return rangeSq;
                }
                if (this.neighbours.Count < this.maxNeighbours)
                {
                    this.neighbours.Add(agent);
                    this.neighbourDists.Add(item);
                }
                int num2 = this.neighbours.Count - 1;
                if (item < this.neighbourDists[num2])
                {
                    while ((num2 != 0) && (item < this.neighbourDists[num2 - 1]))
                    {
                        this.neighbours[num2] = this.neighbours[num2 - 1];
                        this.neighbourDists[num2] = this.neighbourDists[num2 - 1];
                        num2--;
                    }
                    this.neighbours[num2] = agent;
                    this.neighbourDists[num2] = item;
                }
                if (this.neighbours.Count == this.maxNeighbours)
                {
                    rangeSq = this.neighbourDists[this.neighbourDists.Count - 1];
                }
            }
            return rangeSq;
        }

        public void InsertObstacleNeighbour(ObstacleVertex ob1, float rangeSq)
        {
            ObstacleVertex next = ob1.next;
            float item = VectorMath.SqrDistancePointSegment(ob1.position, next.position, this.Position);
            if (item < rangeSq)
            {
                this.obstacles.Add(ob1);
                this.obstacleDists.Add(item);
                int num2 = this.obstacles.Count - 1;
                while ((num2 != 0) && (item < this.obstacleDists[num2 - 1]))
                {
                    this.obstacles[num2] = this.obstacles[num2 - 1];
                    this.obstacleDists[num2] = this.obstacleDists[num2 - 1];
                    num2--;
                }
                this.obstacles[num2] = ob1;
                this.obstacleDists[num2] = item;
            }
        }

        public void Interpolate(float t)
        {
            this.smoothPos = this.prevSmoothPos + ((Vector3) ((this.Position - this.prevSmoothPos) * t));
        }

        public static bool IntersectionFactor(Vector2 start1, Vector2 dir1, Vector2 start2, Vector2 dir2, out float factor)
        {
            float num = (dir2.y * dir1.x) - (dir2.x * dir1.y);
            if (num == 0f)
            {
                factor = 0f;
                return false;
            }
            float num2 = (dir2.x * (start1.y - start2.y)) - (dir2.y * (start1.x - start2.x));
            factor = num2 / num;
            return true;
        }

        private static Color Rainbow(float v)
        {
            Color color = new Color(v, 0f, 0f);
            if (color.r > 1f)
            {
                color.g = color.r - 1f;
                color.r = 1f;
            }
            if (color.g > 1f)
            {
                color.b = color.g - 1f;
                color.g = 1f;
            }
            return color;
        }

        public void SetYPosition(float yCoordinate)
        {
            this.Position = new Vector3(this.Position.x, yCoordinate, this.Position.z);
            this.smoothPos.y = yCoordinate;
            this.prevSmoothPos.y = yCoordinate;
        }

        private float Sqr(float x)
        {
            return (x * x);
        }

        public void Teleport(Vector3 pos)
        {
            this.Position = pos;
            this.smoothPos = pos;
            this.prevSmoothPos = pos;
        }

        private static Vector3 To3D(Vector2 p)
        {
            return new Vector3(p.x, 0f, p.y);
        }

        private Vector2 Trace(VO[] vos, int voCount, Vector2 p, float cutoff, out float score)
        {
            score = 0f;
            float stepScale = this.simulator.stepScale;
            float positiveInfinity = float.PositiveInfinity;
            Vector2 vector = p;
            for (int i = 0; i < 50; i++)
            {
                float num4 = 1f - (((float) i) / 50f);
                num4 *= stepScale;
                Vector2 zero = Vector2.zero;
                float num5 = 0f;
                for (int j = 0; j < voCount; j++)
                {
                    float num7;
                    Vector2 vector3 = vos[j].Sample(p, out num7);
                    zero += vector3;
                    if (num7 > num5)
                    {
                        num5 = num7;
                    }
                }
                Vector2 vector4 = new Vector2(this.desiredVelocity.x, this.desiredVelocity.z) - p;
                float num8 = vector4.magnitude * DesiredVelocityWeight;
                zero += (Vector2) (vector4 * DesiredVelocityScale);
                num5 = Math.Max(num5, num8);
                score = num5;
                if (score < positiveInfinity)
                {
                    positiveInfinity = score;
                }
                vector = p;
                if ((score <= cutoff) && (i > 10))
                {
                    break;
                }
                float sqrMagnitude = zero.sqrMagnitude;
                if (sqrMagnitude > 0f)
                {
                    zero = (Vector2) (zero * (num5 / Mathf.Sqrt(sqrMagnitude)));
                }
                zero = (Vector2) (zero * num4);
                Vector2 vector5 = p;
                p += zero;
                if (this.DebugDraw)
                {
                    UnityEngine.Debug.DrawLine(To3D(vector5) + this.position, To3D(p) + this.position, Rainbow(0.1f / score) * new Color(1f, 1f, 1f, 0.2f));
                }
            }
            score = positiveInfinity;
            return vector;
        }

        public void Update()
        {
            this.velocity = this.newVelocity;
            this.prevSmoothPos = this.smoothPos;
            this.position = this.prevSmoothPos;
            this.position += (Vector3) (this.velocity * this.simulator.DeltaTime);
            this.Position = this.position;
        }

        public float AgentTimeHorizon
        {
            [CompilerGenerated]
            get
            {
                return this.<AgentTimeHorizon>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<AgentTimeHorizon>k__BackingField = value;
            }
        }

        public RVOLayer CollidesWith
        {
            [CompilerGenerated]
            get
            {
                return this.<CollidesWith>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<CollidesWith>k__BackingField = value;
            }
        }

        public bool DebugDraw
        {
            [CompilerGenerated]
            get
            {
                return this.<DebugDraw>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<DebugDraw>k__BackingField = value;
            }
        }

        public Vector3 DesiredVelocity
        {
            [CompilerGenerated]
            get
            {
                return this.<DesiredVelocity>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<DesiredVelocity>k__BackingField = value;
            }
        }

        public float Height
        {
            [CompilerGenerated]
            get
            {
                return this.<Height>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Height>k__BackingField = value;
            }
        }

        public Vector3 InterpolatedPosition
        {
            get
            {
                return this.smoothPos;
            }
        }

        public RVOLayer Layer
        {
            [CompilerGenerated]
            get
            {
                return this.<Layer>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Layer>k__BackingField = value;
            }
        }

        public bool Locked
        {
            [CompilerGenerated]
            get
            {
                return this.<Locked>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Locked>k__BackingField = value;
            }
        }

        public int MaxNeighbours
        {
            [CompilerGenerated]
            get
            {
                return this.<MaxNeighbours>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<MaxNeighbours>k__BackingField = value;
            }
        }

        public float MaxSpeed
        {
            [CompilerGenerated]
            get
            {
                return this.<MaxSpeed>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<MaxSpeed>k__BackingField = value;
            }
        }

        public float NeighbourDist
        {
            [CompilerGenerated]
            get
            {
                return this.<NeighbourDist>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<NeighbourDist>k__BackingField = value;
            }
        }

        public List<ObstacleVertex> NeighbourObstacles
        {
            get
            {
                return null;
            }
        }

        public float ObstacleTimeHorizon
        {
            [CompilerGenerated]
            get
            {
                return this.<ObstacleTimeHorizon>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ObstacleTimeHorizon>k__BackingField = value;
            }
        }

        public Vector3 Position
        {
            [CompilerGenerated]
            get
            {
                return this.<Position>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Position>k__BackingField = value;
            }
        }

        public float Radius
        {
            [CompilerGenerated]
            get
            {
                return this.<Radius>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Radius>k__BackingField = value;
            }
        }

        public Vector3 Velocity
        {
            [CompilerGenerated]
            get
            {
                return this.<Velocity>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<Velocity>k__BackingField = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct VO
        {
            public Vector2 origin;
            public Vector2 center;
            private Vector2 line1;
            private Vector2 line2;
            private Vector2 dir1;
            private Vector2 dir2;
            private Vector2 cutoffLine;
            private Vector2 cutoffDir;
            private float sqrCutoffDistance;
            private bool leftSide;
            private bool colliding;
            private float radius;
            private float weightFactor;
            public VO(Vector2 offset, Vector2 p0, Vector2 dir, float weightFactor)
            {
                this.colliding = true;
                this.line1 = p0;
                this.dir1 = -dir;
                this.origin = Vector2.zero;
                this.center = Vector2.zero;
                this.line2 = Vector2.zero;
                this.dir2 = Vector2.zero;
                this.cutoffLine = Vector2.zero;
                this.cutoffDir = Vector2.zero;
                this.sqrCutoffDistance = 0f;
                this.leftSide = false;
                this.radius = 0f;
                this.weightFactor = weightFactor * 0.5f;
            }

            public VO(Vector2 offset, Vector2 p1, Vector2 p2, Vector2 tang1, Vector2 tang2, float weightFactor)
            {
                this.weightFactor = weightFactor * 0.5f;
                this.colliding = false;
                this.cutoffLine = p1;
                Vector2 vector = p2 - p1;
                this.cutoffDir = vector.normalized;
                this.line1 = p1;
                this.dir1 = tang1;
                this.line2 = p2;
                this.dir2 = tang2;
                this.dir2 = -this.dir2;
                this.cutoffDir = -this.cutoffDir;
                this.origin = Vector2.zero;
                this.center = Vector2.zero;
                this.sqrCutoffDistance = 0f;
                this.leftSide = false;
                this.radius = 0f;
                weightFactor = 5f;
            }

            public VO(Vector2 center, Vector2 offset, float radius, Vector2 sideChooser, float inverseDt, float weightFactor)
            {
                this.weightFactor = weightFactor * 0.5f;
                this.origin = offset;
                weightFactor = 0.5f;
                if (center.magnitude < radius)
                {
                    this.colliding = true;
                    this.leftSide = false;
                    this.line1 = (Vector2) (center.normalized * (center.magnitude - radius));
                    Vector2 vector2 = new Vector2(this.line1.y, -this.line1.x);
                    this.dir1 = vector2.normalized;
                    this.line1 += offset;
                    this.cutoffDir = Vector2.zero;
                    this.cutoffLine = Vector2.zero;
                    this.sqrCutoffDistance = 0f;
                    this.dir2 = Vector2.zero;
                    this.line2 = Vector2.zero;
                    this.center = Vector2.zero;
                    this.radius = 0f;
                }
                else
                {
                    this.colliding = false;
                    center = (Vector2) (center * inverseDt);
                    radius *= inverseDt;
                    Vector2 vector = center + offset;
                    this.sqrCutoffDistance = center.magnitude - radius;
                    this.center = center;
                    this.cutoffLine = (Vector2) (center.normalized * this.sqrCutoffDistance);
                    Vector2 vector3 = new Vector2(-this.cutoffLine.y, this.cutoffLine.x);
                    this.cutoffDir = vector3.normalized;
                    this.cutoffLine += offset;
                    this.sqrCutoffDistance *= this.sqrCutoffDistance;
                    float num = Mathf.Atan2(-center.y, -center.x);
                    float num2 = Mathf.Abs(Mathf.Acos(radius / center.magnitude));
                    this.radius = radius;
                    this.leftSide = VectorMath.RightOrColinear(Vector2.zero, center, sideChooser);
                    this.line1 = (Vector2) (new Vector2(Mathf.Cos(num + num2), Mathf.Sin(num + num2)) * radius);
                    Vector2 vector4 = new Vector2(this.line1.y, -this.line1.x);
                    this.dir1 = vector4.normalized;
                    this.line2 = (Vector2) (new Vector2(Mathf.Cos(num - num2), Mathf.Sin(num - num2)) * radius);
                    Vector2 vector5 = new Vector2(this.line2.y, -this.line2.x);
                    this.dir2 = vector5.normalized;
                    this.line1 += vector;
                    this.line2 += vector;
                }
            }

            public static bool Left(Vector2 a, Vector2 dir, Vector2 p)
            {
                return (((dir.x * (p.y - a.y)) - ((p.x - a.x) * dir.y)) <= 0f);
            }

            public static float Det(Vector2 a, Vector2 dir, Vector2 p)
            {
                return (((p.x - a.x) * dir.y) - (dir.x * (p.y - a.y)));
            }

            public Vector2 Sample(Vector2 p, out float weight)
            {
                if (this.colliding)
                {
                    float num = Det(this.line1, this.dir1, p);
                    if (num >= 0f)
                    {
                        weight = num * this.weightFactor;
                        return (Vector2) ((new Vector2(-this.dir1.y, this.dir1.x) * weight) * Agent.GlobalIncompressibility);
                    }
                    weight = 0f;
                    return new Vector2(0f, 0f);
                }
                float num2 = Det(this.cutoffLine, this.cutoffDir, p);
                if (num2 <= 0f)
                {
                    weight = 0f;
                    return Vector2.zero;
                }
                float num3 = Det(this.line1, this.dir1, p);
                float num4 = Det(this.line2, this.dir2, p);
                if ((num3 >= 0f) && (num4 >= 0f))
                {
                    if (this.leftSide)
                    {
                        if (num2 < this.radius)
                        {
                            weight = num2 * this.weightFactor;
                            return (new Vector2(-this.cutoffDir.y, this.cutoffDir.x) * weight);
                        }
                        weight = num3;
                        return (new Vector2(-this.dir1.y, this.dir1.x) * weight);
                    }
                    if (num2 < this.radius)
                    {
                        weight = num2 * this.weightFactor;
                        return (new Vector2(-this.cutoffDir.y, this.cutoffDir.x) * weight);
                    }
                    weight = num4 * this.weightFactor;
                    return (new Vector2(-this.dir2.y, this.dir2.x) * weight);
                }
                weight = 0f;
                return new Vector2(0f, 0f);
            }

            public float ScalarSample(Vector2 p)
            {
                if (this.colliding)
                {
                    float num = Det(this.line1, this.dir1, p);
                    if (num >= 0f)
                    {
                        return ((num * Agent.GlobalIncompressibility) * this.weightFactor);
                    }
                    return 0f;
                }
                float num2 = Det(this.cutoffLine, this.cutoffDir, p);
                if (num2 <= 0f)
                {
                    return 0f;
                }
                float num3 = Det(this.line1, this.dir1, p);
                float num4 = Det(this.line2, this.dir2, p);
                if ((num3 < 0f) || (num4 < 0f))
                {
                    return 0f;
                }
                if (this.leftSide)
                {
                    if (num2 < this.radius)
                    {
                        return (num2 * this.weightFactor);
                    }
                    return (num3 * this.weightFactor);
                }
                if (num2 < this.radius)
                {
                    return (num2 * this.weightFactor);
                }
                return (num4 * this.weightFactor);
            }
        }
    }
}

