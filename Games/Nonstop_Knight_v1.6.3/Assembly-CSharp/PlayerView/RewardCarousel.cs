namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class RewardCarousel : MonoBehaviour
    {
        [CompilerGenerated]
        private GameLogic.LootTable <LootTable>k__BackingField;
        [CompilerGenerated]
        private RectTransform <RectTm>k__BackingField;
        public const float DIMMED_ALPHA = 0.4f;
        private Coroutine m_animationRoutine;
        private RewardGalleryCell[] m_rewardGalleryCells;
        public const int NUM_CAROUSEL_CELLS = 5;
        public const float PAUSE_BETWEEN_SHIFTS_SECONDS = 2f;
        public RectTransform[] PosAndScaleMarkers;
        public const float REVOLVE_ANIMATION_DURATION = 0.75f;
        public GameObject RewardPrototype;

        public void animate()
        {
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            this.m_animationRoutine = base.StartCoroutine(this.animationRoutine());
        }

        [DebuggerHidden]
        private IEnumerator animationRoutine()
        {
            <animationRoutine>c__Iterator167 iterator = new <animationRoutine>c__Iterator167();
            iterator.<>f__this = this;
            return iterator;
        }

        protected void Awake()
        {
            this.RectTm = base.GetComponent<RectTransform>();
            this.m_rewardGalleryCells = new RewardGalleryCell[5];
            for (int i = 0; i < this.m_rewardGalleryCells.Length; i++)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.RewardPrototype);
                obj2.name = this.RewardPrototype.name + "-" + i;
                obj2.transform.SetParent(this.RectTm);
                this.m_rewardGalleryCells[i] = obj2.GetComponent<RewardGalleryCell>();
                obj2.SetActive(false);
            }
            this.RewardPrototype.SetActive(false);
        }

        private RewardGalleryCell getCarouselCellWithCircularIndex(int leftmost, int offset)
        {
            return this.m_rewardGalleryCells[(leftmost + offset) % 5];
        }

        public void initialize(GameLogic.LootTable lootTable)
        {
            this.LootTable = lootTable;
            UnityUtils.StopCoroutine(this, ref this.m_animationRoutine);
            for (int i = 0; i < 5; i++)
            {
                RewardGalleryCell cell = this.m_rewardGalleryCells[i];
                if (i < 3)
                {
                    this.initializeCarouselCell(cell, i);
                    cell.gameObject.SetActive(true);
                }
                else
                {
                    cell.gameObject.SetActive(false);
                }
            }
        }

        private void initializeCarouselCell(RewardGalleryCell cell, int posAndScaleMarkerIdx)
        {
        }

        public GameLogic.LootTable LootTable
        {
            [CompilerGenerated]
            get
            {
                return this.<LootTable>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<LootTable>k__BackingField = value;
            }
        }

        public RectTransform RectTm
        {
            [CompilerGenerated]
            get
            {
                return this.<RectTm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<RectTm>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <animationRoutine>c__Iterator167 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal RewardCarousel <>f__this;
            internal RewardGalleryCell <cell>__2;
            internal IEnumerator <ie>__1;
            internal int <leftmost>__0;

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
                        this.<leftmost>__0 = 0;
                        break;

                    case 1:
                        goto Label_005E;

                    case 2:
                        goto Label_00DB;

                    default:
                        goto Label_013F;
                }
            Label_002C:
                this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(2f);
            Label_005E:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    goto Label_0141;
                }
                this.<cell>__2 = this.<>f__this.getCarouselCellWithCircularIndex(this.<leftmost>__0, 3);
                this.<>f__this.initializeCarouselCell(this.<cell>__2, 2);
                this.<cell>__2.gameObject.SetActive(true);
                this.<ie>__1 = TimeUtil.WaitForUnscaledSeconds(0.75f);
            Label_00DB:
                while (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 2;
                    goto Label_0141;
                }
                this.<>f__this.getCarouselCellWithCircularIndex(this.<leftmost>__0, 4).RectTm.SetAsFirstSibling();
                this.<>f__this.getCarouselCellWithCircularIndex(this.<leftmost>__0, 2).RectTm.SetAsLastSibling();
                this.<leftmost>__0 = (this.<leftmost>__0 + 1) % 5;
                goto Label_002C;
                this.$PC = -1;
            Label_013F:
                return false;
            Label_0141:
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

