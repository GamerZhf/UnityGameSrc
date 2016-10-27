namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.Events;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeEvent : BaseReferenceHolder, IEvent
    {
        internal NativeEvent(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            Event.Event_Dispose(selfPointer);
        }

        public override string ToString()
        {
            if (base.IsDisposed())
            {
                return "[NativeEvent: DELETED]";
            }
            object[] args = new object[] { this.Id, this.Name, this.Description, this.ImageUrl, this.CurrentCount, this.Visibility };
            return string.Format("[NativeEvent: Id={0}, Name={1}, Description={2}, ImageUrl={3}, CurrentCount={4}, Visibility={5}]", args);
        }

        public ulong CurrentCount
        {
            get
            {
                return Event.Event_Count(base.SelfPtr());
            }
        }

        public string Description
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return Event.Event_Description(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public string Id
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return Event.Event_Id(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public string ImageUrl
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return Event.Event_ImageUrl(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public string Name
        {
            get
            {
                return PInvokeUtilities.OutParamsToString(delegate (StringBuilder out_string, UIntPtr out_size) {
                    return Event.Event_Name(base.SelfPtr(), out_string, out_size);
                });
            }
        }

        public GooglePlayGames.BasicApi.Events.EventVisibility Visibility
        {
            get
            {
                Types.EventVisibility visibility = Event.Event_Visibility(base.SelfPtr());
                Types.EventVisibility visibility2 = visibility;
                if (visibility2 != Types.EventVisibility.HIDDEN)
                {
                    if (visibility2 != Types.EventVisibility.REVEALED)
                    {
                        throw new InvalidOperationException("Unknown visibility: " + visibility);
                    }
                    return GooglePlayGames.BasicApi.Events.EventVisibility.Revealed;
                }
                return GooglePlayGames.BasicApi.Events.EventVisibility.Hidden;
            }
        }
    }
}

