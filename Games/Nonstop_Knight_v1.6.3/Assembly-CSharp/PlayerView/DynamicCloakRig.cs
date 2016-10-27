namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class DynamicCloakRig : MonoBehaviour
    {
        [CompilerGenerated]
        private float <RotationFollowSmoothingFactor>k__BackingField;
        public bool DebugMode;
        private float DebugRunSpeed = 0.3f;
        public const float GAMEPLAY_ROTATION_FOLLOW_SMOOTHING_FACTOR = 30f;
        private float LiftHightFactor = 1.5f;
        private float LiftLowFactor = 8f;
        private float LiftMidFactor = 4f;
        private HeroCharacterAnimator m_characterAnimator;
        private GameObject m_cloakTargetBack;
        private List<ConfigurableJoint> m_highJoints = new List<ConfigurableJoint>();
        private Vector3 m_localOffset;
        private List<ConfigurableJoint> m_lowJoints = new List<ConfigurableJoint>();
        private List<ConfigurableJoint> m_midJoints = new List<ConfigurableJoint>();
        private Dictionary<ConfigurableJoint, Vector3> m_originalLocalPositions = new Dictionary<ConfigurableJoint, Vector3>();
        private Transform m_originalParentTm;
        private float m_separationMovingAverage;
        private bool m_suppressCloak;
        private Transform m_tm;
        public const float MAX_SEPARATION_DISTANCE = 2f;
        public const float MENU_ROTATION_FOLLOW_SMOOTHING_FACTOR = 35f;
        public const float MOVING_AVERAGE_SMOOTHING_FACTOR = 0.08f;
        public const float POSITION_FOLLOW_SMOOTHING_FACTOR = 35f;
        public const float SUPPRESS_AMOUNT_NORMALIZED = 1f;
        public Transform TargetTm;
        public const float TO_TARGET_MAGN_MAX = 0.22f;
        private float TranslateHighZFactor = 1f;
        private float WobbleFrequency = 1f;
        private float WobbleLowFactor = 0.5f;
        private float WobbleMidFactor = 0.5f;

        protected void Awake()
        {
            this.m_tm = base.transform;
            this.m_originalParentTm = base.transform.parent;
            this.m_localOffset = new Vector3(0f, 2.25f, 0f);
            this.m_tm.localPosition = this.m_localOffset;
            this.RotationFollowSmoothingFactor = 35f;
            this.m_cloakTargetBack = new GameObject("CloakTargetBack");
            this.m_cloakTargetBack.transform.SetParent(this.m_tm);
            this.m_cloakTargetBack.transform.localPosition = new Vector3(0f, 0f, -1f);
            MarkerCloak cloak = this.m_cloakTargetBack.AddComponent<MarkerCloak>();
            cloak.shape = AbstractMarker.Shape.SPHERE;
            cloak.customSize = true;
            cloak.customSphereRadius = 0.05f;
            cloak.color = Color.red;
            string[] strArray = new string[] { "CapeLow", "CapeMid", "CapeHigh" };
            foreach (Transform transform in this.TargetTm.GetComponentsInChildren<Transform>())
            {
                for (int i = 0; i < strArray.Length; i++)
                {
                    string str = strArray[i];
                    if (transform.name.StartsWith(str))
                    {
                        int num3 = int.Parse(transform.name.Substring(str.Length));
                        Rigidbody rigidbody = transform.gameObject.AddComponent<Rigidbody>();
                        GameObject obj2 = new GameObject("Controller" + str + num3);
                        obj2.transform.SetParent(base.transform, false);
                        Vector3 position = transform.transform.position;
                        position.y = base.transform.position.y;
                        obj2.transform.position = position;
                        ConfigurableJoint item = obj2.AddComponent<ConfigurableJoint>();
                        item.connectedBody = rigidbody;
                        Rigidbody component = obj2.GetComponent<Rigidbody>();
                        component.useGravity = false;
                        component.isKinematic = true;
                        item.xMotion = ConfigurableJointMotion.Free;
                        item.yMotion = ConfigurableJointMotion.Free;
                        item.zMotion = ConfigurableJointMotion.Free;
                        switch (i)
                        {
                            case 0:
                            {
                                cloak = obj2.AddComponent<MarkerCloak>();
                                cloak.shape = AbstractMarker.Shape.SPHERE;
                                cloak.customSize = true;
                                cloak.customSphereRadius = 0.05f;
                                cloak.color = Color.yellow;
                                item.targetPosition = new Vector3(0f, 0f, 0f);
                                item.angularXMotion = ConfigurableJointMotion.Locked;
                                item.angularYMotion = ConfigurableJointMotion.Locked;
                                item.angularZMotion = ConfigurableJointMotion.Locked;
                                JointDrive drive = new JointDrive();
                                drive.mode = JointDriveMode.Position;
                                drive.positionSpring = float.MaxValue;
                                drive.positionDamper = float.MaxValue;
                                drive.maximumForce = 120f;
                                item.xDrive = drive;
                                item.yDrive = drive;
                                item.zDrive = drive;
                                this.m_lowJoints.Add(item);
                                break;
                            }
                            case 1:
                            {
                                cloak = obj2.AddComponent<MarkerCloak>();
                                cloak.shape = AbstractMarker.Shape.SPHERE;
                                cloak.customSize = true;
                                cloak.customSphereRadius = 0.05f;
                                cloak.color = Color.blue;
                                item.targetPosition = new Vector3(0f, 0f, 0f);
                                item.angularXMotion = ConfigurableJointMotion.Locked;
                                item.angularYMotion = ConfigurableJointMotion.Locked;
                                item.angularZMotion = ConfigurableJointMotion.Locked;
                                JointDrive drive2 = new JointDrive();
                                drive2.mode = JointDriveMode.Position;
                                drive2.positionSpring = float.MaxValue;
                                drive2.positionDamper = float.MaxValue;
                                drive2.maximumForce = 120f;
                                item.xDrive = drive2;
                                item.yDrive = drive2;
                                item.zDrive = drive2;
                                this.m_midJoints.Add(item);
                                break;
                            }
                            case 2:
                            {
                                cloak = obj2.AddComponent<MarkerCloak>();
                                cloak.shape = AbstractMarker.Shape.SPHERE;
                                cloak.customSize = true;
                                cloak.customSphereRadius = 0.05f;
                                cloak.color = Color.magenta;
                                item.targetPosition = new Vector3(0f, 0f, 0f);
                                item.angularXMotion = ConfigurableJointMotion.Locked;
                                item.angularYMotion = ConfigurableJointMotion.Locked;
                                item.angularZMotion = ConfigurableJointMotion.Locked;
                                JointDrive drive3 = new JointDrive();
                                drive3.mode = JointDriveMode.Position;
                                drive3.positionSpring = float.MaxValue;
                                drive3.positionDamper = float.MaxValue;
                                drive3.maximumForce = 120f;
                                item.xDrive = drive3;
                                item.yDrive = drive3;
                                item.zDrive = drive3;
                                this.m_highJoints.Add(item);
                                break;
                            }
                        }
                        this.m_originalLocalPositions.Add(item, obj2.transform.localPosition);
                    }
                }
            }
        }

        protected void FixedUpdate()
        {
            ActiveDungeon activeDungeon = GameLogic.Binder.GameState.ActiveDungeon;
            bool flag = (activeDungeon != null) && (activeDungeon.ActiveRoom != null);
            if (this.m_characterAnimator == null)
            {
                this.m_characterAnimator = this.TargetTm.GetComponentInChildren<HeroCharacterAnimator>();
                if (this.m_characterAnimator != null)
                {
                    this.m_characterAnimator.OnAnimationStateChanged += new AbstractCharacterAnimator.AnimationStateChanged(this.onAnimationStateChanged);
                    this.m_characterAnimator.OnAnimationActionTriggered += new AbstractCharacterAnimator.AnimationActionTriggered(this.onAnimationActionTriggered);
                }
            }
            if (this.m_tm.parent == this.m_originalParentTm)
            {
                Transform parent = this.m_originalParentTm.parent;
                if (parent != null)
                {
                    this.m_tm.SetParent(parent, true);
                    this.m_tm.localScale = Vector3.one;
                    this.TargetTm = this.TargetTm.parent;
                }
            }
            this.m_tm.localRotation = Quaternion.Slerp(this.m_tm.localRotation, this.TargetTm.localRotation, (this.RotationFollowSmoothingFactor * Time.deltaTime) * Time.timeScale);
            if (flag)
            {
                this.m_tm.localPosition = Vector3.Slerp(this.m_tm.localPosition, this.TargetTm.localPosition + this.m_localOffset, (35f * Time.deltaTime) * Time.timeScale);
            }
            else
            {
                this.m_tm.localPosition = this.TargetTm.localPosition + this.m_localOffset;
            }
            Vector3 toTargetXz = Vector3Extensions.ToXzVector3(this.TargetTm.localPosition - this.m_tm.localPosition);
            if (toTargetXz.magnitude > 0.22f)
            {
                toTargetXz = (Vector3) (toTargetXz.normalized * 0.22f);
                this.m_tm.localPosition += toTargetXz;
            }
            if (this.DebugMode)
            {
                this.m_tm.localPosition -= (Vector3) (Vector3.forward * this.DebugRunSpeed);
                toTargetXz = (Vector3) (Vector3.forward * this.DebugRunSpeed);
            }
            this.jointSeparation(Quaternion.Dot(this.TargetTm.localRotation, this.m_tm.localRotation), toTargetXz);
            if (this.m_suppressCloak)
            {
                this.suppressCloak();
            }
        }

        private void jointSeparation(float rotDot, Vector3 toTargetXz)
        {
            float magnitude = toTargetXz.magnitude;
            float num2 = Mathf.Clamp((float) (1f - Math.Abs(rotDot)), (float) 0f, (float) 0.02f);
            this.m_separationMovingAverage = (num2 * 0.08f) + (this.m_separationMovingAverage * 0.92f);
            for (int i = 0; i < this.m_lowJoints.Count; i++)
            {
                ConfigurableJoint joint = this.m_lowJoints[i];
                float num4 = 0.003f + (Mathf.Sin(i + (Time.time * 2f)) * 0.003f);
                float num5 = Mathf.Clamp((float) ((this.m_separationMovingAverage + num4) * 20f), (float) 0f, (float) 2f);
                Vector3 normalized = joint.transform.localPosition.normalized;
                Vector3 vector2 = this.m_originalLocalPositions[joint] + ((Vector3) (normalized * num5));
                joint.transform.localPosition = vector2;
                float num6 = Mathf.Sin(i + (Time.time * this.WobbleFrequency));
                vector2.z += (-this.LiftLowFactor * magnitude) * 0.5f;
                vector2.y += this.LiftLowFactor * magnitude;
                vector2.y += (num6 * magnitude) * this.WobbleLowFactor;
                joint.transform.localPosition = vector2;
            }
            for (int j = 0; j < this.m_midJoints.Count; j++)
            {
                ConfigurableJoint joint2 = this.m_midJoints[j];
                float num8 = 0.006f + (Mathf.Sin(j + (Time.time * 2f)) * 0.006f);
                float num9 = Mathf.Clamp((float) ((this.m_separationMovingAverage + num8) * 5f), (float) 0f, (float) 2f);
                Vector3 vector3 = joint2.transform.localPosition.normalized;
                Vector3 vector4 = this.m_originalLocalPositions[joint2] + ((Vector3) (vector3 * num9));
                joint2.transform.localPosition = vector4;
                float num10 = Mathf.Sin(j + (Time.time * this.WobbleFrequency));
                vector4.z += (-this.LiftMidFactor * magnitude) * 0f;
                vector4.y += this.LiftMidFactor * magnitude;
                vector4.y += (num10 * magnitude) * this.WobbleMidFactor;
                joint2.transform.localPosition = vector4;
            }
            for (int k = 0; k < this.m_highJoints.Count; k++)
            {
                ConfigurableJoint joint3 = this.m_highJoints[k];
                Vector3 vector5 = this.m_originalLocalPositions[joint3];
                vector5.y += this.LiftHightFactor * magnitude;
                vector5.z += this.TranslateHighZFactor * magnitude;
                joint3.transform.localPosition = vector5;
            }
        }

        private void onAnimationActionTriggered(AbstractCharacterAnimator.Action action)
        {
            if (action == AbstractCharacterAnimator.Action.ATTACK_MELEE)
            {
            }
        }

        private void onAnimationStateChanged(AbstractCharacterAnimator.State newState)
        {
        }

        private void onCharacterMeleeAttackContact(CharacterInstance sourceCharacter, Vector3 contactWorldPt, bool importantContact)
        {
            if (sourceCharacter.IsPlayerCharacter)
            {
                this.m_tm.Rotate(Vector3.up, (float) 180f);
            }
        }

        protected void OnDisable()
        {
            if (GameLogic.Binder.EventBus != null)
            {
                GameLogic.Binder.EventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
                GameLogic.Binder.EventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
                GameLogic.Binder.EventBus.OnCharacterMeleeAttackContact -= new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
            }
            if (this.m_characterAnimator != null)
            {
                this.m_characterAnimator.OnAnimationStateChanged -= new AbstractCharacterAnimator.AnimationStateChanged(this.onAnimationStateChanged);
                this.m_characterAnimator.OnAnimationActionTriggered -= new AbstractCharacterAnimator.AnimationActionTriggered(this.onAnimationActionTriggered);
                this.m_characterAnimator = null;
            }
        }

        protected void OnEnable()
        {
            if (GameLogic.Binder.EventBus != null)
            {
                GameLogic.Binder.EventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
                GameLogic.Binder.EventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
                GameLogic.Binder.EventBus.OnCharacterMeleeAttackContact += new GameLogic.Events.CharacterMeleeAttackContact(this.onCharacterMeleeAttackContact);
            }
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            this.RotationFollowSmoothingFactor = 35f;
            this.m_suppressCloak = false;
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.RotationFollowSmoothingFactor = 30f;
            this.m_suppressCloak = false;
        }

        private void suppressCloak()
        {
            float t = Mathf.Clamp01(1f);
            Vector3 vector4 = this.m_cloakTargetBack.transform.localPosition - Vector3.zero;
            Vector3 normalized = vector4.normalized;
            for (int i = this.m_midJoints.Count - 1; i >= 3; i--)
            {
                ConfigurableJoint joint = this.m_midJoints[i];
                Vector3 vector5 = this.m_originalLocalPositions[joint] - Vector3.zero;
                Quaternion to = Quaternion.FromToRotation(vector5.normalized, normalized);
                Quaternion quaternion2 = Quaternion.Slerp(joint.transform.localRotation, to, t);
                joint.transform.localPosition = (Vector3) (quaternion2 * this.m_originalLocalPositions[joint]);
            }
            for (int j = this.m_lowJoints.Count - 1; j >= 3; j--)
            {
                ConfigurableJoint joint2 = this.m_lowJoints[j];
                Vector3 vector6 = this.m_originalLocalPositions[joint2] - Vector3.zero;
                Quaternion quaternion3 = Quaternion.FromToRotation(vector6.normalized, normalized);
                Quaternion quaternion4 = Quaternion.Slerp(joint2.transform.localRotation, quaternion3, t);
                joint2.transform.localPosition = (Vector3) (quaternion4 * this.m_originalLocalPositions[joint2]);
            }
        }

        public float RotationFollowSmoothingFactor
        {
            [CompilerGenerated]
            get
            {
                return this.<RotationFollowSmoothingFactor>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RotationFollowSmoothingFactor>k__BackingField = value;
            }
        }
    }
}

