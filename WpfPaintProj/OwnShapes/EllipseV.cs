﻿using System;
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
        EllipseGeometry ellipse = new EllipseGeometry();

        public EllipseV()
        {
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
