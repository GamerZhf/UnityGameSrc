using Service;
using System;

public class SocialInboxCommand : InboxCommand
{
    public PlatformConnectType socialPlatform;
    public string targetSocialid;
}

