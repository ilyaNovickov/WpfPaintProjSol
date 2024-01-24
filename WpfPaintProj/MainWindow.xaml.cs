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
using System.Text.RegularExpressions;

namespace WpfPaintProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Shape> shapes = new List<Shape>(1);
        private bool isDraw = false;
        private StandartShapes? selectedShape = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            isDraw = true;
            selectedShape = StandartShapes.Ellipse;
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isDraw)
                return;

            switch (selectedShape)
            {
                case null:
                default:
                    return;
                case StandartShapes.Ellipse:

                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 100;
                    ellipse.Height = 100;
                    SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
                    ellipse.Fill = solidColorBrush;
                    Canvas.SetLeft(ellipse, e.GetPosition(canvas).X);
                    Canvas.SetTop(ellipse, e.GetPosition(canvas).Y);
                    canvas.Children.Add(ellipse);
                    break;
            }

            isDraw = false;
            selectedShape = null;

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
