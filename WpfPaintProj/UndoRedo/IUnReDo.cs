﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPaintProj.UndoRedo
{
    public interface IUnReDo
    {

        void Invoke();
        event EventHandler InversedActionInvoked;
    }
}
