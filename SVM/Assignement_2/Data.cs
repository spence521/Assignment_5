using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Assignement_2
{
    public class Data
    {
        public List<Entry> Training_Data { get; private set; }
        public List<Entry> Test_Data { get; private set; }
        public List<Prediction> Predictions { get; private set; }
        public Dictionary<int, AccuracyWB> AccuracyWeightB { get; private set; }
        public Perceptron perceptron { get; set; }
        public double Accuracy { get; set; }
        public double Training_Accuracy { get; set; }
        public double Learning_Rate { get; set; }
        public double Margin { get; set; }
        public double Majority { get; set; }
        public WeightBias BestWeightBias { get; set; }
        public double C { get; set; }
        public double Tradeoff { get; set; }
        public Data(int epochs, double learning_rate, double margin, double c, bool logistic_regression, double tradeoff, Random r, 
            StreamReader train, StreamReader test, StreamReader r1, StreamReader r2, StreamReader r3, StreamReader r4, StreamReader r5)
        {
            double temp_accuracy1;
            double temp_accuracy2;
            double temp_accuracy3;
            double temp_accuracy4;
            double temp_accuracy5;
            Learning_Rate = learning_rate;
            Margin = margin;
            C = c;
            Tradeoff = tradeoff;

            #region First Fold
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();

            SetData(r1, r5);
            SetData(r2);
            SetData(r3);
            SetData(r4);
            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, margin, C, logistic_regression, Tradeoff, r);
            Dictionary<int, double> w = new Dictionary<int, double>();
            double b = (r.NextDouble() * (0.01 + 0.01) - 0.01);
            //for (int i = 1; i < 67693; i++)
            //{
            //    double randomNumber = (r.NextDouble() * (0.01 + 0.01) - 0.01);
            //    if(randomNumber != 0)
            //    {
            //        w.Add(i, randomNumber);
            //    }
            //}
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

            SetData(r1, r4);
            SetData(r2);
            SetData(r3);
            SetData(r5);
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

            SetData(r1, r3);
            SetData(r2);
            SetData(r4);
            SetData(r5);
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

            SetData(r1, r2);
            SetData(r3);
            SetData(r4);
            SetData(r5);
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
            
            SetData(r2, r1);
            SetData(r3);
            SetData(r4);
            SetData(r5);
            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, margin, C, logistic_regression, Tradeoff, r);
            wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                perceptron.ShuffleTraining_Data(r);
            }
            temp_accuracy5 = perceptron.GetAccuracy(Test_Data, wb);
            #endregion


            Accuracy = (temp_accuracy1 + temp_accuracy2 + temp_accuracy3 + temp_accuracy4 + temp_accuracy5) / 5;
        }
        public Data(StreamReader r1, StreamReader r2, Random r, int epochs, double learning_rate, double margin, double c, bool logistic_regression, double tradeoff)
        {
            C = c;
            Tradeoff = tradeoff;
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();
            AccuracyWeightB = new Dictionary<int, AccuracyWB>();
            SetData(r1, r2);
            perceptron = new Perceptron(Training_Data, Test_Data, learning_rate, margin, C, logistic_regression, Tradeoff, r);
            
            Dictionary<int, double> w = new Dictionary<int, double>();
            double b = (r.NextDouble() * (0.01 + 0.01) - 0.01);
            //for (int i = 1; i < 67693; i++)
            //{
            //    double randomNumber = (r.NextDouble() * (0.01 + 0.01) - 0.01);
            //    if (randomNumber != 0)
            //    {
            //        w.Add(i, randomNumber);
            //    }
            //}
            
            WeightBias wb = new WeightBias(w, b, 0);
            for (int i = 0; i < epochs; i++)
            {
                wb = perceptron.CalculateWB(wb);
                AccuracyWeightB.Add(i + 1, new AccuracyWB(perceptron.GetAccuracy(Test_Data, wb), wb));
                perceptron.ShuffleTraining_Data(r);
            }
            AccuracyWB bestAccuracy = AccuracyWeightB.OrderByDescending(x => x.Value.Accuracy).ThenByDescending(y => y.Key).Select(z => z.Value).First();
            Training_Accuracy = perceptron.GetAccuracy(Training_Data, bestAccuracy.Weight_Bias); //Train Accuracy
            Accuracy = bestAccuracy.Accuracy; //Test Accuracy
            BestWeightBias = bestAccuracy.Weight_Bias;
            Learning_Rate = learning_rate;
        }
        public Data(StreamReader r1, StreamReader r2, Random r, double learning_rate, WeightBias bestWB)
        {
            Training_Data = new List<Entry>();
            Test_Data = new List<Entry>();
            AccuracyWeightB = new Dictionary<int, AccuracyWB>();
            Predictions = new List<Prediction>();

            SetData(r1);
            perceptron = new Perceptron(Training_Data, null, learning_rate, 0, 0, false, 0, r);
            Accuracy = perceptron.GetAccuracy(Training_Data, bestWB);           
        }
        public Data(StreamReader r1)
        {
            Training_Data = new List<Entry>();
            SetData(r1);
            int count = 0;
            foreach (var item in Training_Data)
            {
                if(item.Sign == 1)
                {
                    count++;
                }
            }
            double majority = (Convert.ToDouble(count) / Training_Data.Count) * 100;
            if(majority < 50)
            {
                majority = 100 - majority;
            }
            Majority = majority;
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
                if(splitstring.First().First() == '1') { Sign = +1; }
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
                    if (splitstring.First().First() == '1') { Sign = +1; }
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
    }
}
