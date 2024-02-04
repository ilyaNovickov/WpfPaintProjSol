using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPaintProj.UndoRedo
{
    public class RemoveDoAction : DoBase<AddRemoveDelegate, AddRemoveDoArgs>
    {
        public RemoveDoAction(AddRemoveDelegate action, AddRemoveDelegate inverseAction, AddRemoveDoArgs args) : base(action, inverseAction, args)
        {

        }

        protected override void _Invoke()
        {
            inverseAction.Invoke(args.Shape);
        }

        public override IUnReDo GetInversedAction()
        {
            return new AddDoAction(this.InverseAction, this.Action, this.Args);
        }
    }
}
