using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
//using static System.Math;

namespace Assignment_1
{
    public class Data
    {
        //public StreamReader reader { get; set; }
       // public StreamReader reader_2 { get; set; }
        public List<Entry> Training_Data { get; private set; }
        public List<Entry> Test_Data { get; private set; }
        public DecisionTree Tree { get; set; }
        public DecisionTree Tree2 { get; set; }
        public DecisionTree Tree3 { get; set; }
        public DecisionTree Tree4 { get; set; }
        public DecisionTree Tree5 { get; set; }      
        public List<double> Accuracies { get; set; }
        public double Train_Accuracy { get; set; }
        public double Test_Accuracy { get; set; }
        public int Depth { get; set; }
        public double Error { get; set; }
        public double StandardDeviation { get; set; }
        public List<BaggedForest> Forest { get; set; }
        public List<Entry> Training_Data_Forest { get; set; }

        public double Smoothing_Term { get; set; }

        /// <summary>
        /// Perceptron Data
        /// </summary>
        public Perceptron perceptron { get; set; }
        public List<Entry> Cross_Validate_Data { get; private set; }
        public List<Entry> Cross_1 { get; private set; }
        public List<Entry> Cross_2 { get; private set; }
        public List<Entry> Cross_3 { get; private set; }
        public List<Entry> Cross_4 { get; private set; }
        public List<Entry> Cross_5 { get; private set; }
        public double Learning_Rate { get; set; }
        public double Margin { get; set; }
        public double C { get; set; }
        public double Tradeoff { get; set; }
        public Dictionary<int, AccuracyWB> AccuracyWeightB { get; private set; }
        public WeightBias BestWeightBias { get; set; }

        public Data(int epochs, double learning_rate, double margin, double c, bool logistic_regression, double tradeoff, Random r,
            List<Entry> train, List<Entry> test)
        {
            double temp_accuracy1;
            double temp_accuracy2;
            double temp_accuracy3;
            double temp_accuracy4;
            double temp_accuracy5;
            Learning_Rate = learning_rate;
            C = c;
            Tradeoff = tradeoff;
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();
            Cross_Validate_Data = train.Concat(test).ToList();
            Cross_1 = new List<Entry>();
            Cross_2 = new List<Entry>();
            Cross_3 = new List<Entry>();
            Cross_4 = new List<Entry>();
            Cross_5 = new List<Entry>();
            SetValidateData(r);

            #region First Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();
            Training_Data = Cross_1.Concat(Cross_2.Concat(Cross_3.Concat(Cross_4))).ToList();
            Test_Data = Cross_5;

            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, margin, C, logistic_regression, Tradeoff, r);
            Dictionary<int, double> w = new Dictionary<int, double>();
            double b = (r.NextDouble() * (0.01 + 0.01) - 0.01);

            WeightBias wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                perceptron.ShuffleTraining_Data(r);
            }
            temp_accuracy1 = perceptron.GetAccuracy(Test_Data, wb);
            #endregion

