namespace PlayerView
{
    using System;

    public class RoomTransitionMenu : Menu
    {
        public void onNextButtonClicked()
        {
        }

        public override PlayerView.MenuType MenuType
        {
            get
            {
                return PlayerView.MenuType.RoomTransitionMenu;
            }
        }
    }
}

