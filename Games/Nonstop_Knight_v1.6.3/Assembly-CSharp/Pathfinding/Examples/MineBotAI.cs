namespace Pathfinding.Examples
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Seeker)), HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_mine_bot_a_i.php")]
    public class MineBotAI : AIPath
    {
        public Animation anim;
        public float animationSpeed = 0.2f;
        public GameObject endOfPathEffect;
        protected Vector3 lastTarget;
        public float sleepVelocity = 0.4f;

        public override Vector3 GetFeetPosition()
        {
            return base.tr.position;
        }

        public override void OnTargetReached()
        {
            if ((this.endOfPathEffect != null) && (Vector3.Distance(base.tr.position, this.lastTarget) > 1f))
            {
                UnityEngine.Object.Instantiate(this.endOfPathEffect, base.tr.position, base.tr.rotation);
                this.lastTarget = base.tr.position;
            }
        }

        public void Start()
        {
            this.anim["forward"].layer = 10;
            this.anim.Play("awake");
            this.anim.Play("forward");
            this.anim["awake"].wrapMode = WrapMode.Once;
            this.anim["awake"].speed = 0f;
            this.anim["awake"].normalizedTime = 1f;
            base.Start();
        }

        protected void Update()
        {
            Vector3 velocity;
            if (base.canMove)
            {
                Vector3 vel = base.CalculateVelocity(this.GetFeetPosition());
                this.RotateTowards(base.targetDirection);
                vel.y = 0f;
                if (vel.sqrMagnitude <= (this.sleepVelocity * this.sleepVelocity))
                {
                    vel = Vector3.zero;
                }
                if (base.rvoController != null)
                {
                    base.rvoController.Move(vel);
                    velocity = base.rvoController.velocity;
                }
                else if (base.controller != null)
                {
                    base.controller.SimpleMove(vel);
                    velocity = base.controller.velocity;
                }
                else
                {
                    Debug.LogWarning("No NavmeshController or CharacterController attached to GameObject");
                    velocity = Vector3.zero;
                }
            }
            else
            {
                velocity = Vector3.zero;
            }
            Vector3 vector3 = base.tr.InverseTransformDirection(velocity);
            vector3.y = 0f;
            if (velocity.sqrMagnitude <= (this.sleepVelocity * this.sleepVelocity))
            {
                this.anim.Blend("forward", 0f, 0.2f);
            }
            else
            {
                this.anim.Blend("forward", 1f, 0.2f);
                AnimationState state = this.anim["forward"];
                float z = vector3.z;
                state.speed = z * this.animationSpeed;
            }
        }
    }
}

