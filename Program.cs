using System;
using System.IO;
using mystrings;
using Functions;
using libsvm;
using System.Collections;

namespace TrainSVM
{
    class Program
    {
        
        static void Main(string[] args)
        {

            bool kernelparam = false;
            int numberofArgs = args.Length;
            string inputmatrix;
            string path = Directory.GetCurrentDirectory();
            string save_model_name;
            string kerneltype;
            string testfile;
            /* SVM specific initializations
             */
            int degree = 3; // default for none specified
            int r = 1;
            // C and gamma come from using grid.py on the training set resume.mat 982 x 7768
            double C = 2.0;
            double gamma = 0.001953125; // used for Radial Basis Function Kernel (RBF)
            C_SVC svm; // setup the default variable for the SVM

            /* 
             * Three parameters are required, kernel selection, training file and test file
             */

            if (args.Length != 3)
            {
                Console.WriteLine(MyStrings.usage);
                System.Environment.Exit(1);
            }

            if (kernelparam = Int32.TryParse(args[0], out int kernelchoice)&& kernelchoice <=3)
            {
                //Legal value for kernelchoice are 0-3
                //kernelchoice = 1;
            }
            else
            {
                // Not a legal kernel selection
                Console.WriteLine(MyStrings.usage);
                System.Environment.Exit(1);
            }
            inputmatrix = args[1];
            testfile = args[2];
            if (!HelperFunctions.CheckFormat(inputmatrix))
            {
                Console.WriteLine(MyStrings.TrainingFileFormat,inputmatrix);
                System.Environment.Exit(1);
            }
            if (!File.Exists(testfile))
            {
                Console.WriteLine(MyStrings.File_error, inputmatrix);
                System.Environment.Exit(1);
            }

            // Train the SVM

            switch (kernelchoice)
            {
                case 0:
                     svm = new C_SVC(inputmatrix, KernelHelper.LinearKernel(), C);
                     kerneltype = MyStrings.Linear;
                     break;
                case 1:
                    svm = new C_SVC(inputmatrix, KernelHelper.PolynomialKernel(degree, gamma, r), C);
                    kerneltype = MyStrings.Polynomial;
                    break;
                case 2:
                    svm = new C_SVC(inputmatrix, KernelHelper.RadialBasisFunctionKernel(gamma), C);
                    kerneltype = MyStrings.RBF;
                    break;
                case 3:
                    svm = new C_SVC(inputmatrix, KernelHelper.SigmoidKernel(gamma, r),C);
                    kerneltype = MyStrings.Sigmoid;
                    break;
                default:
                    svm = new C_SVC(inputmatrix, KernelHelper.LinearKernel(), C);
                    kerneltype = MyStrings.Linear;
                    break;
            }

            // var accuracy = svm.GetCrossValidationAccuracy(5);
            save_model_name = String.Concat(inputmatrix, ".model");
            svm.Export(save_model_name);
            var predfile = ProblemHelper.ReadProblem(testfile);
            double result = HelperFunctions.PredictTestSet(testfile, svm);
            
            Console.WriteLine(MyStrings.Accuracy, Math.Round(result * 100,2));
            Console.Write("SVM kernel type {0}      ", kerneltype);
            Console.WriteLine(MyStrings.Parameters, C, gamma, degree,r);
           
        }


        

        /* "." means every 1,000 iterations (or every #data iterations is your #data is less than 1,000).
            "*" means that after iterations of using a smaller shrunk problem, we reset to use the whole set. */
        /*  optimization finished, #iter = 219 
            nu = 0.431030 
            obj = -100.877286, rho = 0.424632 
            nSV = 132, nBSV = 107 
            Total nSV = 132
            obj is the optimal objective value of the dual SVM problem. rho is the bias term in the decision
            function sgn(w^Tx - rho). nSV and nBSV are number of support vectors and bounded support vectors
            (i.e., alpha_i = C). nu-svm is a somewhat equivalent form of C-SVM where C is replaced by nu.
            nu simply shows the corresponding parameter.
        */

        /* if a kernel is specified on the command line, then select the corresponding kernel for training the SVM as follows
         * 0 = linear
         * 1 = polynomial
         * 2 = RBF
         * 3 = sigmoid
         * 4 = precomputed
         */



        
        
            

    }

        
   
}
