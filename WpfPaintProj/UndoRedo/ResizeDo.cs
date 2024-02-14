using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace WpfPaintProj.UndoRedo
{
    struct ResizeDo
    {
        public ResizeDo(Shape shape, Point old, Point point, Size oldSize, Size newSize)
        {
            Shape = shape;
            OldPosition = old;
            NewPosition = point;
            OldSize = oldSize;
            NewSize = newSize;
        }
        public Shape Shape { get; }

        public Point OldPosition { get; }

        public Point NewPosition { get; }

        public Size OldSize { get; }

        public Size NewSize { get; }
    }
}
