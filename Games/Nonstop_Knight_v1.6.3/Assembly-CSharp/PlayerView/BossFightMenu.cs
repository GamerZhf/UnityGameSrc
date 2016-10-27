namespace PlayerView
{
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.UI;

    public class BossFightMenu : Menu
    {
        public Text Title;

        public void onGoButtonClicked()
        {
            UnityEngine.Debug.Log("CHANGE ME");
        }

        [DebuggerHidden]
        public override IEnumerator preShowRoutine(MenuContentType targetMenuContentType, [Optional, DefaultParameterValue(null)] object parameter)
        {
            <preShowRoutine>c__Iterator110 iterator = new <preShowRoutine>c__Iterator110();
            iterator.<>f__this = this;
            return iterator;
        }

        public override bool IsOverlayMenu
        {
            get
            {
                return true;
            }
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.BossFight;
            }
        }

        [CompilerGenerated]
        private sealed class <preShowRoutine>c__Iterator110 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal BossFightMenu <>f__this;
            internal ActiveDungeon <ad>__0;
            internal Character <boss>__1;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    this.<ad>__0 = GameLogic.Binder.GameState.ActiveDungeon;
                    this.<boss>__1 = GameLogic.Binder.CharacterResources.getResource(this.<ad>__0.BossId);
                    this.<>f__this.Title.text = MenuHelpers.GetRarityColoredText(this.<boss>__1.Rarity, this.<boss>__1.Name, 1f);
                }
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

