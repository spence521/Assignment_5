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

        public DecisionTree(ref List<Entry> trainingdata, int depth, Random r)
        {
            random = r;
            Labels = new List<int>();
            InformationGains = new Dictionary<int, double>();
            FeaturesTaken = new List<int>();
            IsLeaf = false;
            Children = new List<DecisionTree>();
            Value = 0; //A negative one leaf value mean its not a leaf. 1 is positive label, -1 is negative label
            DepthRemaining = depth;
            TrainingData = trainingdata;
            SetEntropy();
            SetInformationGain();
            DetermineFeature();
            FeaturesTaken.Add(Feature);
            DetermineSubTrees();
        }

        public DecisionTree(bool isLeaf, ref List<Entry> trainingdata, int value, int depthRemaining, ref List<int> featuresTaken, Random r)
        {
            random = r;
            IsLeaf = isLeaf;
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
                if (item.Label == 1) { positive_labels++; }
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
            int Positive_Labels = 0;
            int Negative_Labels = 0;
            foreach (var example in TrainingData)
            {
                if (example.Label == 1)
                {
                    for (int i = 1; i < 67693; i++)
                    {
                        if(example.Vector.ContainsKey(i)) //This means that the feature is +1 and the true label is +1
                        {
                            if (Counts11.ContainsKey(i))
                            {
                                Counts11[i] = Counts11[i] + 1;
                            }
                            else
                            {
                                Counts11.Add(i, 1);
                            }
                        }
                        else //This means that the feature is -1 and the true label is +1
                        {
                            if (Counts10.ContainsKey(i))
                            {
                                Counts10[i] = Counts10[i] + 1;
                            }
                            else
                            {
                                Counts10.Add(i, 1);
                            }
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
                            if (Counts01.ContainsKey(i))
                            {
                                Counts01[i] = Counts01[i] + 1;
                            }
                            else
                            {
                                Counts01.Add(i, 1);
                            }
                        }
                        else //This means that the feature is -1 and the true label is -1
                        {
                            if (Counts00.ContainsKey(i))
                            {
                                Counts00[i] = Counts00[i] + 1;
                            }
                            else
                            {
                                Counts00.Add(i, 1);
                            }
                        }
                    }
                    Negative_Labels++;
                }
            }
            
            for (int i = 1; i < 67693; i++)
            {
                InformationGains.Add(i, CalculateInformationGain(Positive_Labels, Negative_Labels, Counts11[i], Counts01[i], Counts10[i], Counts00[i]));
            }

           
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
            //if (Yes == 0) { Yes = 1; }
            //if (No == 0) { No = 1; }

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
            return Entropy - Feature_Result;
        }
        private void DetermineFeature()
        {
            Feature = InformationGains.OrderByDescending(x => x.Value).First().Key;

            if(InformationGains.All(x => x.Value == 0))
            {
                for (int i = 1; i < 67693; i++)
                {
                    if(!FeaturesTaken.Contains(i))
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
                Distinct_Labels.Add((from h in Datas[i] select h.Label).Distinct().ToList());
                Is_Leaf.Add(Distinct_Labels[i].Count == 1);
                if (Is_Leaf[i]) { LeafValues.Add(Datas[i].Select(p => p.Label).First()); }
                else if (Datas[i].Count < 2)
                {
                    Is_Leaf[i] = true;
                    if (Datas[i].Count == 0) { LeafValues.Add(0); }
                    else { LeafValues.Add(Datas[i].Select(p => p.Label).First()); }
                }
                else { LeafValues.Add(0); } //A 0 leaf value mean its not a leaf. 1 is positive label, -1 is negative label
            }
                        
            if(ResultInLeaf(ref LeftData))
            {
                Is_Leaf[0] = true;
                LeafValues[0] = Datas[0].GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
            }

            if (ResultInLeaf(ref RightData))
            {
                Is_Leaf[1] = true;
                LeafValues[1] = Datas[1].GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
            }           

            if (FeaturesTaken.Count > 67691)
            {
                IsLeaf = true;
                Children = null;
                Value = TrainingData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                Feature = -1;
                return;
            }
            
            if(LeafValues.Distinct().ToList().Count == 1 && LeafValues.Any(x => x != 0)) //Remember we check if the leaf values aren't 0 
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
                        Children.Add(new DecisionTree(Is_Leaf[i], ref data, LeafValues[i], DepthRemaining - 1, ref featuresTakenHelper, random));
                    }
                }
                else
                {
                    for (int i = 0; i < Datas.Count; i++)
                    {
                        if (Is_Leaf[i])
                        {
                            List<Entry> data = Datas[i];
                            Children.Add(new DecisionTree(Is_Leaf[i], ref data, LeafValues[i], DepthRemaining - 1, ref featuresTakenHelper, random));
                        }
                        else
                        {
                            if (Datas[i].Count > 0)
                            {
                                LeafValues[i] = Datas[i].GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
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
                                    LeafValues[i] = sumofDatas.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                                }
                                else //none of the lists contain data points, so get a random lab
                                {
                                    LeafValues[i] = (random.Next() % 2);
                                }
                            }
                            List<Entry> data = Datas[i];
                            Children.Add(new DecisionTree(true, ref data, LeafValues[i], DepthRemaining - 1, ref featuresTakenHelper, random));
                        }
                    }
                }
            }            
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
            if(IsLeaf)
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
            if(IsLeaf)
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
        public int DetermineError(ref List<TrainingData> TestData)
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
        private List<int> DetermineSubError(TrainingData item)
        {
            if (IsLeaf)
            {
                if (item.Label != Value) { return  new List<int> { 1, Value }; }
                else { return new List<int> { 0, Value }; }
            }
            else
            {   
                if (Feature == Features.ScreenNameLength)
                {
                    if (item.screenNameLength == Assignment_1.TrainingData.ScreenNameLength.Range0_3) { return Children[0].DetermineSubError(item); }
                    else if (item.screenNameLength == Assignment_1.TrainingData.ScreenNameLength.Range4_6) { return Children[1].DetermineSubError(item); }
                    else if (item.screenNameLength == Assignment_1.TrainingData.ScreenNameLength.Range7_9) { return Children[2].DetermineSubError(item); }
                    else if (item.screenNameLength == Assignment_1.TrainingData.ScreenNameLength.Range10_12) { return Children[3].DetermineSubError(item); }
                    else { return Children[4].DetermineSubError(item); }
                }
                else if (Feature == Features.DescriptionLength)
                {
                    if (item.descriptionLength == Assignment_1.TrainingData.DescriptionLength.Range0_33) { return Children[0].DetermineSubError(item); }
                    else if (item.descriptionLength == Assignment_1.TrainingData.DescriptionLength.Range34_66) { return Children[1].DetermineSubError(item); }
                    else if (item.descriptionLength == Assignment_1.TrainingData.DescriptionLength.Range67_99) { return Children[2].DetermineSubError(item); }
                    else if (item.descriptionLength == Assignment_1.TrainingData.DescriptionLength.Range100_132) { return Children[3].DetermineSubError(item); }
                    else { return Children[4].DetermineSubError(item); }
                }
                else if (Feature == Features.Days)
                {
                    if (item.Days == Assignment_1.TrainingData.LongevityDays.Range0_200) { return Children[0].DetermineSubError(item); }
                    else if (item.Days == Assignment_1.TrainingData.LongevityDays.Range201_400) { return Children[1].DetermineSubError(item); }
                    else if (item.Days == Assignment_1.TrainingData.LongevityDays.Range401_600) { return Children[2].DetermineSubError(item); }
                    else if (item.Days == Assignment_1.TrainingData.LongevityDays.Range601_800) { return Children[3].DetermineSubError(item); }
                    else { return Children[4].DetermineSubError(item); }
                }
                else if (Feature == Features.Hours)
                {
                    if (item.Hours == Assignment_1.TrainingData.LongevityHours.Range0_8) { return Children[0].DetermineSubError(item); }
                    else if (item.Hours == Assignment_1.TrainingData.LongevityHours.Range9_16) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                } 
                else if (Feature == Features.Minutes)
                {
                    if (item.Minutes == Assignment_1.TrainingData.LongevityMinSec.Range0_15) { return Children[0].DetermineSubError(item); }
                    else if (item.Minutes == Assignment_1.TrainingData.LongevityMinSec.Range16_30) { return Children[1].DetermineSubError(item); }
                    else if (item.Minutes == Assignment_1.TrainingData.LongevityMinSec.Range31_45) { return Children[2].DetermineSubError(item); }
                    else { return Children[3].DetermineSubError(item); }
                }
                else if(Feature == Features.Seconds)
                {
                    if (item.Seconds == Assignment_1.TrainingData.LongevityMinSec.Range0_15) { return Children[0].DetermineSubError(item); }
                    else if (item.Seconds == Assignment_1.TrainingData.LongevityMinSec.Range16_30) { return Children[1].DetermineSubError(item); }
                    else if (item.Seconds == Assignment_1.TrainingData.LongevityMinSec.Range31_45) { return Children[2].DetermineSubError(item); }
                    else { return Children[3].DetermineSubError(item); }
                }                                
                else if (Feature == Features.Following)
                {
                    if (item.Following == Assignment_1.TrainingData.Follow.Range0_100) { return Children[0].DetermineSubError(item); }
                    else if (item.Following == Assignment_1.TrainingData.Follow.Range101_400) { return Children[1].DetermineSubError(item); }
                    else if (item.Following == Assignment_1.TrainingData.Follow.Range401_1400) { return Children[2].DetermineSubError(item); }
                    else if (item.Following == Assignment_1.TrainingData.Follow.Range1401_3000) { return Children[3].DetermineSubError(item); }
                    else if (item.Following == Assignment_1.TrainingData.Follow.Range3001_10000) { return Children[4].DetermineSubError(item); }
                    else { return Children[5].DetermineSubError(item); }
                }
                else if (Feature == Features.Followers)
                {
                    if (item.Followers == Assignment_1.TrainingData.Follow.Range0_100) { return Children[0].DetermineSubError(item); }
                    else if (item.Followers == Assignment_1.TrainingData.Follow.Range101_400) { return Children[1].DetermineSubError(item); }
                    else if (item.Followers == Assignment_1.TrainingData.Follow.Range401_1400) { return Children[2].DetermineSubError(item); }
                    else if (item.Followers == Assignment_1.TrainingData.Follow.Range1401_3000) { return Children[3].DetermineSubError(item); }
                    else if (item.Followers == Assignment_1.TrainingData.Follow.Range3001_10000) { return Children[4].DetermineSubError(item); }
                    else { return Children[5].DetermineSubError(item); }
                }
                else if (Feature == Features.Ratio)
                {
                    if (item.ratio == Assignment_1.TrainingData.Ratio.Range0_1) { return Children[0].DetermineSubError(item); }
                    else if (item.ratio == Assignment_1.TrainingData.Ratio.Range1_3) { return Children[1].DetermineSubError(item); }
                    else if (item.ratio == Assignment_1.TrainingData.Ratio.Range3_8) { return Children[2].DetermineSubError(item); }
                    else { return Children[3].DetermineSubError(item); }
                }
                else if (Feature == Features.TotalTweets)
                {
                    if (item.TotalTweets == Assignment_1.TrainingData.Tweets.Range0_66) { return Children[0].DetermineSubError(item); }
                    else if (item.TotalTweets == Assignment_1.TrainingData.Tweets.Range67_132) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else if (Feature == Features.TweetsPerDay)
                {
                    if (item.tweetsPerDay == Assignment_1.TrainingData.TweetsPerDay.Range0_66) { return Children[0].DetermineSubError(item); }
                    else if (item.tweetsPerDay == Assignment_1.TrainingData.TweetsPerDay.Range67_132) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else if (Feature == Features.AverageLinks)
                {
                    if (item.averageLinks == Assignment_1.TrainingData.AverageLinks.Range0_1) { return Children[0].DetermineSubError(item); }
                    else if (item.averageLinks == Assignment_1.TrainingData.AverageLinks.Range1_2) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else if (Feature == Features.AverageUniqueLinks)
                {
                    if (item.AverageUniqueLinks == Assignment_1.TrainingData.AverageLinks.Range0_1) { return Children[0].DetermineSubError(item); }
                    else if (item.AverageUniqueLinks == Assignment_1.TrainingData.AverageLinks.Range1_2) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else if (Feature == Features.AverageUsername)
                {
                    if (item.averageUsername == Assignment_1.TrainingData.AverageUsername.Range0_2) { return Children[0].DetermineSubError(item); }
                    else if (item.averageUsername == Assignment_1.TrainingData.AverageUsername.Range2_4) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else if (Feature == Features.AverageUniqueUsername)
                {
                    if (item.AverageUniqueUsername == Assignment_1.TrainingData.AverageUsername.Range0_2) { return Children[0].DetermineSubError(item); }
                    else if (item.AverageUniqueUsername == Assignment_1.TrainingData.AverageUsername.Range2_4) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
                else // (Feature == Features.ChangeRate)
                {
                    if (item.changeRate == Assignment_1.TrainingData.ChangeRate.Range0_5) { return Children[0].DetermineSubError(item); }
                    else if (item.changeRate == Assignment_1.TrainingData.ChangeRate.Range5_50) { return Children[1].DetermineSubError(item); }
                    else { return Children[2].DetermineSubError(item); }
                }
            }
        }

    }
}
