using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class AccuracyWB
    {
        public WeightBias Weight_Bias { get; set; }
        public double Accuracy { get; set; }

        public AccuracyWB(double accuracy, WeightBias weight_Bias)
        {
            Weight_Bias = weight_Bias;
            Accuracy = accuracy;
        }
    }
}
