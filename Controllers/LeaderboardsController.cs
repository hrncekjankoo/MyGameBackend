using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Net;
using System.Threading.Tasks;
using MyGame.Domain.Interfaces;
using MyGame.Controllers;

namespace VATAdminModule.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Authorize]
    
    public class LeaderboardsController : BaseController
    {
        private ILeaderboardsDomain LeaderboardsDomain { get; set; }

        public LeaderboardsController(ILeaderboardsDomain LeaderboardsDomain) : base()
        {
            this.LeaderboardsDomain = LeaderboardsDomain;
        }

        [HttpPost("start-league")]
        public IActionResult StartLeague()
        {
            try
            {
                Task.Run(() =>
                {
                    LeaderboardsDomain.StartLeague();
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("leaderboards")]
        public IActionResult GetLeaderboards()
        {
            try
            {
                var result = LeaderboardsDomain.GetLeaderboards();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("clear-leaderboards")]
        public IActionResult ClearLeaderboards()
        {
            try
            {
                LeaderboardsDomain.ClearLeaderboards();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
