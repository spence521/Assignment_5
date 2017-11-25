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
        public List<Entry> Cross_Validate_Data { get; private set; }
        public List<Entry> Cross_1 { get; private set; }
        public List<Entry> Cross_2 { get; private set; }
        public List<Entry> Cross_3 { get; private set; }
        public List<Entry> Cross_4 { get; private set; }
        public List<Entry> Cross_5 { get; private set; }
        public DecisionTree Tree { get; set; }
        public DecisionTree Tree2 { get; set; }
        public DecisionTree Tree3 { get; set; }
        public DecisionTree Tree4 { get; set; }
        public DecisionTree Tree5 { get; set; }      
        public List<double> Accuracies { get; set; }
        public double Accuracy { get; set; }
        public double Test_Accuracy { get; set; }
        public int Depth { get; set; }
        public double Error { get; set; }
        public double StandardDeviation { get; set; }
        public List<BaggedForest> Forest { get; set; }
        public List<Entry> Training_Data_Forest { get; set; }

        public double Smoothing_Term { get; set; }
        
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
            Accuracy = Tree.Accuracy;
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
                Training_Data = new List<Entry>();
                Test_Data = new List<Entry>();
                SetData(r, r2);
                //SetTrainingData();
                List<Entry> trainingDataHelper = Training_Data_Forest.GetRange(0, 100);
                Tree = new DecisionTree(ref trainingDataHelper, null, depth, rand, false, 0);
                //Tree.CollapseTree();
                List<Entry> testDataHelper = Test_Data;
                Error = (Convert.ToDouble(Tree.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
                Accuracy = 100 - Error;
                Depth = Tree.DetermineDepth(0);
                ShuffleForestData(rand);

                Training_Data = new List<Entry>();
                Test_Data = new List<Entry>();
                SetData(r2); // setting the test data equal to Training_Data Here.
                //SetTrainingData();
                Tree.Labels = new List<int>();
                trainingDataHelper = Training_Data; //this is really the test data
                Tree.DetermineError(ref trainingDataHelper);

                Forest.Add(new BaggedForest(Accuracy, Tree.Labels));

                if(i % 3 == 0) { Console.WriteLine(i); }
            }
        }
        #region Data Not Needed
        //public Data(StreamReader train, StreamReader test, StreamReader eval, StreamReader eval_ID, int depth, Random r)
        //{
        //    double temp_error1;
        //    double temp_error2;
        //    double temp_error3; 
        //    double temp_error4;
        //    double temp_error5;
        //    Cross_Validate_Data = new List<Entry>();
        //    Predictions = new List<Prediction>();
        //    Predictions2 = new List<Prediction>();
        //    Predictions3 = new List<Prediction>();
        //    Predictions4 = new List<Prediction>();
        //    Predictions5 = new List<Prediction>();
        //    Predictions_Average = new List<Prediction>();
        //    Cross_1 = new List<Entry>();
        //    Cross_2 = new List<Entry>();
        //    Cross_3 = new List<Entry>();
        //    Cross_4 = new List<Entry>();
        //    Cross_5 = new List<Entry>();
        //    Accuracies = new List<double>();
        //    SetValidateData(train, test, r);

        //    #region First Fold
        //    data_1 = new List<Entry>();
        //    Training_Data = new List<TrainingData>();
        //    data_2 = new List<Entry>();
        //    Test_Data = new List<TrainingData>();
            
        //    data_1 = Cross_1.Concat(Cross_2.Concat(Cross_3.Concat(Cross_4))).ToList();
        //    data_2 = Cross_5;
        //    SetTrainingData();

        //    List<TrainingData> trainingDataHelper = Training_Data;
        //    Tree = new DecisionTree(ref trainingDataHelper, depth, r);
        //    Tree.CollapseTree();

        //    List<TrainingData> testDataHelper = Test_Data;
        //    temp_error1 = (Convert.ToDouble(Tree.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
        //    Tree.Accuracy = 100 - temp_error1;

        //    data_1 = new List<Entry>();
        //    data_2 = new List<Entry>();
        //    Training_Data = new List<TrainingData>();
        //    SetData(eval);
        //    SetTrainingData();
        //    Tree.Labels = new List<int>();
        //    trainingDataHelper = Training_Data;
        //    Tree.DetermineError(ref trainingDataHelper);
        //    Predictions = SetPredictions(eval_ID, Tree.Labels);
        //    #endregion

        //    #region Second Fold
        //    Training_Data = new List<TrainingData>();
        //    Test_Data = new List<TrainingData>();

        //    data_1 = Cross_1.Concat(Cross_2.Concat(Cross_3.Concat(Cross_5))).ToList();
        //    data_2 = Cross_4;
        //    SetTrainingData();

        //    trainingDataHelper = Training_Data;
        //    Tree2 = new DecisionTree(ref trainingDataHelper, depth, r);
        //    Tree2.CollapseTree();
        //    testDataHelper = Test_Data;
        //    temp_error2 = (Convert.ToDouble(Tree2.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
        //    Tree2.Accuracy = 100 - temp_error2;

        //    data_1 = new List<Entry>();
        //    data_2 = new List<Entry>();
        //    Training_Data = new List<TrainingData>();
        //    SetData(eval);
        //    SetTrainingData();
        //    Tree2.Labels = new List<int>();
        //    trainingDataHelper = Training_Data;
        //    Tree2.DetermineError(ref trainingDataHelper);
        //    Predictions2 = SetPredictions(eval_ID, Tree2.Labels);
        //    #endregion

        //    #region Third Fold
        //    Training_Data = new List<TrainingData>();
        //    Test_Data = new List<TrainingData>();

        //    data_1 = Cross_1.Concat(Cross_2.Concat(Cross_4.Concat(Cross_5))).ToList();
        //    data_2 = Cross_3;
        //    SetTrainingData();

        //    trainingDataHelper = Training_Data;
        //    Tree3 = new DecisionTree(ref trainingDataHelper, depth, r);
        //    Tree3.CollapseTree();
        //    testDataHelper = Test_Data;
        //    temp_error3 = (Convert.ToDouble(Tree3.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
        //    Tree3.Accuracy = 100 - temp_error3;

        //    data_1 = new List<Entry>();
        //    data_2 = new List<Entry>();
        //    Training_Data = new List<TrainingData>();
        //    SetData(eval);
        //    SetTrainingData();
        //    Tree3.Labels = new List<int>();
        //    trainingDataHelper = Training_Data;
        //    Tree3.DetermineError(ref trainingDataHelper);
        //    Predictions3 = SetPredictions(eval_ID, Tree3.Labels);
        //    #endregion

        //    #region Fourth Fold
        //    Training_Data = new List<TrainingData>();
        //    Test_Data = new List<TrainingData>();

        //    data_1 = Cross_1.Concat(Cross_3.Concat(Cross_4.Concat(Cross_5))).ToList();
        //    data_2 = Cross_2;
        //    SetTrainingData();

        //    trainingDataHelper = Training_Data;
        //    Tree4 = new DecisionTree(ref trainingDataHelper, depth, r);
        //    Tree4.CollapseTree();
        //    testDataHelper = Test_Data;
        //    temp_error4 = (Convert.ToDouble(Tree4.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
        //    Tree4.Accuracy = 100 - temp_error4;

        //    data_1 = new List<Entry>();
        //    data_2 = new List<Entry>();
        //    Training_Data = new List<TrainingData>();
        //    SetData(eval);
        //    SetTrainingData();
        //    Tree4.Labels = new List<int>();
        //    trainingDataHelper = Training_Data;
        //    Tree4.DetermineError(ref trainingDataHelper);
        //    Predictions4 = SetPredictions(eval_ID, Tree4.Labels);
        //    #endregion

        //    #region Fifth Fold
        //    Training_Data = new List<TrainingData>();
        //    Test_Data = new List<TrainingData>();

        //    data_1 = Cross_2.Concat(Cross_3.Concat(Cross_4.Concat(Cross_5))).ToList();
        //    data_2 = Cross_1;
        //    SetTrainingData();

        //    trainingDataHelper = Training_Data;
        //    Tree5 = new DecisionTree(ref trainingDataHelper, depth, r);
        //    Tree5.CollapseTree();
        //    testDataHelper = Test_Data;
        //    temp_error5 = (Convert.ToDouble(Tree5.DetermineError(ref testDataHelper)) / Convert.ToDouble(Test_Data.Count)) * 100;
        //    Tree5.Accuracy = 100 - temp_error5;

        //    data_1 = new List<Entry>();
        //    data_2 = new List<Entry>();
        //    Training_Data = new List<TrainingData>();
        //    SetData(eval);
        //    SetTrainingData();
        //    Tree5.Labels = new List<int>();
        //    trainingDataHelper = Training_Data;
        //    Tree5.DetermineError(ref trainingDataHelper);
        //    Predictions5 = SetPredictions(eval_ID, Tree5.Labels);
        //    #endregion

        //    SetAveragedPredictions();
        //    StandardDeviation = CalculateStandardDeviation(1-temp_error1, 1-temp_error2, 1-temp_error3, 1-temp_error4, 1-temp_error5);
        //    //Console.WriteLine(temp_error1);
        //    //Console.WriteLine(temp_error2);
        //    //Console.WriteLine(temp_error3);
        //    //Console.WriteLine(temp_error4);
        //    Error = (temp_error1 + temp_error2 + temp_error3 + temp_error4) / 4;
        //    Accuracies.Add(Tree.Accuracy);
        //    Accuracies.Add(Tree2.Accuracy);
        //    Accuracies.Add(Tree3.Accuracy);
        //    Accuracies.Add(Tree4.Accuracy);
        //    Accuracies.Add(Tree5.Accuracy);
        //    Accuracy = Accuracies.Average();
        //    Depth = Tree.DetermineDepth(0);
        //}
        #endregion
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
                Console.WriteLine(i + "\t" + item.Label + " " + item.Vector.ToString());
                i++;
            }
        }
        public void PrintData2()
        {
            int i = 1;
            foreach (var item in Test_Data)
            {
                Console.WriteLine(i + "\t" + item.Label + " " + item.Vector.ToString());
                i++;
            }
        }

        //public void SetTrainingData()
        //{
        //    foreach (var item in data_1)
        //    {
        //        Training_Data.Add(new TrainingData(ScreenNameLength(item.Vector[0]), DescriptionLength(item.Vector[1]), Days(item.Vector[2]), Hours(item.Vector[3]), 
        //            MinSec(item.Vector[4]), MinSec(item.Vector[5]), Follow(item.Vector[6]), Follow(item.Vector[7]), Ratio(item.Vector[8]), Tweets(item.Vector[9]), 
        //            TweetsPerDay(item.Vector[10]), AverageLinks(item.Vector[11]), AverageLinks(item.Vector[12]), AverageUsername(item.Vector[13]), 
        //            AverageUsername(item.Vector[14]), ChangeRate(item.Vector[15]), item.Sign));
        //    }
        //    foreach (var item in data_2)
        //    {
        //        Test_Data.Add(new TrainingData(ScreenNameLength(item.Vector[0]), DescriptionLength(item.Vector[1]), Days(item.Vector[2]), Hours(item.Vector[3]),
        //            MinSec(item.Vector[4]), MinSec(item.Vector[5]), Follow(item.Vector[6]), Follow(item.Vector[7]), Ratio(item.Vector[8]), Tweets(item.Vector[9]),
        //            TweetsPerDay(item.Vector[10]), AverageLinks(item.Vector[11]), AverageLinks(item.Vector[12]), AverageUsername(item.Vector[13]),
        //            AverageUsername(item.Vector[14]), ChangeRate(item.Vector[15]), item.Sign));
        //    }
        //}
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
    }
}
