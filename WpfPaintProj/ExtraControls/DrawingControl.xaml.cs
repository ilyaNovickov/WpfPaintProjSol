using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPaintProj.ExtraControls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class DrawingControl : UserControl
    {
        private ObservableCollection<Layer> drawingCanvas = new ObservableCollection<Layer>();

        public DrawingControl()
        {
            InitializeComponent();
        }
        #region ForCollection
        public ObservableCollection<Layer> Layers
        {
            get => drawingCanvas;
        }

        public void AddLayer()
        {
            drawingCanvas.Add(new Layer());
            this.canvas.Children.Add(drawingCanvas.Last()) ;
        }

        public void AddLayer(Layer canvas)
        {
            drawingCanvas.Add(canvas);
            this.canvas.Children.Add(canvas);
        }

        public void RemoveLayer(Layer canvas)
        {
            drawingCanvas.Remove(canvas);
            this.canvas.Children.Remove(canvas);
        }

        public void RemoveLayerAt(int index)
        {
            drawingCanvas.RemoveAt(index);
            this.canvas.Children.RemoveAt(index);
        }
        #endregion
    }
}
