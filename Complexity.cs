using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF1008_tp1
{
    public class Complexity
    {
        public static int Counter { get; set; }
        public static int Stocker { get; set; }
        static Complexity()
        {
            Counter = 0;
            Stocker = 0;
        }
    }
}
