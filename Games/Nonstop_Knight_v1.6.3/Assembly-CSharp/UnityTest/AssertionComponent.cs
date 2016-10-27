namespace UnityTest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable]
    public class AssertionComponent : MonoBehaviour, IAssertionComponentConfigurator
    {
        [SerializeField]
        public int checkAfterFrames = 1;
        [SerializeField]
        public float checkAfterTime = 1f;
        [SerializeField]
        public CheckMethod checkMethods = CheckMethod.Start;
        [SerializeField]
        public int checksPerformed;
        [SerializeField]
        public bool hasFailed;
        [SerializeField]
        private ActionBase m_ActionBase;
        private int m_CheckOnFrame;
        private int m_CreatedInFileLine = -1;
        private string m_CreatedInFilePath = string.Empty;
        [SerializeField]
        public bool repeatCheckFrame = true;
        [SerializeField]
        public bool repeatCheckTime = true;
        [SerializeField]
        public int repeatEveryFrame = 1;
        [SerializeField]
        public float repeatEveryTime = 1f;

        public void Awake()
        {
            if (!UnityEngine.Debug.isDebugBuild)
            {
                UnityEngine.Object.Destroy(this);
            }
            this.OnComponentCopy();
        }

        private void CheckAssertionFor(CheckMethod checkMethod)
        {
            if (this.IsCheckMethodSelected(checkMethod))
            {
                Assertions.CheckAssertions(this);
            }
        }

        [DebuggerHidden]
        public IEnumerator CheckPeriodically()
        {
            <CheckPeriodically>c__Iterator20 iterator = new <CheckPeriodically>c__Iterator20();
            iterator.<>f__this = this;
            return iterator;
        }

        public static T Create<T>(CheckMethod checkOnMethods, GameObject gameObject, string propertyPath) where T: ActionBase
        {
            IAssertionComponentConfigurator configurator;
            return Create<T>(out configurator, checkOnMethods, gameObject, propertyPath);
        }

        public static T Create<T>(CheckMethod checkOnMethods, GameObject gameObject, string propertyPath, object constValue) where T: ComparerBase
        {
            IAssertionComponentConfigurator configurator;
            return Create<T>(out configurator, checkOnMethods, gameObject, propertyPath, constValue);
        }

        public static T Create<T>(out IAssertionComponentConfigurator configurator, CheckMethod checkOnMethods, GameObject gameObject, string propertyPath) where T: ActionBase
        {
            return CreateAssertionComponent<T>(out configurator, checkOnMethods, gameObject, propertyPath);
        }

        public static T Create<T>(CheckMethod checkOnMethods, GameObject gameObject, string propertyPath, GameObject gameObject2, string propertyPath2) where T: ComparerBase
        {
            IAssertionComponentConfigurator configurator;
            return Create<T>(out configurator, checkOnMethods, gameObject, propertyPath, gameObject2, propertyPath2);
        }

        public static T Create<T>(out IAssertionComponentConfigurator configurator, CheckMethod checkOnMethods, GameObject gameObject, string propertyPath, object constValue) where T: ComparerBase
        {
            T local = CreateAssertionComponent<T>(out configurator, checkOnMethods, gameObject, propertyPath);
            if (constValue == null)
            {
                local.compareToType = ComparerBase.CompareToType.CompareToNull;
                return local;
            }
            local.compareToType = ComparerBase.CompareToType.CompareToConstantValue;
            local.ConstValue = constValue;
            return local;
        }

        public static T Create<T>(out IAssertionComponentConfigurator configurator, CheckMethod checkOnMethods, GameObject gameObject, string propertyPath, GameObject gameObject2, string propertyPath2) where T: ComparerBase
        {
            T local = CreateAssertionComponent<T>(out configurator, checkOnMethods, gameObject, propertyPath);
            local.compareToType = ComparerBase.CompareToType.CompareToObject;
            local.other = gameObject2;
            local.otherPropertyPath = propertyPath2;
            return local;
        }

        private static T CreateAssertionComponent<T>(out IAssertionComponentConfigurator configurator, CheckMethod checkOnMethods, GameObject gameObject, string propertyPath) where T: ActionBase
        {
            AssertionComponent component = gameObject.AddComponent<AssertionComponent>();
            component.checkMethods = checkOnMethods;
            T local = ScriptableObject.CreateInstance<T>();
            component.Action = local;
            component.Action.go = gameObject;
            component.Action.thisPropertyPath = propertyPath;
            configurator = component;
            StackTrace trace = new StackTrace(true);
            string fileName = trace.GetFrame(0).GetFileName();
            for (int i = 1; i < trace.FrameCount; i++)
            {
                StackFrame frame = trace.GetFrame(i);
                if (frame.GetFileName() != fileName)
                {
                    string str2 = frame.GetFileName().Substring(Application.dataPath.Length - "Assets".Length);
                    component.m_CreatedInFilePath = str2;
                    component.m_CreatedInFileLine = frame.GetFileLineNumber();
                    return local;
                }
            }
            return local;
        }

        public void FixedUpdate()
        {
            this.CheckAssertionFor(CheckMethod.FixedUpdate);
        }

        public string GetCreationLocation()
        {
            if (!string.IsNullOrEmpty(this.m_CreatedInFilePath))
            {
                int startIndex = this.m_CreatedInFilePath.LastIndexOf(@"\") + 1;
                return string.Format("{0}, line {1} ({2})", this.m_CreatedInFilePath.Substring(startIndex), this.m_CreatedInFileLine, this.m_CreatedInFilePath);
            }
            return string.Empty;
        }

        public UnityEngine.Object GetFailureReferenceObject()
        {
            return this;
        }

        public bool IsCheckMethodSelected(CheckMethod method)
        {
            return (method == (this.checkMethods & method));
        }

        public void LateUpdate()
        {
            this.CheckAssertionFor(CheckMethod.LateUpdate);
        }

        public void OnBecameInvisible()
        {
            this.CheckAssertionFor(CheckMethod.OnBecameInvisible);
        }

        public void OnBecameVisible()
        {
            this.CheckAssertionFor(CheckMethod.OnBecameVisible);
        }

        public void OnCollisionEnter()
        {
            this.CheckAssertionFor(CheckMethod.OnCollisionEnter);
        }

        public void OnCollisionEnter2D()
        {
            this.CheckAssertionFor(CheckMethod.OnCollisionEnter2D);
        }

        public void OnCollisionExit()
        {
            this.CheckAssertionFor(CheckMethod.OnCollisionExit);
        }

        public void OnCollisionExit2D()
        {
            this.CheckAssertionFor(CheckMethod.OnCollisionExit2D);
        }

        public void OnCollisionStay()
        {
            this.CheckAssertionFor(CheckMethod.OnCollisionStay);
        }

        public void OnCollisionStay2D()
        {
            this.CheckAssertionFor(CheckMethod.OnCollisionStay2D);
        }

        private void OnComponentCopy()
        {
            if (this.m_ActionBase != null)
            {
                IEnumerable<UnityEngine.Object> source = Enumerable.Where<UnityEngine.Object>(Resources.FindObjectsOfTypeAll(typeof(AssertionComponent)), delegate (UnityEngine.Object o) {
                    return (((AssertionComponent) o).m_ActionBase == this.m_ActionBase) && (o != this);
                });
                if (Enumerable.Any<UnityEngine.Object>(source))
                {
                    if (Enumerable.Count<UnityEngine.Object>(source) > 1)
                    {
                        UnityEngine.Debug.LogWarning("More than one refence to comparer found. This shouldn't happen");
                    }
                    AssertionComponent component = Enumerable.First<UnityEngine.Object>(source) as AssertionComponent;
                    this.m_ActionBase = component.m_ActionBase.CreateCopy(component.gameObject, base.gameObject);
                }
            }
        }

        public void OnControllerColliderHit()
        {
            this.CheckAssertionFor(CheckMethod.OnControllerColliderHit);
        }

        public void OnDestroy()
        {
            this.CheckAssertionFor(CheckMethod.OnDestroy);
        }

        public void OnDisable()
        {
            this.CheckAssertionFor(CheckMethod.OnDisable);
        }

        public void OnEnable()
        {
            this.CheckAssertionFor(CheckMethod.OnEnable);
        }

        public void OnJointBreak()
        {
            this.CheckAssertionFor(CheckMethod.OnJointBreak);
        }

        public void OnParticleCollision()
        {
            this.CheckAssertionFor(CheckMethod.OnParticleCollision);
        }

        public void OnTriggerEnter()
        {
            this.CheckAssertionFor(CheckMethod.OnTriggerEnter);
        }

        public void OnTriggerEnter2D()
        {
            this.CheckAssertionFor(CheckMethod.OnTriggerEnter2D);
        }

        public void OnTriggerExit()
        {
            this.CheckAssertionFor(CheckMethod.OnTriggerExit);
        }

        public void OnTriggerExit2D()
        {
            this.CheckAssertionFor(CheckMethod.OnTriggerExit2D);
        }

        public void OnTriggerStay()
        {
            this.CheckAssertionFor(CheckMethod.OnTriggerStay);
        }

        public void OnTriggerStay2D()
        {
            this.CheckAssertionFor(CheckMethod.OnTriggerStay2D);
        }

        public void OnValidate()
        {
            if (Application.isEditor)
            {
                this.OnComponentCopy();
            }
        }

        public bool ShouldCheckOnFrame()
        {
            if (Time.frameCount <= this.m_CheckOnFrame)
            {
                return false;
            }
            if (this.repeatCheckFrame)
            {
                this.m_CheckOnFrame += this.repeatEveryFrame;
            }
            else
            {
                this.m_CheckOnFrame = 0x7fffffff;
            }
            return true;
        }

        public void Start()
        {
            this.CheckAssertionFor(CheckMethod.Start);
            if (this.IsCheckMethodSelected(CheckMethod.AfterPeriodOfTime))
            {
                base.StartCoroutine("CheckPeriodically");
            }
            if (this.IsCheckMethodSelected(CheckMethod.Update))
            {
                this.m_CheckOnFrame = Time.frameCount + this.checkAfterFrames;
            }
        }

        public void Update()
        {
            if (this.IsCheckMethodSelected(CheckMethod.Update) && this.ShouldCheckOnFrame())
            {
                this.CheckAssertionFor(CheckMethod.Update);
            }
        }

        public ActionBase Action
        {
            get
            {
                return this.m_ActionBase;
            }
            set
            {
                this.m_ActionBase = value;
                this.m_ActionBase.go = base.gameObject;
            }
        }

        public AssertionComponent Component
        {
            get
            {
                return this;
            }
        }

        public bool TimeCheckRepeat
        {
            set
            {
                this.repeatCheckTime = value;
            }
        }

        public float TimeCheckRepeatFrequency
        {
            set
            {
                this.repeatEveryTime = value;
            }
        }

        public float TimeCheckStartAfter
        {
            set
            {
                this.checkAfterTime = value;
            }
        }

        public bool UpdateCheckRepeat
        {
            set
            {
                this.repeatCheckFrame = value;
            }
        }

        public int UpdateCheckRepeatFrequency
        {
            set
            {
                this.repeatEveryFrame = value;
            }
        }

        public int UpdateCheckStartOnFrame
        {
            set
            {
                this.checkAfterFrames = value;
            }
        }

        [CompilerGenerated]
        private sealed class <CheckPeriodically>c__Iterator20 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AssertionComponent <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = new WaitForSeconds(this.<>f__this.checkAfterTime);
                        this.$PC = 1;
                        goto Label_009F;

                    case 1:
                        this.<>f__this.CheckAssertionFor(CheckMethod.AfterPeriodOfTime);
                        break;

                    case 2:
                        this.<>f__this.CheckAssertionFor(CheckMethod.AfterPeriodOfTime);
                        break;

                    default:
                        goto Label_009D;
                }
                if (this.<>f__this.repeatCheckTime)
                {
                    this.$current = new WaitForSeconds(this.<>f__this.repeatEveryTime);
                    this.$PC = 2;
                    goto Label_009F;
                }
                this.$PC = -1;
            Label_009D:
                return false;
            Label_009F:
                return true;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

