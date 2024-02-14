using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPaintProj.ExtraControls;

namespace WpfPaintProj.UndoRedo
{
    class UndoRedoManager
    {
        private Stack<IUnReDo> undo = new Stack<IUnReDo>(1);
        private Stack<IUnReDo> redo = new Stack<IUnReDo>(1);

        public Layer Owner { get; private set; }

        public UndoRedoManager(Layer layer)
        {
            Owner = layer;
        }

        public void Undo()
        {
            if (undo.Count == 0)
                return;

            IUnReDo action = undo.Pop();

            action.Invoke();

            redo.Push(action.GetInversedAction());
        }

        public void Redo()
        {
            if (redo.Count == 0)
                return;

            IUnReDo action = redo.Pop();

            action.Invoke();

            undo.Push(action.GetInversedAction());
        }

        public void RegistrAction(IUnReDo action)
        {
            undo.Push(action);
            redo.Clear();
        }

    }

    

    
}
