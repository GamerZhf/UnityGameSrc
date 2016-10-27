using Appboy.Models;
using Appboy.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ApplePushNotificationTester : MonoBehaviour
{
    public string PrintOutObjectValues(object o)
    {
        if (o is IList<object>)
        {
            string str = "[";
            IEnumerator<object> enumerator = ((IList<object>) o).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    str = str + this.PrintOutObjectValues(current) + ", ";
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            return (str + "], ");
        }
        if (!(o is IDictionary<string, object>))
        {
            return o.ToString();
        }
        string str2 = "{";
        IEnumerator<KeyValuePair<string, object>> enumerator2 = ((IDictionary<string, object>) o).GetEnumerator();
        try
        {
            while (enumerator2.MoveNext())
            {
                KeyValuePair<string, object> pair = enumerator2.Current;
                str2 = str2 + pair.Key + " : ";
                str2 = str2 + this.PrintOutObjectValues(pair.Value) + ", ";
            }
        }
        finally
        {
            if (enumerator2 == null)
            {
            }
            enumerator2.Dispose();
        }
        return (str2 + "},");
    }

    private void Start()
    {
        string aJSON = "{\"alert\" : \n    {\"body\" : \"This is a push notification message.\",\n    \"action-loc-key\" : \"slide to open\",\n    \"loc-key\" : \"Welcome to the game!\",\n    \"loc-args\" : [\"Jenna\", \"Frank\", \"Elena\"],\n    \"launch-image\" : \"launchFromPushImage.png\"},\n    \"badge\" : 9,\n    \"sound\" : \"pushNotificationSoundFile\",\n    \"content-available\" : 3}";
        JSONClass json = (JSONClass) JSON.Parse(aJSON);
        Debug.Log("Push Notification event(aps): " + new ApplePushNotification(json));
        aJSON = "{\"extra\" : {\"intKey\" : 12,\"floatKey\" : 13.356,\"doubleKey\" : 3.1415926535897,\"arrayKey\" : [\"a string\", 3, 3.3335, {\"subkey\" : \"subdictionary\"}],\"dictionaryKey\" : {\"levelTwoIntKey\" : 212, \"levelTwoFloatKey\" : 213.356,\"levelTwoDoubleKey\" : 23.1415926535897,\"levelTwoArrayKey\" : [\"a level 3 string\", 33, 33.3335, {\"subsubkey\" : \"subsubdictionary\"}]}}}";
        json = (JSONClass) JSON.Parse(aJSON);
        ApplePushNotification notification = new ApplePushNotification(json);
        string str2 = "{";
        IEnumerator<KeyValuePair<string, object>> enumerator = notification.Extra.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, object> current = enumerator.Current;
                str2 = str2 + current.Key + " : ";
                str2 = str2 + this.PrintOutObjectValues(current.Value) + ", ";
            }
        }
        finally
        {
            if (enumerator == null)
            {
            }
            enumerator.Dispose();
        }
        Debug.Log("Push Notification event(extra): " + (str2 + "}"));
    }
}

