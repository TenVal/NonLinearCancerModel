using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotLinearCancerModel
{
    class DataCancer
    {
        /// <summary>
        /// Patient data store from third-party data
        /// </summary>
        private List<Dictionary<string, List<List<float>>>> patients;

        public List<Dictionary<string, List<List<float>>>> Patients
        {
            get
            {
                return patients;
            }
        }

        public DataCancer()
        {
            for(int i = 1; i < 11; i++)
            {
                this.patients.Add(this.getPersonalDataCancer(i));
            }
        }

        public Dictionary<string, List<List<float>>> getPersonalDataCancer(int number)
        {
            List<List<float>> Diameter = new List<List<float>>();
            List<List<float>> Volume = new List<List<float>>();
            Dictionary<string, List<List<float>>> cancerValues = new Dictionary<string, List<List<float>>>()
            {
                {"Diameter" , Diameter},
                {"Volume" , Volume}
            };

            int count = 0;           
            foreach (KeyValuePair<string, List<List<float>>> kvp in cancerValues)
            {
                List<List<float>> dataReadFile = ActionDataFile.getDataFromFile(kvp.Key, count);
                // easy coping
                cancerValues[kvp.Key] = dataReadFile;
                count++;
            }

            return cancerValues;
        }
    }
}
