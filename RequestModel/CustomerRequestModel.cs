using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Leaderboard.RequestModel
{
    public class CustomerRequestModel
    {
    }
    public class UpdateScoreRequestModel
    {
        [FromRoute(Name = "customerid")]
        [Required(ErrorMessage = $"{nameof(CustomerId)} is required")]
        [Range(0, long.MaxValue, ErrorMessage = $"{nameof(CustomerId)} must positive")]
        public long CustomerId { get; set; }

        [FromRoute(Name = "score")]
        [Required(ErrorMessage = $"{nameof(Score)} is required")]
        [Range(-1000, +1000, ErrorMessage = $"The value of {nameof(Score)} must between -1000 and 1000")]
        public decimal Score { get; set; }
    }
}
