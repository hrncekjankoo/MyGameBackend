using MyGame.Models;
using System.Collections.Generic;
using MyGame.Domain.Interfaces;
using MyGame.Data.Interfaces;


namespace MyGame.Domain.Concrete
{
    public class LeaderboardsDomain : ILeaderboardsDomain
    {
        private ILeaderboardsData LeaderboardsData { get; }
        private IMatchDomain MatchDomain { get; set; }

        public LeaderboardsDomain(ILeaderboardsData LeaderboardsData, IMatchDomain MatchDomain)
        {
            this.LeaderboardsData = LeaderboardsData;
            this.MatchDomain = MatchDomain;
        }

        public void StartLeague()
        {
            MatchDomain.StartLeague(GetLeaderboards());
        }

        public List<Team> GetLeaderboards()
        {
            return LeaderboardsData.GetLeaderboards();
        }

        public void ClearLeaderboards()
        {
            LeaderboardsData.ClearLeaderboards();
        }
    }
}