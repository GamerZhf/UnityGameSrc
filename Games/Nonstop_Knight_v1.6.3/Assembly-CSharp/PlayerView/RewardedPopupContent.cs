namespace PlayerView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class RewardedPopupContent : MenuContent
    {
        public Transform RewardsGrid;

        protected void InitRewardGrid(List<Reward> rewards)
        {
            int num = 0;
            IEnumerator enumerator = this.RewardsGrid.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    if (num < rewards.Count)
                    {
                        current.gameObject.SetActive(true);
                        IAPPopupRewardCell component = current.gameObject.GetComponent<IAPPopupRewardCell>();
                        if (component != null)
                        {
                            component.InitForReward(rewards[num++]);
                        }
                    }
                    else
                    {
                        current.gameObject.SetActive(false);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable == null)
                {
                }
                disposable.Dispose();
            }
        }

        protected override void onAwake()
        {
        }

        protected override void onCleanup()
        {
        }

        protected override void onPreShow([Optional, DefaultParameterValue(null)] object param)
        {
        }

        protected override void onRefresh()
        {
        }

        public override MenuContentType ContentType
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

