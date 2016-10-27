namespace PlayerView
{
    using System;

    public class WalrusCharacterAudio : AbstractCharacterAudio
    {
        protected override void onInitialize()
        {
            base.AggroAudioGroupType = AudioGroupType.SfxGrpGameplay_PetWalrusVoice;
        }
    }
}

