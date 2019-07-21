using System;
using libsvm;



namespace SVMSupport
{
    class Predictions 
    {

        static public double PredictTestSet (string inputfile, C_SVC svm)
        {
            /* Given a test set "Inputfile" and previoulsy trained SVM calculates the accuracty of the
             * the trained SVM. Fucntion returns the percent correct.
             */

            int i;
            double total = 1;
            var predfile = ProblemHelper.ReadProblem(inputfile);
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