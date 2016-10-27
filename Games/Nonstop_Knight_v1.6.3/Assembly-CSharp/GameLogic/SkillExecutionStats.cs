namespace GameLogic
{
    using System;
    using UnityEngine;

    public class SkillExecutionStats
    {
        public int EnemiesAround;
        public int KillCount;
        public bool KillDuringThisFrame;
        public Vector3 LastKillWorldPos = Vector3.zero;
        public Vector3 MovementDir = Vector3.zero;
        public float MovementDurationDynamic;
        public float MovementForce;
        public float MovementLinearDeceleration;
        public bool PreExecuteFailed;
        public bool SpecialCaseThisFrame;
        public CharacterInstance TargetCharacter;
    }
}

