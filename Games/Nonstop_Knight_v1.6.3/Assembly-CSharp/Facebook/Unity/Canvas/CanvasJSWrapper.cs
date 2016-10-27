namespace Facebook.Unity.Canvas
{
    using System;
    using UnityEngine;

    internal class CanvasJSWrapper : ICanvasJSWrapper
    {
        private const string JSSDKBindingFileName = "JSSDKBindings";

        public void DisableFullScreen()
        {
            if (Screen.fullScreen)
            {
                Screen.fullScreen = false;
            }
        }

        public void ExternalCall(string functionName, params object[] args)
        {
            Application.ExternalCall(functionName, args);
        }

        public void ExternalEval(string script)
        {
            Application.ExternalEval(script);
        }

        public string GetSDKVersion()
        {
            return "v2.5";
        }

        public string IntegrationMethodJs
        {
            get
            {
                TextAsset asset = Resources.Load("JSSDKBindings") as TextAsset;
                if (asset != null)
                {
                    return asset.text;
                }
                return null;
            }
        }
    }
}

