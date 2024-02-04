using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace WpfPaintProj.UndoRedo
{
    public delegate void AddRemoveDelegate(Shape shape);

    public class AddDoAction : DoBase<AddRemoveDelegate, AddDoArgs>
    {
        public AddDoAction(AddRemoveDelegate action, AddRemoveDelegate inverseAction, AddDoArgs args) : base(action, inverseAction, args)
        {
            
        }

        protected override void _Invoke()
        {
            inverseAction.Invoke(args.AddedShape);
        }
    }

    public struct AddDoArgs
    {
        public Shape AddedShape { get; set; }
    }
}
