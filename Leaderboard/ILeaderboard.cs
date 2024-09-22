using Leaderboard.Model;

namespace Leaderboard.Leaderboard
{
    public interface ILeaderboard
    {
        public decimal UpdateScore(long customerId,decimal newScore);
        public List<Customer> GetCustomersByRank(int start, int end);
        public List<Customer> GetCustomersByCustomerid(long customerId,int hight,int low);
    }
}
