using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Diagnostics;

namespace NotLinearCancerModel
{
    static class ActionDataFile
    {
        public static MVVM.View.CalculateOneView CalculateOneView
        {
            get => default;
            set
            {
            }
        }

        public static MVVM.View.HomeView HomeView
        {
            get => default;
            set
            {
            }
        }

        internal static DataCancer DataCancer
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// Actions to write, read and do everything with data in program and files
        /// </summary>
        /// 
        static public string writeParametersToFile(string type, int number, Dictionary<string, float> cancerParameters, string pathToSave = @"..\..\..\dataTumor\PredictData\PersonalPatients\")
        {
            string message = "Ok";           
            pathToSave += type + @"\txt\params\" + (number + 1).ToString() + @"Params.txt";
            try
            {
                StreamWriter outputFile = new StreamWriter(pathToSave);
                foreach (var singleParam in cancerParameters)
                {
                    outputFile.WriteLine(string.Format("{0}\t{1}", singleParam.Key, singleParam.Value));
                }
                outputFile.Close();
            }
            catch (FileNotFoundException e)
            {
                message = $"The file was not found: '{e}'";
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }
            catch (DirectoryNotFoundException e)
            {
                message = $"The directory was not found: '{e}'";
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }
            catch (IOException e)
            {
                message = $"The file could not be opened: '{e}'";
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }
            return message;
        }
        

        static public string writeTimeValueToFile(string type, int number, float[] tValues, float[] cancerValues, string pathToSave = @"..\..\..\dataTumor\PredictData\PersonalPatients\")
        {
            string message = "Ok";
            pathToSave += type + @"\timeValue\txt\" + (number + 1).ToString() + type + @".txt";
            
            try
            {
                StreamWriter outputFile = new StreamWriter(pathToSave);
                for (int i = 0; i < tValues.Length; i++)
                {
                    outputFile.WriteLine(string.Format("{0}\t{1}", tValues[i], cancerValues[i]));
                }
                outputFile.Close();
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }
            catch (FileNotFoundException e)
            {
                message = $"The file was not found: '{e}'";
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }
            catch (DirectoryNotFoundException e)
            {
                message = $"The directory was not found: '{e}'";
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }
            catch (IOException e)
            {
                message = $"The file could not be opened: '{e}'";
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }

            return message;
        }


        static public string writeDataToFile(string type, int number, double[,,] data, string pathToSave = @"..\..\..\dataTumor\PredictData\PersonalPatients\")
        {
            string message = "Ok";
            pathToSave += type + @"\txt\" + (number + 1).ToString() + type + @".txt";
            try
            {
                StreamWriter outputFile = new StreamWriter(pathToSave);
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    for (int j = 0; j < data.GetLength(1); j++)
                    {
                        for (int k = 0; k < data.GetLength(2); k++)
                        {
                            outputFile.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", i, j, k, data[i, j, k]));
                        }
                    }
                }
                outputFile.Close();
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }
            catch (FileNotFoundException e)
            {
                message = $"The file was not found: '{e}'";
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }
            catch (DirectoryNotFoundException e)
            {
                message = $"The directory was not found: '{e}'";
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }
            catch (IOException e)
            {
                message = $"The file could not be opened: '{e}'";
                System.Diagnostics.Debug.WriteLine(message);
                Console.WriteLine(message);
            }

            return message;
        }


        static public List<List<float>> getDataFromFile(string type, int number, string pathToFile = @"..\..\..\dataTumor\ModelData\personalPatients\poly3current\")
        {
            List<float> xValues = new List<float>();
            List<float> yValues = new List<float>();
            List<List<float>> xyValues = new List<List<float>>()
            {
                xValues,
                yValues
            };
            pathToFile = pathToFile + type + @"\txt\" + number.ToString() + type + @".txt";
            
            // Read the file and display it line by line.
            try
            {
                foreach (string line in System.IO.File.ReadLines(pathToFile))
                {
                string[] valuesSingleXY = line.Split("\t");
                    xyValues[0].Add(float.Parse(valuesSingleXY[0].Trim(), CultureInfo.InvariantCulture));
                    xyValues[1].Add(float.Parse(valuesSingleXY[1].Trim(), CultureInfo.InvariantCulture));
                }
            }
            catch (FileNotFoundException e)
            {
                System.Diagnostics.Debug.WriteLine($"The file was not found: '{e}'");
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (DirectoryNotFoundException e)
            {
                System.Diagnostics.Debug.WriteLine($"The directory was not found: '{e}'");
                Console.WriteLine($"The directory was not found: '{e}'");
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine($"The file could not be opened: '{e}'");
                Console.WriteLine($"The file could not be opened: '{e}'");
            }

            return xyValues;
        }


        static public Dictionary<string, float> getParametersFromFile(string type, int number, string pathToRead = @"..\..\..\dataTumor\PredictData\PersonalPatients\")
        {
            Dictionary<string, float> cancerParameters = new Dictionary<string, float> ();
            string singleString = "";
            var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ",";
            try
            {
                StreamReader inputFile = new StreamReader(pathToRead);
                while((singleString = inputFile.ReadLine()) != null)
                {
                    string[] splitSingleString = singleString.Split("\t");
                    cancerParameters.Add(splitSingleString[0].Trim(), float.Parse(splitSingleString[1].Trim(), ci));
                }
            }
            catch (FileNotFoundException e)
            {
                System.Diagnostics.Debug.WriteLine($"The file was not found: '{e}'");
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (DirectoryNotFoundException e)
            {
                System.Diagnostics.Debug.WriteLine($"The directory was not found: '{e}'");
                Console.WriteLine($"The directory was not found: '{e}'");
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine($"The file could not be opened: '{e}'");
                Console.WriteLine($"The file could not be opened: '{e}'");
            }
            return cancerParameters;
        }
    }
}
