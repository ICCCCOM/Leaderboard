using Leaderboard.Leaderboard;
using Leaderboard.Leaderboard.Imp;
using Leaderboard.Model;
using Leaderboard.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Leaderboard.Controllers
{
    /// <summary>
    /// Leaderboard Controller
    /// </summary>
    [ApiController]
    [Route("leaderboard")]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboard _leaderboard;

        public LeaderboardController(ILeaderboard leaderboard)
        {
            _leaderboard = leaderboard;
        }

        /// <summary>
        /// Get customers by rank
        /// </summary>
        /// <param name="input">
        /// start: start rank, included in response if exists
        /// end:   endrank, included in response if exists
        /// </param>
        /// <returns>found customers with rank and score.</returns>
        [HttpGet()]
        public List<Customer> GetCustomersByRank([FromQuery] GetCustomersByRankRequestModel input)
        {
            var min = Math.Min(input.Start, input.End);
            var max = Math.Max(input.Start, input.End);
            return _leaderboard.GetCustomersByRank(min, max);
        }


        /// <summary>
        /// Get customers by customerid
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="input">
        /// customerid: customer to lookup in leaderboard
        /// high:       optional. Default zero. number of neighbors whose rank is higher than the specified customer.
        /// low:        optional. Default zero. number of neighbors whose rank is lower than the specified customer.
        /// 
        /// </param>
        /// <returns> found customer and its nearest neighborhoods.</returns>
        [HttpGet("{customerid:long}")]
        public List<Customer> GetCustomersByCustomerId([FromRoute(Name = "customerid")][Range(0, long.MaxValue)] long customerId, [FromQuery] GetCustomersByCustomerIdRequestModel input)
        {
            return _leaderboard.GetCustomersByCustomerid(customerId, input.Low, input.High);
        }
    }
}
