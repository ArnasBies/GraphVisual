using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphVisual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool GraphSizeSpecified;
        private Graph MainGraph;

        public MainWindow()
        {
            GraphSizeSpecified = false;
            MainGraph = new(0, "0");
            InitializeComponent();

            GraphSizeBox.TextChanged += ProcessNewSize;
            GraphFunctionBox.TextChanged += DrawGraph;

            SolidColorBrush mainBrush = new SolidColorBrush();
            mainBrush.Color = Colors.AliceBlue;
            mainBrush.Opacity = 0.9;

            Line line = new Line();
            line.Stroke = mainBrush;
            line.StrokeThickness = 2;

            line.X1 = 0;
            line.Y1 = 0;
            line.X2 = 50;
            line.Y2 = 50;

            GraphCanvas.Children.Add(line);
        }

        //Event Handlers
        private void ProcessNewSize(object sender, TextChangedEventArgs e)
        {
            int EnteredValue;
            float radius;

            if (!int.TryParse(GraphSizeBox.Text, out EnteredValue))
            {
                ChangeGraphSizeIndicators();
                MainGraph.Size = 0;
                GraphSizeSpecified = false;
                return;
            }

            if (EnteredValue <= 0)
            {
                ChangeGraphSizeIndicators();
                MainGraph.Size = 0;
                GraphSizeSpecified = false;
                return;
            }

            MainGraph.Size = EnteredValue;
            radius = (float)EnteredValue / 2;

            ChangeGraphSizeIndicators(radius);
            GraphSizeSpecified = true;

            DrawGraph(this, new TextChangedEventArgs(e.RoutedEvent, new UndoAction()));
        }

        private void DrawGraph(object sender, TextChangedEventArgs e)
        {
            if (!GraphSizeSpecified || String.IsNullOrEmpty(GraphFunctionBox.Text))
            {
                GraphCanvas.Children.Clear();
                return;
            }

            if (!MainGraph.SetNewFunction(GraphFunctionBox.Text))
            {
                GraphCanvas.Children.Clear();
                return;
            }

            GraphCanvas.Children.Clear();

            double halfSize = System.Convert.ToDouble(MainGraph.Size) / 2;

            double y1, y2;
            double lineX, 
                xx = 0, 
                additive = (halfSize * 2) / 200,
                relativeAdditive = Math.Round(GraphCanvas.ActualWidth) / 200;
            Debug.WriteLine($"{additive}: additive, {relativeAdditive}: relative additive, {GraphCanvas.ActualWidth}: graph canvas width ");

            for (double x = -1 * halfSize; x < halfSize; x += additive)
            {
                Line line = new Line();
                line.Stroke = System.Windows.Media.Brushes.DarkSlateBlue;
                line.StrokeThickness = 2;

                lineX = (GraphCanvas.ActualWidth / halfSize) * (x + halfSize);

                try
                {
                    y1 = MainGraph.GetEvaluation(x);
                    y2 = MainGraph.GetEvaluation(x + additive);
                }
                catch
                {
                    GraphCanvas.Children.Clear();
                    return;
                }

                line.X1 = xx;
                line.X2 = xx + relativeAdditive;

                line.Y1 = (Math.Round(GraphCanvas.ActualHeight)/2) - (Math.Round(GraphCanvas.ActualHeight) / (halfSize * 2) * MainGraph.GetEvaluation(x));
                line.Y2 = (Math.Round(GraphCanvas.ActualHeight)/2) - (Math.Round(GraphCanvas.ActualHeight) / (halfSize * 2) * MainGraph.GetEvaluation(x + additive));

             
                if (y1 > halfSize) line.Y1 = 1;
                else if (y1 < halfSize * -1) line.Y1 = GraphCanvas.ActualHeight;
            
                if (y2 > halfSize) line.Y2 = 1;
                else if (y2 < halfSize * -1) line.Y2 = GraphCanvas.ActualHeight;

                xx += relativeAdditive;
                GraphCanvas.Children.Add(line);
            }
        }

        //Utility methods
        private void ChangeGraphSizeIndicators(float radius)
        {
            PosXGraphSize.Text = radius.ToString();
            PosYGraphSize.Text = radius.ToString();
            radius *= -1;
            NegXGraphSize.Text = radius.ToString();
            NegYGraphSize.Text = radius.ToString();
        }

        private void ChangeGraphSizeIndicators()
        {
            PosXGraphSize.Text = "?";
            PosYGraphSize.Text = "?";
            NegXGraphSize.Text = "?";
            NegYGraphSize.Text = "?";
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawGraph(this, new TextChangedEventArgs(e.RoutedEvent, new UndoAction()));
        }
    }
}
