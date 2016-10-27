namespace GooglePlayGames.BasicApi.SavedGame
{
    using System;

    public interface ISavedGameMetadata
    {
        string CoverImageURL { get; }

        string Description { get; }

        string Filename { get; }

        bool IsOpen { get; }

        DateTime LastModifiedTimestamp { get; }

        TimeSpan TotalTimePlayed { get; }
    }
}

