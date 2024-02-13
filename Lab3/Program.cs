using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;


namespace Lab3
{

    class Program
    {
        static void Main(string[] args)
        {
            TestSmoothingSpline();
        }

        static void TestSmoothingSpline()
        {
            string filename = "C:\\Users\\simal\\OneDrive\\Документы\\C#\\Lab3\\spline_1.txt";

            double[] x = new double[] { -1, 0.5, 4, 16, 17, 18.3 };
            //double[] x = new double[] { -1, 2, 5, 8, 11, 14 };

            FValues F_Array = Delegates.FA;
            V1DataArray array_1 = new V1DataArray("Array_1", DateTime.Now, x, F_Array);

            SplineData spline_1 = new SplineData(array_1, 4, 100);

            if (!spline_1.GetSmoothingSpline())
                Console.WriteLine("SPLINE ERROR");

            Console.WriteLine(spline_1.ToLongString("{0:f3}"));

            spline_1.Save(filename, "{0:f3}");
        }

    }
}

