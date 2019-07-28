using System;
using System.IO;
using mystrings;
using libsvm;

namespace Functions
{
    class HelperFunctions
    {
        // Done procecssing rows

        static public int SampleSize(string fname)
        {
            /* Returns the number of samples in a training matrix
             */
            var samples = 0;
            using (var reader = File.OpenText(fname))
            {
                while (reader.ReadLine() != null)
                {
                    samples++;
                }
            }
            return samples;
        }

        
        

        static public string CommandLineParams(string[] args)
        {
            string kernelchoice;
            int kernelSpecifier = 2;
            bool option = false;

            if( args.Length <= 4) { return "none"; } // 5 means no kernel choice
            else
            if (args.Length == 4)
            {
                option = true; // must specify train, label test file
            }
            Int32.TryParse(args[0], out kernelSpecifier); // Covert string represenatin of a number intoa 32 bit int

            int caseSwitch = kernelSpecifier;
            switch (caseSwitch)
            {
                case 0:
                    kernelchoice = "Linear";
                    break;
                case 1:
                    kernelchoice = "Polynomial";
                    break;
                case 2:
                    kernelchoice = "RBF";
                    break;
                default:
                    kernelchoice = "Linear";
                    break;
            }
            return kernelchoice;
        }

        static public bool CheckFormat(string filename)
        {
            bool SVMFormat = false;
            string[] input;
            if (File.Exists(filename))
            {
                input = File.ReadAllLines(filename);
                foreach ( string line in input)
                {
                    if ((line.Contains("+") || line.Contains("-")) && (line.Contains("1") && line.Contains(":"))) // if each line contains expected format let's call it good
                    {
                        SVMFormat = true;
                    }
                }
            }
            else
            {
                Console.WriteLine(MyStrings.File_error, filename);
                System.Environment.Exit(1);
            }

            return SVMFormat;
        }

        static public double PredictTestSet(string inputfile, C_SVC svm)
        {
            /* Given a test set "Inputfile" and previoulsy trained SVM calculates the accuracty of the
             * the trained SVM. Fucntion returns the percent correct.
             */

            int i;
            double total = 1;
            var predfile = ProblemHelper.ReadProblem(inputfile); // Reads in the SVM format file and results in a svm_problem format
            double expectedValue = 0;

            for (i = 0; i < predfile.l; i++)
            {
                var x = predfile.x[i];                  // x is the ith vector sample
                expectedValue = predfile.y[i];
                var predictedValue = svm.Predict(x);    // Make label prediciton 
                if (predictedValue == expectedValue)    // Compare the prediction with actual 
                {
                    total++;
                }
            }
            double result = ((double)total / (double)i);    // Calculate the accuracy and return
            return result;
        }

    }
}