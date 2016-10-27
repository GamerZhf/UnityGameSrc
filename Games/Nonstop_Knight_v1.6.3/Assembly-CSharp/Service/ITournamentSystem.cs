namespace Service
{
    using System;
    using System.Runtime.InteropServices;

    public interface ITournamentSystem
    {
        int CountUnclaimedMilestones();
        void ForceUpsert();
        TournamentView GetTournamentView(string tournamentId);
        RewardMilestone GetUnclaimedRewardMilestoneWithLowestCompletion(out string tournamentId);
        void Initialize();
        void JoinTournament(string tournamentId);

        bool Initialized { get; }

        bool Synchronized { get; }
    }
}

