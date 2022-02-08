using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyHomework
{
    public partial class Form1 : Form
    {
        private int m;          // n. of paths
        private int n;          // number of points for each path
        private int c;          // nb of clusters for histograms

        private Distribution RN;

        private ggPictureBox ggPictureBox1;

        public Form1()
        {
            InitializeComponent();

            ggPictureBox1 = new ggPictureBox(MainPanel);
            ggPictureBox1.BackColor = Color.White;
            ggPictureBox1.Top = MainPanel.Left / 10 + 10;
            ggPictureBox1.Left = MainPanel.Left / 10 + 10;  
            ggPictureBox1.Height = MainPanel.Height / 10 * 8;
            ggPictureBox1.Width = MainPanel.Width / 10 * 4; 
            ggPictureBox1.BorderStyle = BorderStyle.FixedSingle;
            MainPanel.Controls.Add(ggPictureBox1);

            NbPoints.Value = 10;
            NbClusters.Value = 10;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetVariables();
        }

        // Events
        //--------------------------------------------------------

        private void btnRecalc_Click(object sender, EventArgs e)
        {
            SetVariables();
            CreateStatEngineInstance();
            DrawChart();
        }

        private void NbPoints_ValueChanged(object sender, EventArgs e)
        {
            SetVariables();
            CreateStatEngineInstance();
            DrawChart();
        }

        private void variance_ValueChanged(object sender, EventArgs e)
        {
            SetVariables();
            CreateStatEngineInstance();
            DrawChart();
        }

        private void NbPath_ValueChanged(object sender, EventArgs e)
        {
            SetVariables();
            CreateStatEngineInstance();
            DrawChart();
        }

        private void btnClusters_Click(object sender, EventArgs e)
        {
            SetVariables();
            DrawChart();

        }

        private void NbClusters_ValueChanged(object sender, EventArgs e)
        {
            SetVariables();
            DrawChart();
        }

        // Methods
        //--------------------------------------------------------

        private void SetVariables()
        {
            n = (int)NbPoints.Value;
            m = (int)NbPath.Value;
            c = (int)NbClusters.Value;
        }

        private void CreateStatEngineInstance()
        {
            RN = new Distribution(n, m);
            RN.Paths = RN.GenerateDistribution();
            RN.Means = RN.CalculatesMeans();
        }

        private void DrawChart()
        {
            if (RN != null)
            {
                ChartManager CM = new ChartManager(RN, ggPictureBox1);
                CM.DrawChart(c);
            }
        }
    }
}
