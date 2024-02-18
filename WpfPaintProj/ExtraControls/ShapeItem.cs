using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using WpfPaintProj.Helpers;

namespace WpfPaintProj.ExtraControls
{
    public class ShapeItem : INotifyPropertyChanged
    {
        //private PropertyChangedEventHandler propertyChanged;
        private string name;
        private Shape shape;

        public ShapeItem(Shape shape)
        {
            Shape = shape;
            Name = GetPosition();
            //propertyChanged = null;
            PropertyChanged = null;
        }

        public Shape Shape 
        {
            get => shape;
            set
            {
                shape = value;
                Name = GetPosition();
            }
        }

        public string Name 
        {
            get => name;
            private set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        //{
        //    add { propertyChanged += value; } 
        //    remove { propertyChanged -= value; }
        //}

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string GetPosition()
        {
            if (shape != null)
                return Shape.ToString().Split('.').Last() + $" [{Shape.GetCanvasPoint().X}, {Shape.GetCanvasPoint().Y}]";
            return null;
        }

        public void Shape_Moved(object sender, ShapeMovedArgs e)
        {
            Name = GetPosition();
        }

        //public override string ToString()
        //{
        //    return Shape.ToString().Split('.').Last() + $" [{Shape.GetCanvasPoint().X}, {Shape.GetCanvasPoint().Y}]";// + count;
        //}
    }
}
