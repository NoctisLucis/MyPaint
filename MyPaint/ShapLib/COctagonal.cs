using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using MyPaint;
namespace ShapesLib
{
    class COctagonal: CShape
   {
        private Polygon m_Octagonal;

        public COctagonal() { }

        private void DrawOctagonal(Canvas cvs, Point Spt)
        {
            m_Octagonal = new Polygon();
            m_Octagonal.Stroke = m_Stroke;
            m_Octagonal.Fill = m_Fill;
            m_Octagonal.StrokeThickness = m_ThinkNess;
            m_Octagonal.StrokeDashArray = m_Dash;
            
            m_Spt = Spt;
            Canvas.SetLeft(m_Octagonal, Spt.X);
            Canvas.SetTop(m_Octagonal, Spt.Y);

            cvs.Children.Add(m_Octagonal);    
        }

        public override void DrawDown(Canvas cvs, Point Spt)
        {
            base.DrawDown(cvs, Spt);
            DrawOctagonal(cvs, Spt);
        }

        public override void AddAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null)
            {
                Adorner ad = new RectangleAdorner(this.m_Octagonal);
                adLayer.Add(ad);
            }
        }

        public override void RemoveAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null && m_Octagonal != null)
            {
                Adorner[] ad = adLayer.GetAdorners(m_Octagonal);
                if (ad != null)
                    adLayer.Remove(ad[0]);
            }
        }

        public override void DrawMove(Canvas cvs, Point ept)
        {
            base.DrawMove(cvs, ept);
            if (m_Octagonal == null) return;

            var x = Math.Min(ept.X, m_Spt.X);
            var y = Math.Min(ept.Y, m_Spt.Y);

            var w = Math.Max(ept.X, m_Spt.X) - x;
            var h = Math.Max(ept.Y, m_Spt.Y) - y;

            m_Octagonal.Width = w;
            m_Octagonal.Height = h;

            Canvas.SetLeft(m_Octagonal, x);
            Canvas.SetTop(m_Octagonal, y);

            Point o1 = new Point(w*0.3, 0);
            Point o2 = new Point(w*0.7, 0);
            Point o3 = new Point(w, h*0.3);
            Point o4 = new Point(w, h*0.7);
            Point o5 = new Point(w*0.7, h);
            Point o6 = new Point(w*0.3, h);
            Point o7 = new Point(0, h*0.7);
            Point o8 = new Point(0, h*0.3);

            PointCollection oct = new PointCollection();
            oct.Add(o1);
            oct.Add(o2);
            oct.Add(o3);
            oct.Add(o4);
            oct.Add(o5);
            oct.Add(o6);
            oct.Add(o7);
            oct.Add(o8);

            m_Octagonal.Points = oct;
            m_Octagonal.Stretch = Stretch.Fill;
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Octagonal);
        }

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Octagonal);
        }

        public override void ChangeColor(SolidColorBrush color1, LinearGradientBrush color2)
        {
            base.ChangeColor(color1, color2);
            m_Octagonal.Stroke = color1;
            m_Octagonal.Fill = color2;
        }
    }
}
