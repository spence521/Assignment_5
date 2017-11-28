using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_1
{
    public class DecisionTree
    {
        /// <summary>
        /// Enums
        /// </summary>     
        public bool IsLeaf { get; set; }
        public List<DecisionTree> Children { get; private set; }
        //public DecisionTree LeftTree { get; set; } //no
        //public DecisionTree RightTree { get; set; } //yes
        public int Value { get; set; }
        public List<Entry> TrainingData { get; private set; }
        public double Entropy { get; private set; }
        Dictionary<int, double> InformationGains { get; set; }
        public double Error { get; set; } // gets set in SetEntropy
        public int DepthRemaining { get; set; }
        public List<int> FeaturesTaken { get; set; }
        public double Accuracy { get; set; }
        public List<int> Labels { get; set; }
        public Random random { get; set; }
        public int Feature { get; set; }

        //Things needed for Naive Bayes
        public bool Naive_Bayes { get; set; }
        public double Smoothing_Term { get; set; }
        public List<Entry> TestData { get; private set; }
        public List<int> Test_Labels { get; set; }
        public double Test_Accuracy { get; set; }

        public DecisionTree(ref List<Entry> trainingdata, List<Entry> testdata, int depth, Random r, bool naive_bayes, double smoothing_term)
        {
            Naive_Bayes = naive_bayes;
            Smoothing_Term = smoothing_term;
            TestData = testdata;

            random = r;
            Labels = new List<int>();
            Test_Labels = new List<int>();
            InformationGains = new Dictionary<int, double>();
            FeaturesTaken = new List<int>();
            IsLeaf = false;
            Children = new List<DecisionTree>();
            Value = 0; //A negative one leaf value mean its not a leaf. 1 is positive label, -1 is negative label
            DepthRemaining = depth;
            TrainingData = trainingdata;
            if (!Naive_Bayes) { SetEntropy(); }
            SetInformationGain();

            if (!Naive_Bayes)
            {
                DetermineFeature();
                FeaturesTaken.Add(Feature);
                DetermineSubTrees();
            }
        }

        public DecisionTree(bool isLeaf, ref List<Entry> trainingdata, int value, int depthRemaining, ref List<int> featuresTaken, Random r, bool naive_bayes)
        {
            Naive_Bayes = naive_bayes;
            random = r;
            IsLeaf = isLeaf;
            InformationGains = new Dictionary<int, double>();
            Children = new List<DecisionTree>();
            DepthRemaining = depthRemaining;
            Value = value;
            
            if (!IsLeaf)
            {
                FeaturesTaken = featuresTaken;
                TrainingData = trainingdata;
                SetEntropy();
                SetInformationGain();
                FeaturesTaken = featuresTaken;
                DetermineFeature();
                FeaturesTaken.Add(Feature);
                DetermineSubTrees();
            }
            //else
            //{
            //    Console.WriteLine("You are at a Leaf" + value);
            //}
        }
        private void SetEntropy()
        {
            int positive_labels = 0;
            int negative_labels = 0;
            double P;
            double N;
            foreach (var item in TrainingData)
            {
                if (item.Sign == 1) { positive_labels++; }
                else { negative_labels++; }
            }
            P = Convert.ToDouble(positive_labels) / TrainingData.Count;
            N = Convert.ToDouble(negative_labels) / TrainingData.Count;
            Entropy = (-P * Math.Log(P, 2)) - (N * Math.Log(N, 2));
            Error = 1 - (Convert.ToDouble(positive_labels > negative_labels ? positive_labels : negative_labels) / Convert.ToDouble(TrainingData.Count));

            if (double.IsInfinity(Entropy) || double.IsNaN(Entropy) || double.IsNegativeInfinity(Entropy) || double.IsPositiveInfinity(Entropy))
            {
                Console.WriteLine("You have a NaN, or infinity value. SetEntropy");
            }
        }
        private void SetInformationGain()
        {
            //Counts(True Lable)(Feature Label) 67692
            Dictionary<int, double> Counts11 = new Dictionary<int, double>();
            Dictionary<int, double> Counts10 = new Dictionary<int, double>();
            Dictionary<int, double> Counts01 = new Dictionary<int, double>();
            Dictionary<int, double> Counts00 = new Dictionary<int, double>();
            double Positive_Labels = 0;
            double Negative_Labels = 0;
            foreach (var example in TrainingData)
            {
                if (example.Sign == 1)
                {
                    for (int i = 1; i < 67693; i++)
                    {
                        if (example.Vector.ContainsKey(i)) //This means that the feature is +1 and the true label is +1
                        {
                            if (Counts11.ContainsKey(i)) { Counts11[i] = Counts11[i] + 1; }
                            else { Counts11.Add(i, 1); }
                        }
                        else //This means that the feature is -1 and the true label is +1
                        {
                            if (Counts10.ContainsKey(i)) { Counts10[i] = Counts10[i] + 1; }
                            else { Counts10.Add(i, 1); }
                        }
                    }
                    Positive_Labels++;
                }
                else
                {
                    for (int i = 1; i < 67693; i++)
                    {
                        if (example.Vector.ContainsKey(i)) //This means that the feature is +1 and the true label is -1
                        {
                            if (Counts01.ContainsKey(i)) { Counts01[i] = Counts01[i] + 1; }
                            else { Counts01.Add(i, 1); }
                        }
                        else //This means that the feature is -1 and the true label is -1
                        {
                            if (Counts00.ContainsKey(i)) { Counts00[i] = Counts00[i] + 1; }
                            else { Counts00.Add(i, 1); }
                        }
                    }
                    Negative_Labels++;
                }
            }

            if (!Naive_Bayes) //Do Decision tree Stuff
            {
                for (int i = 1; i < 67693; i++)
                {
                    double PosLabel_PosFeature = Counts11.ContainsKey(i) ? Counts11[i] : 0;
                    double NegLabel_PosFeature = Counts01.ContainsKey(i) ? Counts01[i] : 0;
                    double PosLabel_NegFeature = Counts10.ContainsKey(i) ? Counts10[i] : 0;
                    double NegLabel_NegFeature = Counts00.ContainsKey(i) ? Counts00[i] : 0;
                    InformationGains.Add(i, CalculateInformationGain(Positive_Labels, Negative_Labels, PosLabel_PosFeature, NegLabel_PosFeature, PosLabel_NegFeature, NegLabel_NegFeature));
                }
            }
            #region Naive Bayes
            else // Do Naive Bayes Stuff
            {
                double Prob_Yes = Positive_Labels / TrainingData.Count;
                double Prob_No = Negative_Labels / TrainingData.Count;
                //the Si is equal to 2, So i just put 2 there.
                double bottom_Pos = Positive_Labels + (2 * Smoothing_Term);
                double bottom_Neg = Negative_Labels + (2 * Smoothing_Term);
                for (int i = 1; i < 67693; i++)
                {                     
                    if(Counts11.ContainsKey(i)) { Counts11[i] = (Counts11[i] + Smoothing_Term) / bottom_Pos; }
                    else { Counts11.Add(i, Smoothing_Term / bottom_Pos); }

                    if (Counts10.ContainsKey(i)) { Counts10[i] = (Counts10[i] + Smoothing_Term) / bottom_Pos; }
                    else { Counts10.Add(i, Smoothing_Term / bottom_Pos); }

                    if (Counts01.ContainsKey(i)) { Counts01[i] = (Counts01[i] + Smoothing_Term) / bottom_Neg; }
                    else { Counts01.Add(i, Smoothing_Term / bottom_Pos); }

                    if (Counts00.ContainsKey(i)) { Counts00[i] = (Counts00[i] + Smoothing_Term) / bottom_Neg; }
                    else { Counts00.Add(i, Smoothing_Term / bottom_Pos); }
                }
                int correct_values = 0;
                int poss = 0;
                int inff = 0;
                foreach (var example in TrainingData)
                {
                    double Pos = Prob_Yes * double.MaxValue * 1.5;
                    double Neg = Prob_No * double.MaxValue * 1.5;
                    for (int i = 1; i < 67693; i++)
                    {
                        if (example.Vector.ContainsKey(i)) // That means that feature value is 1
                        {
                            Pos = Pos * Counts11[i];// * 1.022;
                        }
                        else //That means the feature value is -1
                        {
                            Pos = Pos * Counts10[i];// * 1.022;
                        }
                        if (example.Vector.ContainsKey(i)) // That means that feature value is 1
                        {
                            Neg = Neg * Counts01[i];// * 1.022;
                        }
                        else //That means the feature value is -1
                        {
                            Neg = Neg * Counts00[i];// * 1.022;
                        }
                    }
                    int yguess;
                    if(Pos == 0) {  poss++; }
                    if (Neg == 0) { poss++; }
                    if (double.IsInfinity(Pos)) { inff++; }
                    if (double.IsInfinity(Neg)) {  inff++; }
                    if (Pos >= Neg)
                    {
                        yguess = 1;
                    }
                    else
                    {
                        yguess = -1;
                    }
                    Labels.Add(yguess);
                    if (yguess == example.Sign)
                    {
                        correct_values++;
                    }
                }
                //Console.WriteLine("0: \t" + poss);
                //Console.WriteLine("inf: \t" + inff);

                Accuracy = correct_values / Convert.ToDouble(TrainingData.Count);



                //Test Data
                correct_values = 0;
                foreach (var example in TestData)
                {
                    double Pos = Prob_Yes;
                    double Neg = Prob_No;
                    for (int i = 1; i < 67693; i++)
                    {
                        if (example.Vector.ContainsKey(i)) // That means that feature value is 1
                        {
                            Pos = Pos * Counts11[i] ;
                        }
                        else //That means the feature value is -1
                        {
                            Pos = Pos * Counts10[i] ;
                        }
                        if (example.Vector.ContainsKey(i)) // That means that feature value is 1
                        {
                            Neg = Neg * Counts01[i] ;
                        }
                        else //That means the feature value is -1
                        {
                            Neg = Neg * Counts00[i] ;
                        }
                    }
                    int yguess;
                    if (Pos >= Neg)
                    {
                        yguess = 1;
                    }
                    else
                    {
                        yguess = -1;
                    }
                    Test_Labels.Add(yguess);
                    if (yguess == example.Sign)
                    {
                        correct_values++;
                    }
                }
                Test_Accuracy = correct_values / Convert.ToDouble(TestData.Count);
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="List_Total">contains the total amount of each option</param>
        /// <param name="P_List">Contains the total amount of the positive results for each option</param>
        /// <param name="N_List">Contains the total amount of the negative results for each option</param>
        /// <returns></returns>
        private double CalculateInformationGain(double Yes, double No, double Yes_P, double Yes_N, double No_P, double No_N)
        {
            double Yes_Positive = Yes_P / (Yes == 0 ? 1 : Yes);
            double Yes_Negative = Yes_N / (Yes == 0 ? 1 : Yes);
            double No_Positive = No_P / (No == 0 ? 1 : No);
            double No_Negative = No_N / (No == 0 ? 1 : No);

            double H_Yes_Positive = Yes_P / (Yes == 0 ? 1 : Yes);
            double H_Yes_Negative = Yes_N / (Yes == 0 ? 1 : Yes);
            double H_No_Positive = No_P / (No == 0 ? 1 : No);
            double H_No_Negative = No_N / (No == 0 ? 1 : No);

            if (Yes_Positive == 0) { H_Yes_Positive = 2; }
            if (Yes_Negative == 0) { H_Yes_Negative = 2; }
            if (No_Positive == 0) { H_No_Positive = 2; }
            if (No_Negative == 0) { H_No_Negative = 2; }

            double Yes_Result = ((-Yes_Positive) * Math.Log(H_Yes_Positive, 2)) - (Yes_Negative * Math.Log(H_Yes_Negative, 2));
            double No_Result = (((-No_Positive) * Math.Log(H_No_Positive, 2)) - ((No_Negative) * Math.Log(H_No_Negative, 2)));

            double Feature_Result = ((Yes / Convert.ToDouble(TrainingData.Count)) * Yes_Result) + ((No / Convert.ToDouble(TrainingData.Count)) * No_Result);
            if (double.IsInfinity(Feature_Result) || double.IsNaN(Feature_Result) || double.IsNegativeInfinity(Feature_Result) || double.IsPositiveInfinity(Feature_Result))
            {
                Console.WriteLine("You have a NaN, or infinity value");
            }
            return Feature_Result;
        }
        private void DetermineFeature()
        {
            Feature = InformationGains.OrderByDescending(x => x.Value).First().Key;

            if (InformationGains.All(x => x.Value == 0))
            {
                for (int i = 1; i < 67693; i++)
                {
                    if (!FeaturesTaken.Contains(i))
                    {
                        Feature = i;
                    }
                }
            }
        }
        private void DetermineSubTrees()
        {
            List<int> LeafValues = new List<int>();
            List<List<Entry>> Datas = new List<List<Entry>>();
            List<bool> Is_Leaf = new List<bool>();
            List<List<int>> Distinct_Labels = new List<List<int>>();
            //char? LeftLeafValue = null;
            List<Entry> LeftData = new List<Entry>();
            List<Entry> RightData = new List<Entry>();
            foreach (var item in TrainingData)
            {
                if (!item.Vector.ContainsKey(Feature))
                {
                    LeftData.Add(item); //left tree is the negative route
                }
                else
                {
                    RightData.Add(item); //Right Tree is the positive route
                }
            }
            Datas.Add(LeftData);
            Datas.Add(RightData);

            for (int i = 0; i < Datas.Count; i++)
            {
                Distinct_Labels.Add((from h in Datas[i] select h.Sign).Distinct().ToList());
                Is_Leaf.Add(Distinct_Labels[i].Count == 1);
                if (Is_Leaf[i]) { LeafValues.Add(Datas[i].Select(p => p.Sign).First()); }
                else if (Datas[i].Count < 2)
                {

                    Is_Leaf[i] = true;
                    if (Datas[i].Count == 0)
                    {
                        LeafValues.Add(Random());
                    }
                    else { LeafValues.Add(Datas[i].Select(p => p.Sign).First()); }
                }
                else {
                    LeafValues.Add(Random());
                } //A 0 leaf value mean its not a leaf. 1 is positive label, -1 is negative label
            }

            if (ResultInLeaf(ref LeftData))
            {
                Is_Leaf[0] = true;
                if (Datas[0].Count != 0)
                {
                    LeafValues[0] = Datas[0].GroupBy(m => m.Sign).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                }
                else
                {
                    LeafValues[0] = Datas[1].GroupBy(m => m.Sign).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                }
            }

            if (ResultInLeaf(ref RightData))
            {
                Is_Leaf[1] = true;
                if (Datas[1].Count != 0)
                {
                    LeafValues[1] = Datas[1].GroupBy(m => m.Sign).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                }
                else
                {
                    LeafValues[1] = Datas[0].GroupBy(m => m.Sign).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                }
            }

            if (FeaturesTaken.Count > 67691)
            {
                IsLeaf = true;
                Children = null;
                Value = TrainingData.GroupBy(m => m.Sign).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                Feature = -1;
                return;
            }

            if (LeafValues.Distinct().ToList().Count == 1 && LeafValues.Any(x => x != 0)) //Remember we check if the leaf values aren't 0 
            {
                IsLeaf = true;
                Children = null;
                Value = LeafValues.First();
                Feature = -1;
            }
            else
            {
                List<int> featuresTakenHelper = FeaturesTaken;
                if (DepthRemaining > 1)
                {
                    for (int i = 0; i < Datas.Count; i++)
                    {
                        List<Entry> data = Datas[i];
                        Children.Add(new DecisionTree(Is_Leaf[i], ref data, LeafValues[i], DepthRemaining - 1, ref featuresTakenHelper, random, Naive_Bayes));
                    }
                }
                else
                {
                    for (int i = 0; i < Datas.Count; i++)
                    {
                        if (Is_Leaf[i])
                        {
                            List<Entry> data = Datas[i];
                            Children.Add(new DecisionTree(Is_Leaf[i], ref data, LeafValues[i], DepthRemaining - 1, ref featuresTakenHelper, random, Naive_Bayes));
                        }
                        else
                        {
                            if (Datas[i].Count > 0)
                            {
                                LeafValues[i] = Datas[i].GroupBy(m => m.Sign).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                            }
                            else
                            {
                                if (Datas.Any(x => x.Count > 0)) //There is at least 1 list that contains data points
                                {
                                    List<Entry> sumofDatas = new List<Entry>();
                                    foreach (var item in Datas)
                                    {
                                        sumofDatas.AddRange(item);
                                    }
                                    LeafValues[i] = sumofDatas.GroupBy(m => m.Sign).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                                }
                                else //none of the lists contain data points, so get a random label
                                {
                                    LeafValues[i] = Random();
                                }
                            }
                            List<Entry> data = Datas[i];
                            Children.Add(new DecisionTree(true, ref data, LeafValues[i], DepthRemaining - 1, ref featuresTakenHelper, random, Naive_Bayes));
                        }
                    }
                }
            }
        }
        public int Random()
        {
            int num = random.Next() % 2;
            if (num == 0)
                return -1;
            else
                return 1;
        }

        public bool ResultInLeaf(ref List<Entry> Data)
        {
            Dictionary<int, bool> Counts = new Dictionary<int, bool>();
            for (int i = 1; i < 67693; i++)
            {
                int p = 0;
                foreach (var item in Data)
                {
                    if (item.Vector.ContainsKey(i))
                    {
                        p++;
                    }
                }
                if (p == Data.Count || p == 0)
                {
                    Counts.Add(i, true); //Means Yes that feature would result as a leaf
                }
                else
                {
                    Counts.Add(i, false); //Means No that feature wouldn't result as a leaf
                }
            }
            return Counts.All(x => x.Value == true);
        }
        public void PrintInformationGain()
        {
            foreach (var item in InformationGains)
            {
                Console.WriteLine("Feature #" + item.Key + "\n\tInformation Gain:\t" + item.Value);
            }
        }
        public int DetermineDepth(int count)
        {
            if (IsLeaf)
            {
                return count++;
            }
            else
            {
                List<int> rp = new List<int>();
                foreach (var item in Children)
                {
                    rp.Add(item.DetermineDepth(count++));
                }
                return rp.Max();
            }
        }
        public void CollapseTree()
        {
            if (!IsLeaf)
            {
                if (Children.All(x => x.IsLeaf))
                {
                    int Children_Equal = Children.Select(x => x.Value).Distinct().ToList().Count;
                    if (Children_Equal == 1)
                    {
                        IsLeaf = true;
                        Value = Children[0].Value;
                        Children = null;
                        Feature = -1;
                    }
                }
                else
                {
                    foreach (var item in Children)
                    {
                        item.CollapseTree();
                    }
                    if (Children.All(x => x.IsLeaf))
                    {
                        int Children_Equal = Children.Select(x => x.Value).Distinct().ToList().Count;
                        if (Children_Equal == 1)
                        {
                            IsLeaf = true;
                            Value = Children[0].Value;
                            Children = null;
                            Feature = -1;
                        }
                    }
                }
            }
        }
        public void TraverseTree()
        {
            if (IsLeaf)
            {
                Console.WriteLine(Value);
            }
            else
            {
                Console.WriteLine(Feature);
                foreach (var item in Children)
                {
                    item.TraverseTree();
                }
            }
        }
        public int DetermineError(ref List<Entry> TestData)
        {
            int errors = 0;
            foreach (var item in TestData)
            { //for each item, i want to traverse down the tree
                List<int> helper = DetermineSubError(item);
                errors += helper.First();
                Labels.Add(helper.Last());
            }
            return errors;
        }
        private List<int> DetermineSubError(Entry item)
        {
            if (IsLeaf)
            {
                if (item.Sign != Value) { return new List<int> { 1, Value }; }
                else { return new List<int> { 0, Value }; }
            }
            else
            {
                if (!item.Vector.ContainsKey(Feature))
                {
                    return Children[0].DetermineSubError(item);
                }
                else
                {
                    return Children[1].DetermineSubError(item);
                }
            }
        }

    }
}
