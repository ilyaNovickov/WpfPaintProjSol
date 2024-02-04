using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace WpfPaintProj.UndoRedo.Move
{
    public delegate void MoveDoDelegate(Point point);

    public class MoveDoAction : DoBase<MoveDoDelegate, MoveDoArgs>
    {
        public MoveDoAction(MoveDoDelegate action, MoveDoDelegate rev, MoveDoArgs point) : base(action, rev, point)
        {

        }
        protected override void _Invoke()
        {
            this.action.Invoke(args.OldPoint);
        }

        public override IUnReDo GetInversedAction()
        {
            return new MoveDoAction(this.action, this.inverseAction, 
                new MoveDoArgs() { CurrentPoint = this.args.OldPoint, OldPoint = this.args.CurrentPoint });
        }
    }

    public struct MoveDoArgs
    {
        public Point CurrentPoint { get; set; }
        public Point OldPoint { get; set; }
    }
}
