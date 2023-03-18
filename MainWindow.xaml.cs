using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
        }

        private void DrawGraph(object sender, TextChangedEventArgs e)
        {
            if (!GraphSizeSpecified) return;

            MainGraph.SetNewFunction(GraphFunctionBox.Text);
        }

        //Utility methods
        private bool InLimits(double x, double y) => x < MainGraph.Size * -1 || x > MainGraph.Size || y < MainGraph.Size * -1 || y > MainGraph.Size;

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
    }
}
