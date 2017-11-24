using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class Entry
    {
        public int Label { get; set; }
        public Dictionary<int, double> Vector { get; set; }

        public Entry(int sign, Dictionary<int, double> vector)
        {
            Label = sign;
            Vector = vector;
        }
        public override string ToString()
        {
            string p = "[";
            foreach (var item in Vector)
            {
                p = p + item + " ";
            }
            p = p + "] " + Label;
            return p;
        }

    }
}
