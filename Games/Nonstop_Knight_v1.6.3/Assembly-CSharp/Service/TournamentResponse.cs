namespace Service
{
    using System;
    using System.Collections.Generic;

    public class TournamentResponse
    {
        public List<TournamentViewRemote> RemoteTournamentViews;

        public TournamentResponse()
        {
            this.RemoteTournamentViews = new List<TournamentViewRemote>();
            this.RemoteTournamentViews = new List<TournamentViewRemote>();
        }

        public TournamentResponse(List<TournamentViewRemote> views)
        {
            this.RemoteTournamentViews = new List<TournamentViewRemote>();
            this.RemoteTournamentViews = views;
        }
    }
}

