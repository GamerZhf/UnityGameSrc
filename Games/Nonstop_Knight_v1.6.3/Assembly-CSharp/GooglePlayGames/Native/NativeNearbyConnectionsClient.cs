namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi.Nearby;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal class NativeNearbyConnectionsClient : INearbyConnectionClient
    {
        private readonly NearbyConnectionsManager mManager;

        internal NativeNearbyConnectionsClient(NearbyConnectionsManager manager)
        {
            this.mManager = Misc.CheckNotNull<NearbyConnectionsManager>(manager);
        }

        public void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, IMessageListener listener)
        {
            Misc.CheckNotNull<string>(remoteEndpointId, "remoteEndpointId");
            Misc.CheckNotNull<byte[]>(payload, "payload");
            Misc.CheckNotNull<IMessageListener>(listener, "listener");
            Logger.d("Calling AcceptConncectionRequest");
            this.mManager.AcceptConnectionRequest(remoteEndpointId, payload, ToMessageListener(listener));
            Logger.d("Called!");
        }

        public void DisconnectFromEndpoint(string remoteEndpointId)
        {
            this.mManager.DisconnectFromEndpoint(remoteEndpointId);
        }

        public string GetAppBundleId()
        {
            return this.mManager.AppBundleId;
        }

        public string GetServiceId()
        {
            return NearbyConnectionsManager.ServiceId;
        }

        private void InternalSend(List<string> recipientEndpointIds, byte[] payload, bool isReliable)
        {
            if (recipientEndpointIds == null)
            {
                throw new ArgumentNullException("recipientEndpointIds");
            }
            if (payload == null)
            {
                throw new ArgumentNullException("payload");
            }
            if (recipientEndpointIds.Contains(null))
            {
                throw new InvalidOperationException("Cannot send a message to a null recipient");
            }
            if (recipientEndpointIds.Count == 0)
            {
                Logger.w("Attempted to send a reliable message with no recipients");
            }
            else
            {
                if (isReliable)
                {
                    if (payload.Length > this.MaxReliableMessagePayloadLength())
                    {
                        throw new InvalidOperationException("cannot send more than " + this.MaxReliableMessagePayloadLength() + " bytes");
                    }
                }
                else if (payload.Length > this.MaxUnreliableMessagePayloadLength())
                {
                    throw new InvalidOperationException("cannot send more than " + this.MaxUnreliableMessagePayloadLength() + " bytes");
                }
                foreach (string str in recipientEndpointIds)
                {
                    if (isReliable)
                    {
                        this.mManager.SendReliable(str, payload);
                    }
                    else
                    {
                        this.mManager.SendUnreliable(str, payload);
                    }
                }
            }
        }

        public string LocalDeviceId()
        {
            return this.mManager.LocalDeviceId();
        }

        public string LocalEndpointId()
        {
            return this.mManager.LocalEndpointId();
        }

        public int MaxReliableMessagePayloadLength()
        {
            return 0x1000;
        }

        public int MaxUnreliableMessagePayloadLength()
        {
            return 0x490;
        }

        public void RejectConnectionRequest(string requestingEndpointId)
        {
            Misc.CheckNotNull<string>(requestingEndpointId, "requestingEndpointId");
            this.mManager.RejectConnectionRequest(requestingEndpointId);
        }

        public void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<ConnectionResponse> responseCallback, IMessageListener listener)
        {
            <SendConnectionRequest>c__AnonStorey294 storey = new <SendConnectionRequest>c__AnonStorey294();
            storey.responseCallback = responseCallback;
            Misc.CheckNotNull<string>(remoteEndpointId, "remoteEndpointId");
            Misc.CheckNotNull<byte[]>(payload, "payload");
            Misc.CheckNotNull<Action<ConnectionResponse>>(storey.responseCallback, "responseCallback");
            Misc.CheckNotNull<IMessageListener>(listener, "listener");
            storey.responseCallback = Callbacks.AsOnGameThreadCallback<ConnectionResponse>(storey.responseCallback);
            using (NativeMessageListenerHelper helper = ToMessageListener(listener))
            {
                this.mManager.SendConnectionRequest(name, remoteEndpointId, payload, new Action<long, NativeConnectionResponse>(storey.<>m__C7), helper);
            }
        }

        public void SendReliable(List<string> recipientEndpointIds, byte[] payload)
        {
            this.InternalSend(recipientEndpointIds, payload, true);
        }

        public void SendUnreliable(List<string> recipientEndpointIds, byte[] payload)
        {
            this.InternalSend(recipientEndpointIds, payload, false);
        }

        public void StartAdvertising(string name, List<string> appIdentifiers, TimeSpan? advertisingDuration, Action<AdvertisingResult> resultCallback, Action<ConnectionRequest> requestCallback)
        {
            <StartAdvertising>c__AnonStorey293 storey = new <StartAdvertising>c__AnonStorey293();
            storey.resultCallback = resultCallback;
            storey.requestCallback = requestCallback;
            Misc.CheckNotNull<List<string>>(appIdentifiers, "appIdentifiers");
            Misc.CheckNotNull<Action<AdvertisingResult>>(storey.resultCallback, "resultCallback");
            Misc.CheckNotNull<Action<ConnectionRequest>>(storey.requestCallback, "connectionRequestCallback");
            if (advertisingDuration.HasValue && (advertisingDuration.Value.Ticks < 0L))
            {
                throw new InvalidOperationException("advertisingDuration must be positive");
            }
            storey.resultCallback = Callbacks.AsOnGameThreadCallback<AdvertisingResult>(storey.resultCallback);
            storey.requestCallback = Callbacks.AsOnGameThreadCallback<ConnectionRequest>(storey.requestCallback);
            this.mManager.StartAdvertising(name, Enumerable.ToList<NativeAppIdentifier>(Enumerable.Select<string, NativeAppIdentifier>(appIdentifiers, new Func<string, NativeAppIdentifier>(NativeAppIdentifier.FromString))), ToTimeoutMillis(advertisingDuration), new Action<long, NativeStartAdvertisingResult>(storey.<>m__C5), new Action<long, NativeConnectionRequest>(storey.<>m__C6));
        }

        public void StartDiscovery(string serviceId, TimeSpan? advertisingTimeout, IDiscoveryListener listener)
        {
            Misc.CheckNotNull<string>(serviceId, "serviceId");
            Misc.CheckNotNull<IDiscoveryListener>(listener, "listener");
            using (NativeEndpointDiscoveryListenerHelper helper = ToDiscoveryListener(listener))
            {
                this.mManager.StartDiscovery(serviceId, ToTimeoutMillis(advertisingTimeout), helper);
            }
        }

        public void StopAdvertising()
        {
            this.mManager.StopAdvertising();
        }

        public void StopAllConnections()
        {
            this.mManager.StopAllConnections();
        }

        public void StopDiscovery(string serviceId)
        {
            Misc.CheckNotNull<string>(serviceId, "serviceId");
            this.mManager.StopDiscovery(serviceId);
        }

        private static NativeEndpointDiscoveryListenerHelper ToDiscoveryListener(IDiscoveryListener listener)
        {
            <ToDiscoveryListener>c__AnonStorey296 storey = new <ToDiscoveryListener>c__AnonStorey296();
            storey.listener = listener;
            storey.listener = new OnGameThreadDiscoveryListener(storey.listener);
            NativeEndpointDiscoveryListenerHelper helper = new NativeEndpointDiscoveryListenerHelper();
            helper.SetOnEndpointFound(new Action<long, NativeEndpointDetails>(storey.<>m__CA));
            helper.SetOnEndpointLostCallback(new Action<long, string>(storey.<>m__CB));
            return helper;
        }

        private static NativeMessageListenerHelper ToMessageListener(IMessageListener listener)
        {
            <ToMessageListener>c__AnonStorey295 storey = new <ToMessageListener>c__AnonStorey295();
            storey.listener = listener;
            storey.listener = new OnGameThreadMessageListener(storey.listener);
            NativeMessageListenerHelper helper = new NativeMessageListenerHelper();
            helper.SetOnMessageReceivedCallback(new NativeMessageListenerHelper.OnMessageReceived(storey.<>m__C8));
            helper.SetOnDisconnectedCallback(new Action<long, string>(storey.<>m__C9));
            return helper;
        }

        private static long ToTimeoutMillis(TimeSpan? span)
        {
            return (!span.HasValue ? 0L : PInvokeUtilities.ToMilliseconds(span.Value));
        }

        [CompilerGenerated]
        private sealed class <SendConnectionRequest>c__AnonStorey294
        {
            internal Action<ConnectionResponse> responseCallback;

            internal void <>m__C7(long localClientId, NativeConnectionResponse response)
            {
                this.responseCallback(response.AsResponse(localClientId));
            }
        }

        [CompilerGenerated]
        private sealed class <StartAdvertising>c__AnonStorey293
        {
            internal Action<ConnectionRequest> requestCallback;
            internal Action<AdvertisingResult> resultCallback;

            internal void <>m__C5(long localClientId, NativeStartAdvertisingResult result)
            {
                this.resultCallback(result.AsResult());
            }

            internal void <>m__C6(long localClientId, NativeConnectionRequest request)
            {
                this.requestCallback(request.AsRequest());
            }
        }

        [CompilerGenerated]
        private sealed class <ToDiscoveryListener>c__AnonStorey296
        {
            internal IDiscoveryListener listener;

            internal void <>m__CA(long localClientId, NativeEndpointDetails endpoint)
            {
                this.listener.OnEndpointFound(endpoint.ToDetails());
            }

            internal void <>m__CB(long localClientId, string lostEndpointId)
            {
                this.listener.OnEndpointLost(lostEndpointId);
            }
        }

        [CompilerGenerated]
        private sealed class <ToMessageListener>c__AnonStorey295
        {
            internal IMessageListener listener;

            internal void <>m__C8(long localClientId, string endpointId, byte[] data, bool isReliable)
            {
                this.listener.OnMessageReceived(endpointId, data, isReliable);
            }

            internal void <>m__C9(long localClientId, string endpointId)
            {
                this.listener.OnRemoteEndpointDisconnected(endpointId);
            }
        }

        protected class OnGameThreadDiscoveryListener : IDiscoveryListener
        {
            private readonly IDiscoveryListener mListener;

            public OnGameThreadDiscoveryListener(IDiscoveryListener listener)
            {
                this.mListener = Misc.CheckNotNull<IDiscoveryListener>(listener);
            }

            public void OnEndpointFound(EndpointDetails discoveredEndpoint)
            {
                <OnEndpointFound>c__AnonStorey299 storey = new <OnEndpointFound>c__AnonStorey299();
                storey.discoveredEndpoint = discoveredEndpoint;
                storey.<>f__this = this;
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__CE));
            }

            public void OnEndpointLost(string lostEndpointId)
            {
                <OnEndpointLost>c__AnonStorey29A storeya = new <OnEndpointLost>c__AnonStorey29A();
                storeya.lostEndpointId = lostEndpointId;
                storeya.<>f__this = this;
                PlayGamesHelperObject.RunOnGameThread(new Action(storeya.<>m__CF));
            }

            [CompilerGenerated]
            private sealed class <OnEndpointFound>c__AnonStorey299
            {
                internal NativeNearbyConnectionsClient.OnGameThreadDiscoveryListener <>f__this;
                internal EndpointDetails discoveredEndpoint;

                internal void <>m__CE()
                {
                    this.<>f__this.mListener.OnEndpointFound(this.discoveredEndpoint);
                }
            }

            [CompilerGenerated]
            private sealed class <OnEndpointLost>c__AnonStorey29A
            {
                internal NativeNearbyConnectionsClient.OnGameThreadDiscoveryListener <>f__this;
                internal string lostEndpointId;

                internal void <>m__CF()
                {
                    this.<>f__this.mListener.OnEndpointLost(this.lostEndpointId);
                }
            }
        }

        protected class OnGameThreadMessageListener : IMessageListener
        {
            private readonly IMessageListener mListener;

            public OnGameThreadMessageListener(IMessageListener listener)
            {
                this.mListener = Misc.CheckNotNull<IMessageListener>(listener);
            }

            public void OnMessageReceived(string remoteEndpointId, byte[] data, bool isReliableMessage)
            {
                <OnMessageReceived>c__AnonStorey297 storey = new <OnMessageReceived>c__AnonStorey297();
                storey.remoteEndpointId = remoteEndpointId;
                storey.data = data;
                storey.isReliableMessage = isReliableMessage;
                storey.<>f__this = this;
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__CC));
            }

            public void OnRemoteEndpointDisconnected(string remoteEndpointId)
            {
                <OnRemoteEndpointDisconnected>c__AnonStorey298 storey = new <OnRemoteEndpointDisconnected>c__AnonStorey298();
                storey.remoteEndpointId = remoteEndpointId;
                storey.<>f__this = this;
                PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__CD));
            }

            [CompilerGenerated]
            private sealed class <OnMessageReceived>c__AnonStorey297
            {
                internal NativeNearbyConnectionsClient.OnGameThreadMessageListener <>f__this;
                internal byte[] data;
                internal bool isReliableMessage;
                internal string remoteEndpointId;

                internal void <>m__CC()
                {
                    this.<>f__this.mListener.OnMessageReceived(this.remoteEndpointId, this.data, this.isReliableMessage);
                }
            }

            [CompilerGenerated]
            private sealed class <OnRemoteEndpointDisconnected>c__AnonStorey298
            {
                internal NativeNearbyConnectionsClient.OnGameThreadMessageListener <>f__this;
                internal string remoteEndpointId;

                internal void <>m__CD()
                {
                    this.<>f__this.mListener.OnRemoteEndpointDisconnected(this.remoteEndpointId);
                }
            }
        }
    }
}

