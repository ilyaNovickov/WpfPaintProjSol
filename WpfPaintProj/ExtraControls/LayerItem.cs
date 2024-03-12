using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPaintProj.ExtraControls
{
    public class LayerItem
    {
        public LayerItem(string name, Layer layer) 
        { 
            Name = name;
            Layer = layer;
        }

        public string Name { get; set; }

        public Layer Layer { get; }
    }
}
