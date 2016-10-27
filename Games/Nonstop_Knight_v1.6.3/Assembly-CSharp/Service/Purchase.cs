namespace Service
{
    using Pathfinding.Serialization.JsonFx;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public abstract class Purchase
    {
        [JsonIgnore]
        public PremiumProduct product;
        [JsonIgnore]
        private EPurchaseState state;
        [JsonIgnore]
        public Dictionary<string, string> TrackingPayload = new Dictionary<string, string>();
        [JsonIgnore]
        public UpdateResponse Update;

        public abstract void AbortPurchase();
        public abstract ValidationRequest CreateValidationRequest();
        [DebuggerHidden]
        public virtual IEnumerator DoAcc()
        {
            <DoAcc>c__Iterator208 iterator = new <DoAcc>c__Iterator208();
            iterator.<>f__this = this;
            return iterator;
        }

        public abstract IEnumerator DoCommit();
        public abstract IEnumerator DoPurchase();
        [DebuggerHidden]
        public virtual IEnumerator DoValidate()
        {
            <DoValidate>c__Iterator207 iterator = new <DoValidate>c__Iterator207();
            iterator.<>f__this = this;
            return iterator;
        }

        [JsonIgnore]
        public EPurchaseState State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
                if (Binder.EventBus != null)
                {
                    Binder.EventBus.IapShopStateChanged();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <DoAcc>c__Iterator208 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Purchase <>f__this;

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
                        this.$current = Binder.ShopService.Acc(this.<>f__this);
                        this.$PC = 1;
                        return true;

                    case 1:
                        this.$PC = -1;
                        break;
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

        [CompilerGenerated]
        private sealed class <DoValidate>c__Iterator207 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal Purchase <>f__this;
            internal ShopService <shop>__0;

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
                        this.<shop>__0 = Binder.ShopService;
                        if ((this.<>f__this.State != EPurchaseState.Success) && (this.<>f__this.State != EPurchaseState.Pending))
                        {
                            break;
                        }
                        this.$current = this.<shop>__0.Validate(this.<>f__this);
                        this.$PC = 1;
                        goto Label_00F7;

                    case 1:
                        if (this.<>f__this.State != EPurchaseState.Validated)
                        {
                            if (this.<>f__this.State == EPurchaseState.Pending)
                            {
                                this.<>f__this.AbortPurchase();
                            }
                            break;
                        }
                        this.$current = this.<>f__this.DoCommit();
                        this.$PC = 2;
                        goto Label_00F7;

                    case 2:
                        Binder.SdkController.Purchase(this.<>f__this.product);
                        Binder.EventBus.IapShopPurchase(this.<>f__this.product);
                        break;

                    default:
                        goto Label_00F5;
                }
                this.$PC = -1;
            Label_00F5:
                return false;
            Label_00F7:
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

