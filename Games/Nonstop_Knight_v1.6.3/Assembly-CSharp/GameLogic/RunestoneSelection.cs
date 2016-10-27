namespace GameLogic
{
    using App;
    using System;

    public class RunestoneSelection : IJsonData
    {
        public string Id;
        public RunestoneSelectionSource Source;

        public void postDeserializeInitialization()
        {
        }
    }
}

