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

    public class OutOfMaterialsMiniPopupContent : MenuContent
    {
        public Text Cost;
        private InputParams m_params;
        private Coroutine m_purchaseRoutine;
        private double m_totalDiamondCost;
        public List<IconWithText> Materials;

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
            this.m_params = (InputParams) param;
            this.onRefresh();
        }

        public void onPurchaseButtonClicked()
        {
            if (UnityUtils.CoroutineRunning(ref this.m_purchaseRoutine))
            {
            }
        }

        protected override void onRefresh()
        {
            base.m_contentMenu.refreshTitle("MISSING MATERIALS!", string.Empty, string.Empty);
            this.m_totalDiamondCost = 0.0;
            int num = 0;
            this.Materials[num].gameObject.SetActive(true);
            if (this.m_params.MissingResources != null)
            {
                foreach (KeyValuePair<ResourceType, double> pair in this.m_params.MissingResources)
                {
                    if (pair.Value > 0.0)
                    {
                        this.Materials[num].gameObject.SetActive(true);
                        if (((ResourceType) pair.Key) == ResourceType.Coin)
                        {
                            this.Materials[num].Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("DungeonHud", "icon_coin_floater");
                        }
                        else
                        {
                            this.Materials[num].Icon.sprite = null;
                        }
                        this.Materials[num].Text.text = pair.Value.ToString();
                        num++;
                    }
                }
            }
            if (this.m_params.MissingItems != null)
            {
                foreach (KeyValuePair<string, int> pair2 in this.m_params.MissingItems)
                {
                    if (pair2.Value > 0)
                    {
                        this.Materials[num].gameObject.SetActive(true);
                        Item item = GameLogic.Binder.ItemResources.getItemForLootTableRollId(pair2.Key, ItemType.UNSPECIFIED);
                        this.Materials[num].Icon.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", item.SpriteId);
                        this.Materials[num].Text.text = pair2.Value.ToString();
                        this.Materials[num].Background.gameObject.SetActive(pair2.Value > 1);
                        this.Materials[num].Text.gameObject.SetActive(pair2.Value > 1);
                        this.Materials[num].Borders.sprite = PlayerView.Binder.SpriteResources.getSprite("Menu", "sprite_ui_iconslot_empty");
                        num++;
                        this.m_totalDiamondCost += 1.0 * ((double) pair2.Value);
                    }
                }
            }
            for (int i = num; i < this.Materials.Count; i++)
            {
                this.Materials[i].gameObject.SetActive(false);
            }
            this.Cost.text = this.m_totalDiamondCost.ToString();
        }

        [DebuggerHidden]
        private IEnumerator purchaseRoutine()
        {
            <purchaseRoutine>c__Iterator15A iteratora = new <purchaseRoutine>c__Iterator15A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        public override MenuContentType ContentType
        {
            get
            {
                return MenuContentType.OutOfMaterialsMiniPopup;
            }
        }

        [CompilerGenerated]
        private sealed class <purchaseRoutine>c__Iterator15A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Dictionary<ResourceType, double>.Enumerator <$s_419>__1;
            internal Dictionary<string, int>.Enumerator <$s_420>__3;
            internal OutOfMaterialsMiniPopupContent <>f__this;
            internal KeyValuePair<ResourceType, double> <kv>__2;
            internal KeyValuePair<string, int> <kv>__4;
            internal Player <player>__0;

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
                    this.<player>__0 = GameLogic.Binder.GameState.Player;
                    if (this.<>f__this.m_totalDiamondCost > 0.0)
                    {
                        CmdGainResources.ExecuteStatic(this.<player>__0, ResourceType.Diamond, -this.<>f__this.m_totalDiamondCost, false, string.Empty, null);
                    }
                    if (this.<>f__this.m_params.MissingResources != null)
                    {
                        this.<$s_419>__1 = this.<>f__this.m_params.MissingResources.GetEnumerator();
                        try
                        {
                            while (this.<$s_419>__1.MoveNext())
                            {
                                this.<kv>__2 = this.<$s_419>__1.Current;
                                Vector3? worldPt = null;
                                CmdGainResources.ExecuteStatic(this.<player>__0, this.<kv>__2.Key, this.<kv>__2.Value, false, string.Empty, worldPt);
                            }
                        }
                        finally
                        {
                            this.<$s_419>__1.Dispose();
                        }
                    }
                    if (this.<>f__this.m_params.MissingItems != null)
                    {
                        this.<$s_420>__3 = this.<>f__this.m_params.MissingItems.GetEnumerator();
                        try
                        {
                            while (this.<$s_420>__3.MoveNext())
                            {
                                this.<kv>__4 = this.<$s_420>__3.Current;
                                CmdGainMaterials.ExecuteStatic(this.<player>__0, this.<kv>__4.Key, this.<kv>__4.Value);
                            }
                        }
                        finally
                        {
                            this.<$s_420>__3.Dispose();
                        }
                    }
                    this.<>f__this.m_purchaseRoutine = null;
                    if (this.<>f__this.m_params.SuccessCallback != null)
                    {
                        this.<>f__this.m_params.SuccessCallback();
                    }
                    else
                    {
                        PlayerView.Binder.MenuSystem.returnToPreviousMenu(true);
                    }
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

        [StructLayout(LayoutKind.Sequential)]
        public struct InputParams
        {
            public Dictionary<ResourceType, double> MissingResources;
            public Dictionary<string, int> MissingItems;
            public System.Action SuccessCallback;
        }
    }
}

