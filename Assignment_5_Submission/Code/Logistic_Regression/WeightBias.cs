using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignement_2
{
    public class WeightBias
    {
        public Dictionary<int, double> Weight { get; set; }
        public double Bias { get; set; }
        public int Updates { get; set; }

        public WeightBias(Dictionary<int, double> weight, double bias, int updates)
        {
            Weight = weight;
            Bias = bias;
            Updates = updates;
        }
    }
}
