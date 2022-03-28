using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NotLinearCancerModel
{
    static class ActionDataFile
    {
        static public string writeDataToFIle(string type, int number, float[,,] data, string pathToSave = @"D:\VolSU\НИР\ScienceArticle\NotLinearCancerModel\NotLinearCancerModel\dataTumor\PredictData\PersonalPatients\")
        {
            string message = "Ok";
            pathToSave += pathToSave + type + @"\txt\" + number.ToString() + type + @".txt";
            try {
                StreamWriter outputFile = new StreamWriter(pathToSave);
                for (int i = 0; i <= data.GetLength(0); i++)
                {
                    for (int j = 0; j <= data.GetLength(0); j++)
                    {
                        for (int k = 0; k <= data.GetLength(0); k++)
                        {
                            outputFile.WriteLine(data[i, j, k]);
                        }
                    }
                }
                Console.WriteLine(message);
            }
            catch (FileNotFoundException e)
            {
                message = $"The file was not found: '{e}'";
                Console.WriteLine(message);
            }
            catch (DirectoryNotFoundException e)
            {
                message = $"The directory was not found: '{e}'"
                Console.WriteLine(message);
            }
            catch (IOException e)
            {
                message = $"The file could not be opened: '{e}'"
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
            int counter = 0;
            pathToFile = pathToFile + type + @"\txt\" + number.ToString() + type + @".txt";
            // Read the file and display it line by line.  
            try
            {
                foreach (string line in System.IO.File.ReadLines(pathToFile))
                {
                    /*loat x = float.Parse(line.Split(" ")[0].Trim());
                    float y = float.Parse(line.Split(" ")[1].Trim());*/
                    xyValues[0].Add(float.Parse(line.Split(" ")[0].Trim()));
                    xyValues[0].Add(float.Parse(line.Split(" ")[1].Trim()));
                    counter++;
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"The directory was not found: '{e}'");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: '{e}'");
            }
            
            return xyValues;
        }
    }
}
