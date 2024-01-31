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
        private Layer selectedLayer = null;

        public ObservableCollection<ShapeItem> Shapes => selectedLayer?.Shapes;

        private bool isDraw = false;
        private StandartShapes? selectedShape = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        public Shape SelectedShape
        {
            get;
            set;
        }

        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedLayer != null)
            {
                selectedLayer.CanSelectShapes = false;
                selectedLayer.ResetSelectedShape();
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
            Canvas.SetLeft(shapeToAdd, e.GetPosition(selectedLayer).X - shapeToAdd.Width / 2d);
            Canvas.SetTop(shapeToAdd, e.GetPosition(selectedLayer).Y - shapeToAdd.Height / 2d);
            selectedLayer.AddShape(shapeToAdd);

            isDraw = false;
            selectedShape = null;
            selectedLayer.CanSelectShapes = true;
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
            selectedLayer = drawingControl.Layers.Last();
            selectedLayer.Background = Brushes.White;
            selectedLayer.MouseDown += canvas_MouseDown;
            selectedLayer.SelectedShapeChange += SelectedLayer_SelectedShapeChange;
            selectedLayer.Width = 500;
            selectedLayer.Height = 500;

            shapesListBox.ItemsSource = selectedLayer.Shapes;
        }

        private void SelectedLayer_SelectedShapeChange(object sender, EventArgs e)
        {
            int index = -1;
            for (int i = 0; i < shapesListBox.Items.Count; i++)
            {
                if (((ShapeItem)shapesListBox.Items[i]).Shape == selectedLayer.SelectedShape)
                    index = i;

            }

            if (index == -1)
            {
                shapesListBox.SelectedItem = null;
                return;
            }

            shapesListBox.SelectedItem = shapesListBox.Items[index];

        }

        private void shapesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            int index = drawingControl.Layers.First().Shapes.IndexOf((ShapeItem)e.AddedItems[0]);
            drawingControl.Layers.First().SelectedShape = drawingControl.Layers.First().Shapes[index].Shape;
        }
    }
}
