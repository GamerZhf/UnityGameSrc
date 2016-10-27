namespace GameLogic
{
    using System;
    using System.Collections;

    public interface ICommand
    {
        IEnumerator executeRoutine();
        string[] serialize();
    }
}

