namespace PlayerView
{
    using System;

    public class GorillaCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_PetGorillaVoice;
        }
    }
}

