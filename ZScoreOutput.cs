using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WLT.BusinessLogic
{
    public class ZScoreOutput
    {
        public List<double> input;
        public List<int> signals;
        public List<double> avgFilter;
        public List<double> filtered_stddev;

        //public List<FuelDataSet> fuelList;

    }

    public static class ZScore
    {
        public static List<FuelDataSet> StartAlgo(List<FuelDataSet> input, int lag, double threshold, double influence)
        {
            // init variables!
            int[] signals = new int[input.Count];

            FuelDataSet[] filteredY = new List<FuelDataSet>(input).ToArray();

            FuelDataSet[] avgFilter = new List<FuelDataSet>(input).ToArray();
            FuelDataSet[] stdFilter = new List<FuelDataSet>(input).ToArray();

            List<FuelDataSet> fuelListData = new List<FuelDataSet>();


            var initialWindow = new List<FuelDataSet>(filteredY).Skip(0).Take(lag).ToList();

            avgFilter[lag - 1].Avg = Mean(initialWindow);
            stdFilter[lag - 1].Avg = StdDev(initialWindow);

            for (int i = lag; i < input.Count; i++)
            {
                if (Math.Abs(input[i].Raw - avgFilter[i - 1].Raw) > threshold * stdFilter[i - 1].Raw)
                {
                    signals[i] = (input[i].Raw > avgFilter[i - 1].Raw) ? 1 : -1;
                    filteredY[i].DeSpiked = influence * input[i].Raw + (1 - influence) * filteredY[i - 1].Raw;
                }
                else
                {
                    signals[i] = 0;
                    filteredY[i] = input[i];
                }

                // Update rolling average and deviation
                var slidingWindow = new List<FuelDataSet>(filteredY).Skip(i - lag).Take(lag + 1).ToList();

                avgFilter[i].Avg = Mean(slidingWindow);
                stdFilter[i].Avg = StdDev(slidingWindow);

                FuelDataSet fuelItem = new FuelDataSet();

                fuelItem.Avg = Mean(slidingWindow);
                fuelItem.Raw = input[i].Raw;
                fuelItem.DeSpiked = StdDev(slidingWindow);
                fuelItem.Date = input[i].Date;

                fuelListData.Add(fuelItem);

            }

            return fuelListData;
        }

        private static double Mean(List<FuelDataSet> list)
        {
            // Simple helper function! 

            double[] data = new double[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                data[i] = list[i].Raw;
            }

            return data.Average();
        }

        private static double StdDev(List<FuelDataSet> list)
        {

            double ret = 0;

            double[] data = new double[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                data[i] = list[i].Raw;
            }

            if (data.Count() > 0)
            {
                double avg = data.Average();
                double sum = data.Sum(d => Math.Pow(d - avg, 2));
                ret = Math.Sqrt((sum) / (data.Count() - 1));
            }
            return ret;
        }
    }

    public class FuelDataSet
    {
        public double Raw { get; set; }
        public double Avg { get; set; }

        public double SingleDeSpiked { get; set; }
        public double DeSpiked { get; set; }
        public DateTime Date { get; set; }
        public int Ign { get; set; }
        public double Speed { get; set; }
        public string Location { get; set; }
        public long ipkCommanTrackingID { get; set; }

        public string Lat { get; set; }
        public string Lon { get; set; }

        public long EngineHours { get; set; }

    }
    public class RefillDrainChunks
    {
        public int ReadingsStartIndex { get; set; }
        public int ReadingsEndIndex { get; set; }
        public int ReadingsCount { get; set; }
        public double ChangeAmount { get; set; }

        public double ReadingsInitialFuel { get; set; }
        public double ReadingsFinalFuel { get; set; }
        public DateTime EventDateTime { get; set; }
        public bool IsRefill { get; set; }
        public string Location { get; set; }
        public long ipkCommanTrackingID { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
    }

}
