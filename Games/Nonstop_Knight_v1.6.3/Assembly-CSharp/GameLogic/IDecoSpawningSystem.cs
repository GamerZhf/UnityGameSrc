namespace GameLogic
{
    using System;
    using System.Collections;

    public interface IDecoSpawningSystem
    {
        void loadAllDecos(Room room);
        IEnumerator loadAllDecosAsync(Room room);
        void unloadAllDecos(Room room);
    }
}

