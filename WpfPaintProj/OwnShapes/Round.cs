using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;

namespace WpfPaintProj.OwnShapes
{
    public class Round: Shape
    {
        private EllipseGeometry geometry;

        public Round()
        {
            this.StrokeThickness = 1;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                TranslateTransform t = new TranslateTransform(ActualWidth / 2, ActualHeight / 2);
                geometry = new EllipseGeometry();
                geometry.Transform = t;
                geometry.RadiusX = this.ActualWidth / 2;
                geometry.RadiusY = this.ActualHeight / 2;
                return geometry;
            }
        }
    }
}
