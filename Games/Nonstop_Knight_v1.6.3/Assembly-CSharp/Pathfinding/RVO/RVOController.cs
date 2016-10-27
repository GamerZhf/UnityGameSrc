namespace Pathfinding.RVO
{
    using Pathfinding;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("Pathfinding/Local Avoidance/RVO Controller"), HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_controller.php")]
    public class RVOController : MonoBehaviour
    {
        private float adjustedY;
        [Tooltip("How far in the time to look for collisions with other agents")]
        public float agentTimeHorizon = 2f;
        private static RVOSimulator cachedSimulator;
        [Tooltip("Center of the agent relative to the pivot point of this game object")]
        public Vector3 center;
        [AstarEnumFlag]
        public RVOLayer collidesWith = -1;
        public bool debug;
        private Vector3 desiredVelocity;
        public bool enableRotation = true;
        private static readonly Color GizmoColor = new Color(0.9411765f, 0.8352941f, 0.1176471f);
        [Tooltip("Height of the agent. In world units")]
        public float height = 1f;
        private Vector3 lastPosition;
        public RVOLayer layer = RVOLayer.DefaultAgent;
        [Tooltip("A locked unit cannot move. Other units will still avoid it. But avoidance quailty is not the best")]
        public bool locked;
        [Tooltip("Automatically set #locked to true when desired velocity is approximately zero")]
        public bool lockWhenNotMoving = true;
        [Tooltip("Layer mask for the ground. The RVOController will raycast down to check for the ground to figure out where to place the agent")]
        public LayerMask mask = -1;
        [Tooltip("Max number of other agents to take into account.\nA smaller value can reduce CPU load, a higher value can lead to better local avoidance quality.")]
        public int maxNeighbours = 10;
        [Tooltip("Max speed of the agent. In world units/second")]
        public float maxSpeed = 2f;
        [Tooltip("Maximum distance to other agents to take them into account for collisions.\nDecreasing this value can lead to better performance, increasing it can lead to better quality of the simulation")]
        public float neighbourDist = 10f;
        [HideInInspector]
        public float obstacleTimeHorizon = 2f;
        [Tooltip("Radius of the agent")]
        public float radius = 5f;
        public float rotationSpeed = 30f;
        private IAgent rvoAgent;
        private Simulator simulator;
        private Transform tr;
        [HideInInspector]
        public float wallAvoidFalloff = 1f;
        [HideInInspector]
        public float wallAvoidForce = 1f;

        public void Awake()
        {
            this.tr = base.transform;
            if (cachedSimulator == null)
            {
                cachedSimulator = UnityEngine.Object.FindObjectOfType<RVOSimulator>();
            }
            if (cachedSimulator == null)
            {
                Debug.LogError("No RVOSimulator component found in the scene. Please add one.");
            }
            else
            {
                this.simulator = cachedSimulator.GetSimulator();
            }
        }

        public void Move(Vector3 vel)
        {
            this.desiredVelocity = vel;
        }

        public void OnDisable()
        {
            if (this.simulator != null)
            {
                this.simulator.RemoveAgent(this.rvoAgent);
            }
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawWireSphere((Vector3) (((base.transform.position + this.center) - ((Vector3.up * this.height) * 0.5f)) + ((Vector3.up * this.radius) * 0.5f)), this.radius);
            Gizmos.DrawLine((base.transform.position + this.center) - ((Vector3) ((Vector3.up * this.height) * 0.5f)), (base.transform.position + this.center) + ((Vector3) ((Vector3.up * this.height) * 0.5f)));
            Gizmos.DrawWireSphere((Vector3) (((base.transform.position + this.center) + ((Vector3.up * this.height) * 0.5f)) - ((Vector3.up * this.radius) * 0.5f)), this.radius);
        }

        public void OnEnable()
        {
            if (this.simulator != null)
            {
                if (this.rvoAgent != null)
                {
                    this.simulator.AddAgent(this.rvoAgent);
                }
                else
                {
                    this.rvoAgent = this.simulator.AddAgent(base.transform.position);
                }
                this.UpdateAgentProperties();
                this.rvoAgent.Teleport(base.transform.position);
                this.adjustedY = this.rvoAgent.Position.y;
            }
        }

        public void Teleport(Vector3 pos)
        {
            this.tr.position = pos;
            this.lastPosition = pos;
            this.rvoAgent.Teleport(pos);
            this.adjustedY = pos.y;
        }

        public void Update()
        {
            if (this.rvoAgent != null)
            {
                RaycastHit hit;
                if (this.lastPosition != this.tr.position)
                {
                    this.Teleport(this.tr.position);
                }
                if (this.lockWhenNotMoving)
                {
                    this.locked = this.desiredVelocity == Vector3.zero;
                }
                this.UpdateAgentProperties();
                Vector3 interpolatedPosition = this.rvoAgent.InterpolatedPosition;
                interpolatedPosition.y = this.adjustedY;
                if ((this.mask != 0) && Physics.Raycast(interpolatedPosition + ((Vector3) ((Vector3.up * this.height) * 0.5f)), Vector3.down, out hit, float.PositiveInfinity, (int) this.mask))
                {
                    this.adjustedY = hit.point.y;
                }
                else
                {
                    this.adjustedY = 0f;
                }
                interpolatedPosition.y = this.adjustedY;
                this.rvoAgent.SetYPosition(this.adjustedY);
                Vector3 zero = Vector3.zero;
                if ((this.wallAvoidFalloff > 0f) && (this.wallAvoidForce > 0f))
                {
                    List<ObstacleVertex> neighbourObstacles = this.rvoAgent.NeighbourObstacles;
                    if (neighbourObstacles != null)
                    {
                        for (int i = 0; i < neighbourObstacles.Count; i++)
                        {
                            Vector3 position = neighbourObstacles[i].position;
                            Vector3 lineEnd = neighbourObstacles[i].next.position;
                            Vector3 vector5 = this.position - VectorMath.ClosestPointOnSegment(position, lineEnd, this.position);
                            if ((vector5 != position) && (vector5 != lineEnd))
                            {
                                float sqrMagnitude = vector5.sqrMagnitude;
                                vector5 = (Vector3) (vector5 / (sqrMagnitude * this.wallAvoidFalloff));
                                zero += vector5;
                            }
                        }
                    }
                }
                this.rvoAgent.DesiredVelocity = this.desiredVelocity + ((Vector3) (zero * this.wallAvoidForce));
                this.tr.position = (interpolatedPosition + ((Vector3) ((Vector3.up * this.height) * 0.5f))) - this.center;
                this.lastPosition = this.tr.position;
                if (this.enableRotation && (this.velocity != Vector3.zero))
                {
                    base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.LookRotation(this.velocity), (Time.deltaTime * this.rotationSpeed) * Mathf.Min(this.velocity.magnitude, 0.2f));
                }
            }
        }

        protected void UpdateAgentProperties()
        {
            this.rvoAgent.Radius = this.radius;
            this.rvoAgent.MaxSpeed = this.maxSpeed;
            this.rvoAgent.Height = this.height;
            this.rvoAgent.AgentTimeHorizon = this.agentTimeHorizon;
            this.rvoAgent.ObstacleTimeHorizon = this.obstacleTimeHorizon;
            this.rvoAgent.Locked = this.locked;
            this.rvoAgent.MaxNeighbours = this.maxNeighbours;
            this.rvoAgent.DebugDraw = this.debug;
            this.rvoAgent.NeighbourDist = this.neighbourDist;
            this.rvoAgent.Layer = this.layer;
            this.rvoAgent.CollidesWith = this.collidesWith;
        }

        public Vector3 position
        {
            get
            {
                return this.rvoAgent.InterpolatedPosition;
            }
        }

        public Vector3 velocity
        {
            get
            {
                return this.rvoAgent.Velocity;
            }
        }
    }
}

