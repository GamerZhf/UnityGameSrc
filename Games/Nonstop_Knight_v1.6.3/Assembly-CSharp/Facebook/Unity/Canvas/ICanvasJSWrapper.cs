namespace Facebook.Unity.Canvas
{
    using System;

    internal interface ICanvasJSWrapper
    {
        void DisableFullScreen();
        void ExternalCall(string functionName, params object[] args);
        void ExternalEval(string script);
        string GetSDKVersion();

        string IntegrationMethodJs { get; }
    }
}

