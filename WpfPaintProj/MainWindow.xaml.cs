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
using WpfPaintProj.ExtraControls;
using WpfPaintProj.OwnShapes;

namespace WpfPaintProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Layer drawingCanvas = null;

        private bool isDraw = false;
        private StandartShapes? selectedShape = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            if (drawingCanvas != null)
            {
                drawingCanvas.CanSelectShapes = false;
                drawingCanvas.ResetSelectedShape();
            }

            isDraw = true;
            if (sender == ellipseButton)
                selectedShape = StandartShapes.Ellipse;
            else if (sender == rectButton)
                selectedShape = StandartShapes.Rectangele;
            else if (sender == triangleButton)
                selectedShape = StandartShapes.Triangle;
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
                    ellipse.Width = widthUpDown.Value.Value;
                    ellipse.Height = heightUpDown.Value.Value;
                    ellipse.Fill = new SolidColorBrush(bgColorPicker.SelectedColor.Value);
                    ellipse.Stroke = new SolidColorBrush(foreColorPicker.SelectedColor.Value);
                    Canvas.SetLeft(ellipse, e.GetPosition(drawingCanvas).X - ellipse.Width / 2d);
                    Canvas.SetTop(ellipse, e.GetPosition(drawingCanvas).Y - ellipse.Height / 2d);
                    drawingCanvas.AddShape(ellipse);
                    break;
                case StandartShapes.Rectangele:
                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = widthUpDown.Value.Value;
                    rectangle.Height = heightUpDown.Value.Value;
                    rectangle.Fill = new SolidColorBrush(bgColorPicker.SelectedColor.Value);
                    rectangle.Stroke = new SolidColorBrush(foreColorPicker.SelectedColor.Value);
                    Canvas.SetLeft(rectangle, e.GetPosition(drawingCanvas).X - rectangle.Width / 2d);
                    Canvas.SetTop(rectangle, e.GetPosition(drawingCanvas).Y - rectangle.Height / 2d);
                    drawingCanvas.AddShape(rectangle);
                    break;
            }

            isDraw = false;
            selectedShape = null;
            drawingCanvas.CanSelectShapes = true;
        }

        #region ChooseColor
        //private void ChooseColor_MouseClick(object sender, RoutedEventArgs e)
        //{
        //    popup.IsOpen = !popup.IsOpen;
        //    colorSender = sender;
        //}

        //private void SelectColor_Click(object sender, RoutedEventArgs e)
        //{
        //    ((Button)colorSender).Background = new SolidColorBrush(colorCanvas.SelectedColor.Value);
        //}

        //private void popup_Closed(object sender, EventArgs e)
        //{
        //    colorSender = null;
        //}
        #endregion

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            this.drawingControl.AddLayer();
            drawingCanvas = drawingControl.Layers.Last();
            drawingCanvas.Background = Brushes.White;
            drawingCanvas.MouseDown += canvas_MouseDown;
            drawingCanvas.Width = 500;
            drawingCanvas.Height = 500;

            //DrawingCanvas canvas = new DrawingCanvas();
            //canvas.Width = 1000;
            //canvas.Height = 1000;
            //if (selectedShape != null)
            //    canvas.CanSelectShapes = true;
            //canvas.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            //canvas.MouseDown += canvas_MouseDown;
            //mainScrollViewer.Content = canvas;
            //this.drawingCanvas = canvas;

            //OwnShapes.MyWeirdShape tr = new OwnShapes.MyWeirdShape();

            //tr.Width = 100;
            //tr.Height = 100;
            //tr.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 255));

            //canvas.AddShape(tr);
        }
    }
}
