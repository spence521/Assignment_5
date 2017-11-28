using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms.DataVisualization.Charting;

namespace Assignement_2
{
    public class Program
    {
        static void Main(string[] args)
        {
            #region Datas
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
            #endregion
            StreamReader Train;
            StreamReader Test;
            StreamReader Cross_1;
            StreamReader Cross_2;
            StreamReader Cross_3;
            StreamReader Cross_4;
            StreamReader Cross_5;
            Random r = new Random(0);
            #region Arguments
            if (args.Length == 0)
            {
                System.Console.WriteLine("Please enter a file argument.");
                return;
            }
            else if (args.Length > 6)
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

                #region Part 2 (Logistic Regression)
                #region Datas
                data1 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data2 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data3 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data4 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data5 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data6 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data7 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data8 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data9 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data10 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data11 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data12 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data13 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data14 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data15 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data16 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data17 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data18 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data19 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data20 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data21 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data22 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data23 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data24 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data25 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data26 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data27 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data28 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data29 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data30 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data31 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data32 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data33 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data34 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data35 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                data36 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
                #endregion
                List<Data> ListOfDatas = new List<Data>()
                {
                    data1, data2, data3, data4 , data5 , data6 , data7 , data8 , data9 , data10, data11,
                    data12, data13, data14, data15, data16, data17, data18, data19, data20, data21, data22, data23, data24, data25, data26,
                    data27, data28, data29, data30, data31, data32, data33, data34, data35, data36
                };

                Data LargestData = ListOfDatas.OrderByDescending(w => w.Accuracy).First();
                double learning_rate = LargestData.Learning_Rate; //0.01
                double tradeoff = LargestData.Tradeoff; //10000
                Console.WriteLine("\n*******Logistic Regression (Part 2)*******");
                Console.WriteLine("The best hyperparameters are: \n\t" + "Learning Rate:\t" + learning_rate
                    + "\n\t" + "Tradeoff:\t" + tradeoff);
                Data dataTest = new Data(Train, Test, r, 20, learning_rate, 0, 0, true, tradeoff);
                Console.WriteLine("The total number of updates/mistakes for the best Weight and Bias: \n\t" + dataTest.BestWeightBias.Updates);
                Console.WriteLine("Training Set Accuracy: \n\t" + Math.Round(dataTest.Training_Accuracy, 3));
                Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest.Accuracy, 3));
                Console.WriteLine("Average cross-validation Accuracy: \n\t" + Math.Round(LargestData.Accuracy, 3));
                Console.WriteLine("---------------------------------------------------------------------------------------");
                #endregion

            }
            #endregion

            #region Non-Arguments
            //string startupPath = System.IO.Directory.GetCurrentDirectory();

            //Train = File.OpenText(startupPath + @"\speeches.train.liblinear");
            //Test = File.OpenText(startupPath + @"\speeches.test.liblinear");
            //Cross_1 = File.OpenText(startupPath + @"\training00.data");
            //Cross_2 = File.OpenText(startupPath + @"\training01.data");
            //Cross_3 = File.OpenText(startupPath + @"\training02.data");
            //Cross_4 = File.OpenText(startupPath + @"\training03.data");
            //Cross_5 = File.OpenText(startupPath + @"\training04.data");
            
            #region Part 2 (Logistic Regression)
            //#region Datas
            //data1 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data2 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data3 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data4 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data5 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data6 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 0.1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data7 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data8 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data9 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data10 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data11 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data12 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 1, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data13 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data14 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data15 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data16 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data17 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data18 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 10, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data19 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data20 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data21 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data22 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data23 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data24 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 100, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data25 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data26 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data27 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data28 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data29 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data30 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 1000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data31 = new Data(10, 10     /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data32 = new Data(10, 1      /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data33 = new Data(10, 0.1    /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data34 = new Data(10, 0.01   /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data35 = new Data(10, 0.001  /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //data36 = new Data(10, 0.0001 /*Learning Rate*/, 0, 0, true, 10000, r, Train, Test, Cross_1, Cross_2, Cross_3, Cross_4, Cross_5);
            //#endregion
            //List<Data> ListOfDatas = new List<Data>()
            //    {
            //        data1, data2, data3, data4 , data5 , data6 , data7 , data8 , data9 , data10, data11,
            //        data12, data13, data14, data15, data16, data17, data18, data19, data20, data21, data22, data23, data24, data25, data26,
            //        data27, data28, data29, data30, data31, data32, data33, data34, data35, data36
            //    };

            //Data LargestData = ListOfDatas.OrderByDescending(w => w.Accuracy).First();
            //double learning_rate = LargestData.Learning_Rate; //0.01
            //double tradeoff = LargestData.Tradeoff; //10000
            //Console.WriteLine("\n*******Logistic Regression (Part 2)*******");
            //Console.WriteLine("The best hyperparameters are: \n\t" + "Learning Rate:\t" + learning_rate
            //    + "\n\t" + "Tradeoff:\t" + tradeoff);
            //Data dataTest = new Data(Train, Test, r, 20, learning_rate, 0, 0, true, tradeoff);
            //Console.WriteLine("The total number of updates/mistakes for the best Weight and Bias: \n\t" + dataTest.BestWeightBias.Updates);
            //Console.WriteLine("Training Set Accuracy: \n\t" + Math.Round(dataTest.Training_Accuracy, 3));
            //Console.WriteLine("Test Set Accuracy: \n\t" + Math.Round(dataTest.Accuracy, 3));
            //Console.WriteLine("---------------------------------------------------------------------------------------");
            #endregion

            //Console.ReadKey(false);
            #endregion
        }
    }
}
