using MyGame.Models;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using MyGame.Data.Interfaces;
using MyGame.Domain.Interfaces;
using MyGame.Services;
using Microsoft.AspNetCore.SignalR;

namespace MyGame.Domain.Concrete
{
    public class MatchDomain : IMatchDomain
    {
        private bool leagueStarted = false;
        private ILeaderboardsData LeaderboardsData { get; set; }
        private IHubContext<MatchHub> hubContext;

        public MatchDomain(ILeaderboardsData LeaderboardsData, IHubContext<MatchHub> hubContext)
        {
            this.LeaderboardsData = LeaderboardsData;
            this.hubContext = hubContext;
        }

        public void StartLeague(List<Team> teams)
        {
            if(!leagueStarted)
            {
                leagueStarted = true;

                List<Fixture> fixtures = CalculateFixtures(teams);

                foreach(Fixture fixture in fixtures)
                {
                    StartMatch(fixture);

                    //Update leaderboards
                    LeaderboardsData.UpdateLeaderboards(fixture);

                    //return to websocket 
                    string homeTeamName = LeaderboardsData.GetTeamName(fixture.homeTeam);
                    string awayTeamName = LeaderboardsData.GetTeamName(fixture.awayTeam);
                    //hubContext.SendFixture(homeTeamName, awayTeamName, fixture.homeTeamGoals, fixture.awayTeamGoals);
                    string message = $"{homeTeamName}-{awayTeamName} {fixture.homeTeamGoals}:{fixture.awayTeamGoals}";
                    this.hubContext.Clients.All.SendAsync("ReceiveFixture", message);
                }
            }
        }

        public Fixture StartMatch(Fixture fixture)
        {
            for(int i = 0; i < 10; i++)
            {
                int teamAction = GetRandom(0,2);

                //fire on 50%
                if(GetRandom(0,100) > 50)
                {
                    if(GetRandom(0,100) > 50)
                    {
                        if(teamAction == 0)
                        {
                            fixture.homeTeamGoals++;
                        }
                        else
                        {
                            fixture.awayTeamGoals++;
                        }
                    }
                }

                Thread.Sleep(200);
            }

            if(fixture.homeTeamGoals > fixture.awayTeamGoals)
            {
                fixture.homeTeamPoints = 3; 
                fixture.awayTeamPoints = 0;
            }
            else if(fixture.awayTeamGoals > fixture.homeTeamGoals)
            {
                fixture.homeTeamPoints = 0;
                fixture.awayTeamPoints = 3;
            }
            else
            {
                fixture.homeTeamPoints = 1;
                fixture.awayTeamPoints = 1;
            }
            
            return fixture;
        }

        private int GetRandom(int min, int max)
        {
            Random rnd = new Random();
            
            return rnd.Next(min,max);
        }

        private List<Fixture> CalculateFixtures(List<Team> teams)
        {
            List<Fixture> fixtures = new List<Fixture>();
            for (int i = 0; i < teams.Count; i++)
            {
                for (int j = 0; j < teams.Count; j++)
                {
                    if (teams[i].teamId != teams[j].teamId)
                    {
                        fixtures.Add(new Fixture() { homeTeam = teams[i].teamId, awayTeam = teams[j].teamId });
                    }
                }
            }

            fixtures.Reverse();

            int rounds = (teams.Count - 1) * 2;
            int gamesPerRound = rounds / 2;

            List<Fixture> sortedFixtures = new List<Fixture>();

            for (int i = 0; i < rounds; i++)
            {
                sortedFixtures.AddRange(TakeUnique(fixtures, gamesPerRound));
            }

            return sortedFixtures;
        }

        private List<Fixture> TakeUnique(List<Fixture> fixtures, int gamesPerRound)
        {
            List<Fixture> result = new List<Fixture>();

            for (int i = 0; i < gamesPerRound; i++)
            {
                for (int j = fixtures.Count - 1; j >= 0; j--)
                {
                    //check to see if any teams in current fixtue have already been used this game week and ignore if they have
                    if (!result.Any(r => r.homeTeam == fixtures[j].homeTeam || r.awayTeam == fixtures[j].homeTeam || r.homeTeam == fixtures[j].awayTeam || r.awayTeam == fixtures[j].awayTeam))
                    {
                        //teams not yet used
                        result.Add(fixtures[j]);
                        fixtures.RemoveAt(j);
                    }
                }
            }

            return result;
        }
    }
}