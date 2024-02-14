using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using WpfPaintProj.ExtraControls;
using WpfPaintProj.Helpers;

namespace WpfPaintProj.UndoRedo
{
    class ResizeDoRe : IUnReDo
    {
        public Layer Owner { get; private set; }
        public ResizeDo Args { get; private set; }

        public ResizeDoRe(Layer layer, ResizeDo args)
        {
            Args = args;
            Owner = layer;
        }

        public void Invoke()
        {
            Args.Shape.SetCanvasPoint(Args.OldPosition);
            Args.Shape.Width = Args.OldSize.Width;
            Args.Shape.Height = Args.OldSize.Height;
        }

        public IUnReDo GetInversedAction()
        {
            return new ResizeDoRe(this.Owner, 
                new ResizeDo(Args.Shape, 
                Args.NewPosition, Args.OldPosition,
                Args.NewSize, Args.OldSize));
        }
    }
}
