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
            List<List<TrainingData>> Datas = new List<List<TrainingData>>();
            List<bool> Is_Leaf = new List<bool>();
            List<List<int>> Distinct_Labels = new List<List<int>>();
            List<List<TrainingData.ScreenNameLength>> screenNameLength = new List<List<TrainingData.ScreenNameLength>>();
            List<List<TrainingData.DescriptionLength>> descriptionLength = new List<List<TrainingData.DescriptionLength>>();
            List<List<TrainingData.LongevityDays>> Days = new List<List<TrainingData.LongevityDays>>();
            List<List<TrainingData.LongevityHours>> Hours = new List<List<TrainingData.LongevityHours>>();
            List<List<TrainingData.LongevityMinSec>> Minutes = new List<List<TrainingData.LongevityMinSec>>();
            List<List<TrainingData.LongevityMinSec>> Seconds = new List<List<TrainingData.LongevityMinSec>>();
            List<List<TrainingData.Follow>> Following = new List<List<TrainingData.Follow>>();
            List<List<TrainingData.Follow>> Followers = new List<List<TrainingData.Follow>>();
            List<List<TrainingData.Ratio>> ratio = new List<List<TrainingData.Ratio>>();
            List<List<TrainingData.Tweets>> TotalTweets = new List<List<TrainingData.Tweets>>();
            List<List<TrainingData.TweetsPerDay>> TweetsPerDay = new List<List<TrainingData.TweetsPerDay>>();
            List<List<TrainingData.AverageLinks>> averageLinks = new List<List<TrainingData.AverageLinks>>();
            List<List<TrainingData.AverageLinks>> AverageUniqueLinks = new List<List<TrainingData.AverageLinks>>();
            List<List<TrainingData.AverageUsername>> averageUsername = new List<List<TrainingData.AverageUsername>>();
            List<List<TrainingData.AverageUsername>> AverageUniqueUsername = new List<List<TrainingData.AverageUsername>>();
            List<List<TrainingData.ChangeRate>> changeRate = new List<List<TrainingData.ChangeRate>>();
            //char? LeftLeafValue = null;
            //List<TrainingData> LeftData;
            if (Feature == Features.ScreenNameLength)
            {
                Datas.Add(TrainingData.Where(x => x.screenNameLength == Assignment_1.TrainingData.ScreenNameLength.Range0_3).ToList());
                Datas.Add(TrainingData.Where(x => x.screenNameLength == Assignment_1.TrainingData.ScreenNameLength.Range4_6).ToList());
                Datas.Add(TrainingData.Where(x => x.screenNameLength == Assignment_1.TrainingData.ScreenNameLength.Range7_9).ToList());
                Datas.Add(TrainingData.Where(x => x.screenNameLength == Assignment_1.TrainingData.ScreenNameLength.Range10_12).ToList());
                Datas.Add(TrainingData.Where(x => x.screenNameLength == Assignment_1.TrainingData.ScreenNameLength.RangeGT_12).ToList());

            }
            else if (Feature == Features.DescriptionLength)
            {
                Datas.Add(TrainingData.Where(x => x.descriptionLength == Assignment_1.TrainingData.DescriptionLength.Range0_33).ToList());
                Datas.Add(TrainingData.Where(x => x.descriptionLength == Assignment_1.TrainingData.DescriptionLength.Range34_66).ToList());
                Datas.Add(TrainingData.Where(x => x.descriptionLength == Assignment_1.TrainingData.DescriptionLength.Range67_99).ToList());
                Datas.Add(TrainingData.Where(x => x.descriptionLength == Assignment_1.TrainingData.DescriptionLength.Range100_132).ToList());
                Datas.Add(TrainingData.Where(x => x.descriptionLength == Assignment_1.TrainingData.DescriptionLength.RangeGT_132).ToList());
            }
            else if (Feature == Features.Days)
            {
                Datas.Add(TrainingData.Where(x => x.Days == Assignment_1.TrainingData.LongevityDays.Range0_200).ToList());
                Datas.Add(TrainingData.Where(x => x.Days == Assignment_1.TrainingData.LongevityDays.Range201_400).ToList());
                Datas.Add(TrainingData.Where(x => x.Days == Assignment_1.TrainingData.LongevityDays.Range401_600).ToList());
                Datas.Add(TrainingData.Where(x => x.Days == Assignment_1.TrainingData.LongevityDays.Range601_800).ToList());
                Datas.Add(TrainingData.Where(x => x.Days == Assignment_1.TrainingData.LongevityDays.RangeGT_800).ToList());
            }
            else if (Feature == Features.Hours)
            {
                Datas.Add(TrainingData.Where(x => x.Hours == Assignment_1.TrainingData.LongevityHours.Range0_8).ToList());
                Datas.Add(TrainingData.Where(x => x.Hours == Assignment_1.TrainingData.LongevityHours.Range9_16).ToList());
                Datas.Add(TrainingData.Where(x => x.Hours == Assignment_1.TrainingData.LongevityHours.RangeGT_16).ToList());
            }
            else if (Feature == Features.Minutes)
            {
                Datas.Add(TrainingData.Where(x => x.Minutes == Assignment_1.TrainingData.LongevityMinSec.Range0_15).ToList());
                Datas.Add(TrainingData.Where(x => x.Minutes == Assignment_1.TrainingData.LongevityMinSec.Range16_30).ToList());
                Datas.Add(TrainingData.Where(x => x.Minutes == Assignment_1.TrainingData.LongevityMinSec.Range31_45).ToList());
                Datas.Add(TrainingData.Where(x => x.Minutes == Assignment_1.TrainingData.LongevityMinSec.RangeGT_45).ToList());
            }
            else if (Feature == Features.Seconds)
            {
                Datas.Add(TrainingData.Where(x => x.Seconds == Assignment_1.TrainingData.LongevityMinSec.Range0_15).ToList());
                Datas.Add(TrainingData.Where(x => x.Seconds == Assignment_1.TrainingData.LongevityMinSec.Range16_30).ToList());
                Datas.Add(TrainingData.Where(x => x.Seconds == Assignment_1.TrainingData.LongevityMinSec.Range31_45).ToList());
                Datas.Add(TrainingData.Where(x => x.Seconds == Assignment_1.TrainingData.LongevityMinSec.RangeGT_45).ToList());
            }
            else if (Feature == Features.Following)
            {
                Datas.Add(TrainingData.Where(x => x.Following == Assignment_1.TrainingData.Follow.Range0_100).ToList());
                Datas.Add(TrainingData.Where(x => x.Following == Assignment_1.TrainingData.Follow.Range101_400).ToList());
                Datas.Add(TrainingData.Where(x => x.Following == Assignment_1.TrainingData.Follow.Range401_1400).ToList());
                Datas.Add(TrainingData.Where(x => x.Following == Assignment_1.TrainingData.Follow.Range1401_3000).ToList());
                Datas.Add(TrainingData.Where(x => x.Following == Assignment_1.TrainingData.Follow.Range3001_10000).ToList());
                Datas.Add(TrainingData.Where(x => x.Following == Assignment_1.TrainingData.Follow.RangeGT_10000).ToList());
            }
            else if (Feature == Features.Followers)
            {
                Datas.Add(TrainingData.Where(x => x.Followers == Assignment_1.TrainingData.Follow.Range0_100).ToList());
                Datas.Add(TrainingData.Where(x => x.Followers == Assignment_1.TrainingData.Follow.Range101_400).ToList());
                Datas.Add(TrainingData.Where(x => x.Followers == Assignment_1.TrainingData.Follow.Range401_1400).ToList());
                Datas.Add(TrainingData.Where(x => x.Followers == Assignment_1.TrainingData.Follow.Range1401_3000).ToList());
                Datas.Add(TrainingData.Where(x => x.Followers == Assignment_1.TrainingData.Follow.Range3001_10000).ToList());
                Datas.Add(TrainingData.Where(x => x.Followers == Assignment_1.TrainingData.Follow.RangeGT_10000).ToList());
            }
            else if (Feature == Features.Ratio)
            {
                Datas.Add(TrainingData.Where(x => x.ratio == Assignment_1.TrainingData.Ratio.Range0_1).ToList());
                Datas.Add(TrainingData.Where(x => x.ratio == Assignment_1.TrainingData.Ratio.Range1_3).ToList());
                Datas.Add(TrainingData.Where(x => x.ratio == Assignment_1.TrainingData.Ratio.Range3_8).ToList());
                Datas.Add(TrainingData.Where(x => x.ratio == Assignment_1.TrainingData.Ratio.RangeGT_8).ToList());
            }
            else if (Feature == Features.TotalTweets)
            {
                Datas.Add(TrainingData.Where(x => x.TotalTweets == Assignment_1.TrainingData.Tweets.Range0_66).ToList());
                Datas.Add(TrainingData.Where(x => x.TotalTweets == Assignment_1.TrainingData.Tweets.Range67_132).ToList());
                Datas.Add(TrainingData.Where(x => x.TotalTweets == Assignment_1.TrainingData.Tweets.RangeGT_132).ToList());
            }
            else if (Feature == Features.TweetsPerDay)
            {
                Datas.Add(TrainingData.Where(x => x.tweetsPerDay == Assignment_1.TrainingData.TweetsPerDay.Range0_66).ToList());
                Datas.Add(TrainingData.Where(x => x.tweetsPerDay == Assignment_1.TrainingData.TweetsPerDay.Range67_132).ToList());
                Datas.Add(TrainingData.Where(x => x.tweetsPerDay == Assignment_1.TrainingData.TweetsPerDay.RangeGT_132).ToList());
            }
            else if (Feature == Features.AverageLinks)
            {
                Datas.Add(TrainingData.Where(x => x.averageLinks == Assignment_1.TrainingData.AverageLinks.Range0_1).ToList());
                Datas.Add(TrainingData.Where(x => x.averageLinks == Assignment_1.TrainingData.AverageLinks.Range1_2).ToList());
                Datas.Add(TrainingData.Where(x => x.averageLinks == Assignment_1.TrainingData.AverageLinks.RangeGT_2).ToList());
            }
            else if (Feature == Features.AverageUniqueLinks)
            {
                Datas.Add(TrainingData.Where(x => x.AverageUniqueLinks == Assignment_1.TrainingData.AverageLinks.Range0_1).ToList());
                Datas.Add(TrainingData.Where(x => x.AverageUniqueLinks == Assignment_1.TrainingData.AverageLinks.Range1_2).ToList());
                Datas.Add(TrainingData.Where(x => x.AverageUniqueLinks == Assignment_1.TrainingData.AverageLinks.RangeGT_2).ToList());
            }
            else if (Feature == Features.AverageUsername)
            {
                Datas.Add(TrainingData.Where(x => x.averageUsername == Assignment_1.TrainingData.AverageUsername.Range0_2).ToList());
                Datas.Add(TrainingData.Where(x => x.averageUsername == Assignment_1.TrainingData.AverageUsername.Range2_4).ToList());
                Datas.Add(TrainingData.Where(x => x.averageUsername == Assignment_1.TrainingData.AverageUsername.RangeGT_4).ToList());
            }
            else if (Feature == Features.AverageUniqueUsername)
            {
                Datas.Add(TrainingData.Where(x => x.AverageUniqueUsername == Assignment_1.TrainingData.AverageUsername.Range0_2).ToList());
                Datas.Add(TrainingData.Where(x => x.AverageUniqueUsername == Assignment_1.TrainingData.AverageUsername.Range2_4).ToList());
                Datas.Add(TrainingData.Where(x => x.AverageUniqueUsername == Assignment_1.TrainingData.AverageUsername.RangeGT_4).ToList());
            }
            else //change rate
            {
                Datas.Add(TrainingData.Where(x => x.changeRate == Assignment_1.TrainingData.ChangeRate.Range0_5).ToList());
                Datas.Add(TrainingData.Where(x => x.changeRate == Assignment_1.TrainingData.ChangeRate.Range5_50).ToList());
                Datas.Add(TrainingData.Where(x => x.changeRate == Assignment_1.TrainingData.ChangeRate.RangeGT_50).ToList());
            }

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
                else { LeafValues.Add(-1); } //A negative one leaf value mean its not a leaf. 1 is positive label, 0 is negative label
            }

            foreach (var item in Datas)
            {
                screenNameLength.Add((from h in item select h.screenNameLength).Distinct().ToList()); // this determines if there are more than 1 of the same result from the feature 
                descriptionLength.Add((from h in item select h.descriptionLength).Distinct().ToList());
                Days.Add((from h in item select h.Days).Distinct().ToList());
                Hours.Add((from h in item select h.Hours).Distinct().ToList());
                Minutes.Add((from h in item select h.Minutes).Distinct().ToList());
                Seconds.Add((from h in item select h.Seconds).Distinct().ToList());
                Following.Add((from h in item select h.Following).Distinct().ToList());
                Followers.Add((from h in item select h.Followers).Distinct().ToList());
                ratio.Add((from h in item select h.ratio).Distinct().ToList());
                TotalTweets.Add((from h in item select h.TotalTweets).Distinct().ToList());
                TweetsPerDay.Add((from h in item select h.tweetsPerDay).Distinct().ToList());
                averageLinks.Add((from h in item select h.averageLinks).Distinct().ToList());
                AverageUniqueLinks.Add((from h in item select h.AverageUniqueLinks).Distinct().ToList());
                averageUsername.Add((from h in item select h.averageUsername).Distinct().ToList());
                AverageUniqueUsername.Add((from h in item select h.AverageUniqueUsername).Distinct().ToList());
                changeRate.Add((from h in item select h.changeRate).Distinct().ToList());
            }
            for (int i = 0; i < Datas.Count; i++)
            {
                if(screenNameLength[i].Count == 1 && descriptionLength[i].Count == 1 && Days[i].Count == 1 && Hours[i].Count == 1 && Minutes[i].Count == 1 &&
                    Seconds[i].Count == 1 && Following[i].Count == 1 && Followers[i].Count == 1 && ratio[i].Count == 1 && TotalTweets[i].Count == 1 && TweetsPerDay[i].Count == 1 
                    && averageLinks[i].Count == 1 && AverageUniqueLinks[i].Count == 1 && averageUsername[i].Count == 1 && AverageUniqueUsername[i].Count == 1 && changeRate[i].Count == 1)
                {
                    Is_Leaf[i] = true;
                    LeafValues[i] = Datas[i].GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                }
            }            
            
            if(FeaturesTaken.Count > 15)
            {
                IsLeaf = true;
                Children = null;
                Value = TrainingData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                Feature = Features.None;
                return;
            }
            
            if(LeafValues.Distinct().ToList().Count == 1 && LeafValues.Any(x => x != -1))
            {
                IsLeaf = true;
                Children = null;
                Value = LeafValues.First();
                Feature = Features.None;
            }
            else
            {
                List<Features> featuresTakenHelper = FeaturesTaken;
                if (DepthRemaining > 1)
                {
                    for (int i = 0; i < Datas.Count; i++)
                    {
                        List<TrainingData> data = Datas[i];
                        Children.Add(new DecisionTree(Is_Leaf[i], ref data, LeafValues[i], DepthRemaining - 1, ref featuresTakenHelper, random));
                    }
                }
                else
                {
                    for (int i = 0; i < Datas.Count; i++)
                    {
                        if (Is_Leaf[i])
                        {
                            List<TrainingData> data = Datas[i];
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
                                    List<TrainingData> sumofDatas = new List<TrainingData>();
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
                            List<TrainingData> data = Datas[i];
                            Children.Add(new DecisionTree(true, ref data, LeafValues[i], DepthRemaining - 1, ref featuresTakenHelper, random));
                        }
                    }
                    //if (IsLeftLeaf && IsRightLeaf)
                    //{
                    //    LeftTree = new DecisionTree(IsLeftLeaf, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //    RightTree = new DecisionTree(IsRightLeaf, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //}
                    //else if (IsLeftLeaf)
                    //{
                    //    RightLeafValue = RightData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //    LeftTree = new DecisionTree(IsLeftLeaf, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //    RightTree = new DecisionTree(true, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //}
                    //else if (IsRightLeaf)
                    //{
                    //    LeftLeafValue = LeftData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //    LeftTree = new DecisionTree(true, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //    RightTree = new DecisionTree(IsRightLeaf, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //}
                    //else
                    //{
                    //    if (LeftData.Count == 0 && RightData.Count == 0)
                    //    {
                    //        LeftLeafValue = '-';
                    //        RightLeafValue = '+';
                    //    }
                    //    else if (LeftData.Count == 0)
                    //    {
                    //        LeftLeafValue = RightData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //    }
                    //    else if (RightData.Count == 0)
                    //    {
                    //        RightLeafValue = LeftData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //    }
                    //    else
                    //    {
                    //        LeftLeafValue = LeftData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //        RightLeafValue = RightData.GroupBy(m => m.Label).OrderByDescending(r => r.Count()).Take(1).Select(p => p.Key).First();
                    //    }
                    //    LeftTree = new DecisionTree(true, ref LeftData, LeftLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //    RightTree = new DecisionTree(true, ref RightData, RightLeafValue, DepthRemaining - 1, ref featuresTakenHelper);
                    //}
                }
            }            
        }
        public void PrintInformationGain()
        {
            Console.WriteLine(informationGain.ToString());
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
                        Feature = Features.None;
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
                            Feature = Features.None;
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
