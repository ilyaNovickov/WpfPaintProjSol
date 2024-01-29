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
using System.Collections.ObjectModel;

namespace WpfPaintProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Layer drawingCanvas = null;

        public ObservableCollection<ShapeItem> Shapes => drawingCanvas?.Shapes;

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
            //else if (sender == squareButton)
            //    selectedShape = StandartShapes.Square;
            //else if (sender == roundButton)
            //    selectedShape = StandartShapes.Round;
            else if (sender == rhombButton)
                selectedShape = StandartShapes.Rhomb;
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isDraw)
                return;

            Shape shapeToAdd = null;

            switch (selectedShape)
            {
                case null:
                default:
                    return;
                case StandartShapes.Ellipse:
                    shapeToAdd = new Ellipse();
                    shapeToAdd.Width = widthUpDown.Value.Value;
                    shapeToAdd.Height = heightUpDown.Value.Value;
                    break;
                case StandartShapes.Rectangele:
                    shapeToAdd = new Rectangle();
                    shapeToAdd.Width = widthUpDown.Value.Value;
                    shapeToAdd.Height = heightUpDown.Value.Value;
                    break;
                case StandartShapes.Triangle:
                    shapeToAdd = new Triangle();
                    shapeToAdd.Width = widthUpDown.Value.Value;
                    shapeToAdd.Height = heightUpDown.Value.Value;
                    break;
                case StandartShapes.Round:
                    shapeToAdd = new Round();
                    shapeToAdd.Width = widthUpDown.Value.Value;
                    shapeToAdd.Height = heightUpDown.Value.Value;
                    break;
                case StandartShapes.Square:
                    shapeToAdd = new Square();
                    shapeToAdd.Width = widthUpDown.Value.Value;
                    shapeToAdd.Height = heightUpDown.Value.Value;
                    break;
                case StandartShapes.Rhomb:
                    shapeToAdd = new Rhomb();
                    shapeToAdd.Width = widthUpDown.Value.Value;
                    shapeToAdd.Height = heightUpDown.Value.Value;
                    break;
            }
            shapeToAdd.Fill = new SolidColorBrush(bgColorPicker.SelectedColor.Value);
            shapeToAdd.Stroke = new SolidColorBrush(foreColorPicker.SelectedColor.Value);
            Canvas.SetLeft(shapeToAdd, e.GetPosition(drawingCanvas).X - shapeToAdd.Width / 2d);
            Canvas.SetTop(shapeToAdd, e.GetPosition(drawingCanvas).Y - shapeToAdd.Height / 2d);
            drawingCanvas.AddShape(shapeToAdd);

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

            shapesListBox.ItemsSource = drawingCanvas.Shapes;
        }
    }
}
