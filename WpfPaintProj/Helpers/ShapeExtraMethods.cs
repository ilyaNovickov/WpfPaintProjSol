using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfPaintProj.Helpers
{
    internal static class ShapeExtraMethods
    {
        public static void Offset(this Shape shape, double dx, double dy)
        {
            Canvas.SetLeft(shape, Canvas.GetLeft(shape) + dx);
            Canvas.SetTop(shape, Canvas.GetTop(shape) + dy);
        }

        public static void SetCanvasPoint(this Shape shape, double x, double y)
        {
            Canvas.SetLeft(shape, x);
            Canvas.SetTop(shape, y);
        }

        public static void SetCanvasPoint(this Shape shape, Point point)
        {
            Canvas.SetLeft(shape, point.X);
            Canvas.SetTop(shape, point.Y);
        }

        public static Point GetCanvasPoint(this Shape shape)
        {
            return new Point(Canvas.GetLeft(shape), Canvas.GetTop(shape));
        }

        public static IEnumerable<Shape> GetEllipseControlPoints(this Ellipse ellipse)
        {
            return GetRectangleControlPoints(new Rect(ellipse.GetCanvasPoint().X, 
                ellipse.GetCanvasPoint().Y, ellipse.Width, ellipse.Height));
        }

        public static IEnumerable<Shape> GetRectangleControlPoints(this Rectangle rect)
        {
            return GetRectangleControlPoints(new Rect(rect.GetCanvasPoint().X,
                rect.GetCanvasPoint().Y, rect.Width, rect.Height));
        }

        public static IEnumerable<Shape> GetShapeControlPoints(this Shape shape)
        {
            return GetRectangleControlPoints(new Rect(shape.GetCanvasPoint().X,
                shape.GetCanvasPoint().Y, shape.Width, shape.Height));
        }

        private static IEnumerable<Shape> GetRectangleControlPoints(Rect rectangle)
        {
            Dictionary<string, Point> points = new Dictionary<string, Point>
            {
                { "TOPLEFT", new Point(rectangle.X, rectangle.Y) },

                { "TOP", new Point(rectangle.X + rectangle.Width / 2d, rectangle.Y) },
                { "TOPRIGHT", new Point(rectangle.X + rectangle.Width, rectangle.Y) },

                { "LEFT", new Point(rectangle.X, rectangle.Y + rectangle.Height / 2d) },
                { "BOTTOMLEFT", new Point(rectangle.X, rectangle.Y + rectangle.Height) },

                { "BOTTOM", new Point(rectangle.X + rectangle.Width / 2d, rectangle.Y + rectangle.Height) },
                { "BOTTOMRIGHT", new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height) },
                { "RIGHT", new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height / 2d) },
            };

            List<Shape> shapes = new List<Shape>(1);

            foreach (KeyValuePair<string, Point> pair in points)
            {
                Rectangle rect = new Rectangle()
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0)),
                    Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0))
                };
                rect.Name = pair.Key;
                rect.SetCanvasPoint(pair.Value.X - 5, pair.Value.Y - 5);

                shapes.Add(rect);
            }

            return shapes;
        }

        //public static IEnumerable<Shape> GetRectangleControlPoints(this Rectangle rectangle)
        //{
        //    List<Point> points = new List<Point>()
        //    {
        //        new Point(rectangle.GetCanvasPoint().X, rectangle.GetCanvasPoint().Y),

        //        new Point(rectangle.GetCanvasPoint().X + rectangle.Width / 2d, rectangle.GetCanvasPoint().Y),
        //        new Point(rectangle.GetCanvasPoint().X + rectangle.Width, rectangle.GetCanvasPoint().Y),

        //        new Point(rectangle.GetCanvasPoint().X, rectangle.GetCanvasPoint().Y + rectangle.Height / 2d),
        //        new Point(rectangle.GetCanvasPoint().X, rectangle.GetCanvasPoint().Y + rectangle.Height),

        //        new Point(rectangle.GetCanvasPoint().X + rectangle.Width / 2d, rectangle.GetCanvasPoint().Y + rectangle.Height),
        //        new Point(rectangle.GetCanvasPoint().X + rectangle.Width, rectangle.GetCanvasPoint().Y + rectangle.Height),
        //        new Point(rectangle.GetCanvasPoint().X + rectangle.Width, rectangle.GetCanvasPoint().Y + rectangle.Height / 2d),
        //    };

        //    List<Shape> shapes = new List<Shape>(1);

        //    foreach (Point point in points)
        //    {
        //        Rectangle rect = new Rectangle()
        //        {
        //            Width = 10,
        //            Height = 10,
        //            Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0)),
        //            Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0))

        //        };

        //        rect.SetCanvasPoint(point.X - 5, point.Y - 5);

        //        shapes.Add(rect);
        //    }

        //    return shapes;
        //}
    }
}
