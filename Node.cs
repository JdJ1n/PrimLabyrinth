using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF1008_tp1
{
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public List<Arc> Relates { get; set; }
        public Node(int x, int y)
        {
            X = x;
            Y = y;
            Relates = new();
        }

        public void AddRelate(Node n, int t)
        {
            if (n.X == X && n.Y == Y + 1)
            {
                Relates.Add(new Arc(true, t, this, n));
            }
            else if (n.X == X + 1 && n.Y == Y)
            {
                Relates.Add(new Arc(false, t, this, n));
            }
            else if ((n.X == X && n.Y == Y - 1) || (n.X == X - 1 && n.Y == Y))
            {
                Relates.Add(n.Relates.Single(n => n.Relates.Contains(this)));
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Node node &&
                   X == node.X &&
                   Y == node.Y && Relates == node.Relates;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Relates);
        }
    }
}
