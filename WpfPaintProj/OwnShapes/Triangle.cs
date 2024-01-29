using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace WpfPaintProj.OwnShapes
{
    public class Triangle : Shape
    {
        private Geometry geometry;

        public Triangle()
        {
            this.StrokeThickness = 1;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                CacheDefiningGeometry();

                return geometry;
            }
        }

        private void CacheDefiningGeometry()
        {
            PointCollection points = new PointCollection()
            {
                new Point(Width / 2d, 0),
                new Point(Width, Height),
                new Point(0, Height),
            };
            PathFigure pathFigure = new PathFigure();
            if (points == null)
            {
                geometry = Geometry.Empty;
                return;
            }

            if (points.Count > 0)
            {
                pathFigure.StartPoint = points[0];
                if (points.Count > 1)
                {
                    Point[] array = new Point[points.Count - 1];
                    for (int i = 1; i < points.Count; i++)
                    {
                        array[i - 1] = points[i];
                    }

                    pathFigure.Segments.Add(new PolyLineSegment(array, isStroked: true));
                }

                pathFigure.IsClosed = true;
            }

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            pathGeometry.FillRule = FillRule.EvenOdd;
            geometry = pathGeometry;
        }
    }
}
