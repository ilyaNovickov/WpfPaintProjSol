using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace WpfPaintProj.UndoRedo
{
    public class AddDoAction : DoBase
    {
        public AddDoAction(Delegate action, Delegate inverseAction, ValueType args) : base(action, inverseAction, args)
        {
            
        }

        protected override void _Invoke()
        {
            ((Action<Shape>)inverseAction).Invoke(((AddDoArgs)args).AddedShape);
        }
    }

    public struct AddDoArgs
    {
        public Shape AddedShape { get; set; }
    }
}
