using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPaintProj.UndoRedo
{
    public abstract class DoBase
    {
        protected Delegate action;
        protected Delegate inverseAction;
        protected ValueType args;

        public DoBase(Delegate action, Delegate inverseAction, ValueType args) 
        {
            this.action = action;
            this.inverseAction = inverseAction;
            this.args = args;
        }

        public event EventHandler InversedActionInvoked;

        public Delegate Action { get { return action; } }
        public Delegate InverseAction { get { return inverseAction; } }
        public ValueType Args { get { return args; } }

        public void Invoke()
        {
            _Invoke();
            InversedActionInvoked?.Invoke(this, EventArgs.Empty);
        }

        protected abstract void _Invoke();

    }
}
