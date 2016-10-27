namespace Pathfinding.Examples
{
    using Pathfinding;
    using Pathfinding.RVO;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_lightweight_r_v_o.php"), RequireComponent(typeof(MeshFilter))]
    public class LightweightRVO : MonoBehaviour
    {
        public int agentCount = 100;
        private List<IAgent> agents;
        public float agentTimeHorizon = 10f;
        private List<Color> colors;
        public bool debug;
        public float exampleScale = 100f;
        private List<Vector3> goals;
        private Vector3[] interpolatedVelocities;
        public int maxNeighbours = 10;
        public float maxSpeed = 2f;
        private Mesh mesh;
        private Color[] meshColors;
        public float neighbourDist = 15f;
        [HideInInspector]
        public float obstacleTimeHorizon = 10f;
        public float radius = 3f;
        public Vector3 renderingOffset = ((Vector3) (Vector3.up * 0.1f));
        private Simulator sim;
        private int[] tris;
        public RVOExampleType type;
        private Vector2[] uv;
        private Vector3[] verts;

        public void CreateAgents(int num)
        {
            this.agentCount = num;
            this.agents = new List<IAgent>(this.agentCount);
            this.goals = new List<Vector3>(this.agentCount);
            this.colors = new List<Color>(this.agentCount);
            this.sim.ClearAgents();
            if (this.type == RVOExampleType.Circle)
            {
                float num2 = (Mathf.Sqrt((((this.agentCount * this.radius) * this.radius) * 4f) / 3.141593f) * this.exampleScale) * 0.05f;
                for (int j = 0; j < this.agentCount; j++)
                {
                    Vector3 position = (Vector3) (new Vector3(Mathf.Cos(((j * 3.141593f) * 2f) / ((float) this.agentCount)), 0f, Mathf.Sin(((j * 3.141593f) * 2f) / ((float) this.agentCount))) * num2);
                    IAgent item = this.sim.AddAgent(position);
                    this.agents.Add(item);
                    this.goals.Add(-position);
                    this.colors.Add(AstarMath.HSVToRGB((j * 360f) / ((float) this.agentCount), 0.8f, 0.6f));
                }
            }
            else if (this.type == RVOExampleType.Line)
            {
                for (int k = 0; k < this.agentCount; k++)
                {
                    Vector3 vector2 = new Vector3((((k % 2) != 0) ? ((float) (-1)) : ((float) 1)) * this.exampleScale, 0f, ((k / 2) * this.radius) * 2.5f);
                    IAgent agent2 = this.sim.AddAgent(vector2);
                    this.agents.Add(agent2);
                    this.goals.Add(new Vector3(-vector2.x, vector2.y, vector2.z));
                    this.colors.Add(((k % 2) != 0) ? Color.blue : Color.red);
                }
            }
            else if (this.type == RVOExampleType.Point)
            {
                for (int m = 0; m < this.agentCount; m++)
                {
                    Vector3 vector3 = (Vector3) (new Vector3(Mathf.Cos(((m * 3.141593f) * 2f) / ((float) this.agentCount)), 0f, Mathf.Sin(((m * 3.141593f) * 2f) / ((float) this.agentCount))) * this.exampleScale);
                    IAgent agent3 = this.sim.AddAgent(vector3);
                    this.agents.Add(agent3);
                    this.goals.Add(new Vector3(0f, vector3.y, 0f));
                    this.colors.Add(AstarMath.HSVToRGB((m * 360f) / ((float) this.agentCount), 0.8f, 0.6f));
                }
            }
            else if (this.type == RVOExampleType.RandomStreams)
            {
                float radius = (Mathf.Sqrt((((this.agentCount * this.radius) * this.radius) * 4f) / 3.141593f) * this.exampleScale) * 0.05f;
                for (int n = 0; n < this.agentCount; n++)
                {
                    float f = (UnityEngine.Random.value * 3.141593f) * 2f;
                    float num9 = (UnityEngine.Random.value * 3.141593f) * 2f;
                    Vector3 vector4 = (Vector3) (new Vector3(Mathf.Cos(f), 0f, Mathf.Sin(f)) * this.uniformDistance(radius));
                    IAgent agent4 = this.sim.AddAgent(vector4);
                    this.agents.Add(agent4);
                    this.goals.Add((Vector3) (new Vector3(Mathf.Cos(num9), 0f, Mathf.Sin(num9)) * this.uniformDistance(radius)));
                    this.colors.Add(AstarMath.HSVToRGB(num9 * 57.29578f, 0.8f, 0.6f));
                }
            }
            for (int i = 0; i < this.agents.Count; i++)
            {
                IAgent agent5 = this.agents[i];
                agent5.Radius = this.radius;
                agent5.MaxSpeed = this.maxSpeed;
                agent5.AgentTimeHorizon = this.agentTimeHorizon;
                agent5.ObstacleTimeHorizon = this.obstacleTimeHorizon;
                agent5.MaxNeighbours = this.maxNeighbours;
                agent5.NeighbourDist = this.neighbourDist;
                agent5.DebugDraw = (i == 0) && this.debug;
            }
            this.verts = new Vector3[4 * this.agents.Count];
            this.uv = new Vector2[this.verts.Length];
            this.tris = new int[(this.agents.Count * 2) * 3];
            this.meshColors = new Color[this.verts.Length];
        }

        public void OnGUI()
        {
            if (GUILayout.Button("2", new GUILayoutOption[0]))
            {
                this.CreateAgents(2);
            }
            if (GUILayout.Button("10", new GUILayoutOption[0]))
            {
                this.CreateAgents(10);
            }
            if (GUILayout.Button("100", new GUILayoutOption[0]))
            {
                this.CreateAgents(100);
            }
            if (GUILayout.Button("500", new GUILayoutOption[0]))
            {
                this.CreateAgents(500);
            }
            if (GUILayout.Button("1000", new GUILayoutOption[0]))
            {
                this.CreateAgents(0x3e8);
            }
            if (GUILayout.Button("5000", new GUILayoutOption[0]))
            {
                this.CreateAgents(0x1388);
            }
            GUILayout.Space(5f);
            if (GUILayout.Button("Random Streams", new GUILayoutOption[0]))
            {
                this.type = RVOExampleType.RandomStreams;
                this.CreateAgents((this.agents == null) ? 100 : this.agents.Count);
            }
            if (GUILayout.Button("Line", new GUILayoutOption[0]))
            {
                this.type = RVOExampleType.Line;
                this.CreateAgents((this.agents == null) ? 10 : Mathf.Min(this.agents.Count, 100));
            }
            if (GUILayout.Button("Circle", new GUILayoutOption[0]))
            {
                this.type = RVOExampleType.Circle;
                this.CreateAgents((this.agents == null) ? 100 : this.agents.Count);
            }
            if (GUILayout.Button("Point", new GUILayoutOption[0]))
            {
                this.type = RVOExampleType.Point;
                this.CreateAgents((this.agents == null) ? 100 : this.agents.Count);
            }
        }

        public void Start()
        {
            this.mesh = new Mesh();
            RVOSimulator simulator = UnityEngine.Object.FindObjectOfType(typeof(RVOSimulator)) as RVOSimulator;
            if (simulator == null)
            {
                Debug.LogError("No RVOSimulator could be found in the scene. Please add a RVOSimulator component to any GameObject");
            }
            else
            {
                this.sim = simulator.GetSimulator();
                base.GetComponent<MeshFilter>().mesh = this.mesh;
                this.CreateAgents(this.agentCount);
            }
        }

        private float uniformDistance(float radius)
        {
            float num = UnityEngine.Random.value + UnityEngine.Random.value;
            if (num > 1f)
            {
                return (radius * (2f - num));
            }
            return (radius * num);
        }

        public void Update()
        {
            if ((this.agents != null) && (this.mesh != null))
            {
                if (this.agents.Count != this.goals.Count)
                {
                    Debug.LogError("Agent count does not match goal count");
                }
                else
                {
                    for (int i = 0; i < this.agents.Count; i++)
                    {
                        Vector3 interpolatedPosition = this.agents[i].InterpolatedPosition;
                        Vector3 vector = this.goals[i] - interpolatedPosition;
                        vector = Vector3.ClampMagnitude(vector, 1f);
                        this.agents[i].DesiredVelocity = (Vector3) (vector * this.agents[i].MaxSpeed);
                    }
                    if ((this.interpolatedVelocities == null) || (this.interpolatedVelocities.Length < this.agents.Count))
                    {
                        Vector3[] vectorArray = new Vector3[this.agents.Count];
                        if (this.interpolatedVelocities != null)
                        {
                            for (int k = 0; k < this.interpolatedVelocities.Length; k++)
                            {
                                vectorArray[k] = this.interpolatedVelocities[k];
                            }
                        }
                        this.interpolatedVelocities = vectorArray;
                    }
                    for (int j = 0; j < this.agents.Count; j++)
                    {
                        IAgent agent = this.agents[j];
                        this.interpolatedVelocities[j] = Vector3.Lerp(this.interpolatedVelocities[j], agent.Velocity, (agent.Velocity.magnitude * Time.deltaTime) * 4f);
                        Vector3 rhs = (Vector3) (this.interpolatedVelocities[j].normalized * agent.Radius);
                        if (rhs == Vector3.zero)
                        {
                            rhs = new Vector3(0f, 0f, agent.Radius);
                        }
                        Vector3 vector4 = Vector3.Cross(Vector3.up, rhs);
                        Vector3 vector5 = agent.InterpolatedPosition + this.renderingOffset;
                        int index = 4 * j;
                        int num5 = 6 * j;
                        this.verts[index] = (vector5 + rhs) - vector4;
                        this.verts[index + 1] = (vector5 + rhs) + vector4;
                        this.verts[index + 2] = (vector5 - rhs) + vector4;
                        this.verts[index + 3] = (vector5 - rhs) - vector4;
                        this.uv[index] = new Vector2(0f, 1f);
                        this.uv[index + 1] = new Vector2(1f, 1f);
                        this.uv[index + 2] = new Vector2(1f, 0f);
                        this.uv[index + 3] = new Vector2(0f, 0f);
                        this.meshColors[index] = this.colors[j];
                        this.meshColors[index + 1] = this.colors[j];
                        this.meshColors[index + 2] = this.colors[j];
                        this.meshColors[index + 3] = this.colors[j];
                        this.tris[num5] = index;
                        this.tris[num5 + 1] = index + 1;
                        this.tris[num5 + 2] = index + 2;
                        this.tris[num5 + 3] = index;
                        this.tris[num5 + 4] = index + 2;
                        this.tris[num5 + 5] = index + 3;
                    }
                    this.mesh.Clear();
                    this.mesh.vertices = this.verts;
                    this.mesh.uv = this.uv;
                    this.mesh.colors = this.meshColors;
                    this.mesh.triangles = this.tris;
                    this.mesh.RecalculateNormals();
                }
            }
        }

        public enum RVOExampleType
        {
            Circle,
            Line,
            Point,
            RandomStreams
        }
    }
}

