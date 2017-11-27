#set -o nounset
mcs Code/SVM/*.cs
mv Code/SVM/AccuracyWB.exe Code/SVM/Program.exe
mcs Code/Logistic_Regression/*.cs
mv Code/Logistic_Regression/AccuracyWB.exe Code/Logistic_Regression/Program.exe
mcs Code/Bagged_Forest/*.cs
mv Code/Bagged_Forest/AccuracyWB.exe Code/Bagged_Forest/Program.exe
mono Code/SVM/Program.exe Data_Files/speeches.train.liblinear Data_Files/speeches.test.liblinear Data_Files/training00.data Data_Files/training01.data Data_Files/training02.data Data_Files/training03.data Data_Files/training04.data

mono Code/Logistic_Regression/Program.exe Data_Files/speeches.train.liblinear Data_Files/speeches.test.liblinear Data_Files/training00.data Data_Files/training01.data Data_Files/training02.data Data_Files/training03.data Data_Files/training04.data

mono Code/Bagged_Forest/Program.exe Data_Files/speeches.train.liblinear Data_Files/speeches.test.liblinear 
#rm Code/Program.exe
exit 0
