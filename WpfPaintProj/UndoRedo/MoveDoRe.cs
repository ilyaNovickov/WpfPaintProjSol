using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPaintProj.ExtraControls;
using WpfPaintProj.Helpers;

namespace WpfPaintProj.UndoRedo
{
    class MoveDoRe : IUnReDo
    {
        public Layer Owner { get; private set; }
        public MoveDo Args { get; private set; }

        public MoveDoRe(Layer layer, MoveDo args)
        {
            Args = args;
            Owner = layer;
        }

        public void Invoke()
        {
            Args.Shape.SetCanvasPoint(Args.OldPosition);
        }

        public IUnReDo GetInversedAction()
        {
            return new MoveDoRe(this.Owner, new MoveDo(Args.Shape, Args.NewPosition, Args.OldPosition));
        }
    }
}
