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
    class CTriangle: CShape
    {
        private Polygon m_Triangle;

        public CTriangle() { }

        private void DrawTriangle(Canvas cvs, Point Spt)
        {
            m_Triangle = new Polygon();
            m_Triangle.Stroke = m_Stroke;
            m_Triangle.Fill = m_Fill;
            m_Triangle.StrokeThickness = m_ThinkNess;
            m_Triangle.StrokeDashArray = m_Dash;
            
            m_Spt = Spt;
            Canvas.SetLeft(m_Triangle, Spt.X);
            Canvas.SetTop(m_Triangle, Spt.Y);

            cvs.Children.Add(m_Triangle);    
        }

        public override void DrawDown(Canvas cvs, Point Spt)
        {
            base.DrawDown(cvs, Spt);
            DrawTriangle(cvs, Spt);
        }

        public override void AddAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null)
            {
                Adorner ad = new RectangleAdorner(this.m_Triangle);
                adLayer.Add(ad);
            }
        }

        public override void RemoveAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null && m_Triangle != null)
            {
                Adorner[] ad = adLayer.GetAdorners(m_Triangle);
                if (ad != null)
                    adLayer.Remove(ad[0]);
            }
        }

        public override void DrawMove(Canvas cvs, Point ept)
        {
            base.DrawMove(cvs, ept);
            if (m_Triangle == null) return;

            var x = Math.Min(ept.X, m_Spt.X);
            var y = Math.Min(ept.Y, m_Spt.Y);

            var w = Math.Max(ept.X, m_Spt.X) - x;
            var h = Math.Max(ept.Y, m_Spt.Y) - y;

            m_Triangle.Width = w;
            m_Triangle.Height = h;

            Canvas.SetLeft(m_Triangle, x);
            Canvas.SetTop(m_Triangle, y);

            Point a = new Point(w/2, 0);
            Point b = new Point(0, h) ;
            Point c = new Point(w, h);
            PointCollection triang = new PointCollection();
            triang.Add(a);
            triang.Add(b);
            triang.Add(c);

            m_Triangle.Points = triang;
            m_Triangle.Stretch = Stretch.Fill;
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Triangle);
        }

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Triangle);
        }

        public override void ChangeColor(SolidColorBrush color1, LinearGradientBrush color2)
        {
            base.ChangeColor(color1, color2);
            m_Triangle.Stroke = color1;
            m_Triangle.Fill = color2;
        }
    }
}
