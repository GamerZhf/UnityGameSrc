namespace PlayerView
{
    using GameLogic;
    using System;

    public interface ICameraMode
    {
        CharacterInstance getTarget();
        void initialize(RoomCamera roomCamera);
        void setTarget(CharacterInstance targetCharacter);
        void update(float dt);
    }
}