            #region Second Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            Training_Data = Cross_1.Concat(Cross_2.Concat(Cross_3.Concat(Cross_5))).ToList();
            Test_Data = Cross_4;

            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, margin, C, logistic_regression, Tradeoff, r);
            wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                perceptron.ShuffleTraining_Data(r);
            }
            temp_accuracy2 = perceptron.GetAccuracy(Test_Data, wb);
            #endregion

            #region Third Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            Training_Data = Cross_1.Concat(Cross_2.Concat(Cross_4.Concat(Cross_5))).ToList();
            Test_Data = Cross_3;

            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, margin, C, logistic_regression, Tradeoff, r);
            wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                perceptron.ShuffleTraining_Data(r);
            }
            temp_accuracy3 = perceptron.GetAccuracy(Test_Data, wb);
            #endregion

            #region Fourth Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            Training_Data = Cross_1.Concat(Cross_3.Concat(Cross_4.Concat(Cross_5))).ToList();
            Test_Data = Cross_2;

            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, margin, C, logistic_regression, Tradeoff, r);
            wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                perceptron.ShuffleTraining_Data(r);
            }
            temp_accuracy4 = perceptron.GetAccuracy(Test_Data, wb);
            #endregion

            #region Fifth Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            Training_Data = Cross_2.Concat(Cross_3.Concat(Cross_4.Concat(Cross_5))).ToList();
            Test_Data = Cross_1;

            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, margin, C, logistic_regression, Tradeoff, r);
            wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                perceptron.ShuffleTraining_Data(r);
            }
            temp_accuracy5 = perceptron.GetAccuracy(Test_Data, wb);
            #endregion

            Test_Accuracy = (temp_accuracy1 + temp_accuracy2 + temp_accuracy3 + temp_accuracy4 + temp_accuracy5) / 5;
        }
        public Data(List<Entry> r1, List<Entry> r2, Random r, int epochs, double learning_rate, double margin, double c, bool logistic_regression, double tradeoff)
        {
            C = c;
            Tradeoff = tradeoff;
            Training_Data = r1;
            Test_Data = r2;
            AccuracyWeightB = new Dictionary<int, AccuracyWB>();
            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, margin, C, logistic_regression, Tradeoff, r);

            Dictionary<int, double> w = new Dictionary<int, double>();
            double b = (r.NextDouble() * (0.01 + 0.01) - 0.01);

            WeightBias wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                AccuracyWeightB.Add(i + 1, new AccuracyWB(perceptron.GetAccuracy(Test_Data, wb), wb));
                perceptron.ShuffleTraining_Data(r);
            }
            AccuracyWB bestAccuracy = AccuracyWeightB.OrderByDescending(x => x.Value.Accuracy).ThenByDescending(y => y.Key).Select(z => z.Value).First();
            Train_Accuracy = perceptron.GetAccuracy(Training_Data, bestAccuracy.Weight_Bias); //Train Accuracy
            Test_Accuracy = bestAccuracy.Accuracy; //Test Accuracy
            BestWeightBias = bestAccuracy.Weight_Bias;
            Learning_Rate = learning_rate;
        }
        /// <summary>
        /// Naive Bayes Constructor
        /// </summary>
        public Data(double smoothing_term, Random r, StreamReader r1, StreamReader r2, StreamReader r3, StreamReader r4, StreamReader r5)
        {
            double temp_accuracy1;
            double temp_accuracy2;
            double temp_accuracy3;
            double temp_accuracy4;
            double temp_accuracy5;
            Smoothing_Term = smoothing_term;

            #region First Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            SetData(r1, r5);
            SetData(r2);
            SetData(r3);
            SetData(r4);
            List<Entry> trainingDataHelper = Training_Data;
            Tree = new DecisionTree(ref trainingDataHelper, Test_Data, 0, r, true, Smoothing_Term);
            temp_accuracy1 = Tree.Test_Accuracy;
            #endregion

            #region Second Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            SetData(r1, r4);
            SetData(r2);
            SetData(r3);
            SetData(r5);
            trainingDataHelper = Training_Data;
            Tree = new DecisionTree(ref trainingDataHelper, Test_Data, 0, r, true, Smoothing_Term);
            temp_accuracy2 = Tree.Test_Accuracy;
            #endregion

            #region Third Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            SetData(r1, r3);
            SetData(r2);
            SetData(r4);
            SetData(r5);
            trainingDataHelper = Training_Data;
            Tree = new DecisionTree(ref trainingDataHelper, Test_Data, 0, r, true, Smoothing_Term);
            temp_accuracy3 = Tree.Test_Accuracy;
            #endregion

            #region Fourth Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            SetData(r1, r2);
            SetData(r3);
            SetData(r4);
            SetData(r5);
            trainingDataHelper = Training_Data;
            Tree = new DecisionTree(ref trainingDataHelper, Test_Data, 0, r, true, Smoothing_Term);
            temp_accuracy4 = Tree.Test_Accuracy;
            #endregion

            #region Fifth Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            SetData(r2, r1);
            SetData(r3);
            SetData(r4);
            SetData(r5);
            trainingDataHelper = Training_Data;
            Tree = new DecisionTree(ref trainingDataHelper, Test_Data, 0, r, true, Smoothing_Term);
            temp_accuracy5 = Tree.Test_Accuracy;
            #endregion

            Test_Accuracy = (temp_accuracy1 + temp_accuracy2 + temp_accuracy3 + temp_accuracy4 + temp_accuracy5) / 5;
        }
        /// <summary>
        /// Naive Bayes Constructor
        /// </summary>
        /// <param name="r"></param>
        /// <param name="r2"></param>
        /// <param name="rand"></param>
        /// <param name="smoothing_term"></param>
        public Data(StreamReader r, StreamReader r2, Random rand, double smoothing_term)
        {
            Smoothing_Term = smoothing_term;
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();
            SetData(r, r2);
            List<Entry> trainingDataHelper = Training_Data;
            Tree = new DecisionTree(ref trainingDataHelper, Test_Data,  0, rand, true, Smoothing_Term);
            Train_Accuracy = Tree.Accuracy;
            Test_Accuracy = Tree.Test_Accuracy;
        }
        public Data(StreamReader r, StreamReader r2, int depth, Random rand, int ForestSize)
        {
            Forest = new List<BaggedForest>();
            Training_Data_Forest = new List<Entry>();
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();
            SetData(r);
            //SetTrainingData();
            Training_Data_Forest = Training_Data;

            for (int i = 0; i < ForestSize; i++)
            {
                ShuffleForestData(rand);
                Training_Data = new List<Entry>();
                Test_Data = new List<Entry>();
                SetData(r, r2);
                //SetTrainingData();
                List<Entry> trainingDataHelper = Training_Data_Forest.GetRange(0, 100);
                Tree = new DecisionTree(ref trainingDataHelper, null, depth, rand, false, 0);
                //Tree.CollapseTree();
                List<Entry> testDataHelper = Test_Data;
                Error = (Convert.ToDouble(Tree.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
                Test_Accuracy = 100 - Error;
                Depth = Tree.DetermineDepth(0);
                List<int> Test_Predictions = Tree.Labels;

                Training_Data = new List<Entry>();
                SetData(r); 
                Tree.Labels = new List<int>();
                trainingDataHelper = Training_Data; //this is really the train data
                Train_Accuracy = 100 - ((Tree.DetermineError(ref trainingDataHelper) / Convert.ToDouble(Training_Data.Count)) * 100);
                List<int> Train_Predictions = Tree.Labels;

                Forest.Add(new BaggedForest(Train_Accuracy, Test_Accuracy, Train_Predictions, Test_Predictions));

                //if(i % 5 == 0) { Console.WriteLine(i); }
            }
        }
        public Data(StreamReader r, StreamReader r2)
        {
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();
            SetData(r, r2);
        }
        public void SetData(StreamReader reader, StreamReader reader_2 = null)
        {
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            string line;
            while ((line = reader.ReadLine()) != null)
            {

                int Sign;
                Dictionary<int, double> Vector = new Dictionary<int, double>();
                string[] splitstring = line.Split();
                if (splitstring.First().First() == '1') { Sign = 1; }
                else { Sign = -1; }
                foreach (var item in splitstring)
                {
                    if (item.Contains(":"))
                    {
                        string[] s = item.Split(':');
                        Vector.Add(Convert.ToInt32(s[0]), Convert.ToDouble(s[1]));
                    }
                }
                Training_Data.Add(new Entry(Sign, Vector));
            }
            if (reader_2 != null)
            {
                reader_2.DiscardBufferedData();
                reader_2.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                string line2;
                while ((line2 = reader_2.ReadLine()) != null)
                {
                    int Sign;
                    Dictionary<int, double> Vector = new Dictionary<int, double>();
                    string[] splitstring = line2.Split();
                    if (splitstring.First().First() == '1') { Sign = 1; }
                    else { Sign = -1; }
                    foreach (var item in splitstring)
                    {
                        if (item.Contains(":"))
                        {
                            string[] s = item.Split(':');
                            Vector.Add(Convert.ToInt32(s[0]), Convert.ToDouble(s[1]));
                        }
                    }
                    Test_Data.Add(new Entry(Sign, Vector));
                }
            }
        }
        public void PrintData1()
        {
            int i = 1;
            foreach (var item in Training_Data)
            {
                Console.WriteLine(i + "\t" + item.Sign + " " + item.Vector.ToString());
                i++;
            }
        }
        public void PrintData2()
        {
            int i = 1;
            foreach (var item in Test_Data)
            {
                Console.WriteLine(i + "\t" + item.Sign + " " + item.Vector.ToString());
                i++;
            }
        }
        public void PrintTrainingData()
        {
            int i = 1;
            foreach (var item in Training_Data)
            {
                Console.WriteLine(i.ToString() + "\t" + item.ToString());
                i++;
            }
        }
        private double CalculateStandardDeviation(double acc1, double acc2, double acc3, double acc4, double acc5)
        {
            List<double> list = new List<double>() { acc1, acc2, acc3, acc4, acc5 };
            double AverageOfValues = list.Average();
            double SumOfValues = list.Sum(r => Math.Pow(r - AverageOfValues, 2));
            return Math.Sqrt((SumOfValues) / (list.Count));
        }        
        public void TraverseTree()
        {
            Tree.TraverseTree();
        }
        public List<Prediction> SetPredictions(StreamReader ids, List<int> labels)
        {
            List<Prediction> predictions = new List<Prediction>();
            ids.DiscardBufferedData();
            ids.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            int i = 0;
            string id;
            while ((id = ids.ReadLine()) != null)
            {
                predictions.Add(new Prediction(Convert.ToInt32(id), labels[i]));
                i++;
            }
            return predictions;
        }
        //public void SetAveragedPredictions()
        //{
        //    for (int i = 0; i < Predictions.Count; i++)
        //    {
        //        List<int> items = new List<int> { Predictions[i].Label, Predictions2[i].Label, Predictions3[i].Label, Predictions4[i].Label, Predictions5[i].Label };
        //        int prediction = items.GroupBy(m => m).OrderByDescending(g => g.Count()).Select(g => g.Key).First();
        //        Predictions_Average.Add(new Prediction(Predictions[i].Id, prediction));
        //    }
        //}
        private void ShuffleForestData(Random rand)
        {
            Training_Data_Forest = Training_Data_Forest.OrderBy(i => rand.Next()).ToList();
        }

        public void SetValidateData(Random rSeed)
        {
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            Cross_Validate_Data = Cross_Validate_Data.OrderBy(i => rSeed.Next()).ToList();
            int seperator = Convert.ToInt32(Math.Floor(Convert.ToDecimal(Cross_Validate_Data.Count) / 5M));
            Cross_1 = Cross_Validate_Data.GetRange(0, seperator);
            Cross_2 = Cross_Validate_Data.GetRange(seperator, seperator);
            Cross_3 = Cross_Validate_Data.GetRange(2 * seperator, seperator);
            Cross_4 = Cross_Validate_Data.GetRange(3 * seperator, seperator);
            Cross_5 = Cross_Validate_Data.GetRange(4 * seperator, Cross_Validate_Data.Count - (4 * seperator));
        }
    }
}
