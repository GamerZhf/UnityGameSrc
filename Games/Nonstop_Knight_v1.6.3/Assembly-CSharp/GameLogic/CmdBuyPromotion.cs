namespace GameLogic
{
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdBuyPromotion : ICommand
    {
        private Player m_player;
        private RemotePromotion m_promotion;

        public CmdBuyPromotion(Player player, RemotePromotion promotion)
        {
            this.m_player = player;
            this.m_promotion = promotion;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator93 iterator = new <executeRoutine>c__Iterator93();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(Player player, RemotePromotion promotion)
        {
            if (promotion.PromotionType == EPromotionType.Reward)
            {
                List<ProductReward> costs = Service.Binder.PromotionManager.GetCosts(promotion);
                List<Reward> rewards = Service.Binder.PromotionManager.GetRewards(promotion);
                foreach (ProductReward reward in costs)
                {
                    if (reward.key.Equals(ResourceType.Diamond.ToString()))
                    {
                        Vector3? worldPt = null;
                        CmdGainResources.ExecuteStatic(player, ResourceType.Diamond, (double) -reward.amount, false, string.Empty, worldPt);
                    }
                    if (reward.key.Equals(ResourceType.Token.ToString()))
                    {
                        Vector3? nullable2 = null;
                        CmdGainResources.ExecuteStatic(player, ResourceType.Token, (double) -reward.amount, false, string.Empty, nullable2);
                    }
                    if (reward.key.Equals(ResourceType.Coin.ToString()))
                    {
                        Vector3? nullable3 = null;
                        CmdGainResources.ExecuteStatic(player, ResourceType.Coin, (double) -reward.amount, false, string.Empty, nullable3);
                    }
                }
                RewardHelper.ClaimReward(rewards, true);
            }
            else
            {
                UnityEngine.Debug.LogError("Cannot buy promotion: " + promotion.promotionid);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator93 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdBuyPromotion <>f__this;

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
                    CmdBuyPromotion.ExecuteStatic(this.<>f__this.m_player, this.<>f__this.m_promotion);
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

