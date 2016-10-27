namespace GameLogic
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public interface ICommandProcessor
    {
        Coroutine execute(ICommand command, [Optional, DefaultParameterValue(0f)] float delaySeconds);
        Coroutine executeCharacterSpecific(CharacterInstance character, ICommand command, [Optional, DefaultParameterValue(0f)] float delaySeconds);
        void stopCommand(MonoBehaviour owner, ref Coroutine commandCoroutine);
    }
}

