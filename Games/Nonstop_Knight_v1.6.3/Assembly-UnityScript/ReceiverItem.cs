using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class ReceiverItem
{
    public string action = "OnSignal";
    public float delay;
    public GameObject receiver;

    public override IEnumerator SendWithDelay(MonoBehaviour sender)
    {
        return new $SendWithDelay$86(sender, this).GetEnumerator();
    }

    [Serializable, CompilerGenerated]
    internal sealed class $SendWithDelay$86 : GenericGenerator<WaitForSeconds>
    {
        internal ReceiverItem $self_$90;
        internal MonoBehaviour $sender$89;

        public $SendWithDelay$86(MonoBehaviour sender, ReceiverItem self_)
        {
            this.$sender$89 = sender;
            this.$self_$90 = self_;
        }

        public override IEnumerator<WaitForSeconds> GetEnumerator()
        {
            return new $(this.$sender$89, this.$self_$90);
        }

        [Serializable, CompilerGenerated]
        internal sealed class $ : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
        {
            internal ReceiverItem $self_$88;
            internal MonoBehaviour $sender$87;

            public $(MonoBehaviour sender, ReceiverItem self_)
            {
                this.$sender$87 = sender;
                this.$self_$88 = self_;
            }

            public override bool MoveNext()
            {
                // This item is obfuscated and can not be translated.
                switch (base._state)
                {
                    case 2:
                        if (this.$self_$88.receiver == null)
                        {
                            Debug.LogWarning(((((("No receiver of signal \"" + this.$self_$88.action) + "\" on object ") + this.$sender$87.name) + " (") + this.$sender$87.GetType().Name) + ")", this.$sender$87);
                            break;
                        }
                        this.$self_$88.receiver.SendMessage(this.$self_$88.action);
                        break;

                    default:
                        goto Label_00D4;
                }
                this.YieldDefault(1);
            Label_00D4:
                return false;
            }
        }
    }
}

