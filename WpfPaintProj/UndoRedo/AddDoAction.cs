using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace WpfPaintProj.UndoRedo
{
    public delegate void AddRemoveDelegate(Shape shape);

    public class AddDoAction : DoBase<AddRemoveDelegate, AddRemoveDoArgs>
    {
        public AddDoAction(AddRemoveDelegate action, AddRemoveDelegate inverseAction, AddRemoveDoArgs args) : base(action, inverseAction, args)
        {
            
        }

        protected override void _Invoke()
        {
            inverseAction.Invoke(args.Shape);
        }

        public override IUnReDo GetInversedAction()
        {
            return new RemoveDoAction(this.InverseAction, this.Action, this.Args);
        }
    }

    public struct AddRemoveDoArgs
    {
        public Shape Shape { get; set; }
    }
}
