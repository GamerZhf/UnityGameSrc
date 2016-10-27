namespace Service
{
    using System;

    public interface IRTLProcessor
    {
        bool OnEnd(ref string finalString);
        bool OnStart(ref string originalInput);
    }
}

