using System.IO;

namespace mystrings
{
    class MyStrings
    {
        public const string usage = "Usage: TrainSVM [optional] kernel [0-4]:" +
            "0 = Linear" +
            "1 = Polynomial" +
            "2 = RBF" +
            "3 = Sigmoid <source file> <label file> <test file>";
        public const string File_error = "Error file: {0} not found";
        public const string Label_open_error = " Label file {0} does not exist";
        public const string Accuracy = " Cross Validatin Accuracy is {0}";
        
    }
   
}