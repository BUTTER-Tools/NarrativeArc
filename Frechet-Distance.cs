/*
MIT License

Copyright (c) 2017 Logan Apple

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/


using System;
using System.Collections.Generic;

using PluginContracts;

namespace NarrativeArc
{
    public partial class NarrativeArc : Plugin
    {

        public static double EuclideanDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static double ComputeDistance(double[,] distances, int i, int j, List<double[]> P, List<double[]> Q)
        {
            if (distances[i, j] > -1)
                return distances[i, j];

            if (i == 0 && j == 0)
                distances[i, j] = EuclideanDistance(P[0][0], P[0][1], Q[0][0], Q[0][1]);
            else if (i > 0 && j == 0)
                distances[i, j] = Math.Max(ComputeDistance(distances, i - 1, 0, P, Q),
                                           EuclideanDistance(P[i][0], P[i][1], Q[0][0], Q[0][1]));
            else if (i == 0 && j > 0)
                distances[i, j] = Math.Max(ComputeDistance(distances, 0, j - 1, P, Q),
                                           EuclideanDistance(P[0][0], P[0][1], Q[j][0], Q[j][1]));
            else if (i > 0 && j > 0)
                distances[i, j] = Math.Max(Math.Min(ComputeDistance(distances, i - 1, j, P, Q),
                                           Math.Min(ComputeDistance(distances, i - 1, j - 1, P, Q),
                                                    ComputeDistance(distances, i, j - 1, P, Q))),
                                                    EuclideanDistance(P[i][0], P[i][1], Q[j][0], Q[j][1]));
            else
                distances[i, j] = Double.PositiveInfinity;

            return distances[i, j];
        }

        public static double FrechetDistance(List<double[]> P, List<double[]> Q)
        {
            double[,] distances = new double[P.Count, Q.Count];
            for (int y = 0; y < P.Count; y++)
                for (int x = 0; x < Q.Count; x++)
                    distances[y, x] = -1;

            return ComputeDistance(distances, P.Count - 1, Q.Count - 1, P, Q);
        }

    }
}
