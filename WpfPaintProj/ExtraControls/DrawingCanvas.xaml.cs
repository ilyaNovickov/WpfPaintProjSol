﻿using System;
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

        private List<Shape> controlShapes = new List<Shape>(1);

        public DrawingCanvas()
        {
            InitializeComponent();
        }

        public bool CanSelectShapes { get; set; }

        public Shape SelectedShape
        {
            get => selectedShape;
            private set
            {
                selectedShape = value;

                foreach (Shape shape in controlShapes)
                {
                    this.Canvas.Children.Remove(shape);
                }
                controlShapes.Clear();

                if (selectedShape != null)
                {
                    List<Point> points = GetControlPoints(selectedShape);

                    Rectangle rect = new Rectangle();
                    rect.Width = 10;
                    rect.Height = 10;
                    rect.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    rect.Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    Canvas.SetLeft(rect, Canvas.GetLeft(selectedShape) + selectedShape.Width / 2d -  rect.Width / 2d);
                    Canvas.SetTop(rect, Canvas.GetTop(selectedShape) + selectedShape.Height / 2d - rect.Height / 2d);

                    controlShapes.Add(rect);

                    this.Canvas.Children.Add(rect);
                }
            }
        }

        public void AddShape(Shape shape)
        {
            this.Canvas.Children.Add(shape);
        }

        public void ResetSelectedShape()
        {
            SelectedShape = null;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!CanSelectShapes)
                return;


            

            foreach (UIElement element in this.Canvas.Children)
            {
                if (element is Shape shape)
                {
                    HitTestResult res = VisualTreeHelper.HitTest(this.Canvas, e.GetPosition(this.Canvas));
                    if (res != null)
                    {
                        if (controlShapes.Contains((Shape)res.VisualHit))
                            return;

                        this.SelectedShape = (Shape)res.VisualHit;
                        break;
                    }
                }
            }
        }

        private List<Point> GetControlPoints(Shape shape)
        {
            List<Point> points = new List<Point>(1);

            Point point = new Point();

            point.X = Canvas.GetLeft(shape);
            point.X = Canvas.GetTop(shape);

            points.Add(point);

            return points;
        }
    }
}
