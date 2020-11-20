using System.Collections.Generic;
using MyGame.Models;

namespace MyGame.Domain.Interfaces
{
    public interface ILeaderboardsDomain
    {
        void StartLeague();
        List<Team> GetLeaderboards();
        void ClearLeaderboards();

    }
}