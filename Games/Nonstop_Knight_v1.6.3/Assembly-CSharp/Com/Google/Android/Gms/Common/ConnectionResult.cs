namespace Com.Google.Android.Gms.Common
{
    using Google.Developers;
    using System;

    public class ConnectionResult : JavaObjWrapper
    {
        private const string CLASS_NAME = "com/google/android/gms/common/ConnectionResult";

        public ConnectionResult(int arg_int_1)
        {
            object[] args = new object[] { arg_int_1 };
            base.CreateInstance("com/google/android/gms/common/ConnectionResult", args);
        }

        public ConnectionResult(IntPtr ptr) : base(ptr)
        {
        }

        public ConnectionResult(int arg_int_1, object arg_object_2)
        {
            object[] args = new object[] { arg_int_1, arg_object_2 };
            base.CreateInstance("com/google/android/gms/common/ConnectionResult", args);
        }

        public ConnectionResult(int arg_int_1, object arg_object_2, string arg_string_3)
        {
            object[] args = new object[] { arg_int_1, arg_object_2, arg_string_3 };
            base.CreateInstance("com/google/android/gms/common/ConnectionResult", args);
        }

        public int describeContents()
        {
            return base.InvokeCall<int>("describeContents", "()I", new object[0]);
        }

        public bool equals(object arg_object_1)
        {
            object[] args = new object[] { arg_object_1 };
            return base.InvokeCall<bool>("equals", "(Ljava/lang/Object;)Z", args);
        }

        public int getErrorCode()
        {
            return base.InvokeCall<int>("getErrorCode", "()I", new object[0]);
        }

        public string getErrorMessage()
        {
            return base.InvokeCall<string>("getErrorMessage", "()Ljava/lang/String;", new object[0]);
        }

        public object getResolution()
        {
            return base.InvokeCall<object>("getResolution", "()Landroid/app/PendingIntent;", new object[0]);
        }

        public int hashCode()
        {
            return base.InvokeCall<int>("hashCode", "()I", new object[0]);
        }

        public bool hasResolution()
        {
            return base.InvokeCall<bool>("hasResolution", "()Z", new object[0]);
        }

        public bool isSuccess()
        {
            return base.InvokeCall<bool>("isSuccess", "()Z", new object[0]);
        }

        public void startResolutionForResult(object arg_object_1, int arg_int_2)
        {
            object[] args = new object[] { arg_object_1, arg_int_2 };
            base.InvokeCallVoid("startResolutionForResult", "(Landroid/app/Activity;I)V", args);
        }

        public string toString()
        {
            return base.InvokeCall<string>("toString", "()Ljava/lang/String;", new object[0]);
        }

        public void writeToParcel(object arg_object_1, int arg_int_2)
        {
            object[] args = new object[] { arg_object_1, arg_int_2 };
            base.InvokeCallVoid("writeToParcel", "(Landroid/os/Parcel;I)V", args);
        }

        public static int API_UNAVAILABLE
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "API_UNAVAILABLE");
            }
        }

        public static int CANCELED
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "CANCELED");
            }
        }

        public static int CONTENTS_FILE_DESCRIPTOR
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "CONTENTS_FILE_DESCRIPTOR");
            }
        }

        public static object CREATOR
        {
            get
            {
                return JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/common/ConnectionResult", "CREATOR", "Landroid/os/Parcelable$Creator;");
            }
        }

        public static int DEVELOPER_ERROR
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "DEVELOPER_ERROR");
            }
        }

        public static int DRIVE_EXTERNAL_STORAGE_REQUIRED
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "DRIVE_EXTERNAL_STORAGE_REQUIRED");
            }
        }

        public static int INTERNAL_ERROR
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "INTERNAL_ERROR");
            }
        }

        public static int INTERRUPTED
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "INTERRUPTED");
            }
        }

        public static int INVALID_ACCOUNT
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "INVALID_ACCOUNT");
            }
        }

        public static int LICENSE_CHECK_FAILED
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "LICENSE_CHECK_FAILED");
            }
        }

        public static int NETWORK_ERROR
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "NETWORK_ERROR");
            }
        }

        public static string NULL
        {
            get
            {
                return JavaObjWrapper.GetStaticStringField("com/google/android/gms/common/ConnectionResult", "NULL");
            }
        }

        public static int PARCELABLE_WRITE_RETURN_VALUE
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "PARCELABLE_WRITE_RETURN_VALUE");
            }
        }

        public static int RESOLUTION_REQUIRED
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "RESOLUTION_REQUIRED");
            }
        }

        public static int SERVICE_DISABLED
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_DISABLED");
            }
        }

        public static int SERVICE_INVALID
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_INVALID");
            }
        }

        public static int SERVICE_MISSING
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_MISSING");
            }
        }

        public static int SERVICE_MISSING_PERMISSION
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_MISSING_PERMISSION");
            }
        }

        public static int SERVICE_UPDATING
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_UPDATING");
            }
        }

        public static int SERVICE_VERSION_UPDATE_REQUIRED
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SERVICE_VERSION_UPDATE_REQUIRED");
            }
        }

        public static int SIGN_IN_FAILED
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SIGN_IN_FAILED");
            }
        }

        public static int SIGN_IN_REQUIRED
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SIGN_IN_REQUIRED");
            }
        }

        public static int SUCCESS
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "SUCCESS");
            }
        }

        public static int TIMEOUT
        {
            get
            {
                return JavaObjWrapper.GetStaticIntField("com/google/android/gms/common/ConnectionResult", "TIMEOUT");
            }
        }
    }
}

