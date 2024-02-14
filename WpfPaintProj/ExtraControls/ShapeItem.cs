using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using WpfPaintProj.Helpers;

namespace WpfPaintProj.ExtraControls
{
    public struct ShapeItem
    {
        //private static int count = 0;

        public ShapeItem(Shape shape)
        {
            //Name = null;
            Shape = shape;
            //count++;
        }

        //public string Name { get; set; }
        public Shape Shape { get; set; }

        public override string ToString()
        {
            return Shape.ToString().Split('.').Last() + $" [{Shape.GetCanvasPoint().X}, {Shape.GetCanvasPoint().Y}]";// + count;
        }
    }
}
