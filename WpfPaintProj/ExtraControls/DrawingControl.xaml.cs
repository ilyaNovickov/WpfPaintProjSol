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
        private ObservableCollection<DrawingCanvas> drawingCanvas = new ObservableCollection<DrawingCanvas>();

        public DrawingControl()
        {
            InitializeComponent();
        }
        #region ForCollection
        public ObservableCollection<DrawingCanvas> Layers
        {
            get => drawingCanvas;
        }

        public void AddLayer()
        {
            drawingCanvas.Add(new DrawingCanvas());
        }

        public void AddLayer(DrawingCanvas canvas)
        {
            drawingCanvas.Add(canvas);
        }

        public void RemoveLayer(DrawingCanvas canvas)
        {
            drawingCanvas.Remove(canvas);
        }

        public void RemoveLayerAt(int index)
        {
            drawingCanvas.RemoveAt(index);
        }
        #endregion
    }
}
