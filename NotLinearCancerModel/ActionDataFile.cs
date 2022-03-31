﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace NotLinearCancerModel
{
    static class ActionDataFile
    {
        /// <summary>
        /// Actions to write, read and do everything with data in program and files
        /// </summary>
        static public string writeDataToFile(string type, int number, float[,,] data, string pathToSave = @"D:\VolSU\НИР\ScienceArticle\NotLinearCancerModel\NotLinearCancerModel\dataTumor\PredictData\PersonalPatients\")
        {
            string message = "Ok";
            pathToSave += pathToSave + type + @"\txt\" + number.ToString() + type + @".txt";
            try
            {
                StreamWriter outputFile = new StreamWriter(pathToSave);
                for (int i = 0; i <= data.GetLength(0); i++)
                {
                    for (int j = 0; j <= data.GetLength(0); j++)
                    {
                        for (int k = 0; k <= data.GetLength(0); k++)
                        {
                            outputFile.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", i, j, k, data[i, j, k]));
                        }
                    }
                }
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

        static public List<List<float>> getDataFromFile(string type, int number, string pathToFile = @"D:\VolSU\НИР\ScienceArticle\NotLinearCancerModel\NotLinearCancerModel\dataTumor\ModelData\personalPatients\poly3current\")
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
    }
}
