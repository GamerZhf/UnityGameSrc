namespace MATSDK
{
    using System;
    using System.Text;
    using UnityEngine;

    public class MATDelegate : MonoBehaviour
    {
        public static string DecodeFrom64(string encodedString)
        {
            MonoBehaviour.print("MATDelegate.DecodeFrom64(string)");
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedString));
        }

        public void onAdActionEnd(string empty)
        {
            MonoBehaviour.print("MATDelegate onAdActionEnd");
        }

        public void onAdActionStart(string willLeaveApplication)
        {
            MonoBehaviour.print("MATDelegate onAdActionStart: willLeaveApplication = " + willLeaveApplication);
        }

        public void onAdClick(string empty)
        {
            MonoBehaviour.print("MATDelegate onAdClick");
        }

        public void onAdClosed(string empty)
        {
            MonoBehaviour.print("MATDelegate onAdClosed");
        }

        public void onAdLoad(string placement)
        {
            MonoBehaviour.print("MATDelegate onAdLoad: placement = " + placement);
        }

        public void onAdLoadFailed(string error)
        {
            MonoBehaviour.print("MATDelegate onAdLoadFailed: " + error);
        }

        public void onAdRequestFired(string request)
        {
            MonoBehaviour.print("MATDelegate onAdRequestFired: request = " + request);
        }

        public void onAdShown(string empty)
        {
            MonoBehaviour.print("MATDelegate onAdShown");
        }

        public void trackerDidEnqueueRequest(string refId)
        {
            MonoBehaviour.print("MATDelegate trackerDidEnqueueRequest: " + refId);
        }

        public void trackerDidFail(string error)
        {
            MonoBehaviour.print("MATDelegate trackerDidFail: " + error);
        }

        public void trackerDidFailDeeplink(string error)
        {
            MonoBehaviour.print("MATDelegate trackerDidFailDeeplink: " + error);
        }

        public void trackerDidReceiveDeeplink(string url)
        {
            MonoBehaviour.print("MATDelegate trackerDidReceiveDeeplink: " + url);
        }

        public void trackerDidSucceed(string data)
        {
            MonoBehaviour.print("MATDelegate trackerDidSucceed: " + data);
        }
    }
}

