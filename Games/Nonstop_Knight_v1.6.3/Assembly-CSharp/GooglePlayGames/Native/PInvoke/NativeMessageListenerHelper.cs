namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class NativeMessageListenerHelper : BaseReferenceHolder
    {
        internal NativeMessageListenerHelper() : base(MessageListenerHelper.MessageListenerHelper_Construct())
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            MessageListenerHelper.MessageListenerHelper_Dispose(selfPointer);
        }

        [MonoPInvokeCallback(typeof(MessageListenerHelper.OnDisconnectedCallback))]
        private static void InternalOnDisconnectedCallback(long id, string lostEndpointId, IntPtr userData)
        {
            Action<long, string> action = Callbacks.IntPtrToPermanentCallback<Action<long, string>>(userData);
            if (action != null)
            {
                try
                {
                    action(id, lostEndpointId);
                }
                catch (Exception exception)
                {
                    Logger.e("Error encountered executing NativeMessageListenerHelper#InternalOnDisconnectedCallback. Smothering to avoid passing exception into Native: " + exception);
                }
            }
        }

        [MonoPInvokeCallback(typeof(MessageListenerHelper.OnMessageReceivedCallback))]
        private static void InternalOnMessageReceivedCallback(long id, string name, IntPtr data, UIntPtr dataLength, bool isReliable, IntPtr userData)
        {
            OnMessageReceived received = Callbacks.IntPtrToPermanentCallback<OnMessageReceived>(userData);
            if (received != null)
            {
                try
                {
                    received(id, name, Callbacks.IntPtrAndSizeToByteArray(data, dataLength), isReliable);
                }
                catch (Exception exception)
                {
                    Logger.e("Error encountered executing NativeMessageListenerHelper#InternalOnMessageReceivedCallback. Smothering to avoid passing exception into Native: " + exception);
                }
            }
        }

        internal void SetOnDisconnectedCallback(Action<long, string> callback)
        {
            MessageListenerHelper.MessageListenerHelper_SetOnDisconnectedCallback(base.SelfPtr(), new MessageListenerHelper.OnDisconnectedCallback(NativeMessageListenerHelper.InternalOnDisconnectedCallback), Callbacks.ToIntPtr(callback));
        }

        internal void SetOnMessageReceivedCallback(OnMessageReceived callback)
        {
            MessageListenerHelper.MessageListenerHelper_SetOnMessageReceivedCallback(base.SelfPtr(), new MessageListenerHelper.OnMessageReceivedCallback(NativeMessageListenerHelper.InternalOnMessageReceivedCallback), Callbacks.ToIntPtr(callback));
        }

        internal delegate void OnMessageReceived(long localClientId, string remoteEndpointId, byte[] data, bool isReliable);
    }
}

