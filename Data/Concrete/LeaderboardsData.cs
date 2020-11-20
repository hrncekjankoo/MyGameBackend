using System.Linq;
using System.Collections.Generic;
using MyGame.Models;
using MyGame.Data.Interfaces;

namespace MyGame.Data.Concrete
{
    public class LeaderboardsData : ILeaderboardsData
    {
        public List<Team> GetLeaderboards()
        {
            using(var context = new MyContext())
            {
                List<Team> result = context.Teams.OrderByDescending(a => a.points).ToList();;

                return result;
            }
        }

        public void ClearLeaderboards()
        {
            using(var context = new MyContext())
            {
                foreach(var team in context.Teams)
                {
                    team.goalsAgainst = 0;
                    team.goalsFor = 0;
                    team.points = 0;
                    team.round = 0;
                }

                context.SaveChanges();
            }
        }

        public string GetTeamName(int Id)
        {
            using(var context = new MyContext())
            {
                return context.Teams.Where(a => a.teamId == Id).Select(b => b.teamName).FirstOrDefault();
            }
        }

        public void UpdateLeaderboards(Fixture fixture)
        {
            using(var context = new MyContext())
            {
                var home = context.Teams.Where(a => a.teamId == fixture.homeTeam).FirstOrDefault();

                home.points = home.points + fixture.homeTeamPoints;
                home.goalsFor = home.goalsFor + fixture.homeTeamGoals;
                home.goalsAgainst = home.goalsAgainst + fixture.awayTeamGoals;
                home.round++;

                var away = context.Teams.Where(a => a.teamId == fixture.awayTeam).FirstOrDefault();

                away.points = away.points + fixture.awayTeamPoints;
                away.goalsFor = away.goalsFor + fixture.awayTeamGoals;
                away.goalsAgainst = away.goalsAgainst + fixture.homeTeamGoals;
                away.round++;

                context.SaveChanges();
            }
        }
    }
}