namespace AiView
{
    using App;
    using GameLogic;
    using Pathfinding;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class PursuitAiBehaviour : AiBehaviour
    {
        [CompilerGenerated]
        private bool <LineOfSightToTargetCharacter>k__BackingField;
        [CompilerGenerated]
        private bool <WithinAttackDistance>k__BackingField;

        public PursuitAiBehaviour(CharacterInstance character)
        {
            base.Character = character;
        }

        public override void update(float dt)
        {
            Vector3 normalized;
            Vector3 vector9;
            Vector3 vector10;
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            Vector3 zero = Vector3.zero;
            if (base.Character.TargetCharacter != null)
            {
                zero = base.Character.TargetCharacter.PhysicsBody.Transform.position;
            }
            else if (base.Character.ManualTargetPos != Vector3.zero)
            {
                zero = base.Character.ManualTargetPos;
            }
            else
            {
                if (base.Character.Velocity != Vector3.zero)
                {
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(base.Character, Vector3.zero, base.Character.Facing);
                }
                return;
            }
            Vector3 a = Vector3Extensions.ToXzVector3(base.Character.PhysicsBody.Transform.position);
            Vector3 b = Vector3Extensions.ToXzVector3(zero);
            Vector3 vector4 = b - a;
            Path activePath = base.Character.PhysicsBody.PathPlanner.ActivePath;
            bool flag = Vector3.Distance(a, b) <= (base.Character.Radius * 1.5f);
            bool flag2 = false;
            if (activePath != null)
            {
                flag2 = !base.Character.IsPlayerCharacter && (activePath.GetTotalLength() > ConfigGameplay.MAX_MONSER_PURSUIT_PATH_LENGTH);
            }
            Vector3 vector5 = b;
            if ((flag || (activePath == null)) || flag2)
            {
                goto Label_0234;
            }
            PathPlanner pathPlanner = base.Character.PhysicsBody.PathPlanner;
            Vector3 position = base.Character.PhysicsBody.Transform.position;
            if ((pathPlanner.ActiveVectorPath == null) || (pathPlanner.ActiveVectorPath.Count == 0))
            {
                goto Label_0234;
            }
            Vector3 self = pathPlanner.ActiveVectorPath[pathPlanner.ActiveWaypoint];
            self.y = position.y;
        Label_01EB:
            vector10 = position - self;
            if ((vector10.sqrMagnitude < (pathPlanner.MoveNextDist * pathPlanner.MoveNextDist)) && (pathPlanner.ActiveWaypoint != (pathPlanner.ActiveVectorPath.Count - 1)))
            {
                pathPlanner.ActiveWaypoint++;
                self = pathPlanner.ActiveVectorPath[pathPlanner.ActiveWaypoint];
                self.y = position.y;
                goto Label_01EB;
            }
            vector5 = Vector3Extensions.ToXzVector3(self);
        Label_0234:
            if (activePath != null)
            {
                Vector3 vector11 = vector5 - a;
                normalized = vector11.normalized;
                vector9 = (Vector3) (normalized * base.Character.MovementSpeed(true));
            }
            else if (base.Character.IsPlayerCharacter)
            {
                normalized = base.Character.Facing;
                vector9 = Vector3.zero;
            }
            else
            {
                normalized = vector4.normalized;
                vector9 = (Vector3) (normalized * base.Character.MovementSpeed(true));
            }
            if (base.Character.TargetCharacter == null)
            {
                this.WithinAttackDistance = false;
                this.LineOfSightToTargetCharacter = false;
            }
            else
            {
                this.WithinAttackDistance = activeDungeon.ActiveRoom.characterWithinAttackDistance(base.Character, base.Character.TargetCharacter);
                this.LineOfSightToTargetCharacter = false;
            }
            if (base.Character.UsesRangedAttack && (base.Character.TargetCharacter != null))
            {
                this.LineOfSightToTargetCharacter = activeDungeon.ActiveRoom.lineOfSightCharacterToCharacter(base.Character, base.Character.TargetCharacter);
                if (this.WithinAttackDistance && this.LineOfSightToTargetCharacter)
                {
                    vector9 = Vector3.zero;
                    normalized = vector4;
                }
            }
            else if (this.WithinAttackDistance)
            {
                vector9 = Vector3.zero;
                normalized = vector4;
            }
            if (vector9 != Vector3.zero)
            {
                vector9 = (Vector3) (vector9 * Easing.Apply(base.Character.RunAccelerationTimer.normalizedProgress(), ConfigGameplay.CHARACTER_FULLSPEED_ACCELERATION_EASING));
            }
            CmdSetCharacterVelocityAndFacing.ExecuteStatic(base.Character, vector9, normalized);
        }

        public bool LineOfSightToTargetCharacter
        {
            [CompilerGenerated]
            get
            {
                return this.<LineOfSightToTargetCharacter>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LineOfSightToTargetCharacter>k__BackingField = value;
            }
        }

        public bool WithinAttackDistance
        {
            [CompilerGenerated]
            get
            {
                return this.<WithinAttackDistance>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<WithinAttackDistance>k__BackingField = value;
            }
        }
    }
}

