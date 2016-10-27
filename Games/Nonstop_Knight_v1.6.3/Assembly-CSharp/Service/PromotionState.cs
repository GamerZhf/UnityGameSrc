namespace Service
{
    using System;
    using System.Collections;

    public class PromotionState
    {
        private const byte ACTIVATED_ONCE = 4;
        private const byte CONSUMED = 2;
        private const byte DUNGEON_RELOADED = 5;
        private readonly BitArray m_controlBits;
        private ulong m_state;
        private byte[] m_stateBytes;
        private const byte TASKPANEL_SHOWN = 3;
        private const ulong TIMESTAMP = 0xffffffffffffffL;
        private const byte TRIGGER_SOLVED = 1;
        private const byte WAIT_FOR_TRIGGER = 0;

        public PromotionState(ulong state)
        {
            this.m_state = state;
            this.m_stateBytes = BitConverter.GetBytes(state);
            byte[] buffer1 = new byte[] { this.m_stateBytes[this.m_stateBytes.Length - 1] };
            this.m_controlBits = new BitArray(buffer1);
        }

        private byte ConvertToByte(BitArray bits)
        {
            byte[] array = new byte[1];
            bits.CopyTo(array, 0);
            return array[0];
        }

        public ulong GetAsLong()
        {
            this.m_stateBytes[this.m_stateBytes.Length - 1] = this.ConvertToByte(this.m_controlBits);
            return BitConverter.ToUInt64(this.m_stateBytes, 0);
        }

        private bool GetState(byte stateBit)
        {
            return this.m_controlBits[stateBit];
        }

        private void SetState(byte stateBit, bool state)
        {
            this.m_controlBits[stateBit] = state;
        }

        public bool ActivatedOnce
        {
            get
            {
                return this.GetState(4);
            }
            set
            {
                this.SetState(4, value);
            }
        }

        public bool Active
        {
            get
            {
                return ((!this.GetState(0) || this.GetState(1)) && !this.GetState(2));
            }
        }

        public bool Consumed
        {
            get
            {
                return this.GetState(2);
            }
            set
            {
                this.SetState(2, value);
            }
        }

        public bool DungeonReloaded
        {
            get
            {
                return this.GetState(5);
            }
            set
            {
                this.SetState(5, value);
            }
        }

        public bool TaskpanelShown
        {
            get
            {
                return this.GetState(3);
            }
            set
            {
                this.SetState(3, value);
            }
        }

        public ulong Timestamp
        {
            get
            {
                return (this.m_state & ((ulong) 0xffffffffffffffL));
            }
            set
            {
                this.m_stateBytes = BitConverter.GetBytes(value);
                this.m_state = this.GetAsLong();
            }
        }

        public bool TriggerSolved
        {
            get
            {
                return this.GetState(1);
            }
            set
            {
                this.SetState(1, value);
            }
        }

        public bool WaitingForTrigger
        {
            get
            {
                return this.GetState(0);
            }
            set
            {
                this.SetState(0, value);
            }
        }
    }
}

