using System.Collections.Generic;
using MyGame.Models;

namespace MyGame.Domain.Interfaces
{
    public interface IMatchDomain
    {
        Fixture StartMatch(Fixture fixture);
        void StartLeague(List<Team> teams);
    }
}