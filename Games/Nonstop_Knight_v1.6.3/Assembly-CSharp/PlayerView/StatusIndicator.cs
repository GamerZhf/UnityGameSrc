namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class StatusIndicator : MonoBehaviour, IPoolable
    {
        [CompilerGenerated]
        private CharacterInstance <FollowCharacter>k__BackingField;
        [CompilerGenerated]
        private GameLogic.SkillType <SkillType>k__BackingField;
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        public const float ANGULAR_VELOCITY = 40f;
        public List<MeshRenderer> Icons;

        protected void Awake()
        {
            this.Tm = base.transform;
        }

        public void cleanUpForReuse()
        {
        }

        public void initialize(CharacterInstance followTm, GameLogic.SkillType skillType, Material sharedMaterial)
        {
            this.FollowCharacter = followTm;
            this.Tm.position = followTm.PhysicsBody.Transform.position;
            this.Tm.rotation = Quaternion.identity;
            this.SkillType = skillType;
            for (int i = 0; i < this.Icons.Count; i++)
            {
                this.Icons[i].sharedMaterial = sharedMaterial;
            }
        }

        protected void LateUpdate()
        {
            this.Tm.position = this.FollowCharacter.PhysicsBody.Transform.position;
            this.Tm.Rotate(Vector3.up, (float) (40f * Time.deltaTime));
        }

        public CharacterInstance FollowCharacter
        {
            [CompilerGenerated]
            get
            {
                return this.<FollowCharacter>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<FollowCharacter>k__BackingField = value;
            }
        }

        public GameLogic.SkillType SkillType
        {
            [CompilerGenerated]
            get
            {
                return this.<SkillType>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<SkillType>k__BackingField = value;
            }
        }

        public Transform Tm
        {
            [CompilerGenerated]
            get
            {
                return this.<Tm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Tm>k__BackingField = value;
            }
        }
    }
}

