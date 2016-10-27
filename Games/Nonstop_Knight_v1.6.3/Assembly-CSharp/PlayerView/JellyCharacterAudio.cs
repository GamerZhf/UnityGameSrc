namespace PlayerView
{
    using System;

    public class JellyCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_JellyMove;
        }
    }
}

