using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using WpfPaintProj.Helpers;

namespace WpfPaintProj.ExtraControls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class DrawingCanvas : UserControl
    {
        private Shape selectedShape = null;

        private Shape moveShape = null;
        private List<Shape> resizeShapes = new List<Shape>(1);
        private Shape decoRect = null;

        private bool isDragging = false;
        private bool isResize = false;

        private Point oldPoint = new Point(0, 0);

        private List<Shape> controlShapes = new List<Shape>(1);

        public DrawingCanvas()
        {
            InitializeComponent();
        }

        public bool CanSelectShapes { get; set; }

        public Shape SelectedShape
        {
            get => selectedShape;
            private set
            {
                selectedShape = value;

                foreach (Shape shape in controlShapes)
                {
                    this.Canvas.Children.Remove(shape);
                }
                controlShapes.Clear();

                if (selectedShape != null)
                {
                    GetControlPoints(selectedShape);
                }
            }
        }

        private void GetControlPoints(Shape shape)
        {
            Rectangle rect = new Rectangle()
            {
                Width = shape.Width,
                Height = shape.Height,
                Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                StrokeDashArray = new DoubleCollection() { 4, 4 }
            };
            rect.SetCanvasPoint(shape.GetCanvasPoint());
            controlShapes.Add(rect);
            this.Canvas.Children.Add(rect);
            decoRect = rect;

            foreach (Shape shape1 in shape.GetShapeControlPoints())
            {
                this.Canvas.Children.Add(shape1);
                controlShapes.Add(shape1);
                resizeShapes.Add(shape1);
            }

            GetMoveControlPoint(shape);
        }

        private void GetMoveControlPoint(Shape shape)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 10;
            rect.Height = 10;
            rect.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            rect.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            Canvas.SetLeft(rect, Canvas.GetLeft(selectedShape) + selectedShape.Width / 2d - rect.Width / 2d);
            Canvas.SetTop(rect, Canvas.GetTop(selectedShape) + selectedShape.Height / 2d - rect.Height / 2d);
            controlShapes.Add(rect);
            this.Canvas.Children.Add(rect);
            moveShape = rect;
        }

        public void AddShape(Shape shape)
        {
            this.Canvas.Children.Add(shape);
        }

        public void ResetSelectedShape()
        {
            SelectedShape = null;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!CanSelectShapes)
                return;

            foreach (UIElement element in this.Canvas.Children)
            {
                if (element is Shape shape)
                {
                    HitTestResult res = VisualTreeHelper.HitTest(this.Canvas, e.GetPosition(this.Canvas));
                    if (res != null && res.VisualHit is Shape clickedShape)
                    {
                        if (controlShapes.Contains(clickedShape))
                        {
                            if (clickedShape == moveShape)
                            {
                                this.Cursor = Cursors.SizeAll;
                                isDragging = true;
                            }
                            else if (resizeShapes.Contains(clickedShape))
                            {
                                isResize = true;
                                OnResizePointClicked(clickedShape);
                            }
                            oldPoint = e.GetPosition(Canvas);
                            return;
                        }

                        this.SelectedShape = clickedShape;
                        break;
                    }
                }
            }
        }

        private void OnResizePointClicked(Shape shape)
        {
            switch (shape.Name)
            {
                case "TOP":
                    this.Cursor = Cursors.ScrollN;
                    break;
                case "BOTTOM":
                    this.Cursor = Cursors.ScrollS;
                    break;
                case "LEFT":
                    this.Cursor = Cursors.ScrollW;
                    break;
                case "RIGHT":
                    this.Cursor = Cursors.ScrollE;
                    break;
                case "TOPLEFT":
                    this.Cursor = Cursors.ScrollNW;
                    break;
                case "TOPRIGHT":
                    this.Cursor = Cursors.ScrollNE;
                    break;
                case "BOTTOMLEFT":
                    this.Cursor = Cursors.ScrollSW;
                    break;
                case "BOTTOMRIGHT":
                    this.Cursor = Cursors.ScrollSE;
                    break;
                default:
                    throw new Exception("Неизвесная точка");

            }
        }


        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            isResize = false;
            isDragging = false;
            this.Cursor = Cursors.Arrow;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isResize = false;
            isDragging = false;
            this.Cursor = Cursors.Arrow;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point pos = e.GetPosition(Canvas);

                selectedShape.Offset(pos.X - oldPoint.X, pos.Y - oldPoint.Y);

                foreach (Shape shapes in controlShapes)
                {
                    shapes.Offset(pos.X - oldPoint.X, pos.Y - oldPoint.Y);
                }

                oldPoint = pos;
            }  
            else if (isResize)
            {

            }
        }   
    }
}
