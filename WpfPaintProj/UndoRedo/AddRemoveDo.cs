using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace WpfPaintProj.UndoRedo
{
    struct AddRemoveDo
    {
        public AddRemoveDo(Shape shape)
        {
            Shape = shape;
        }
        public Shape Shape { get; }
    }
}
