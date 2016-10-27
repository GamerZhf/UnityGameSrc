namespace Facebook.Unity
{
    public interface ILoginResult : IResult
    {
        Facebook.Unity.AccessToken AccessToken { get; }
    }
}

