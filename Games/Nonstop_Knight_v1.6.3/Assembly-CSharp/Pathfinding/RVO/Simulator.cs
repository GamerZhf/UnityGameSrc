namespace Pathfinding.RVO
{
    using Pathfinding.RVO.Sampled;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    public class Simulator
    {
        private List<Agent> agents;
        public SamplingAlgorithm algorithm;
        private WorkerContext coroutineWorkerContext = new WorkerContext();
        private float deltaTime;
        private float desiredDeltaTime = 0.05f;
        private bool doCleanObstacles;
        private bool doubleBuffering = true;
        private bool doUpdateObstacles;
        private bool interpolation = true;
        private float lastStep = -99999f;
        private float lastStepInterpolationReference = -9999f;
        public List<ObstacleVertex> obstacles;
        private bool oversampling;
        private float prevDeltaTime;
        private RVOQuadtree quadtree = new RVOQuadtree();
        public float qualityCutoff = 0.05f;
        public float stepScale = 1.5f;
        private float wallThickness = 1f;
        private Worker[] workers;

        public Simulator(int workers, bool doubleBuffering)
        {
            this.workers = new Worker[workers];
            this.doubleBuffering = doubleBuffering;
            for (int i = 0; i < workers; i++)
            {
                this.workers[i] = new Worker(this);
            }
            this.agents = new List<Agent>();
            this.obstacles = new List<ObstacleVertex>();
        }

        public IAgent AddAgent(IAgent agent)
        {
            if (agent == null)
            {
                throw new ArgumentNullException("Agent must not be null");
            }
            Agent item = agent as Agent;
            if (item == null)
            {
                throw new ArgumentException("The agent must be of type Agent. Agent was of type " + agent.GetType());
            }
            if ((item.simulator != null) && (item.simulator == this))
            {
                throw new ArgumentException("The agent is already in the simulation");
            }
            if (item.simulator != null)
            {
                throw new ArgumentException("The agent is already added to another simulation");
            }
            item.simulator = this;
            if (this.Multithreading && this.doubleBuffering)
            {
                for (int i = 0; i < this.workers.Length; i++)
                {
                    this.workers[i].WaitOne();
                }
            }
            this.agents.Add(item);
            return agent;
        }

        public IAgent AddAgent(Vector3 position)
        {
            Agent item = new Agent(position);
            if (this.Multithreading && this.doubleBuffering)
            {
                for (int i = 0; i < this.workers.Length; i++)
                {
                    this.workers[i].WaitOne();
                }
            }
            this.agents.Add(item);
            item.simulator = this;
            return item;
        }

        public ObstacleVertex AddObstacle(ObstacleVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("Obstacle must not be null");
            }
            if (this.Multithreading && this.doubleBuffering)
            {
                for (int i = 0; i < this.workers.Length; i++)
                {
                    this.workers[i].WaitOne();
                }
            }
            this.obstacles.Add(v);
            this.UpdateObstacles();
            return v;
        }

        public ObstacleVertex AddObstacle(Vector3[] vertices, float height)
        {
            return this.AddObstacle(vertices, height, Matrix4x4.identity, RVOLayer.DefaultObstacle);
        }

        public ObstacleVertex AddObstacle(Vector3 a, Vector3 b, float height)
        {
            ObstacleVertex item = new ObstacleVertex();
            ObstacleVertex vertex2 = new ObstacleVertex();
            item.layer = RVOLayer.DefaultObstacle;
            vertex2.layer = RVOLayer.DefaultObstacle;
            item.prev = vertex2;
            vertex2.prev = item;
            item.next = vertex2;
            vertex2.next = item;
            item.position = a;
            vertex2.position = b;
            item.height = height;
            vertex2.height = height;
            vertex2.ignore = true;
            Vector2 vector = new Vector2(b.x - a.x, b.z - a.z);
            item.dir = vector.normalized;
            vertex2.dir = -item.dir;
            if (this.Multithreading && this.doubleBuffering)
            {
                for (int i = 0; i < this.workers.Length; i++)
                {
                    this.workers[i].WaitOne();
                }
            }
            this.obstacles.Add(item);
            this.UpdateObstacles();
            return item;
        }

        public ObstacleVertex AddObstacle(Vector3[] vertices, float height, Matrix4x4 matrix, [Optional, DefaultParameterValue(2)] RVOLayer layer)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException("Vertices must not be null");
            }
            if (vertices.Length < 2)
            {
                throw new ArgumentException("Less than 2 vertices in an obstacle");
            }
            ObstacleVertex item = null;
            ObstacleVertex vertex2 = null;
            bool flag = matrix == Matrix4x4.identity;
            if (this.Multithreading && this.doubleBuffering)
            {
                for (int j = 0; j < this.workers.Length; j++)
                {
                    this.workers[j].WaitOne();
                }
            }
            for (int i = 0; i < vertices.Length; i++)
            {
                ObstacleVertex vertex3 = new ObstacleVertex();
                if (item == null)
                {
                    item = vertex3;
                }
                else
                {
                    vertex2.next = vertex3;
                }
                vertex3.prev = vertex2;
                vertex3.layer = layer;
                vertex3.position = !flag ? matrix.MultiplyPoint3x4(vertices[i]) : vertices[i];
                vertex3.height = height;
                vertex2 = vertex3;
            }
            vertex2.next = item;
            item.prev = vertex2;
            ObstacleVertex next = item;
            do
            {
                Vector3 vector = next.next.position - next.position;
                Vector2 vector2 = new Vector2(vector.x, vector.z);
                next.dir = vector2.normalized;
                next = next.next;
            }
            while (next != item);
            this.obstacles.Add(item);
            this.UpdateObstacles();
            return item;
        }

        private void BuildQuadtree()
        {
            this.quadtree.Clear();
            if (this.agents.Count > 0)
            {
                Rect r = Rect.MinMaxRect(this.agents[0].position.x, this.agents[0].position.y, this.agents[0].position.x, this.agents[0].position.y);
                for (int i = 1; i < this.agents.Count; i++)
                {
                    Vector3 position = this.agents[i].position;
                    float left = Mathf.Min(r.xMin, position.x);
                    float top = Mathf.Min(r.yMin, position.z);
                    float right = Mathf.Max(r.xMax, position.x);
                    r = Rect.MinMaxRect(left, top, right, Mathf.Max(r.yMax, position.z));
                }
                this.quadtree.SetBounds(r);
                for (int j = 0; j < this.agents.Count; j++)
                {
                    this.quadtree.Insert(this.agents[j]);
                }
            }
        }

        private void CleanObstacles()
        {
            for (int i = 0; i < this.obstacles.Count; i++)
            {
                ObstacleVertex vertex = this.obstacles[i];
                ObstacleVertex next = vertex;
                do
                {
                    while (next.next.split)
                    {
                        next.next = next.next.next;
                        next.next.prev = next;
                    }
                    next = next.next;
                }
                while (next != vertex);
            }
        }

        public void ClearAgents()
        {
            if (this.Multithreading && this.doubleBuffering)
            {
                for (int j = 0; j < this.workers.Length; j++)
                {
                    this.workers[j].WaitOne();
                }
            }
            for (int i = 0; i < this.agents.Count; i++)
            {
                this.agents[i].simulator = null;
            }
            this.agents.Clear();
        }

        ~Simulator()
        {
            this.OnDestroy();
        }

        public List<Agent> GetAgents()
        {
            return this.agents;
        }

        public List<ObstacleVertex> GetObstacles()
        {
            return this.obstacles;
        }

        public void OnDestroy()
        {
            if (this.workers != null)
            {
                for (int i = 0; i < this.workers.Length; i++)
                {
                    this.workers[i].Terminate();
                }
            }
        }

        public void RemoveAgent(IAgent agent)
        {
            if (agent == null)
            {
                throw new ArgumentNullException("Agent must not be null");
            }
            Agent item = agent as Agent;
            if (item == null)
            {
                throw new ArgumentException("The agent must be of type Agent. Agent was of type " + agent.GetType());
            }
            if (item.simulator != this)
            {
                throw new ArgumentException("The agent is not added to this simulation");
            }
            if (this.Multithreading && this.doubleBuffering)
            {
                for (int i = 0; i < this.workers.Length; i++)
                {
                    this.workers[i].WaitOne();
                }
            }
            item.simulator = null;
            if (!this.agents.Remove(item))
            {
                throw new ArgumentException("Critical Bug! This should not happen. Please report this.");
            }
        }

        public void RemoveObstacle(ObstacleVertex v)
        {
            if (v == null)
            {
                throw new ArgumentNullException("Vertex must not be null");
            }
            if (this.Multithreading && this.doubleBuffering)
            {
                for (int i = 0; i < this.workers.Length; i++)
                {
                    this.workers[i].WaitOne();
                }
            }
            this.obstacles.Remove(v);
            this.UpdateObstacles();
        }

        private void ScheduleCleanObstacles()
        {
            this.doCleanObstacles = true;
        }

        public void Update()
        {
            if (this.lastStep < 0f)
            {
                this.lastStep = Time.time;
                this.deltaTime = this.DesiredDeltaTime;
                this.prevDeltaTime = this.deltaTime;
                this.lastStepInterpolationReference = this.lastStep;
            }
            if ((Time.time - this.lastStep) >= this.DesiredDeltaTime)
            {
                for (int i = 0; i < this.agents.Count; i++)
                {
                    this.agents[i].Interpolate((Time.time - this.lastStepInterpolationReference) / this.DeltaTime);
                }
                this.lastStepInterpolationReference = Time.time;
                this.prevDeltaTime = this.DeltaTime;
                this.deltaTime = Time.time - this.lastStep;
                this.lastStep = Time.time;
                this.deltaTime = Math.Max(this.deltaTime, 0.0005f);
                if (this.Multithreading)
                {
                    if (this.doubleBuffering)
                    {
                        for (int num2 = 0; num2 < this.workers.Length; num2++)
                        {
                            this.workers[num2].WaitOne();
                        }
                        if (!this.Interpolation)
                        {
                            for (int num3 = 0; num3 < this.agents.Count; num3++)
                            {
                                this.agents[num3].Interpolate(1f);
                            }
                        }
                    }
                    if (this.doCleanObstacles)
                    {
                        this.CleanObstacles();
                        this.doCleanObstacles = false;
                        this.doUpdateObstacles = true;
                    }
                    if (this.doUpdateObstacles)
                    {
                        this.doUpdateObstacles = false;
                    }
                    this.BuildQuadtree();
                    for (int j = 0; j < this.workers.Length; j++)
                    {
                        this.workers[j].start = (j * this.agents.Count) / this.workers.Length;
                        this.workers[j].end = ((j + 1) * this.agents.Count) / this.workers.Length;
                    }
                    for (int k = 0; k < this.workers.Length; k++)
                    {
                        this.workers[k].Execute(1);
                    }
                    for (int m = 0; m < this.workers.Length; m++)
                    {
                        this.workers[m].WaitOne();
                    }
                    for (int n = 0; n < this.workers.Length; n++)
                    {
                        this.workers[n].Execute(0);
                    }
                    if (!this.doubleBuffering)
                    {
                        for (int num8 = 0; num8 < this.workers.Length; num8++)
                        {
                            this.workers[num8].WaitOne();
                        }
                        if (!this.Interpolation)
                        {
                            for (int num9 = 0; num9 < this.agents.Count; num9++)
                            {
                                this.agents[num9].Interpolate(1f);
                            }
                        }
                    }
                }
                else
                {
                    if (this.doCleanObstacles)
                    {
                        this.CleanObstacles();
                        this.doCleanObstacles = false;
                        this.doUpdateObstacles = true;
                    }
                    if (this.doUpdateObstacles)
                    {
                        this.doUpdateObstacles = false;
                    }
                    this.BuildQuadtree();
                    for (int num10 = 0; num10 < this.agents.Count; num10++)
                    {
                        this.agents[num10].Update();
                        this.agents[num10].BufferSwitch();
                    }
                    for (int num11 = 0; num11 < this.agents.Count; num11++)
                    {
                        this.agents[num11].CalculateNeighbours();
                        this.agents[num11].CalculateVelocity(this.coroutineWorkerContext);
                    }
                    if (this.oversampling)
                    {
                        for (int num12 = 0; num12 < this.agents.Count; num12++)
                        {
                            this.agents[num12].Velocity = this.agents[num12].newVelocity;
                        }
                        for (int num13 = 0; num13 < this.agents.Count; num13++)
                        {
                            Vector3 newVelocity = this.agents[num13].newVelocity;
                            this.agents[num13].CalculateVelocity(this.coroutineWorkerContext);
                            this.agents[num13].newVelocity = (Vector3) ((newVelocity + this.agents[num13].newVelocity) * 0.5f);
                        }
                    }
                    if (!this.Interpolation)
                    {
                        for (int num14 = 0; num14 < this.agents.Count; num14++)
                        {
                            this.agents[num14].Interpolate(1f);
                        }
                    }
                }
            }
            if (this.Interpolation)
            {
                for (int num15 = 0; num15 < this.agents.Count; num15++)
                {
                    this.agents[num15].Interpolate((Time.time - this.lastStepInterpolationReference) / this.DeltaTime);
                }
            }
        }

        public void UpdateObstacle(ObstacleVertex obstacle, Vector3[] vertices, Matrix4x4 matrix)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException("Vertices must not be null");
            }
            if (obstacle == null)
            {
                throw new ArgumentNullException("Obstacle must not be null");
            }
            if (vertices.Length < 2)
            {
                throw new ArgumentException("Less than 2 vertices in an obstacle");
            }
            if (obstacle.split)
            {
                throw new ArgumentException("Obstacle is not a start vertex. You should only pass those ObstacleVertices got from AddObstacle method calls");
            }
            if (this.Multithreading && this.doubleBuffering)
            {
                for (int i = 0; i < this.workers.Length; i++)
                {
                    this.workers[i].WaitOne();
                }
            }
            int index = 0;
            ObstacleVertex next = obstacle;
            do
            {
                while (next.next.split)
                {
                    next.next = next.next.next;
                    next.next.prev = next;
                }
                if (index >= vertices.Length)
                {
                    Debug.DrawLine(next.prev.position, next.position, Color.red);
                    throw new ArgumentException("Obstacle has more vertices than supplied for updating (" + vertices.Length + " supplied)");
                }
                next.position = matrix.MultiplyPoint3x4(vertices[index]);
                index++;
                next = next.next;
            }
            while (next != obstacle);
            next = obstacle;
            do
            {
                Vector3 vector = next.next.position - next.position;
                Vector2 vector2 = new Vector2(vector.x, vector.z);
                next.dir = vector2.normalized;
                next = next.next;
            }
            while (next != obstacle);
            this.ScheduleCleanObstacles();
            this.UpdateObstacles();
        }

        public void UpdateObstacles()
        {
            this.doUpdateObstacles = true;
        }

        public float DeltaTime
        {
            get
            {
                return this.deltaTime;
            }
        }

        public float DesiredDeltaTime
        {
            get
            {
                return this.desiredDeltaTime;
            }
            set
            {
                this.desiredDeltaTime = Math.Max(value, 0f);
            }
        }

        public bool Interpolation
        {
            get
            {
                return this.interpolation;
            }
            set
            {
                this.interpolation = value;
            }
        }

        public bool Multithreading
        {
            get
            {
                return ((this.workers != null) && (this.workers.Length > 0));
            }
        }

        public bool Oversampling
        {
            get
            {
                return this.oversampling;
            }
            set
            {
                this.oversampling = value;
            }
        }

        public float PrevDeltaTime
        {
            get
            {
                return this.prevDeltaTime;
            }
        }

        public RVOQuadtree Quadtree
        {
            get
            {
                return this.quadtree;
            }
        }

        public float WallThickness
        {
            get
            {
                return this.wallThickness;
            }
            set
            {
                this.wallThickness = Math.Max(value, 0f);
            }
        }

        public enum SamplingAlgorithm
        {
            AdaptiveSampling,
            GradientDescent
        }

        private class Worker
        {
            private Simulator.WorkerContext context = new Simulator.WorkerContext();
            public int end;
            public AutoResetEvent runFlag = new AutoResetEvent(false);
            public Simulator simulator;
            public int start;
            public int task;
            private bool terminate;
            [NonSerialized]
            public Thread thread;
            public ManualResetEvent waitFlag = new ManualResetEvent(true);

            public Worker(Simulator sim)
            {
                this.simulator = sim;
                this.thread = new Thread(new ThreadStart(this.Run));
                this.thread.IsBackground = true;
                this.thread.Name = "RVO Simulator Thread";
                this.thread.Start();
            }

            public void Execute(int task)
            {
                this.task = task;
                this.waitFlag.Reset();
                this.runFlag.Set();
            }

            public void Run()
            {
                this.runFlag.WaitOne();
                while (!this.terminate)
                {
                    try
                    {
                        List<Agent> agents = this.simulator.GetAgents();
                        if (this.task == 0)
                        {
                            for (int i = this.start; i < this.end; i++)
                            {
                                agents[i].CalculateNeighbours();
                                agents[i].CalculateVelocity(this.context);
                            }
                        }
                        else if (this.task == 1)
                        {
                            for (int j = this.start; j < this.end; j++)
                            {
                                agents[j].Update();
                                agents[j].BufferSwitch();
                            }
                        }
                        else
                        {
                            if (this.task != 2)
                            {
                                Debug.LogError("Invalid Task Number: " + this.task);
                                throw new Exception("Invalid Task Number: " + this.task);
                            }
                            this.simulator.BuildQuadtree();
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError(exception);
                    }
                    this.waitFlag.Set();
                    this.runFlag.WaitOne();
                }
            }

            public void Terminate()
            {
                this.terminate = true;
            }

            public void WaitOne()
            {
                this.waitFlag.WaitOne();
            }
        }

        internal class WorkerContext
        {
            public Vector2[] bestPos = new Vector2[3];
            public float[] bestScores = new float[4];
            public float[] bestSizes = new float[3];
            public const int KeepCount = 3;
            public Vector2[] samplePos = new Vector2[50];
            public float[] sampleSize = new float[50];
            public Agent.VO[] vos = new Agent.VO[20];
        }
    }
}

