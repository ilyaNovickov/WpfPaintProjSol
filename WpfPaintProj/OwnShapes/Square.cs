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
    public class Square : Shape
    {
        private Geometry geometry;

        public Square()
        {
            this.StrokeThickness = 1;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {                
                return new RectangleGeometry(new Rect(0, 0, Width, Height));
            }
        }
    }
}
