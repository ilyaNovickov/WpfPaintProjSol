using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPaintProj.ExtraControls;
using System.Windows;
using System.Windows.Shapes;

namespace WpfPaintProj.UndoRedo
{
    struct MoveDo
    {
        public MoveDo(Shape shape, Point old, Point point)
        {
            Shape = shape;
            OldPosition = old;
            NewPosition = point;
        }
        public Shape Shape { get; }

        public Point OldPosition { get; }

        public Point NewPosition { get; }
    }
}
