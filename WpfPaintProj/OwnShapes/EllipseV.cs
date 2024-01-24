using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace WpfPaintProj.OwnShapes
{
    internal class EllipseV : Shape
    {
        EllipseGeometry ellipse;
        public static readonly DependencyProperty TextBoxRProperty = DependencyProperty.Register("TextBoxR", typeof(TextBox), typeof(EllipseV), new FrameworkPropertyMetadata(null));
        public TextBox TextBox
        {
            get { return (TextBox)GetValue(TextBoxRProperty); }
            set { SetValue(TextBoxRProperty, value); }
        }
        public EllipseV()
        {
            ellipse = new EllipseGeometry();

            this.Stroke = Brushes.Gray;
            this.StrokeThickness = 3;
        }
        protected override Geometry DefiningGeometry
        {
            get
            {
                TranslateTransform t = new TranslateTransform(ActualWidth / 2, ActualHeight / 2);
                ellipse.Transform = t;
                ellipse.RadiusX = this.ActualWidth / 2;
                ellipse.RadiusY = this.ActualHeight / 2;
                return ellipse;
            }
        }
    }
}
