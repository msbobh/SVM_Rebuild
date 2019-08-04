using System.IO;

namespace mystrings
{
    class MyStrings
    {
        public const string usage = "Usage: TrainSVM [kernel 0-4] <training file> <test file>\n" +
            "0 = Linear\n" +
            "1 = Polynomial\n" +
            "2 = RBF\n" +
            "3 = Sigmoid";
        public const string File_error = "Error file: {0} not found";
        public const string Label_open_error = " Label file {0} does not exist";
        public const string Accuracy = " Training accuracy is {0}%";
        public const string Linear = "Linear";
        public const string Polynomial = "Polynomial";
        public const string RBF = "RBF";
        public const string Sigmoid = "Sigmoid";
        public const string Parameters = "C = {0}, Gamma = {1}, Degree = {2}, r = {3}";
        public const string TrainingFileFormat = "Training file {0} is not in the correct format";

    }
   
}