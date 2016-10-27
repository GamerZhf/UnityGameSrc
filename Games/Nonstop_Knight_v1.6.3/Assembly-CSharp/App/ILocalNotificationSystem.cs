namespace App
{
    using System;

    public interface ILocalNotificationSystem
    {
        bool AskingInputFromPlayer { get; }

        bool Registered { get; }
    }
}

