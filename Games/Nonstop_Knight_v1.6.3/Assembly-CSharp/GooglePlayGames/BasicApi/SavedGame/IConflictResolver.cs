namespace GooglePlayGames.BasicApi.SavedGame
{
    using System;

    public interface IConflictResolver
    {
        void ChooseMetadata(ISavedGameMetadata chosenMetadata);
    }
}

