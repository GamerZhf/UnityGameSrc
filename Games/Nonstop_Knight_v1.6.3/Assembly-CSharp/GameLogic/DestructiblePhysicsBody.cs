namespace GameLogic
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DestructiblePhysicsBody : MonoBehaviour
    {
        public OnHit Hit;

        public delegate void OnHit(SkillType fromSkill);
    }
}

