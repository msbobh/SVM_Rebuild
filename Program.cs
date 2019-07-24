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
            bool properformat = false;
            bool needsFormatting = false;
            bool done = false;
            int vectorlength; // number of features
            int kernelchoice; // integer representation of selected kernel
            int numberofArgs = args.Length;
            string inputmatrix, labelfile;
            string savefilename = null;
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

            // lets see if we have a kernel param
            if (kernelparam = Int32.TryParse(args[0], out kernelchoice))
            {
                //Legal value for kernelchoice are 0-3
                if (kernelchoice > 3)
                {
                    Console.WriteLine(MyStrings.usage);
                    System.Environment.Exit(1);
                }
                    
            if (kernelparam && numberofArgs == 1)
            {
                // Not a legal command line
                Console.WriteLine(MyStrings.usage);
                System.Environment.Exit(1);

            }
            switch (numberofArgs)
            {
                case 1:
                    needsFormatting = HelperFunctions.CheckFormat(args[0]);
                    if (needsFormatting)
                    {
                        Console.WriteLine(MyStrings.usage);
                        System.Environment.Exit(1);
                    }
                    break;

                case 2:
                    needsFormatting = HelperFunctions.CheckFormat(args[1]);
                    if (kernelparam && needsFormatting)
                    {
                        Console.WriteLine(MyStrings.usage);
                        System.Environment.Exit(1);
                    }
                    else
                    {
                        inputmatrix = args[1];
                        
                    }
                    break;

                case 3:
                        if (kernelparam && (needsFormatting = HelperFunctions.CheckFormat(args[1])))
                        {
                            inputmatrix = args[1];
                            labelfile = args[2];
                        }
                        else
                        {
                            inputmatrix = args[1];
                            testfile = args[2];
                        }
                        break;

                    case 4: // kernel matrix labels training or kernel matrix training
                        if (needsFormatting = HelperFunctions.CheckFormat(args[1]))
                        {
                            inputmatrix = args[1];
                            labelfile = args[2];
                            testfile = args[3]
                        }
                        else
                        {
                            Console.WriteLine(MyStrings.usage);
                            System.Environment.Exit(1);
                        }

                        break;

            }
            if (numberofArgs == 1)
                inputmatrix = args[0];
            else
                inputmatrix = args[1];
            // stopped here on Tuesday evening 7/23
            /*
             * **********************************
             * bunch of stuff that does not work well below this
             * **********************************
             */

            
                if (numberofArgs < 1 )
            {
                Console.WriteLine(MyStrings.usage);
                System.Environment.Exit(1);
            } // Exit if no params passed on the command line

            /* At least one command line parameter we can continue, but it can't be an int.
             * so check for that next.
             */
            if (numberofArgs == 1 && Int32.TryParse(args[0], out kernelchoice))
             {
                Console.WriteLine(MyStrings.usage); // single paramater can't be int
                System.Environment.Exit(1);
             }
            else 
            if (numberofArgs == 1) // Assume the only argument is the file name and check if it needs formatting, if no reformat is requied we can train and save the model
            {
                kernelparam = false;
                properformat = HelperFunctions.CheckFormat(args[0]);
                inputmatrix = args[0];
                savefilename = inputmatrix.Replace(".mat", ".svm"); // update the suffix
                svm = new C_SVC(savefilename, KernelHelper.LinearKernel(), C);
                save_model_name = savefilename.Replace(".svm", ".model");
                svm.Export(save_model_name);
                done = true;

            }

            if (numberofArgs >= 1)
            {
                if (Int32.TryParse(args[0], out kernelchoice))
                {
                    kernelparam = true;

                    switch (numberofArgs)
                    {
                        case 2:
                            needsFormatting = !HelperFunctions.CheckFormat(args[1]);
                            inputmatrix = args[1];
                            if (needsFormatting)
                            {
                                Console.WriteLine("Missing label file");
                                System.Environment.Exit(1);
                            }
                            break;

                        case 3:
                            needsFormatting = HelperFunctions.CheckFormat(args[1]);
                            inputmatrix = args[1];
                            labelfile = args[2];
                            break;

                        case 4:
                            needsFormatting = HelperFunctions.CheckFormat(args[1]);
                            inputmatrix = args[1];
                            labelfile = args[2];
                            testfile = args[3];
                            break;

                        default:

                            Console.WriteLine("too many parameters");
                            Console.WriteLine(MyStrings.usage);
                            System.Environment.Exit(1);
                            break;
                    }

                }
                
            }
            // *********************** not sure taht i need this
            inputmatrix = args[1];
            if (inputmatrix.Contains(".mat"))
            {
                savefilename = inputmatrix.Replace(".mat", ".svm"); // update the suffix
            }
            
            if (!done && needsFormatting && args.Length >= 2)
            {
                inputmatrix = args[1];
                labelfile = args[2];
                vectorlength = HelperFunctions.VectorLength(inputmatrix); // Get the number of features
                string[] labels = new string[HelperFunctions.SampleSize(labelfile)]; // Calculate the number of labels and use to create storage

                /* if the input matrix is not already in the correct format Call reformat function
                * result is that a file is written that is the LIBSVM format, expects the 
                * labels to be in a separate file
                *
                * Reformatdata(string[] data, string labels, string fname)
                * 
                */
                savefilename = String.Concat(inputmatrix, ".svm");
                HelperFunctions.Reformatdata(inputmatrix, labels, savefilename, vectorlength);

            }


            // Train the SVM

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

            // 7/23/19 fix up save file name, kernelchoice does not seem to be in the rigth place, also logic flow thru above switch and if statements needs some review

            Int32.TryParse(args[0], out kernelchoice);
            if (!savefilename.Contains("*.svm"))
            {
                savefilename = String.Concat(inputmatrix, ".svm");
            }


            if (kernelparam)
            {
                int caseSwitch = kernelchoice;
                switch (caseSwitch)
                {
                    case 0:
                        svm = new C_SVC(savefilename, KernelHelper.LinearKernel(), C);
                        kerneltype = "Linear";
                        break;
                    case 1:
                        svm = new C_SVC(savefilename, KernelHelper.PolynomialKernel(degree, gamma, r), C);
                        kerneltype = "Polynomial";
                        break;
                    case 2:
                        svm = new C_SVC(savefilename, KernelHelper.RadialBasisFunctionKernel(gamma), C);
                        kerneltype = "RBF";
                        break;
                    default:
                        svm = new C_SVC(savefilename, KernelHelper.LinearKernel(), C);
                        kerneltype = "Linear";
                        break;
                }

            }
            else
            {
                svm = new C_SVC(savefilename, KernelHelper.LinearKernel(), C);
                kerneltype = "Linear";
            }

            // For RBF kernel, linear kernel would be KernelHelper.LinearKernel
            // 
            // var accuracy = svm.GetCrossValidationAccuracy(5);
            save_model_name = savefilename.Replace(".svm", ".model");
            svm.Export(save_model_name);
            /*
             * ********** Stoppted here for checking file input formats
             */


            //double accuracy = svm.Predict(testfile);
            //Console.WriteLine(MyStrings.Accuracy, accuracy * 100);
            Console.WriteLine("SVM kernel type {0}", kerneltype);
            

        }

        
    }
}
