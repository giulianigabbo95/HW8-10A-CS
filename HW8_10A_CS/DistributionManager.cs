using System;
using System.Linq;
using System.Collections.Generic;

namespace MyHomework
{
    public class RandomPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public RandomPoint()
        {
            X = 0;
            Y = 0;
        }

        public RandomPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class RandomPath
    {
        public List<RandomPoint> Points { get; set; }

        public RandomPath()
        {
            Points = new List<RandomPoint>();
        }
    }

    public class Distribution
    {
        #region MEMBERS

        public readonly int LimitINF = 0;
        public readonly int LimitSUP = 1000;

        public List<RandomPath> Paths { get; set; }
        public List<double> Means { get; set; }

        private Random R;

        private int noPoints { get; set; }
        private int noPaths { get; set; }

        #endregion

        #region CONSTRUCTOR

        public Distribution(int nbPoints, int nbPaths)
        {
            noPoints = nbPoints;
            noPaths = nbPaths;

            this.Paths = new List<RandomPath>();

            R = new Random();
        }

        #endregion

        #region PUBLIC

        public List<RandomPath> GenerateDistribution()
        {
            List<RandomPath> paths = new List<RandomPath>();

            for (int i = 0; i < noPaths; i++)
            {
                var path = new RandomPath();

                for (int x = 1; x <= noPoints; x++)
                {
                    RandomPoint p = GetRandomUniformVariable();
                    path.Points.Add(p);
                }

                paths.Add(path);
            }

            return paths;
        }

        public List<double> CalculatesMeans()
        {
            List<double> means = new List<double>();

            for (int i = 0; i < noPaths; i++)
            {
                var path = Paths[i];
                means.Add(0);

                for (int j = 0; j < path.Points.Count; j++)
                {
                    var point = path.Points[j];
                    var rnd =  R.Next(point.X, point.Y);

                    means[i] += (rnd - means[i]) / (double)(j + 1);
                }
            }

            return means;
        }

        #endregion

        #region PRIVATE

        private double GetRandomRademacherVariable()
        {
            var r1 = R.NextDouble();
            var r2 = r1 < 0.5 ? 1d : 0d;
            var rand_rademacher = r2 * 2 - 1;
            return rand_rademacher;
        }

        private double GetRandomNormalVariable()
        {
            double u1;
            double u2;
            double rand_normal;

            // //< Method 1 >
            //double rSquared;

            //do
            //{
            //    u1 = 2.0 * R.NextDouble() - 1.0;
            //    u2 = 2.0 * R.NextDouble() - 1.0;
            //    rSquared = (u1 * u1) + (u2 * u2);
            //}
            //while (rSquared >= 1.0);
            //// </Method 1>

            ////polar tranformation 
            //double p = Math.Sqrt(-2.0 * Math.Log(rSquared) / rSquared);
            //rand_normal = u1 * p; //* sigma + mu;

            // <Method 2>
            u1 = R.NextDouble();
            u2 = R.NextDouble();
            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            rand_normal = rand_std_normal; //* sigma + mu;
            // </Method 2>

            // result
            return rand_normal;
        }

        public RandomPoint GetRandomUniformVariable()
        {
            int rInf = R.Next(LimitINF, LimitSUP);
            int rSup = R.Next(rInf, LimitSUP);

            var p = new RandomPoint() { X = rInf, Y = rSup };

            return p;
        }

        #endregion
    }
}
