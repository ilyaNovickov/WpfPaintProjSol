using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPaintProj.ExtraControls;

namespace WpfPaintProj.UndoRedo
{ 
    class RemoveDoRe : IUnReDo
    {
        public Layer Owner { get; private set; }
        public AddRemoveDo Args { get; private set; }
        public RemoveDoRe(Layer layer, AddRemoveDo args)
        {
            Args = args;
            Owner = layer;
        }
        public void Invoke()
        {
            Owner.__AddShape(Args.Shape);
        }

        public IUnReDo GetInversedAction()
        {
            return new AddDoRe(this.Owner, this.Args);
        }
    }
}
