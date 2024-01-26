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

        private bool isDragging = false;

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
                    GetMoveControlPoint(selectedShape);

                    //List<Point> points = GetControlPoints(selectedShape);
                    Rectangle rect = new Rectangle()
                    {
                        Width = selectedShape.Width,
                        Height = selectedShape.Height,
                        Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                        StrokeDashArray = new DoubleCollection() { 4, 4 }
                    };
                    rect.SetCanvasPoint(selectedShape.GetCanvasPoint());
                    controlShapes.Add(rect);
                    this.Canvas.Children.Add(rect);

                    foreach (Shape shape in selectedShape.GetShapeControlPoints())
                    {
                        this.Canvas.Children.Add(shape);
                        controlShapes.Add(shape);
                        resizeShapes.Add(shape);
                    }

                }
            }
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
                    if (res != null)
                    {
                        if (controlShapes.Contains((Shape)res.VisualHit))
                        {
                            if (((Shape)res.VisualHit) == moveShape)
                                isDragging = true;
                            else if (resizeShapes.Contains((Shape)res.VisualHit))
                                return;
                            oldPoint = e.GetPosition(Canvas);
                            return;
                        }

                        this.SelectedShape = (Shape)res.VisualHit;
                        break;
                    }
                }
            }
        }

        private List<Point> GetControlPoints(Shape shape)
        {
            List<Point> points = new List<Point>(1);

            Point point = new Point();

            point.X = Canvas.GetLeft(shape);
            point.X = Canvas.GetTop(shape);

            points.Add(point);

            return points;
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging)
                return;

            Point pos = e.GetPosition(Canvas);

            selectedShape.Offset(pos.X - oldPoint.X, pos.Y - oldPoint.Y);

            foreach (Shape shapes in controlShapes)
            {
                shapes.Offset(pos.X - oldPoint.X, pos.Y - oldPoint.Y);
            }

            oldPoint = pos;
        }   
    }
}
