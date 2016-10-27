namespace Facebook.Unity
{
    public interface IAccessTokenRefreshResult : IResult
    {
        Facebook.Unity.AccessToken AccessToken { get; }
    }
}

