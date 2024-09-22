using Leaderboard.Model;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection.PortableExecutable;
using System.Xml;
using System.Xml.Linq;

namespace Leaderboard.Leaderboard.Imp
{
    /// <summary>
    /// Implementation of leaderboard (SkipList)
    /// </summary>
    public class SkipListLeaderboard : ILeaderboard
    {
        private readonly SkipList<Customer> _rankList;

        //Node information associated with customer ID
        private readonly ConcurrentDictionary<long, Customer> _customers;

        public SkipListLeaderboard(SkipList<Customer> rankList, ConcurrentDictionary<long, Customer> customers)
        {
            _rankList = rankList;
            _customers = customers;
        }
        /// <summary>
        /// Get node by rank
        /// </summary>
        /// <param name="skipList"></param>
        /// <param name="rank"></param>
        /// <returns></returns>
        private SkipListNode<Customer> GetByRank(SkipList<Customer> skipList, int rank)
        {
            var current = skipList.Header;
            int traversed = 1;
            for (int i = skipList.MaxLevel - 1; i >= 0; i--)
            {
                while (current.Forward[i] != null && traversed + current.Span[i] <= rank)
                {
                    // Accumulated span
                    traversed += current.Span[i];
                    current = current.Forward[i];
                }
            }
            return current.Forward[0];
        }

        /// <summary>
        /// Get customers by customer id
        /// </summary>
        /// <param name="customerId">Identify a customer</param>
        /// <param name="low">low</param>
        /// <param name="hight">hight</param>
        /// <returns>Found list</returns>
        /// <exception cref="Exception">Customer id not found</exception>
        public List<Customer> GetCustomersByCustomerid(long customerId, int low, int hight)
        {
            if (!_customers.ContainsKey(customerId))
            {
                throw new Exception("Customer not found.");
            }
            var res = new List<Customer>();
            var customer = _customers[customerId];
            // All customers whose score is greater than zero participate in a competition
            if (customer.Score <= 0)
            {
                throw new Exception("Sorry, according to the rules, your score is not in the competition");
            }
            var current = _rankList.Header;
            int traversed = 0;

            // First, find the last one whose score is less than the target score
            for (int i = _rankList.MaxLevel - 1; i >= 0; i--)
            {
                while (current.Forward[i] != null && current.Forward[i].Value.Score > customer.Score)
                {
                    // Accumulated span
                    traversed += current.Span[i];
                    current = current.Forward[i];
                }
            }
            // Special treatment for situations where the first score is the same
            if (current.Value == null && current.Forward[0].Value.Score == customer.Score)
            {
                traversed += 1;
                current = current.Forward[0];
            }
            // Secondly, find equal nodes through the [0] level linked list
            while (current.Value.CustomerID != customerId && current.Forward[0] != null)
            {
                // Accumulated span,step is 1
                traversed += 1;
                current = current.Forward[0];
            }

            // Get the previous N(param:hight) nodes
            var prev = current.Backward;
            for (int i = 1; i <= hight && prev != null && prev.Value != null; i++)
            {
                // Calculate the rank
                prev.Value.Rank = traversed - i;
                res.Insert(0, prev.Value);
                prev = prev.Backward;
            }
            customer.Rank = traversed;
            res.Add(customer);

            // Get the next m(param:low) nodes
            var next = current.Forward[0];
            for (int i = 1; i <= low && next != null; i++)
            {
                next.Value.Rank = traversed + i;
                res.Add(next.Value);
                next = next.Forward[0];
            }
            return res;
        }

        /// <summary>
        /// Update score
        /// </summary>
        /// <param name="customerId">Customers to be updated</param>
        /// <param name="newScore">The score value to be calculated</param>
        /// <returns></returns>
        public decimal UpdateScore(long customerId, decimal newScore)
        {
            // If customer id exists,update,use dic;
            if (_customers.ContainsKey(customerId))
            {
                var customer = _customers[customerId];
                _rankList.Remove(customer);
                // update score
                customer.Score = customer.Score + newScore;
                // insert to skip list
                if (customer.Score > 0)
                {
                    _rankList.Insert(customerId, customer);
                }
                _customers[customerId] = customer;
                return customer.Score;
            }
            // If customer id not exists,insert
            else
            {
                var customer = new Customer
                {
                    CustomerID = customerId,
                    Score = newScore
                };
                if (customer.Score > 0)
                {
                    _rankList.Insert(customerId, customer);
                }
                _customers[customerId] = customer;
                return customer.Score;
            }
        }
        /// <summary>
        /// Get customers by rank
        /// </summary>
        /// <param name="startRank">Score range begins</param>
        /// <param name="endRank">Score range ends</param>
        /// <returns>List of score ranges</returns>
        public List<Customer> GetCustomersByRank(int startRank, int endRank)
        {
            // First find the node that starts the score range, and then retrieve the corresponding number of nodes backwards
            var result = new List<Customer>();
            var count = endRank - startRank;
            var current = GetByRank(_rankList, startRank);

            for (int i = 0; i < count + 1 && current != null; i++)
            {
                current.Value.Rank = startRank + i;
                result.Add(current.Value);
                current = current.Forward[0];
            }

            return result;
        }
    }
}
