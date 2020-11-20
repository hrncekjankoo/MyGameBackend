using System.ComponentModel.DataAnnotations;  

namespace MyGame.Models
{
    public class Team
    {
        public int teamId { get; set; }
        public string teamName { get; set; }
        public int round { get; set; }
        public int goalsFor { get; set; }
        public int goalsAgainst { get; set; }
        public int points { get; set; }
    }

    public class Fixture
    {
        public int homeTeam { get; set; }
        public int homeTeamPoints { get; set; }
        public int homeTeamGoals { get; set; }
        public bool homeTeamFire { get; set; }
        public int awayTeam { get; set; }
        public int awayTeamPoints { get; set; }
        public int awayTeamGoals { get; set; }
        public bool awayTeamFire { get; set; }
    }
}


