using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignement_2
{
    public class Entry
    {
        public int Sign { get; set; }
        public Dictionary<int, double> Vector { get; set; }
        public Entry(int sign, Dictionary<int, double> vector)
        {
            Sign = sign;
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
