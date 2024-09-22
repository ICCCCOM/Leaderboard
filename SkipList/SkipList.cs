using Leaderboard.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Collections.Concurrent;

namespace Leaderboard
{
    public class SkipListNode<T>
    {
        public T Value { get; set; }

        // Skip array
        public SkipListNode<T>[] Forward { get; set; }

        // Previous node
        public SkipListNode<T> Backward { get; set; }  // 新增：前驱节点指针

        // Point to the next node span
        public int[] Span { get; set; }
        public SkipListNode(int level, T value)
        {
            Value = value;
            Forward = new SkipListNode<T>[level];
            Span = new int[level];
        }
    }
    public class SkipList<T> where T : IComparable<T>
    {
        public SkipListNode<T> Header;
        public readonly int MaxLevel;
        private readonly Random _random = new Random();
        private object _obj = new();

        public SkipList(int maxLevel)
        {
            MaxLevel = maxLevel;
            // Create Header node
            Header = new SkipListNode<T>(MaxLevel, default(T));
        }

        // Random level
        private int RandomLevel()
        {
            int level = 1;
            // 50% chance of upgrading layers
            while (_random.NextDouble() < 0.5 && level < MaxLevel)
            {
                level++;
            }
            return level;
        }

        // Insert node
        public void Insert(long key, T value)
        {
            lock (_obj)
            {
                var newLevel = RandomLevel();
                // Record the search path
                var update = new SkipListNode<T>[MaxLevel];
                var rank = new int[MaxLevel];
                var current = Header;

                // Calculate the number of nodes that can be skipped at each layer and record the update path
                for (int i = MaxLevel - 1; i >= 0; i--)
                {
                    // Set the value of the higher-level node
                    rank[i] = i == MaxLevel - 1 ? 0 : rank[i + 1];
                    // found node
                    while (current.Forward[i] != null && current.Forward[i].Value.CompareTo(value) < 0)
                    {
                        rank[i] += current.Span[i];
                        current = current.Forward[i];
                    }
                    update[i] = current;
                }

                var newNode = new SkipListNode<T>(newLevel, value);

                // Assign the path to the new node
                for (int i = 0; i < newLevel; i++)
                {
                    newNode.Forward[i] = update[i].Forward[i];
                    update[i].Forward[i] = newNode;

                    // Calculate the new node span
                    newNode.Span[i] = update[i].Span[i] - (rank[0] - rank[i]);
                    update[i].Span[i] = rank[0] - rank[i] + 1;

                    // Set Forward point (skip)
                    if (newNode.Forward[i] != null)
                    {
                        newNode.Forward[i].Backward = newNode;
                    }
                }

                // Set backward point
                newNode.Backward = update[0];

                for (int i = newLevel; i < MaxLevel; i++)
                {
                    update[i].Span[i]++;
                }
            }
        }

        // Remove node
        public void Remove(T value)
        {
            lock (_obj)
            {
                // Record the search path
                var update = new SkipListNode<T>[MaxLevel];
                var current = Header;
                // Found node
                for (int i = MaxLevel - 1; i >= 0; i--)
                {
                    while (current.Forward[i] != null && current.Forward[i].Value.CompareTo(value) < 0)
                    {
                        current = current.Forward[i];
                    }
                    update[i] = current;
                }

                current = current.Forward[0];
                // if found
                if (current != null && current.Value.CompareTo(value) == 0)
                {
                    for (int i = 0; i < MaxLevel; i++)
                    {
                        // Calculate data and update pointers,Remove the current node
                        if (update[i].Forward[i] == current)
                        {
                            update[i].Span[i] += current.Span[i] - 1;
                            update[i].Forward[i] = current.Forward[i];

                            // update previous node
                            if (current.Forward[i] != null)
                            {
                                current.Forward[i].Backward = update[i];
                            }
                        }
                        else
                        {
                            update[i].Span[i]--;
                        }
                    }
                }
            }
        }
    }
}
