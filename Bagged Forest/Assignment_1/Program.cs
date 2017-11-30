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
            Data data1;
            Data data2;
            Data data3;
            Data data4;
            Data data5;
            Data data6;
            Data data7;
            Data data8;
            Data data9;
            Data data10;
            Data data11;
            Data data12;
            Data data13;
            Data data14;
            Data data15;
            Data data16;
            Data data17;
            Data data18;
            Data data19;
            Data data20;
            Data data21;
            Data data22;
            Data data23;
            Data data24;
            Data data25;
            Data data26;
            Data data27;
            Data data28;
            Data data29;
            Data data30;
            Data data31;
            Data data32;
            Data data33;
            Data data34;
            Data data35;
            Data data36;
            Data data37;
            Data data38;
            Data data39;
            Data data40;
            Data data41;
            Data data42;
            StreamReader Train;
            StreamReader Test;
            //StreamReader Train_Forest;
            //StreamReader Test_Forest;
            StreamReader Cross_1;
            StreamReader Cross_2;
            StreamReader Cross_3;
            StreamReader Cross_4;
            StreamReader Cross_5;
            Random r = new Random(2);
            
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
                Cross_1 = File.OpenText(args[2]);
                Cross_2 = File.OpenText(args[3]);
                Cross_3 = File.OpenText(args[4]);
                Cross_4 = File.OpenText(args[5]);
                Cross_5 = File.OpenText(args[6]);                
                //string startupPath = System.IO.Directory.GetCurrentDirectory();
                //Train = File.OpenText(startupPath + @"\speeches.train.liblinear");
                //Test = File.OpenText(startupPath + @"\speeches.test.liblinear");
                //Cross_1 = File.OpenText(startupPath + @"\training00.data");
                //Cross_2 = File.OpenText(startupPath + @"\training01.data");
                //Cross_3 = File.OpenText(startupPath + @"\training02.data");
                //Cross_4 = File.OpenText(startupPath + @"\training03.data");
                //Cross_5 = File.OpenText(startupPath + @"\training04.data"); 
                //Train_Forest = File.OpenText(startupPath + @"\tree.train.data");
                //Test_Forest = File.OpenText(startupPath + @"\tree.test.data");

                #region Naive Bayes
                data1 = new Data(2.0, r, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data2 = new Data(1.5, r, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data3 = new Data(1.0, r, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data4 = new Data(0.5, r, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                List<Data> ListOfDatas = new List<Data>() { data1, data2, data3, data4 };

                Data LargestData = ListOfDatas.OrderByDescending(w => w.Test_Accuracy).First();
                double smoothing_term = LargestData.Smoothing_Term;
                Console.WriteLine("\n*******Naive Bayes (Part 3)*******");
                Console.WriteLine("The best hyperparameter is: \n\t" + "Smoothing Term:\t" + smoothing_term);
                Data DataTree = new Data(Train, Test, r, smoothing_term);
                Console.WriteLine("Training Accuracy:\n\t" + DataTree.Train_Accuracy);
                Console.WriteLine("Test Accuracy:\n\t" + DataTree.Test_Accuracy);
                Console.WriteLine("Average cross-validation Accuracy: \n\t" + Math.Round(LargestData.Test_Accuracy, 3));
                #endregion

                #region Bagged Forest
                int ForestSize = 1000;
                DataTree = new Data(Train, Test, 3, r, ForestSize);

                List<Entry> SVM_LR_Train_Data = new List<Entry>();
                List<Entry> SVM_LR_Test_Data = new List<Entry>();
                List<int> FinalPredictions_Train = new List<int>();
                List<int> FinalPredictions_Test = new List<int>();
                for (int i = 0; i < DataTree.Forest[0].Train_Predictions.Count; i++)
                {
                    List<int> helper_train = new List<int>();
                    foreach (var tree in DataTree.Forest) //loops through each Tree
                    {
                        helper_train.Add(tree.Train_Predictions[i]);
                    }
                    int Most_Occured_Label_train = helper_train.GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).First();
                    FinalPredictions_Train.Add(Most_Occured_Label_train);

                    //This is needed for SVM and LR to get the training data
                    Dictionary<int, double> vector = new Dictionary<int, double>();
                    for (int j = 1; j < helper_train.Count + 1; j++)
                    {
                        if (helper_train[j - 1] == 1)
                        {
                            vector.Add(j, helper_train[j - 1]);
                        }
                    }
                    SVM_LR_Train_Data.Add(new Entry(DataTree.Training_Data[i].Sign, vector));
                }
                for (int i = 0; i < DataTree.Forest[0].Test_Predictions.Count; i++)
                {
                    List<int> helper_test = new List<int>();
                    foreach (var tree in DataTree.Forest) //loops through each Tree
                    {
                        helper_test.Add(tree.Test_Predictions[i]);
                    }
                    int Most_Occured_Label_test = helper_test.GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).First();
                    FinalPredictions_Test.Add(Most_Occured_Label_test); //This is adding in increasing order of the test set

                    //This is needed for SVM and LR to get the test data
                    Dictionary<int, double> vector = new Dictionary<int, double>();
                    for (int j = 1; j < helper_test.Count + 1; j++)
                    {
                        if (helper_test[j - 1] == 1)
                        {
                            vector.Add(j, helper_test[j - 1]);
                        }
                    }
                    SVM_LR_Test_Data.Add(new Entry(DataTree.Test_Data[i].Sign, vector));
                }
                int correct_labeling_train = 0;
                int correct_labeling_test = 0;
                for (int i = 0; i < DataTree.Training_Data.Count /*Train data size */; i++)
                {
                    if (DataTree.Training_Data[i].Sign == FinalPredictions_Train[i]) //correct labeling
                    {
                        correct_labeling_train++;
                    }
                }
                for (int i = 0; i < DataTree.Test_Data.Count /*Test data size 940*/; i++)
                {
                    if (DataTree.Test_Data[i].Sign == FinalPredictions_Test[i]) //correct labeling
                    {
                        correct_labeling_test++;
                    }
                }
                double Accuracy_Train = (Convert.ToDouble(correct_labeling_train) / FinalPredictions_Train.Count) * 100;
                double Accuracy_Test = (Convert.ToDouble(correct_labeling_test) / FinalPredictions_Test.Count) * 100;
                Console.WriteLine("\n*******Bagged Forest (Part 4)*******");
                Console.WriteLine("Train Set Accuracy:\t" + Accuracy_Train);
                Console.WriteLine("Test Set Accuracy:\t" + Accuracy_Test);
                #endregion

                #region Generating Data for SVM and LR
                //string to_file = "";
                //foreach (var item in SVM_LR_Train_Data)
                //{
                //    to_file += item.Sign + " ";
                //    foreach (var item2 in item.Vector)
                //    {
                //        to_file += item2.Key + ":" + item2.Value + " ";
                //    }
                //    to_file += Environment.NewLine;
                //}
                //File.WriteAllText(startupPath + @"\tree.train.data", to_file);

                //to_file = "";
                //foreach (var item in SVM_LR_Test_Data)
                //{
                //    to_file += item.Sign + " ";
                //    foreach (var item2 in item.Vector)
                //    {
                //        to_file += item2.Key + ":" + item2.Value + " ";
                //    }
                //    to_file += Environment.NewLine;
                //}
                //File.WriteAllText(startupPath + @"\tree.test.data", to_file);
                #endregion

                #region SVM over Trees
                Data dataTest;
                #region Datas
                data1 = new Data(10, 0.00001, 0, 10, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data2 = new Data(10, 1, 0, 10, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data3 = new Data(10, 0.1, 0, 10, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data4 = new Data(10, 0.01, 0, 10, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data5 = new Data(10, 0.001, 0, 10, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data6 = new Data(10, 0.0001, 0, 10, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data7 = new Data(10, 0.00001, 0, 1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data8 = new Data(10, 1, 0, 1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data9 = new Data(10, 0.1, 0, 1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data10 = new Data(10, 0.01, 0, 1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data11 = new Data(10, 0.001, 0, 1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data12 = new Data(10, 0.0001, 0, 1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data13 = new Data(10, 0.00001, 0, 0.1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data14 = new Data(10, 1, 0, 0.1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data15 = new Data(10, 0.1, 0, 0.1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data16 = new Data(10, 0.01, 0, 0.1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data17 = new Data(10, 0.001, 0, 0.1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data18 = new Data(10, 0.0001, 0, 0.1, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data19 = new Data(10, 0.00001, 0, 0.01, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data20 = new Data(10, 1, 0, 0.01, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data21 = new Data(10, 0.1, 0, 0.01, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data22 = new Data(10, 0.01, 0, 0.01, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data23 = new Data(10, 0.001, 0, 0.01, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data24 = new Data(10, 0.0001, 0, 0.01, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data25 = new Data(10, 0.00001, 0, 0.001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data26 = new Data(10, 1, 0, 0.001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data27 = new Data(10, 0.1, 0, 0.001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data28 = new Data(10, 0.01, 0, 0.001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data29 = new Data(10, 0.001, 0, 0.001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data30 = new Data(10, 0.0001, 0, 0.001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data31 = new Data(10, 0.00001, 0, 0.0001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data32 = new Data(10, 1, 0, 0.0001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data33 = new Data(10, 0.1, 0, 0.0001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data34 = new Data(10, 0.01, 0, 0.0001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data35 = new Data(10, 0.001, 0, 0.0001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data36 = new Data(10, 0.0001, 0, 0.0001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data37 = new Data(10, 0.00001, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data38 = new Data(10, 1, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data39 = new Data(10, 0.1, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data40 = new Data(10, 0.01, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data41 = new Data(10, 0.001, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data42 = new Data(10, 0.0001, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                #endregion
                ListOfDatas = new List<Data>()
                {
                    data1, data2, data3, data4 , data5 , data6 , data7 , data8 , data9 , data10, data11,
                    data12, data13, data14, data15, data16, data17, data18, data19, data20, data21, data22, data23, data24, data25, data26,
                    data27, data28, data29, data30, data31, data32, data33, data34, data35, data36, data37, data38, data39, data40, data41,
                    data42
                };
                LargestData = ListOfDatas.OrderByDescending(w => w.Test_Accuracy).First();
                double learning_rate = LargestData.Learning_Rate; //0.0001
                double c = LargestData.C; //10
                Console.WriteLine("\n*******SVM over Trees (Part 5)*******");
                Console.WriteLine("The best hyperparameters are: \n\t" + "Learning Rate:\t" + learning_rate
                    + "\n\t" + "Regularization/Loss Tradeoff:\t" + c);
                dataTest = new Data(SVM_LR_Train_Data, SVM_LR_Test_Data, r, 20, learning_rate, 0, c, false, 0);
                Console.WriteLine("The total number of updates/mistakes for the best Weight and Bias: \n\t" + dataTest.BestWeightBias.Updates);
                Console.WriteLine("Training Set Accuracy: \n\t" + Math.Round(dataTest.Train_Accuracy, 3));
                Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest.Test_Accuracy, 3));
                Console.WriteLine("Average cross-validation Accuracy: \n\t" + Math.Round(LargestData.Test_Accuracy, 3));
                Console.WriteLine("---------------------------------------------------------------------------------------");
                #endregion

                #region Logistic Regression over Trees
                #region Datas
                data1 = new Data(10, 0.00001, 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data2 = new Data(10, 1, 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data3 = new Data(10, 0.1, 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data4 = new Data(10, 0.01, 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data5 = new Data(10, 0.001, 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data6 = new Data(10, 0.0001, 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data7 = new Data(10, 0.00001, 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data8 = new Data(10, 1, 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data9 = new Data(10, 0.1, 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data10 = new Data(10, 0.01, 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data11 = new Data(10, 0.001, 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data12 = new Data(10, 0.0001, 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data13 = new Data(10, 0.00001, 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data14 = new Data(10, 1, 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data15 = new Data(10, 0.1, 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data16 = new Data(10, 0.01, 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data17 = new Data(10, 0.001, 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data18 = new Data(10, 0.0001, 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data19 = new Data(10, 0.00001, 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data20 = new Data(10, 1, 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data21 = new Data(10, 0.1, 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data22 = new Data(10, 0.01, 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data23 = new Data(10, 0.001, 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data24 = new Data(10, 0.0001, 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data25 = new Data(10, 0.00001, 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data26 = new Data(10, 1, 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data27 = new Data(10, 0.1, 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data28 = new Data(10, 0.01, 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data29 = new Data(10, 0.001, 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data30 = new Data(10, 0.0001, 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data31 = new Data(10, 0.00001, 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data32 = new Data(10, 1, 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data33 = new Data(10, 0.1, 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data34 = new Data(10, 0.01, 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data35 = new Data(10, 0.001, 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                data36 = new Data(10, 0.0001, 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
                #endregion
                ListOfDatas = new List<Data>()
                {
                    data1, data2, data3, data4 , data5 , data6 , data7 , data8 , data9 , data10, data11,
                    data12, data13, data14, data15, data16, data17, data18, data19, data20, data21, data22, data23, data24, data25, data26,
                    data27, data28, data29, data30, data31, data32, data33, data34, data35, data36
                };

                LargestData = ListOfDatas.OrderByDescending(w => w.Test_Accuracy).First();
                learning_rate = LargestData.Learning_Rate; //0.01
                double tradeoff = LargestData.Tradeoff; //10000
                Console.WriteLine("\n*******Logistic Regression over Trees (Part 6)*******");
                Console.WriteLine("The best hyperparameters are: \n\t" + "Learning Rate:\t" + learning_rate
                    + "\n\t" + "Tradeoff:\t" + tradeoff);
                dataTest = new Data(SVM_LR_Train_Data, SVM_LR_Test_Data, r, 20, learning_rate, 0, 0, true, tradeoff);
                Console.WriteLine("The total number of updates/mistakes for the best Weight and Bias: \n\t" + dataTest.BestWeightBias.Updates);
                Console.WriteLine("Training Set Accuracy: \n\t" + Math.Round(dataTest.Train_Accuracy, 3));
                Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest.Test_Accuracy, 3));
                Console.WriteLine("Average cross-validation Accuracy: \n\t" + Math.Round(LargestData.Test_Accuracy, 3));
                Console.WriteLine("---------------------------------------------------------------------------------------");
                #endregion
            }
            #endregion

            #region Non-arguments
            //string startupPath = System.IO.Directory.GetCurrentDirectory();
            //Train = File.OpenText(startupPath + @"\speeches.train.liblinear");
            //Test = File.OpenText(startupPath + @"\speeches.test.liblinear");
            //Cross_1 = File.OpenText(startupPath + @"\training00.data");
            //Cross_2 = File.OpenText(startupPath + @"\training01.data");
            //Cross_3 = File.OpenText(startupPath + @"\training02.data");
            //Cross_4 = File.OpenText(startupPath + @"\training03.data");
            //Cross_5 = File.OpenText(startupPath + @"\training04.data");
            ////Train_Forest = File.OpenText(startupPath + @"\tree.train.data");
            ////Test_Forest = File.OpenText(startupPath + @"\tree.test.data");

            //#region Naive Bayes
            //data1 = new Data(2.0, r, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data2 = new Data(1.5, r, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data3 = new Data(1.0, r, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data4 = new Data(0.5, r, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //List<Data> ListOfDatas = new List<Data>() { data1, data2, data3, data4 };

            //Data LargestData = ListOfDatas.OrderByDescending(w => w.Test_Accuracy).First();
            //double smoothing_term = LargestData.Smoothing_Term;
            //Console.WriteLine("\n*******Naive Bayes (Part 3)*******");
            //Console.WriteLine("The best hyperparameter is: \n\t" + "Smoothing Term:\t" + smoothing_term);
            //Data DataTree = new Data(Train, Test, r, smoothing_term);
            //Console.WriteLine("Training Accuracy:\n\t" + DataTree.Train_Accuracy);
            //Console.WriteLine("Test Accuracy:\n\t" + DataTree.Test_Accuracy);
            //Console.WriteLine("Average cross-validation Accuracy: \n\t" + Math.Round(LargestData.Test_Accuracy, 3));
            //#endregion

            //#region Bagged Forest
            //int ForestSize = 1000;
            //DataTree = new Data(Train, Test, 3, r, ForestSize);

            //List<Entry> SVM_LR_Train_Data = new List<Entry>();
            //List<Entry> SVM_LR_Test_Data = new List<Entry>();
            //List<int> FinalPredictions_Train = new List<int>();
            //List<int> FinalPredictions_Test = new List<int>();
            //for (int i = 0; i < DataTree.Forest[0].Train_Predictions.Count; i++)
            //{
            //    List<int> helper_train = new List<int>();
            //    foreach (var tree in DataTree.Forest) //loops through each Tree
            //    {
            //        helper_train.Add(tree.Train_Predictions[i]);
            //    }
            //    int Most_Occured_Label_train = helper_train.GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).First();
            //    FinalPredictions_Train.Add(Most_Occured_Label_train);

            //    //This is needed for SVM and LR to get the training data
            //    Dictionary<int, double> vector = new Dictionary<int, double>();
            //    for (int j = 1; j < helper_train.Count + 1; j++)
            //    {
            //        if (helper_train[j - 1] == 1)
            //        {
            //            vector.Add(j, helper_train[j - 1]);
            //        }
            //    }
            //    SVM_LR_Train_Data.Add(new Entry(DataTree.Training_Data[i].Sign, vector));
            //}
            //for (int i = 0; i < DataTree.Forest[0].Test_Predictions.Count; i++)
            //{
            //    List<int> helper_test = new List<int>();
            //    foreach (var tree in DataTree.Forest) //loops through each Tree
            //    {
            //        helper_test.Add(tree.Test_Predictions[i]);
            //    }
            //    int Most_Occured_Label_test = helper_test.GroupBy(x => x).OrderByDescending(y => y.Count()).Select(z => z.Key).First();
            //    FinalPredictions_Test.Add(Most_Occured_Label_test); //This is adding in increasing order of the test set

            //    //This is needed for SVM and LR to get the test data
            //    Dictionary<int, double> vector = new Dictionary<int, double>();
            //    for (int j = 1; j < helper_test.Count + 1; j++)
            //    {
            //        if (helper_test[j - 1] == 1)
            //        {
            //            vector.Add(j, helper_test[j - 1]);
            //        }
            //    }
            //    SVM_LR_Test_Data.Add(new Entry(DataTree.Test_Data[i].Sign, vector));
            //}
            //int correct_labeling_train = 0;
            //int correct_labeling_test = 0;
            //for (int i = 0; i < DataTree.Training_Data.Count /*Train data size */; i++)
            //{
            //    if (DataTree.Training_Data[i].Sign == FinalPredictions_Train[i]) //correct labeling
            //    {
            //        correct_labeling_train++;
            //    }
            //}
            //for (int i = 0; i < DataTree.Test_Data.Count /*Test data size 940*/; i++)
            //{
            //    if (DataTree.Test_Data[i].Sign == FinalPredictions_Test[i]) //correct labeling
            //    {
            //        correct_labeling_test++;
            //    }
            //}
            //double Accuracy_Train = (Convert.ToDouble(correct_labeling_train) / FinalPredictions_Train.Count) * 100;
            //double Accuracy_Test = (Convert.ToDouble(correct_labeling_test) / FinalPredictions_Test.Count) * 100;
            //Console.WriteLine("\n*******Bagged Forest (Part 4)*******");
            //Console.WriteLine("Train Set Accuracy:\t" + Accuracy_Train);
            //Console.WriteLine("Test Set Accuracy:\t" + Accuracy_Test);
            //#endregion

            //#region Generating Data for SVM and LR
            //string to_file = "";
            //foreach (var item in SVM_LR_Train_Data)
            //{
            //    to_file += item.Sign + " ";
            //    foreach (var item2 in item.Vector)
            //    {
            //        to_file += item2.Key + ":" + item2.Value + " ";
            //    }
            //    to_file += Environment.NewLine;
            //}
            //File.WriteAllText(startupPath + @"\tree.train.data", to_file);

            //to_file = "";
            //foreach (var item in SVM_LR_Test_Data)
            //{
            //    to_file += item.Sign + " ";
            //    foreach (var item2 in item.Vector)
            //    {
            //        to_file += item2.Key + ":" + item2.Value + " ";
            //    }
            //    to_file += Environment.NewLine;
            //}
            //File.WriteAllText(startupPath + @"\tree.test.data", to_file);
            //#endregion

            //#region SVM over Trees
            //Data dataTest;
            //#region Datas
            //data1 = new Data(10, 0.00001, 0 , 10 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data2 = new Data(10, 1      , 0 , 10 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data3 = new Data(10, 0.1    , 0 , 10 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data4 = new Data(10, 0.01   , 0 , 10 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data5 = new Data(10, 0.001  , 0 , 10 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data6 = new Data(10, 0.0001 , 0 , 10 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data7 = new Data(10, 0.00001, 0 , 1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data8 = new Data(10, 1      , 0 , 1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data9 = new Data(10, 0.1    , 0 , 1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data10 = new Data(10, 0.01   , 0 , 1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data11 = new Data(10, 0.001  , 0 , 1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data12 = new Data(10, 0.0001 , 0 , 1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data13 = new Data(10, 0.00001, 0 , 0.1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data14 = new Data(10, 1      , 0 , 0.1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data15 = new Data(10, 0.1    , 0 , 0.1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data16 = new Data(10, 0.01   , 0 , 0.1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data17 = new Data(10, 0.001  , 0 , 0.1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data18 = new Data(10, 0.0001 , 0 , 0.1 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data19 = new Data(10, 0.00001, 0 , 0.01 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data20 = new Data(10, 1      , 0 , 0.01 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data21 = new Data(10, 0.1    , 0 , 0.01 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data22 = new Data(10, 0.01   , 0 , 0.01 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data23 = new Data(10, 0.001  , 0 , 0.01 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data24 = new Data(10, 0.0001 , 0 , 0.01 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data25 = new Data(10, 0.00001, 0 , 0.001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data26 = new Data(10, 1      , 0 , 0.001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data27 = new Data(10, 0.1    , 0 , 0.001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data28 = new Data(10, 0.01   , 0 , 0.001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data29 = new Data(10, 0.001  , 0 , 0.001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data30 = new Data(10, 0.0001 , 0 , 0.001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data31 = new Data(10, 0.00001, 0 , 0.0001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data32 = new Data(10, 1      , 0 , 0.0001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data33 = new Data(10, 0.1    , 0 , 0.0001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data34 = new Data(10, 0.01   , 0 , 0.0001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data35 = new Data(10, 0.001  , 0 , 0.0001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data36 = new Data(10, 0.0001 , 0 , 0.0001 , false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data37 = new Data(10, 0.00001, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data38 = new Data(10, 1, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data39 = new Data(10, 0.1, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data40 = new Data(10, 0.01, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data41 = new Data(10, 0.001, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data42 = new Data(10, 0.0001, 0, 0.00001, false, 0, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //#endregion
            //ListOfDatas = new List<Data>()
            //    {
            //        data1, data2, data3, data4 , data5 , data6 , data7 , data8 , data9 , data10, data11,
            //        data12, data13, data14, data15, data16, data17, data18, data19, data20, data21, data22, data23, data24, data25, data26,
            //        data27, data28, data29, data30, data31, data32, data33, data34, data35, data36, data37, data38, data39, data40, data41,
            //        data42
            //    };
            //LargestData = ListOfDatas.OrderByDescending(w => w.Test_Accuracy).First();
            //double learning_rate = LargestData.Learning_Rate; //0.0001
            //double c = LargestData.C; //10
            //Console.WriteLine("\n*******SVM over Trees (Part 5)*******");
            //Console.WriteLine("The best hyperparameters are: \n\t" + "Learning Rate:\t" + learning_rate
            //    + "\n\t" + "Regularization/Loss Tradeoff:\t" + c);
            //dataTest = new Data(SVM_LR_Train_Data, SVM_LR_Test_Data, r, 20, learning_rate, 0, c, false, 0);
            //Console.WriteLine("The total number of updates/mistakes for the best Weight and Bias: \n\t" + dataTest.BestWeightBias.Updates);
            //Console.WriteLine("Training Set Accuracy: \n\t" + Math.Round(dataTest.Train_Accuracy, 3));
            //Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest.Test_Accuracy, 3));
            //Console.WriteLine("Average cross-validation Accuracy: \n\t" + Math.Round(LargestData.Test_Accuracy, 3));
            //Console.WriteLine("---------------------------------------------------------------------------------------");
            //#endregion

            //#region Logistic Regression over Trees
            //#region Datas
            //data1 = new Data(10, 0.00001, 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data2 = new Data(10, 1      , 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data3 = new Data(10, 0.1    , 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data4 = new Data(10, 0.01   , 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data5 = new Data(10, 0.001  , 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data6 = new Data(10, 0.0001 , 0, 0, true, 0.1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data7 = new Data(10, 0.00001, 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data8 = new Data(10, 1      , 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data9 = new Data(10, 0.1    , 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data10 = new Data(10, 0.01   , 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data11 = new Data(10, 0.001  , 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data12 = new Data(10, 0.0001 , 0, 0, true, 1, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data13 = new Data(10, 0.00001, 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data14 = new Data(10, 1      , 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data15 = new Data(10, 0.1    , 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data16 = new Data(10, 0.01   , 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data17 = new Data(10, 0.001  , 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data18 = new Data(10, 0.0001 , 0, 0, true, 10, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data19 = new Data(10, 0.00001, 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data20 = new Data(10, 1      , 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data21 = new Data(10, 0.1    , 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data22 = new Data(10, 0.01   , 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data23 = new Data(10, 0.001  , 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data24 = new Data(10, 0.0001 , 0, 0, true, 100, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data25 = new Data(10, 0.00001, 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data26 = new Data(10, 1      , 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data27 = new Data(10, 0.1    , 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data28 = new Data(10, 0.01   , 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data29 = new Data(10, 0.001  , 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data30 = new Data(10, 0.0001 , 0, 0, true, 1000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data31 = new Data(10, 0.00001, 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data32 = new Data(10, 1      , 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data33 = new Data(10, 0.1    , 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data34 = new Data(10, 0.01   , 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data35 = new Data(10, 0.001  , 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //data36 = new Data(10, 0.0001 , 0, 0, true, 10000, r, SVM_LR_Train_Data, SVM_LR_Test_Data);
            //#endregion
            //ListOfDatas = new List<Data>()
            //    {
            //        data1, data2, data3, data4 , data5 , data6 , data7 , data8 , data9 , data10, data11,
            //        data12, data13, data14, data15, data16, data17, data18, data19, data20, data21, data22, data23, data24, data25, data26,
            //        data27, data28, data29, data30, data31, data32, data33, data34, data35, data36
            //    };

            //LargestData = ListOfDatas.OrderByDescending(w => w.Test_Accuracy).First();
            //learning_rate = LargestData.Learning_Rate; //0.01
            //double tradeoff = LargestData.Tradeoff; //10000
            //Console.WriteLine("\n*******Logistic Regression over Trees (Part 6)*******");
            //Console.WriteLine("The best hyperparameters are: \n\t" + "Learning Rate:\t" + learning_rate
            //    + "\n\t" + "Tradeoff:\t" + tradeoff);
            //dataTest = new Data(SVM_LR_Train_Data, SVM_LR_Test_Data, r, 20, learning_rate, 0, 0, true, tradeoff);
            //Console.WriteLine("The total number of updates/mistakes for the best Weight and Bias: \n\t" + dataTest.BestWeightBias.Updates);
            //Console.WriteLine("Training Set Accuracy: \n\t" + Math.Round(dataTest.Train_Accuracy, 3));
            //Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest.Test_Accuracy, 3));
            //Console.WriteLine("Average cross-validation Accuracy: \n\t" + Math.Round(LargestData.Test_Accuracy, 3));
            //Console.WriteLine("---------------------------------------------------------------------------------------");
            //#endregion
           
            //Console.ReadKey(false);
            #endregion
        }

        //static decimal helper(double P, double N)
        //{
        //    return decimal.Parse(((-P * Math.Log(P, 2)) - (N * Math.Log(N, 2))).ToString());
        //}
    }


}