using Leaderboard.Leaderboard;
using Leaderboard.RequestModel;
using Microsoft.AspNetCore.Mvc;

namespace Leaderboard.Controllers
{
    /// <summary>
    /// Customer Controller
    /// </summary>
    [ApiController]
    [Route("customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ILeaderboard _leaderboard;

        public CustomerController(ILeaderboard leaderboard)
        {
            _leaderboard = leaderboard;
        }

        /// <summary>
        /// Update Score
        /// </summary>
        /// <param name="input">
        /// customerid : arbitrary positive int64 number
        /// score :      Adecimal numberin range of [-1000, +1000]. When it is
        ///              positive, this number represents the score to be increased by; When
        ///              it is negative, this number represents the score to be decreased by.
        /// </param>
        /// <returns>Current score after update</returns>
        [HttpPost("/customer/{customerid:long}/score/{score:decimal}")]
        public decimal UpdateScore([FromRoute] UpdateScoreRequestModel input)
        {
            return _leaderboard.UpdateScore(input.CustomerId, input.Score);
        }
    }
}
