using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfPaintProj.Helpers;
using WpfPaintProj.UndoRedo;


namespace WpfPaintProj.ExtraControls
{
    public class Layer : Canvas
    {
        private ObservableCollection<ShapeItem> shapes = new ObservableCollection<ShapeItem>();

        public ObservableCollection<ShapeItem> Shapes
        {
            get => shapes;
        }

        //Выбранная фигура
        private ShapeItem selectedShape = new ShapeItem(null);

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

            undoManager = new UndoRedoManager(this);
        }

        public bool CanSelectShapes { get; set; }

        public Shape SelectedShape
        {
            get => selectedShape != null ? selectedShape.Shape : null;
            set
            {
                Shape prevValue = selectedShape.Shape;

                if (value == selectedShape.Shape)
                    return;
                //selectedShape.Shape = value;

                //Очистка от вспомогательных фигур
                foreach (Shape shape in controlShapes)
                {
                    this.Children.Remove(shape);
                }
                controlShapes.Clear();
                resizeShapes.Clear();

                selectedShape.Shape = value;//Shapes[this.Children.IndexOf(value)];

                if (SelectedShape != null)
                {
                    GetControlPoints(SelectedShape);
                }

                //if (prevValue != value)
                    SelectedShapeChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler SelectedShapeChange;

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
            Canvas.SetLeft(rect, Canvas.GetLeft(SelectedShape) + SelectedShape.Width / 2d - rect.Width / 2d);
            Canvas.SetTop(rect, Canvas.GetTop(SelectedShape) + SelectedShape.Height / 2d - rect.Height / 2d);
            controlShapes.Add(rect);
            this.Children.Add(rect);
            moveShape = rect;
        }
        #region AddRemoveMethods
        public void AddShape(Shape shape)
        {
            _AddShape(shape);
        }

        internal void __AddShape(Shape shape)
        {
            this.Children.Add(shape);
            ShapeItem shapeItem = new ShapeItem(shape);
            //selectedShape = shapeItem;
            
            this.ShapeMoved += shapeItem.Shape_Moved;
            shapes.Add(shapeItem);
        }

        private void _AddShape(Shape shape)
        {
            __AddShape(shape);
            SelectedShape = shape;
            undoManager.RegistrAction(new AddDoRe(this, new AddRemoveDo(shape)));
            //undoStack.Push(new AddDoAction(this.__AddShape,
            //    this.__RemoveShape, new AddRemoveDoArgs() { Shape = shape }));
            //redoStack.Clear();
            
        }

        public void RemoveShape(Shape shape)
        {
            _RemoveShape(shape);
        }

        internal void __RemoveShape(Shape shape)
        {
            //List<ShapeItem> extra = shapes.ToList();
            
            
            int index = Children.IndexOf(shape);
            if (index == -1)
                return;
            this.ShapeMoved -= shapes.ElementAt(index).Shape_Moved;
            shapes.RemoveAt(index);
            
            this.Children.RemoveAt(index);

            if (SelectedShape == shape)
                SelectedShape = null;
        }

        private void _RemoveShape(Shape shape)
        {
            __RemoveShape(shape);
            undoManager.RegistrAction(new RemoveDoRe(this, new AddRemoveDo(shape)));
            //undoStack.Push(new RemoveDoAction(this.__RemoveShape, 
            //    this.__AddShape, new AddRemoveDoArgs() { Shape = shape }));
            //redoStack.Clear();
            
        }

        public void RemoveSelectedShape()
        {
            RemoveShape(SelectedShape);
            SelectedShape = null;

        }
#endregion

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
                                oldShapePosition = SelectedShape.GetCanvasPoint();
                            }
                            //Попало в точку для изменения размера
                            else if (resizeShapes.Contains(clickedShape))
                            {
                                this.oldSize = new Size(shape.Width, shape.Height);
                                oldShapePosition = SelectedShape.GetCanvasPoint();
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
                if (Canvas.GetRight(SelectedShape) is double.NaN && Canvas.GetBottom(SelectedShape) is double.NaN)
                {
                    this.Children.Remove(SelectedShape);
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

            if (isDragging)
            {
                isDragging = false;

                undoManager.RegistrAction(new MoveDoRe(this, new MoveDo(SelectedShape, oldShapePosition,
                    SelectedShape.GetCanvasPoint())));

                //undoStack.Push(new MoveDoAction(this.MoveShapeTo, 
                //    this.MoveShapeTo, 
                //    new MoveDoArgs() { CurrentPoint = selectedShape.GetCanvasPoint(), 
                //        OldPoint = oldShapePosition, Shape = selectedShape }));
            }
            if (isResize)
            {
                isResize = false;

                undoManager.RegistrAction(new ResizeDoRe(this, new ResizeDo(SelectedShape, oldShapePosition,
                    SelectedShape.GetCanvasPoint(), oldSize, new Size(SelectedShape.Width, SelectedShape.Height))));

                //undoStack.Push(new ResizeDoAction(ResizeSelectedShape, ResizeSelectedShape, new ResizeArgs()
                //{
                //    Direction = this.resizeDirection,
                //    Shape = selectedShape,
                //    NewSize = new Size(selectedShape.Width, selectedShape.Height),
                //    OldSize = this.oldSize
                //}));
            }

            this.Cursor = Cursors.Arrow;
        }

        public event EventHandler<ShapeMovedArgs> ShapeMoved;

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //Перемещение фигуры
            if (isDragging)
            {
                Point pos = e.GetPosition(this);

                MoveSelectedShape(pos.X - oldPoint.X, pos.Y - oldPoint.Y);

                ShapeMoved?.Invoke(this, new ShapeMovedArgs(SelectedShape, SelectedShape.GetCanvasPoint(), oldPoint));

                oldPoint = pos;
            }
            //Изменение размера фигуры
            else if (isResize)
            {
                Point pos = e.GetPosition(this);

                ResizeSelectedShape(SelectedShape, this.resizeDirection, pos.X - oldPoint.X, pos.Y - oldPoint.Y);

                ShapeMoved?.Invoke(this, new ShapeMovedArgs(SelectedShape, SelectedShape.GetCanvasPoint(), oldPoint));

                oldPoint = pos;
            }
        }

