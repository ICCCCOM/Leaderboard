using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Leaderboard.RequestModel
{
    public class LeaderboardRequestModel
    {
    }
    public class GetCustomersByRankRequestModel
    {
        [FromQuery(Name = "start")]
        [Required(ErrorMessage = $"{nameof(Start)} is required")]
        [Range(1, int.MaxValue, ErrorMessage = $"The value of {nameof(Start)} must positive")]
        public int Start { get; set; }

        [FromQuery(Name = "end")]
        [Required(ErrorMessage = $"{nameof(End)} is required")]
        [Range(1, int.MaxValue, ErrorMessage = $"The value of {nameof(End)} must positive")]
        public int End { get; set; }
    }

    public class GetCustomersByCustomerIdRequestModel
    {
        [FromQuery(Name = "high")]
        [Range(0, int.MaxValue, ErrorMessage = $"The value of {nameof(High)} must positive")]
        public int High { get; set; }

        [FromQuery(Name = "low")]
        [Range(0, int.MaxValue, ErrorMessage = $"The value of {nameof(Low)} must positive")]
        public int Low { get; set; }
    }
}
