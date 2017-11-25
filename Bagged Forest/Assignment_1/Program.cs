using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Data data2;
            Data data3;
            Data data4;
            Data data5;
            Data data6;
            Data data7;
            Data data8;
            Data data9;
            StreamReader Train;
            StreamReader Test;
            Random r = new Random(2);
            int depth = int.MaxValue;

            #region Arguments
            //if (args.Length == 0)
            //{
            //    System.Console.WriteLine("Please enter a file argument.");
            //    return;
            //}
            //else if (args.Length > 1) //at least two arguments
            //{
            //    Train = File.OpenText(args[0]);
            //    Test = File.OpenText(args[1]);
            //    if (args.Length > 4) { depth = Convert.ToInt32(args[4]); }
            //    int ForestSize = 1;
            //    Data DataTree = new Data(Train, Test, int.MaxValue, r, ForestSize);

            //    List<int> FinalPredictions = new List<int>();
            //    for (int i = 0; i < DataTree.Forest[0].Predictions.Count; i++)
            //    {
            //        List<int> helper = new List<int>();
            //        foreach (var tree in DataTree.Forest) //loops through each Tree
            //        {
            //            helper.Add(tree.Predictions[i]);
            //        }
            //        int Most_Occured_Label = helper.GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).First();
            //        // int ID = DataTree.Forest[1].Predictions[i].Id;
            //        FinalPredictions.Add(Most_Occured_Label); //This is adding in increasing order of the test set
            //    }
            //    int correct_labeling = 0;
            //    for (int i = 0; i < 940 /*Test data size*/; i++)
            //    {
            //        if(DataTree.Training_Data[i].Label == FinalPredictions[i]) //correct labeling
            //        {
            //            correct_labeling++;
            //        }
            //    }
            //    double Accuracy = (correct_labeling / FinalPredictions.Count) * 100;
            //    Console.WriteLine("Test Set Accuracy:\t" + Accuracy);            
            //    Console.WriteLine(DataTree.Depth);
            //}
            #endregion

            #region Non-arguments
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            Train = File.OpenText(startupPath + @"\speeches.train.liblinear");
            Test = File.OpenText(startupPath + @"\speeches.test.liblinear");
            #region Bagged Forest
            //int ForestSize = 1000;
            //Data DataTree = new Data(Train, Test, 3, r, ForestSize);

            //List<int> FinalPredictions = new List<int>();
            //for (int i = 0; i < DataTree.Forest[0].Predictions.Count; i++)
            //{
            //    List<int> helper = new List<int>();
            //    foreach (var tree in DataTree.Forest) //loops through each Tree
            //    {
            //        helper.Add(tree.Predictions[i]);
            //    }
            //    int Most_Occured_Label = helper.GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).First();
            //    // int ID = DataTree.Forest[1].Predictions[i].Id;
            //    FinalPredictions.Add(Most_Occured_Label); //This is adding in increasing order of the test set
            //}
            //int correct_labeling = 0;
            //for (int i = 0; i < 940 /*Test data size*/; i++)
            //{
            //    if (DataTree.Training_Data[i].Label == FinalPredictions[i]) //correct labeling
            //    {
            //        correct_labeling++;
            //    }
            //}
            //double Accuracy = (Convert.ToDouble(correct_labeling) / FinalPredictions.Count) * 100;
            //Console.WriteLine("Test Set Accuracy:\t" + Accuracy);

            #endregion

            #region Naive Bayes
            Data DataTree = new Data(Train, Test, r, 1);
            Console.WriteLine("Training Accuracy:\t" + DataTree.Accuracy);
            Console.WriteLine("Test Accuracy:\t" + DataTree.Test_Accuracy);
            #endregion
            Console.ReadKey(false);
            #endregion
        }

        //static decimal helper(double P, double N)
        //{
        //    return decimal.Parse(((-P * Math.Log(P, 2)) - (N * Math.Log(N, 2))).ToString());
        //}
    }


}