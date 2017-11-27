using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class BaggedForest
    {
        public double Train_Accuracy { get; set; }
        public double Test_Accuracy { get; set; }
        public List<int> Train_Predictions { get; set; }
        public List<int> Test_Predictions { get; set; }
        public BaggedForest(double train_accuracy, double test_accuracy, List<int> train_predictions, List<int> test_predictions)
        {
            Train_Accuracy = train_accuracy;
            Test_Accuracy = test_accuracy;
            Train_Predictions = train_predictions;
            Test_Predictions = test_predictions;
        }   
    }
}
