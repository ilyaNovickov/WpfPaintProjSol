using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfPaintProj.Helpers;

namespace WpfPaintProj.ExtraControls
{
    internal class Layer : Canvas
    {
        //Выбранная фигура
        private Shape selectedShape = null;

        //Фигура для перемещения
        private Shape moveShape = null;
        //Фигуры для изменения размера
        private List<Shape> resizeShapes = new List<Shape>(1);
        //Декоративная фигура
        private Shape decoRect = null;

        //Список всех вспомогательных точек
        private List<Shape> controlShapes = new List<Shape>(1);

        //Перемещение фигуры
        private bool isDragging = false;
        //Изменение размера фигуры
        private bool isResize = false;
        //Направление изменения фигуры
        private ResizeDirection resizeDirection = ResizeDirection.None;

        //Прошлое положение курсора мыши
        private Point oldPoint = new Point(0, 0);

        public Layer()
        {
            this.MouseDown += Canvas_MouseDown;
            this.MouseUp += Canvas_MouseUp;
            this.MouseLeave += Canvas_MouseLeave;
            this.MouseMove += Canvas_MouseMove;
        }

        public bool CanSelectShapes { get; set; }

        public Shape SelectedShape
        {
            get => selectedShape;
            private set
            {
                selectedShape = value;

                //Очистка от вспомогательных фигур
                foreach (Shape shape in controlShapes)
                {
                    this.Children.Remove(shape);
                }
                controlShapes.Clear();
                resizeShapes.Clear();

                if (selectedShape != null)
                {
                    GetControlPoints(selectedShape);
                }
            }
        }

        //Получение вспомогательных точек
        private void GetControlPoints(Shape shape)
        {
            //Точка для декора
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
            this.Children.Add(rect);
            decoRect = rect;

            //Точки для изменения размера
            foreach (Shape shape1 in shape.GetShapeControlPoints())
            {
                this.Children.Add(shape1);
                controlShapes.Add(shape1);
                resizeShapes.Add(shape1);
            }

            //Точка для перемещения фигуры
            GetMoveControlPoint(shape);
        }

        //Получение точки полкчения фигуры
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
            this.Children.Add(rect);
            moveShape = rect;
        }

        public void AddShape(Shape shape)
        {
            this.Children.Add(shape);
        }

        public void ResetSelectedShape()
        {
            SelectedShape = null;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!CanSelectShapes)
                return;

            this.CaptureMouse();

            foreach (UIElement element in this.Children)
            {
                if (element is Shape shape)
                {
                    HitTestResult res = VisualTreeHelper.HitTest(this, e.GetPosition(this));
                    if (res != null && res.VisualHit is Shape clickedShape)
                    {
                        if (controlShapes.Contains(clickedShape))
                        {
                            //Попало в точку для перемещения
                            if (clickedShape == moveShape)
                            {
                                this.Cursor = Cursors.SizeAll;
                                isDragging = true;
                            }
                            //Попало в точку для изменения размера
                            else if (resizeShapes.Contains(clickedShape))
                            {
                                isResize = true;
                                OnResizePointClicked(clickedShape);
                            }
                            oldPoint = e.GetPosition(this);
                            return;
                        }

                        this.SelectedShape = clickedShape;
                        return;
                    }
                }
            }

            SelectedShape = null;
        }

        //Получение направления изменения размера
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
            //Удаление объекта вне видимости слоя
            if (isDragging)
            {
                if (Canvas.GetRight(selectedShape) is double.NaN && Canvas.GetBottom(selectedShape) is double.NaN)
                {
                    this.Children.Remove(selectedShape);
                    SelectedShape = null;
                }
            }

            isResize = false;
            isDragging = false;
            this.Cursor = Cursors.Arrow;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.ReleaseMouseCapture();

            isResize = false;
            isDragging = false;
            this.Cursor = Cursors.Arrow;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //Перемещение фигуры
            if (isDragging)
            {
                Point pos = e.GetPosition(this);

                MoveSelectedShape(pos.X - oldPoint.X, pos.Y - oldPoint.Y);

                oldPoint = pos;
            }
            //Изменение размера фигуры
            else if (isResize)
            {
                Point pos = e.GetPosition(this);

                ResizeSelectedShape(pos.X - oldPoint.X, pos.Y - oldPoint.Y);

                oldPoint = pos;
            }
        }

        //Перемещение контрольных точек при перемещении
        private void OffsetPointsOnResizing()
        {
            int index = 0;
            foreach (KeyValuePair<string, Point> pair in selectedShape.GetPointsofBorderControlPoints())
            {
                resizeShapes[index].SetCanvasCenterPoint(pair.Value.X, pair.Value.Y);
                index++;
            }
            moveShape.SetCanvasCenterPoint(Canvas.GetLeft(selectedShape) + selectedShape.Width / 2d,
                Canvas.GetTop(selectedShape) + selectedShape.Height / 2d);
            decoRect.SetCanvasPoint(selectedShape.GetCanvasPoint().X, selectedShape.GetCanvasPoint().Y);
            decoRect.Width = selectedShape.Width;
            decoRect.Height = selectedShape.Height;
        }

        //Изменение размера выбранной фигуры
        private void ResizeSelectedShape(double dx, double dy)
        {
            try
            {
                switch (this.resizeDirection)
                {
                    case ResizeDirection.Top:
                        selectedShape.Height -= dy;
                        selectedShape.Offset(0, dy);
                        break;
                    case ResizeDirection.Bottom:
                        selectedShape.Height += dy;
                        selectedShape.Offset(0, 0);
                        break;
                    case ResizeDirection.Left:
                        selectedShape.Width -= dx;
                        selectedShape.Offset(dx, 0);
                        break;
                    case ResizeDirection.Right:
                        selectedShape.Width += dx;
                        selectedShape.Offset(0, 0);
                        break;
                    case ResizeDirection.TopRight:
                        selectedShape.Width += dx;
                        selectedShape.Height -= dy;
                        selectedShape.Offset(0, dy);
                        break;
                    case ResizeDirection.BottomRight:
                        selectedShape.Width += dx;
                        selectedShape.Height += dy;
                        selectedShape.Offset(0, 0);
                        break;
                    case ResizeDirection.TopLeft:
                        selectedShape.Width -= dx;
                        selectedShape.Height -= dy;
                        selectedShape.Offset(dx, dy);
                        break;
                    case ResizeDirection.BottomLeft:
                        selectedShape.Width -= dx;
                        selectedShape.Height += dy;
                        selectedShape.Offset(dx, 0);
                        break;
                    case ResizeDirection.None:
                    default:
                        break;
                }
                OffsetPointsOnResizing();
            }
            catch { }
        }

        //Перемещение выбранной фигуры
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
