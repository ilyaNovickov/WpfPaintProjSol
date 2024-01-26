using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
    }
}
