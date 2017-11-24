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
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter a file argument.");
                return;
            }
            else if (args.Length > 1) //at least two arguments
            {
                Train = File.OpenText(args[0]);
                Test = File.OpenText(args[1]);
                if (args.Length > 4) { depth = Convert.ToInt32(args[4]); }
                int ForestSize = 1;
                Data DataTree = new Data(Train, Test, 3, r, ForestSize);

                List<int> FinalPredictions = new List<int>();
                for (int i = 0; i < DataTree.Forest[0].Predictions.Count; i++)
                {
                    List<int> helper = new List<int>();
                    foreach (var tree in DataTree.Forest) //loops through each Tree
                    {
                        helper.Add(tree.Predictions[i]);
                    }
                    int Most_Occured_Label = helper.GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).First();
                    // int ID = DataTree.Forest[1].Predictions[i].Id;
                    FinalPredictions.Add(Most_Occured_Label); //This is adding in increasing order of the test set
                }
            
                //BaggedForest BestTree = DataTree.Forest.OrderByDescending(x => x.Accuracy).First();
                // public Data(Random rand, StreamReader r, StreamReader r2, StreamReader eval, StreamReader eval_ID, int depth)
                //foreach (var item in data8.Accuracies)
                //{
                //    Console.WriteLine(item);
                //}
                //GenerateCSV(data.Predictions, @"\Decision_Tree_1.csv");
                //GenerateCSV(data.Predictions2, @"\Decision_Tree_2.csv");
                //GenerateCSV(data.Predictions3, @"\Decision_Tree_3.csv");
                //GenerateCSV(data.Predictions4, @"\Decision_Tree_4.csv");
                //GenerateCSV(data.Predictions5, @"\Decision_Tree_5.csv");
                //GenerateCSV(data.Predictions_Average, @"\Decision_Tree_Average.csv");
                //

                GenerateCSV(FinalPredictions, "Bagged_Forest.csv");
                Console.WriteLine(DataTree.Depth);
            }

            #endregion

            #region Non-arguments
            //Train = File.OpenText(startupPath + @"\data.train");
            //Test = File.OpenText(startupPath + @"\data.test");
            //Eval = File.OpenText(startupPath + @"\data.eval.anon");
            //Eval_ID = File.OpenText(startupPath + @"\data.eval.id");
            //data = new Data(Train, Test, Eval, Eval_ID, depth, r);
            //GenerateCSV(data.Predictions, @"\Simple_Perceptron.csv");
            //Console.WriteLine("\t" + Math.Round(100 - data.Error, 3) + "% Accuracy\n"
            //    + "Standard Deviation:\t" + Math.Round(data.StandardDeviation, 4));
            //foreach (var item in data.Accuracy)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine(data.Depth);
            ////data.PrintData2();
            //Console.ReadKey(false);
            #endregion
        }
        static void GenerateCSV(List<Prediction> predictions, string name)
        {
            StringBuilder csv = new StringBuilder();
            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string Path = startupPath  + name;
            csv.AppendLine("Id,Prediction");

            foreach (var item in predictions)
            {
                csv.AppendLine(string.Format("{0},{1}", item.Id, item.Label));
            }
            File.AppendAllText(Path, csv.ToString());
        }
        //static decimal helper(double P, double N)
        //{
        //    return decimal.Parse(((-P * Math.Log(P, 2)) - (N * Math.Log(N, 2))).ToString());
        //}
    }


}