        //Перемещение контрольных точек при перемещении
        private void OffsetPointsOnResizing()
        {
            int index = 0;
            foreach (KeyValuePair<string, Point> pair in SelectedShape.GetPointsofBorderControlPoints())
            {
                resizeShapes[index].SetCanvasCenterPoint(pair.Value.X, pair.Value.Y);
                index++;
            }
            moveShape.SetCanvasCenterPoint(Canvas.GetLeft(SelectedShape) + SelectedShape.Width / 2d,
                Canvas.GetTop(SelectedShape) + SelectedShape.Height / 2d);
            decoRect.SetCanvasPoint(SelectedShape.GetCanvasPoint().X, SelectedShape.GetCanvasPoint().Y);
            decoRect.Width = SelectedShape.Width;
            decoRect.Height = SelectedShape.Height;
        }

        //Изменение размера выбранной фигуры
        private void ResizeSelectedShape(Shape shape, ResizeDirection direction, double dx, double dy)
        {
            try
            {
                switch (this.resizeDirection)
                {
                    case ResizeDirection.Top:
                        shape.Height -= dy;
                        shape.Offset(0, dy);
                        break;
                    case ResizeDirection.Bottom:
                        shape.Height += dy;
                        shape.Offset(0, 0);
                        break;
                    case ResizeDirection.Left:
                        shape.Width -= dx;
                        shape.Offset(dx, 0);
                        break;
                    case ResizeDirection.Right:
                        shape.Width += dx;
                        shape.Offset(0, 0);
                        break;
                    case ResizeDirection.TopRight:
                        shape.Width += dx;
                        shape.Height -= dy;
                        shape.Offset(0, dy);
                        break;
                    case ResizeDirection.BottomRight:
                        shape.Width += dx;
                        shape.Height += dy;
                        shape.Offset(0, 0);
                        break;
                    case ResizeDirection.TopLeft:
                        shape.Width -= dx;
                        shape.Height -= dy;
                        shape.Offset(dx, dy);
                        break;
                    case ResizeDirection.BottomLeft:
                        shape.Width -= dx;
                        shape.Height += dy;
                        shape.Offset(dx, 0);
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
            SelectedShape.Offset(dx, dy);

            foreach (Shape shapes in controlShapes)
            {
                shapes.Offset(dx, dy);
            }
        }

        private void MoveShapeTo(Shape shape, Point point)
        {
            if (shape == SelectedShape)
            {
                double dx = point.X - shape.GetCanvasPoint().X;
                double dy = point.Y - shape.GetCanvasPoint().Y;

                foreach (Shape shape1 in this.controlShapes)
                {
                    shape1.Offset(dx, dy);
                }
            }

            shape.SetCanvasPoint(point);    
        }

        private UndoRedoManager undoManager;


        //private bool saveDoStory = true;
        private Point oldShapePosition = new Point(0,0);
        private Size oldSize = new Size(0, 0);

        //private Stack<IUnReDo> undoStack = new Stack<IUnReDo>(1);
        //private Stack<IUnReDo> redoStack = new Stack<IUnReDo>(1);

        public void Undo()
        {
            ResetSelectedShape();
            undoManager.Undo();

            //if (undoStack.Count == 0)
            //    return;
            //IUnReDo action = undoStack.Pop();
            //action.Invoke();
            //redoStack.Push(action.GetInversedAction());
        }

        public void Redo()
        {
            ResetSelectedShape();
            undoManager.Redo();

            //if (redoStack.Count == 0)
            //    return;
            //IUnReDo action = redoStack.Pop();
            //action.Invoke();
            //undoStack.Push(action.GetInversedAction());
        }

    }

    public struct ShapeMovedArgs
    {
        public ShapeMovedArgs(Shape shape, Point newPosition, Point oldPosition)
        {
            Shape = shape;
            NewPosition = newPosition;
            OldPosition = oldPosition;
        }

        public Shape Shape { get; }
        public Point NewPosition { get; }
        public Point OldPosition { get; }
    }
}
