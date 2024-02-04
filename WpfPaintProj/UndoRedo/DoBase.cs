using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPaintProj.UndoRedo
{
    public abstract class DoBase
    {
        private Delegate action;
        private Delegate inverseAction;
        private ValueType args;

        public DoBase(Delegate action, Delegate inverseAction, ValueType args) 
        {
            this.action = action;
            this.inverseAction = inverseAction;
            this.args = args;
        }

        public Delegate Action { get { return action; } }
        public Delegate InverseAction { get { return inverseAction; } }
        public ValueType Args { get { return args; } }

        public abstract void Invoke();

    }
}
