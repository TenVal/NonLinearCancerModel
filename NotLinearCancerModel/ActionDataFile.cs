﻿using System;
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
        static public string writeParametersToFile(Dictionary<string, float> cancerParameters, string pathToSave = @"dataTumor\PredictData\PersonalPatients\")
        {
            string message = "Ok";
            //pathToSave += type + @"\txt\params\" + (number + 1).ToString() + @"Params.txt";

            Debug.WriteLine($"\nwriteParametersToFile\t{pathToSave}");
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
        

        static public string writeTimeValueToFile(float[] tValues,
                                                float[] cancerValues,
                                                string pathToSave = @"dataTumor\PredictData\PersonalPatients\")
        {
            string message = "Ok";
            Debug.WriteLine($"\nwriteTimeValueToFile\t{pathToSave}");
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


        static public string writeDataToFile(double[,,] data,  string pathToSave = @"dataTumor\PredictData\PersonalPatients\")
        {
            string message = "Ok";
            Debug.WriteLine($"\nwriteDataToFile\t{pathToSave}");

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


        static public List<List<float>> getDataFromFile(/*string type, 
                                                    int number, */
                                                    string pathToFile = @"dataTumor\ExperimentalData\")
        {
            List<float> xValues = new List<float>();
            List<float> yValues = new List<float>();
            List<List<float>> xyValues = new List<List<float>>()
            {
                xValues,
                yValues
            };
            // Read the file and display it line by line.
            try
            {
                foreach (string line in System.IO.File.ReadLines(pathToFile))
                {
                string[] valuesSingleXY = line.Split("\t");
                    xyValues[0].Add(float.Parse((valuesSingleXY[0].Trim()).Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture));
                    xyValues[1].Add(float.Parse((valuesSingleXY[1].Trim()).Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture));
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


        static public Dictionary<string, float> getParametersFromFile(string pathToRead = @"dataTumor\PredictData\PersonalPatients\")
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
                    cancerParameters.Add((splitSingleString[0].Trim()).Replace(".", ","), float.Parse(splitSingleString[1].Trim(), ci));
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


        static public List<List<float>> getDataXYZFromFile(string pathToFile)
        {
            List<float> xValues = new List<float>();
            List<float> yValues = new List<float>();
            List<float> zValues = new List<float>();
            List<List<float>> xyzValue = new List<List<float>>();
            // Read the file and display it line by line.
            try
            {
                foreach (string line in System.IO.File.ReadLines(pathToFile))
                {
                    string[] valuesSingleXYZ = line.Split("\t");
                    xyzValue[0].Add(float.Parse((valuesSingleXYZ[0].Trim()).Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture));
                    xyzValue[1].Add(float.Parse((valuesSingleXYZ[1].Trim()).Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture));
                    xyzValue[2].Add(float.Parse((valuesSingleXYZ[2].Trim()).Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture));
                    xyzValue[3].Add(float.Parse((valuesSingleXYZ[3].Trim()).Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture));
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

            return xyzValue;
        }


        static public string copyAllFiles(string pathFrom, string pathTo)
        {
            try
            {
                
                DirectoryInfo dirInfo = new DirectoryInfo(pathFrom);
                /*foreach (FileInfo file in dirInfo.GetFiles("*Volume.txt"))
                {
                    File.Copy(file.FullName, pathTo + file.Name.TrimEnd(new char[] { '.', 't', 'x'}) + "Old.txt", true);
                    pathTo = String.Format(@"{0}\{1}Old.txt", pathTo, file.Name.TrimEnd(new char[] { '.', 't', 'x' }));
                    Debug.WriteLine($"\ncopyAllFiles\t{pathTo}");
                }*/
                File.Copy(pathFrom, pathTo, true);
                Debug.WriteLine($"\ncopyAllFiles\t{pathTo}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{ex}");
                Console.WriteLine($"{ex}");
            }
            return "Ok";
        }



        static public string copyDir(string FromDir, string ToDir)
        {
            Directory.CreateDirectory(ToDir);
            foreach (string s1 in Directory.GetFiles(FromDir))
            {
                string s2 = ToDir + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(FromDir))
            {
                copyDir(s, ToDir + "\\" + Path.GetFileName(s));
            }
            return "Ok";
        }


        static public float[][] getDynamicDataFromFile(string filePath)
        {
            int currentLine = 0;
            int count = System.IO.File.ReadAllLines(filePath).Length;
            float[][] values = new float[2][];
            values[0] = new float[count];
            values[1] = new float[count];
            try
            {
                foreach (string line in System.IO.File.ReadLines(filePath))
                {
                    string[] valuesSingleXY = line.Split("\t");
                    valuesSingleXY[0] = valuesSingleXY[0].Replace(",", ".");
                    valuesSingleXY[1] = valuesSingleXY[1].Replace(",", ".");
                    Debug.WriteLine(String.Format("Get {0}\t{1}", valuesSingleXY[0], valuesSingleXY[1]));

                    values[0][currentLine] = float.Parse(valuesSingleXY[0].Trim(), CultureInfo.InvariantCulture.NumberFormat);
                    values[1][currentLine] = float.Parse(valuesSingleXY[1].Trim(), CultureInfo.InvariantCulture.NumberFormat);
                    Debug.WriteLine(String.Format("Get 2 {0}\t{1}", values[0][currentLine], values[1][currentLine]));

                    currentLine++;
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

            return values;
        }
    }
}
