using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHomework
{
    public class ChartManager
    {
        #region Members

        private ggPictureBox ggPictBox;
        private Bitmap bmp;
        private Rectangle viewPort;
        private Graphics G;

        private int nbPoints;         // number of points for each path

        private Random R = new Random();
        private Distribution D;

        private Pen blackPen = new Pen(Color.Black);
        private Pen whitePen = new Pen(Color.White);

        #endregion

        #region Constructor

        public ChartManager(Distribution rn, ggPictureBox pictureBox)
        {
            ggPictBox = pictureBox;
            bmp = new Bitmap(ggPictBox.Width, ggPictBox.Height);

            G = Graphics.FromImage(bmp);
            D = rn;

            nbPoints = D.Paths[0].Points.Count;
        }

        #endregion

        #region Public

        public void DrawChart(int nbClusters = 0)
        {
            viewPort = new Rectangle(0, 0, ggPictBox.Width, ggPictBox.Height - 20);
            G.FillRectangle(Brushes.MidnightBlue, viewPort);

            double minX = 20;
            double maxX = nbPoints;
            double minY = D.Means.Min(p => p);
            double maxY = D.Means.Max(p => p);  

            double rangeX = maxX - minX;
            double rangeY = maxY - minY;

            int noCluster = nbClusters == 0 ? 10 : nbClusters;
            DrawHistogram(noCluster, minX, minY, rangeX, rangeY);

            ggPictBox.Image = bmp;
        }

        #endregion

        #region Private

        private void DrawHistogram(int nbClusters, double startX, double startY, double rangeX, double rangeY)
        {
            int x = (int)AdjustX(0, startX, rangeX) + 70;

            int w = 0;
            int y;
            int h;
            int maxOccurs = 0;

            // Find min and max value in T
            var tPoints = new List<double>();
            foreach (var mean in D.Means)
            {
                tPoints.Add(mean);
            }
            tPoints.Sort();
            var mint = tPoints.First();
            var maxt = tPoints.Last();
            var rng = maxt - mint > 0 ? maxt - mint : maxt;

            // Adjusts coordinates to viewport
            var y1 = (int)AdjustY(mint, startY, rangeY);
            var y2 = (int)AdjustY(maxt, startY, rangeY);

            // Creates clusters for histogram 
            var clusters = new List<double>();
            double clusterSize = rng / nbClusters;
            for (int i = 0; i < nbClusters; i++)
            {
                // valori positivi
                var min = mint;
                var max = min + clusterSize;
                var occurs = tPoints.Where(d => d >= min && d < max).Count();
                clusters.Add(occurs);
                mint = max;
            }

            // Calculates height of bars
            h = (y1 - y2) / clusters.Count;

            // Draws bars
            y = (int)y2;
            for (int i = 0; i < clusters.Count; i++)
            {
                w = (int)clusters[i];

                if (w > maxOccurs)
                    maxOccurs = w;

                if (i == clusters.Count - 1)
                {
                    h = (int)(y1 - y);
                }

                Rectangle rectangle = new Rectangle(x, y - 1, w + 1, h);
                G.DrawRectangle(Pens.Black, rectangle);

                rectangle = new Rectangle(x, y, w + 1, h - 1);
                G.FillRectangle(Brushes.Gold, rectangle);

                G.DrawString(clusters[i].ToString(), new Font(FontFamily.GenericSansSerif, 7, FontStyle.Bold), Brushes.White, new Point(x - 30, y));
                y = y + h;
            }

            var font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
            G.DrawString($"First Order Stat: {Math.Round(tPoints.First(), 0).ToString()} - Last Order Stat: {Math.Round(tPoints.Last(),0).ToString()}", font, Brushes.Black, new Point(x, y));
        }

        private List<PointF> GetAdjustedPoints(List<RandomPoint> points, double startX, double startY, double rangeX, double rangeY)
        {
            // Adjusts all points to viewport area
            List<PointF> adjustedPoints = new List<PointF>();

            foreach (RandomPoint point in points)
            {
                var adjPoint = AdjustPoint(point, startX, startY, rangeX, rangeY);
                adjustedPoints.Add(adjPoint);
            }

            return adjustedPoints;
        }

        private PointF AdjustPoint(RandomPoint point, double startX, double startY, double rangeX, double rangeY)
        {
            // Adjusts the point to viewport area
            PointF adjustedPoint = new PointF();

            var X = AdjustX(point.X, startX, rangeX);
            var Y = AdjustY(point.Y, startY, rangeY);
            adjustedPoint = new PointF((float)X, (float)Y);

            return adjustedPoint;
        }

        private float AdjustX(double x, double startX, double rangeX)
        {
            return (float)(viewPort.Left + viewPort.Width * ((x - startX) / rangeX));
        }

        private float AdjustY(double y, double startY, double rangeY)
        {
            return (float)(viewPort.Top + viewPort.Height - (viewPort.Height * ((y - startY) / rangeY)));
        }

        #endregion
    }
}
