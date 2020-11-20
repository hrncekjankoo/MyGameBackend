using System.Collections.Generic;
using MyGame.Models;

namespace MyGame.Data.Interfaces
{
    public interface ILeaderboardsData
    {
        List<Team> GetLeaderboards();
        void ClearLeaderboards();
        void UpdateLeaderboards(Fixture fixture);
        string GetTeamName(int Id);
    }
}