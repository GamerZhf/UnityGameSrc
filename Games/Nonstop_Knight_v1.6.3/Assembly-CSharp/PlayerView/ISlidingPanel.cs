namespace PlayerView
{
    using System;

    public interface ISlidingPanel
    {
        bool canBeOpened();

        PlayerView.MenuType MenuType { get; }

        OffscreenOpenClose Panel { get; }
    }
}

