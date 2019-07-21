using System;
using System.IO;
using mystrings;

namespace Functions
{
    class HelperFunctions
    {
        static public void Reformatdata(string matrix, string[] labels, string fname, int vectorlength)
        
        /* The pair <index>:<value> gives a feature (attribute) value: <index> is
           an integer starting from 1 and <value> is a real number. The only
           exception is the precomputed kernel, where <index> starts from 0; see
           the section of precomputed kernels. Indices must be in ASCENDING
           order.*/
        {
            // Local variables
            int labelindex = 0;
            string[] data = new string[vectorlength - 1];
            string[] build = new string[vectorlength];
            string compressed; // used to hold the vector after stripping out the spaces
            data = File.ReadAllLines(matrix); // Read in training data

            // Create the output file
            StreamWriter outfile = null;
            try { outfile = new StreamWriter(fname); }
            catch (Exception e)
            {
                Console.WriteLine(MyStrings.File_error, e);
                System.Environment.Exit(0);
            }

            foreach (var row in data)   // Process one row at a time
            {
                if (labels[labelindex] == "0")  // LibSVM maps 0's to -1 and 1's to 1
                {
                    outfile.Write("-1 ");
                }
                else
                {
                    outfile.Write("1 ");
                }
                // Write out the label at the beginning of the row, then strip out spaces and reduce the vector by a factor of 2
                for (int i = 1; i <= (vectorlength - 1) / 2; i++)
                {
                    compressed = row.Replace(" ", "");
                    outfile.Write("{0}:{1} ", i, compressed[i]); // step through the row and add index and ":"
                }
                outfile.WriteLine();
                labelindex++;

            }
            outfile.Close();
            return;
        } // Done procecssing rows

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

        
        static public int VectorLength(string vectorfile)
        {
            /* Get the size of the feature vector from the data so we can set it automatically */
            string line;
            int Count = 0;
            StreamReader file = new StreamReader(vectorfile);
            line = file.ReadLine();
            Count = line.Length / 2; // Length returns the number of spaces as well as ints
            return (Count);
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
        
    }
}