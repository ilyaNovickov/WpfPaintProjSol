using System;
using System.Collections.Generic;
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
    public partial class DrawingCanvas : UserControl
    {
        private Shape selectedShape = null;

        public DrawingCanvas()
        {
            InitializeComponent();
        }

        public Shape SelectedShape
        {
            get => selectedShape;
            private set
            {
                selectedShape = value;
                if (selectedShape != null)
                {

                }
            }
        }

        public void AddShape(Shape shape)
        {
            this.Canvas.Children.Add(shape);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = new Ellipse();


        }
    }
}
