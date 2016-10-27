namespace UnityTest
{
    using System;

    public interface IAssertionComponentConfigurator
    {
        AssertionComponent Component { get; }

        bool TimeCheckRepeat { set; }

        float TimeCheckRepeatFrequency { set; }

        float TimeCheckStartAfter { set; }

        bool UpdateCheckRepeat { set; }

        int UpdateCheckRepeatFrequency { set; }

        int UpdateCheckStartOnFrame { set; }
    }
}

