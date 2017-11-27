using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class Entry
    {
        public int Sign { get; set; }
        public Dictionary<int, double> Vector { get; set; }

        public Entry(int label, Dictionary<int, double> vector)
        {
            Sign = label;
            Vector = vector;
        }
        public override string ToString()
        {
            string p = "[";
            foreach (var item in Vector)
            {
                p = p + item + " ";
            }
            p = p + "] " + Sign;
            return p;
        }

    }
}
