using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class SplineData
    {

        public V1DataArray DataArray { get; set; }

        public int NodesCount { get; set; }

        public double[] SplineY { get; set; }

        public int MaxIter { get; set; }

        public int StopReason { get; set; }

        public double MinRes { get; set; }

        public List<SplineDataItem> ListSplineData { get; set; }

        public SplineData(V1DataArray dataArray, int nodesCount, int maxIter)
        {
            DataArray = dataArray;
            NodesCount = nodesCount;
            MaxIter = maxIter;
            ListSplineData = new List<SplineDataItem>();
            SplineY = new double[DataArray.X.Length];
        }

        public bool GetSmoothingSpline()
        {
            double[] GridY = new double[NodesCount];
            GridY[0] = DataArray.Y[0][0];
            GridY[NodesCount - 1] = DataArray.Y[0][DataArray.X.Length - 1];
            int error = 24;
            int status = 0;
            double minRes = 0;

            try 
            {
                SplineSmoothing(DataArray.X.Length, NodesCount, DataArray.X, DataArray.Y[0], GridY, SplineY, MaxIter, 
                                ref minRes, ref status, ref error);

                StopReason = status;
                MinRes = minRes;

                for (int i = 0; i < DataArray.X.Length; i++)
                {
                    SplineDataItem Node = new SplineDataItem(DataArray.X[i], DataArray.Y[0][i], SplineY[i]);
                    ListSplineData.Add(Node);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in function GetSmoothingSpline: {ex.Message}");
                return false;
            }
        }

        [DllImport("..\\..\\..\\..\\x64\\Debug\\MKL_CPP.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SplineSmoothing(int LenX, int NodesCount, double[] X, double[] Y, double[] GridY, 
                                                  double[] SplineY, int MaxIter, ref double ResFinal, ref int StopCriteria, ref int error);

        public string ToLongString(string format)
        {
            string str_res = $"V1_DATA_ARRAY:\n\n{DataArray.ToLongString(format)}";

            str_res += "SPLINE_DATA:\n\n";

            foreach (SplineDataItem item in ListSplineData)
            {
                str_res += item.ToLongString(format);
            }

            str_res += $"MIN_RESIDUAL:    {MinRes}\n\n";

            str_res += $"STOP_REASON:     {GetStopReason(StopReason)}\n\n";

            str_res += $"ITERATIONS_NUM:  {MaxIter}\n\n";

            return str_res;
        }

        public bool Save(string filename, string format)
        {
            StreamWriter? writer = null;
            try
            {
                writer = new StreamWriter(filename, false);
                writer.WriteLine(this.ToLongString(format));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in function Save: {ex.Message}");
                return false;
            }
            finally
            {
                if (writer != null) writer.Dispose();
            }
        }

        private string GetStopReason(int StopReason)
        {
            switch (StopReason)
            {
                case 1: return "Превышено заданное число итераций";
                case 2: return "Размер доверительной области < 1.0E-12";
                case 3: return "Норма невязки < 1.0E-12";
                case 4: return "Норма строк матрицы Якоби < 1.0E-12";
                case 5: return "Пробный шаг < 1.0E-12";
                case 6: return "Разность нормы функции и погрешности < 1.0E-12";
                default: return "";
            }
        }

    }
}
