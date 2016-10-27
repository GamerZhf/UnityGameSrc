namespace Service.SupersonicAds
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class VideoResult
    {
        [CompilerGenerated]
        private int <Amount>k__BackingField;
        [CompilerGenerated]
        private object <CustomData>k__BackingField;
        [CompilerGenerated]
        private PlayVideoError <Error>k__BackingField;
        [CompilerGenerated]
        private int <Id>k__BackingField;
        [CompilerGenerated]
        private string <Name>k__BackingField;
        [CompilerGenerated]
        private bool <Success>k__BackingField;
        private static int nextId;

        public VideoResult(bool _success, PlayVideoError _error)
        {
            this.Id = nextId++;
            this.Success = _success;
            this.Amount = -1;
            this.Name = null;
            this.Error = _error;
        }

        public VideoResult(bool _success, [Optional, DefaultParameterValue(-1)] int _amount, [Optional, DefaultParameterValue(null)] string _name, [Optional, DefaultParameterValue(null)] object _customData)
        {
            this.Id = nextId++;
            this.Success = _success;
            this.Amount = _amount;
            this.Name = _name;
            this.CustomData = _customData;
        }

        public int Amount
        {
            [CompilerGenerated]
            get
            {
                return this.<Amount>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Amount>k__BackingField = value;
            }
        }

        public object CustomData
        {
            [CompilerGenerated]
            get
            {
                return this.<CustomData>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<CustomData>k__BackingField = value;
            }
        }

        public PlayVideoError Error
        {
            [CompilerGenerated]
            get
            {
                return this.<Error>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Error>k__BackingField = value;
            }
        }

        public int Id
        {
            [CompilerGenerated]
            get
            {
                return this.<Id>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Id>k__BackingField = value;
            }
        }

        public string Name
        {
            [CompilerGenerated]
            get
            {
                return this.<Name>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Name>k__BackingField = value;
            }
        }

        public bool Success
        {
            [CompilerGenerated]
            get
            {
                return this.<Success>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Success>k__BackingField = value;
            }
        }
    }
}

