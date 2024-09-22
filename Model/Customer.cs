namespace Leaderboard.Model
{
    /// <summary>
    /// Customer Model
    /// </summary>
    public class Customer : IComparable<Customer>
    {
        /// <summary>
        /// Identify a customer
        /// </summary>
        public long CustomerID { get; set; }

        /// <summary>
        /// Thedefault score of a customer is zero
        /// </summary>
        public decimal Score { get; set; }


        public decimal Rank { get; set; }


        /// <summary>
        /// Compare two customer.
        /// Ranking the top with high scores
        /// Two customers with the same score, their ranksare determined by their CustomerID, lower is first.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Customer? other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            if (Score > other.Score)
            {
                return -1;
            }
            else if (Score < other.Score)
            {
                return 1;
            }
            else if (CustomerID == other.CustomerID)
            {
                return 0;
            }
            else
            {
                return CustomerID < other.CustomerID ? -1 : 1;
            }
        }
    }
}
