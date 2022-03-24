using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NotLinearCancerModel
{
    class ActionDataFile
    {
        public List<List<float>> getDataFromFile(string type, int number, string pathToFile = @"D:\VolSU\НИР\ScienceArticle\NotLinearCancerModel\NotLinearCancerModel\dataTumor\ModelData\personalPatients\poly3current\")
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
            foreach (string line in System.IO.File.ReadLines(pathToFile))
            {
                /*loat x = float.Parse(line.Split(" ")[0].Trim());
                float y = float.Parse(line.Split(" ")[1].Trim());*/
                xyValues[0].Add(float.Parse(line.Split(" ")[0].Trim()));
                xyValues[0].Add(float.Parse(line.Split(" ")[1].Trim()));
                counter++;
            }
            return xyValues;
        }
    }
}
