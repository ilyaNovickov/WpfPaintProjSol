using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfPaintProj.OwnShapes
{
    public interface IControlPoints
    {
        List<Point> ControlPoints { get; }
    }
}
