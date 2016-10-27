namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [SelectionBase]
    public class MenuCharacterView : MonoBehaviour, IDragHandler, IEventSystemHandler
    {
        [CompilerGenerated]
        private PlayerView.CharacterView <CharacterView>k__BackingField;
        [CompilerGenerated]
        private UnityEngine.Transform <Transform>k__BackingField;
        private Coroutine m_dragRotationRoutine;
        private float m_lastCharacterDragDeltaX;

        protected void Awake()
        {
            this.Transform = base.transform;
        }

        [DebuggerHidden]
        private IEnumerator dragRotationRoutine()
        {
            <dragRotationRoutine>c__IteratorEB reb = new <dragRotationRoutine>c__IteratorEB();
            reb.<>f__this = this;
            return reb;
        }

        protected void LateUpdate()
        {
            if (this.CharacterView != null)
            {
                this.Transform.position = this.CharacterView.Transform.position;
            }
        }

        protected void OnDisable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStarted -= new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded -= new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            UnityUtils.StopCoroutine(this, ref this.m_dragRotationRoutine);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData != null)
            {
                this.m_lastCharacterDragDeltaX = eventData.delta.x;
            }
        }

        protected void OnEnable()
        {
            GameLogic.IEventBus eventBus = GameLogic.Binder.EventBus;
            eventBus.OnGameplayStarted += new GameLogic.Events.GameplayStarted(this.onGameplayStarted);
            eventBus.OnGameplayEnded += new GameLogic.Events.GameplayEnded(this.onGameplayEnded);
            this.m_dragRotationRoutine = UnityUtils.StartCoroutine(this, this.dragRotationRoutine());
        }

        private void onGameplayEnded(ActiveDungeon activeDungeon)
        {
            this.m_lastCharacterDragDeltaX = 0f;
            if (this.CharacterView != null)
            {
                this.CharacterView.Transform.localRotation = Quaternion.identity;
            }
        }

        private void onGameplayStarted(ActiveDungeon activeDungeon)
        {
            this.m_lastCharacterDragDeltaX = 0f;
            if (this.CharacterView != null)
            {
                this.CharacterView.Transform.localRotation = Quaternion.identity;
            }
        }

        public void setTarget(PlayerView.CharacterView targetCharacterView)
        {
            this.CharacterView = targetCharacterView;
        }

        public PlayerView.CharacterView CharacterView
        {
            [CompilerGenerated]
            get
            {
                return this.<CharacterView>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<CharacterView>k__BackingField = value;
            }
        }

        public UnityEngine.Transform Transform
        {
            [CompilerGenerated]
            get
            {
                return this.<Transform>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Transform>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <dragRotationRoutine>c__IteratorEB : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal MenuCharacterView <>f__this;
            internal float <angle>__0;

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
                        break;

                    case 1:
                        break;
                        this.$PC = -1;
                        goto Label_00D4;

                    default:
                        goto Label_00D4;
                }
                if (Mathf.Abs(this.<>f__this.m_lastCharacterDragDeltaX) > 0.1f)
                {
                    this.<angle>__0 = Mathf.Clamp((float) (-this.<>f__this.m_lastCharacterDragDeltaX * 0.35f), (float) -35f, (float) 35f);
                    if (this.<>f__this.CharacterView != null)
                    {
                        this.<>f__this.CharacterView.Transform.Rotate((Vector3) (Vector3.up * this.<angle>__0), Space.World);
                    }
                    this.<>f__this.m_lastCharacterDragDeltaX *= 0.94f;
                }
                this.$current = null;
                this.$PC = 1;
                return true;
            Label_00D4:
                return false;
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

