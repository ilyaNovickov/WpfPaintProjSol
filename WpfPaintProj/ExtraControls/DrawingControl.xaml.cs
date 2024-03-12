using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
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
using WpfPaintProj.UndoRedo;

namespace WpfPaintProj.ExtraControls
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class DrawingControl : UserControl, INotifyPropertyChanged
    {
        private List<Layer> layers = new List<Layer>();
        private ObservableCollection<LayerItem> drawingCanvas = new ObservableCollection<LayerItem>();

        private Layer selectedLayer = null;

        public DrawingControl()
        {
            InitializeComponent();
        }

        public Layer SelectedLayer
        {
            get => selectedLayer;
            
            set
            {
                selectedLayer = value;
                OnPropertyChanged("SelectedLayer");
            }
        }

        #region ForCollection
        public ObservableCollection<LayerItem> Layers
        {
            get => drawingCanvas;
        }

        public void AddLayer()
        {
            Layer layer = new Layer();
            layer.Width = 100;
            layer.Height = 100;
            layer.SizeChanged += Layer_SizeChanged;
            this.canvas.Children.Add(layer);//drawingCanvas.Last());
            UpdateSize(layer);
            drawingCanvas.Add(new LayerItem("test" + drawingCanvas.Count, layer));
            layers.Add(layer);
        }

        private void Layer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Layer layer = (Layer)sender;
           
            UpdateSize();
        }

        public void AddLayer(Layer canvas)
        {
            this.canvas.Children.Add(canvas);
            canvas.SizeChanged += Layer_SizeChanged;
            UpdateSize(canvas);
            drawingCanvas.Add(new LayerItem("test" + drawingCanvas.Count, canvas));
            layers.Add(canvas);
        }

        //public void RemoveLayer(Layer canvas)
        //{
        //    drawingCanvas.Remove(canvas);
        //    this.canvas.Children.Remove(canvas);
        //    UpdateSize();
        //}

        //public void RemoveLayerAt(int index)
        //{
        //    drawingCanvas.RemoveAt(index);
        //    this.canvas.Children.RemoveAt(index);
        //    UpdateSize();
        //}

        public void UpdateSize(Layer layer)
        {
            if (layer.Width > canvas.Width)
            {
                canvas.Width = layer.Width;
            }
            if (layer.Height > canvas.Height)
            {
                canvas.Height = layer.Height;
            }
        }

        public void UpdateSize()
        {
            Size maxSize = new Size(0, 0);

            foreach (Layer layer in this.layers)
            {
                if (layer.Width > maxSize.Width)
                {
                    maxSize.Width = layer.Width;
                }
                if (layer.Height > maxSize.Height)
                {
                    maxSize.Height = layer.Height;
                }
            }

            this.canvas.Width = maxSize.Width;
            this.canvas.Height = maxSize.Height;
        }
        #endregion

        #region ForUndo
        public void Undo()
        {
            layers.First().Undo();
        }

        public void Redo()
        {
            layers.First().Redo();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
