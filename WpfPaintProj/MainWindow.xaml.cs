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
        private object colorSender = null;

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

        #region ChooseColor
        private void ChooseColor_MouseClick(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = !popup.IsOpen;
            colorSender = sender;
        }

        private void SelectColor_Click(object sender, RoutedEventArgs e)
        {
            ((Button)colorSender).Background = new SolidColorBrush(colorCanvas.SelectedColor.Value);
        }

        private void popup_Closed(object sender, EventArgs e)
        {
            colorSender = null;
        }
        #endregion
    }
}
