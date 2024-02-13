﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    struct SplineDataItem
    {

        public double X { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }

        public SplineDataItem(double x = 0, double y1 = 0, double y2 = 0)
        {
            X = x;
            Y1 = y1;
            Y2 = y2;
        }

        public string ToLongString(string format)
        {
            return $"X:          {string.Format(format, X)}\nY1:         {string.Format(format, Y1)}\nY1_Spline:  {string.Format("{0:f15}", Y2)}\n\n";
        }

        public override string ToString()
        {
            return $"X:  {X}\nY1: {Y1}\nY2: {Y2}\n\n";
        }

    }
}
