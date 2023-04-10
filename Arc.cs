using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace INF1008_tp1
{
    public class Arc:IComparable
    {
        public bool Orien { get; set; }
        public List<Node> Relates { get; set; }
        public int Weight { get; set; }
        public Arc(bool orien, int weight, Node a, Node b)
        {
            Orien = orien;
            Weight = weight;
            Relates = new() { a, b };
        }
        public override bool Equals(object? obj)
        {
            return obj is Arc arc &&
                   Orien == arc.Orien &&
                   Weight == arc.Weight &&
                   Relates == arc.Relates;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Orien, Weight, Relates);
        }


        public int CompareTo(object? obj)
        {
            if (obj is not null and Arc)
            {
                return Weight.CompareTo(((Arc)obj).Weight);
            }
            return 0;
        }
    }
}
