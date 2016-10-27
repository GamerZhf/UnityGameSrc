namespace GooglePlayGames.BasicApi.Nearby
{
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ConnectionRequest
    {
        private readonly EndpointDetails mRemoteEndpoint;
        private readonly byte[] mPayload;
        public ConnectionRequest(string remoteEndpointId, string remoteDeviceId, string remoteEndpointName, string serviceId, byte[] payload)
        {
            Logger.d("Constructing ConnectionRequest");
            this.mRemoteEndpoint = new EndpointDetails(remoteEndpointId, remoteDeviceId, remoteEndpointName, serviceId);
            this.mPayload = Misc.CheckNotNull<byte[]>(payload);
        }

        public EndpointDetails RemoteEndpoint
        {
            get
            {
                return this.mRemoteEndpoint;
            }
        }
        public byte[] Payload
        {
            get
            {
                return this.mPayload;
            }
        }
    }
}

