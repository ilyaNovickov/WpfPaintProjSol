using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPaintProj.UndoRedo
{
    public abstract class DoBase<T, K> : IUnReDo where T : Delegate where K : struct
    {
        protected T action;
        protected T inverseAction;
        protected K args;

        public DoBase(T action, T inverseAction, K args) 
        {
            this.action = action;
            this.inverseAction = inverseAction;
            this.args = args;
        }

        public event EventHandler InversedActionInvoked;

        public T Action { get { return action; } }
        public T InverseAction { get { return inverseAction; } }
        public K Args { get { return args; } }

        public void Invoke()
        {
            _Invoke();
            InversedActionInvoked?.Invoke(this, EventArgs.Empty);
        }

        protected abstract void _Invoke();

    }
}
