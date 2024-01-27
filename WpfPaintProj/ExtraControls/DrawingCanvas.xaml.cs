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
        private ResizeDirection resizeDirection = ResizeDirection.None;

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
                resizeShapes.Clear();

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
                Fill = Brushes.Transparent,
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

            Canvas.CaptureMouse();

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
                        return;
                    }
                }
            }

            SelectedShape = null;
        }

        private void OnResizePointClicked(Shape shape)
        {
            switch (shape.Name)
            {
                case "TOP":
                    this.Cursor = Cursors.ScrollN;
                    resizeDirection = ResizeDirection.Top;
                    break;
                case "BOTTOM":
                    this.Cursor = Cursors.ScrollS;
                    resizeDirection = ResizeDirection.Bottom;
                    break;
                case "LEFT":
                    this.Cursor = Cursors.ScrollW;
                    resizeDirection = ResizeDirection.Left;
                    break;
                case "RIGHT":
                    this.Cursor = Cursors.ScrollE;
                    resizeDirection = ResizeDirection.Right;
                    break;
                case "TOPLEFT":
                    this.Cursor = Cursors.ScrollNW;
                    resizeDirection = ResizeDirection.TopLeft;
                    break;
                case "TOPRIGHT":
                    this.Cursor = Cursors.ScrollNE;
                    resizeDirection = ResizeDirection.TopRight;
                    break;
                case "BOTTOMLEFT":
                    this.Cursor = Cursors.ScrollSW;
                    resizeDirection = ResizeDirection.BottomLeft;
                    break;
                case "BOTTOMRIGHT":
                    this.Cursor = Cursors.ScrollSE;
                    resizeDirection = ResizeDirection.BottomRight;
                    break;
                default:
                    throw new Exception("Неизвесная точка");

            }
        }


        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Canvas)
            {
                
            }    
            else if (sender is Shape)
            {

                
            }
            isResize = false;
            isDragging = false;
            this.Cursor = Cursors.Arrow;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Canvas.ReleaseMouseCapture(); 

            isResize = false;
            isDragging = false;
            this.Cursor = Cursors.Arrow;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            

            void foo()
            {
                int index = 0;
                foreach (KeyValuePair<string, Point> pair in selectedShape.GetPointsofBorderControlPoints())
                {
                    resizeShapes[index].SetCanvasCenterPoint(pair.Value.X, pair.Value.Y);
                    index++;
                }
                Canvas.SetLeft(moveShape, Canvas.GetLeft(selectedShape) + selectedShape.Width / 2d - 5);
                Canvas.SetTop(moveShape, Canvas.GetTop(selectedShape) + selectedShape.Height / 2d - 5);
                decoRect.SetCanvasPoint(selectedShape.GetCanvasPoint().X, selectedShape.GetCanvasPoint().Y);
                decoRect.Width = selectedShape.Width;
                decoRect.Height = selectedShape.Height;
            }

            if (isDragging)
            {
                Point pos = e.GetPosition(Canvas);

                MoveSelectedShape(pos.X - oldPoint.X, pos.Y - oldPoint.Y);

                oldPoint = pos;
            }  
            else if (isResize)
            {
                Point pos = e.GetPosition(Canvas);

                double dx = pos.X - oldPoint.X;
                double dy = pos.Y - oldPoint.Y;

                try
                {
                    switch (this.resizeDirection)
                    {
                        case ResizeDirection.Top:
                            selectedShape.Height -= dy;
                            selectedShape.Offset(0, dy);
                            foo();
                            break;
                        case ResizeDirection.Bottom:
                            break;
                        case ResizeDirection.Left:
                            break;
                        case ResizeDirection.Right:
                            break;
                        case ResizeDirection.TopRight:
                            break;
                        case ResizeDirection.BottomRight:
                            break;
                        case ResizeDirection.TopLeft:
                            break;
                        case ResizeDirection.BottomLeft:
                            break;
                        case ResizeDirection.None:
                        default:
                            break;
                    }
                }
                catch { }

                oldPoint = pos;
            }
        }

        private void MoveSelectedShape(double dx, double dy)
        {
            selectedShape.Offset(dx, dy);

            foreach (Shape shapes in controlShapes)
            {
                shapes.Offset(dx, dy);
            }
        }

        //private void Canvas_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (selectedShape == null)
        //        return;

        //    double dx = 0;
        //    double dy = 0;

        //    if (e.Key == Key.Up)
        //    {
        //        dy += 1d;
        //    }
        //    if (e.Key == Key.Down)
        //    {
        //        dy -= 1d;
        //    }
        //    if (e.Key == Key.Left)
        //    {
        //        dx += 1d;
        //    }
        //    if (e.Key == Key.Right)
        //    {
        //        dx -= 1d;
        //    }

        //    MoveSelectedShape(dx, dy);
        //}
    }
}
