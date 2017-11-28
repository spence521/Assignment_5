using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignement_2
{
    public class Perceptron
    {
        public List<Entry> Training_Data { get; private set; }
        public List<Entry> Test_Data { get; private set; }
        public List<int> Labels { get; set;}
        public double Learning_Rate { get; set; }
        public double Initial_Learning_Rate { get; set; }
        public int T_Count { get; set; }
        public double Margin { get; set; }
        public double C { get; set; }
        public double Tradeoff { get; set; }
        public bool Logistic_Regression { get; set; }
        public Random R { get; set; }
        public Perceptron(List<Entry> train, List<Entry> test, double learning_rate, double margin, double c, bool logistic_regression, double tradeoff, Random r)
        {
            Training_Data = train;
            Test_Data = test;
            Learning_Rate = learning_rate;
            Initial_Learning_Rate = learning_rate;
            Margin = margin;   
            Labels = new List<int>();
            C = c;
            Tradeoff = tradeoff;
            Logistic_Regression = logistic_regression;
            R = r;       
        }

        public WeightBias CalculateWB(WeightBias wb)
        {
            Dictionary<int, double> w = wb.Weight;
            double b = wb.Bias;
            int errors = wb.Updates;
            foreach (var item in Training_Data)
            {
                int y = item.Sign; // true label
                Dictionary<int, double> x = item.Vector;
                double xw = 0;
                foreach (var xi in x)
                {
                    if (w.ContainsKey(xi.Key))
                    {
                        xw = xw + (w[xi.Key] * xi.Value);
                    }
                }
                xw += b;
                if (Logistic_Regression) //Logistic Regression
                {
                    foreach (var xi in x)  //foreach (KeyValuePair<int, double> wi in w) //update this 16 here.
                    {
                        if (w.ContainsKey(xi.Key)) //if contains key
                        {
                            w[xi.Key] = ((1 - (2 * Learning_Rate / Tradeoff)) * w[xi.Key]) + ((Learning_Rate * y * xi.Value) / (Math.Exp(y * xw) + 1));
                        }
                        else //if doesn't contain key, it would x[wi.Key] would result to 0, so:
                        {
                            w[xi.Key] = ((1 - (2 * Learning_Rate / Tradeoff)) * RandomNumber()) + ((Learning_Rate * y * xi.Value) / (Math.Exp(y * xw) + 1));
                        }
                    }
                    b = ((1 - (2 * Learning_Rate / Tradeoff)) * b) + ((Learning_Rate * y) / (Math.Exp(y * b) + 1));

                    errors++;
                }
                else //Support Vector Machine (SVM)
                {
                    if (y * xw <= 1)
                    {
                        foreach (var xi in x)  //foreach (KeyValuePair<int, double> wi in w) //update this 16 here.
                        {
                            if (w.ContainsKey(xi.Key)) //if contains key
                            {
                                w[xi.Key] = ((1 - Learning_Rate) * w[xi.Key]) + (Learning_Rate * C * y * xi.Value);
                            }
                            else //if doesn't contain key, it would x[wi.Key] would result to 0, so:
                            {
                                w.Add(xi.Key, ((1 - Learning_Rate) * RandomNumber()) + (Learning_Rate * C * y * xi.Value));
                            }
                        }
                        b = ((1 - Learning_Rate) * b) + (Learning_Rate * C * y);
                        errors++;
                    }
                    else
                    {
                        foreach (var xi in x) 
                        {
                            if (w.ContainsKey(xi.Key))
                            {
                                w[xi.Key] = ((1 - Learning_Rate) * w[xi.Key]);
                            }
                            else
                            {
                                w.Add(xi.Key, ((1 - Learning_Rate) * RandomNumber()));
                            }
                        }
                        b = ((1 - Learning_Rate) * b);
                    }
                }               
            }
            return new WeightBias(w, b, errors);
        }

        public double GetAccuracy(List<Entry> test_Data, WeightBias wb)
        {
            Dictionary<int, double> w = wb.Weight;
            double b = wb.Bias;
            double TotalErrors = 0;
            foreach (var item in test_Data)
            {
                int y = item.Sign;
                int yguess;
                Dictionary<int, double> x = item.Vector;
                double xw = 0;
                foreach (var xi in x)
                {
                    if (w.ContainsKey(xi.Key))
                    {
                        xw = xw + (w[xi.Key] * xi.Value);
                    }
                }
                xw += b;
                if(xw >= 0)
                {
                    yguess = +1;
                }
                else
                {
                    yguess = -1;
                }
                Labels.Add(yguess);
                if (y != yguess)
                {
                    TotalErrors++;
                }
            }
            return 100 - ((TotalErrors / Convert.ToDouble(test_Data.Count)) * 100);
        }
        public void ShuffleTraining_Data(Random rSeed)
        {
            Training_Data = Training_Data.OrderBy(i => rSeed.Next()).ToList();
        }
        private double RandomNumber()
        {
            return (R.NextDouble() * (0.01 + 0.01) - 0.01);
        }
    }
}
