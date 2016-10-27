namespace GooglePlayGames.BasicApi.Nearby
{
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct EndpointDetails
    {
        private readonly string mEndpointId;
        private readonly string mDeviceId;
        private readonly string mName;
        private readonly string mServiceId;
        public EndpointDetails(string endpointId, string deviceId, string name, string serviceId)
        {
            this.mEndpointId = Misc.CheckNotNull<string>(endpointId);
            this.mDeviceId = Misc.CheckNotNull<string>(deviceId);
            this.mName = Misc.CheckNotNull<string>(name);
            this.mServiceId = Misc.CheckNotNull<string>(serviceId);
        }

        public string EndpointId
        {
            get
            {
                return this.mEndpointId;
            }
        }
        public string DeviceId
        {
            get
            {
                return this.mDeviceId;
            }
        }
        public string Name
        {
            get
            {
                return this.mName;
            }
        }
        public string ServiceId
        {
            get
            {
                return this.mServiceId;
            }
        }
    }
}

