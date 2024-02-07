using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using WpfPaintProj.Helpers;

namespace WpfPaintProj.UndoRedo.Resize
{
    public delegate void ResizeDoDelegate(Shape shape, ResizeDirection direction, double dx, double dy);
    class ResizeDoAction : DoBase<ResizeDoDelegate, ResizeArgs>
    {
        public ResizeDoAction(ResizeDoDelegate action, ResizeDoDelegate rev, ResizeArgs point) : base(action, rev, point)
        {

        }
        protected override void _Invoke()
        {
            double dx = args.NewSize.Width-args.OldSize.Width;
            double dy = args.NewSize.Height - args.OldSize.Height;
            this.action.Invoke(args.Shape, args.Direction, -dx, -dy);
        }

        public override IUnReDo GetInversedAction()
        {
            return new ResizeDoAction(this.action, this.inverseAction,
                new ResizeArgs() { 
                    Direction = this.args.Direction, Shape = args.Shape,
                    OldSize = args.NewSize, NewSize = args.OldSize
                });
        }
    }

    public struct ResizeArgs
    {
        public Shape Shape { get; set; }
        public Size OldSize { get; set; }
        public Size NewSize { get; set; }
        public Point OldPosition { get; set; }
        public Point NewPosition { get; set; }

        public double Dx { get; set; }
        public double Dy { get; set; }
        public ResizeDirection Direction { get; set; }
    }
}
