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
    class CArrow : CShape
    {
        private Polygon m_Arrow;

        public CArrow() { }

        private void DrawArrow(Canvas cvs, Point Spt)
        {
            m_Arrow = new Polygon();
            m_Arrow.Stroke = m_Stroke;
            m_Arrow.Fill = m_Fill;
            m_Arrow.StrokeThickness = m_ThinkNess;
            m_Arrow.StrokeDashArray = m_Dash;

            m_Spt = Spt;
            Canvas.SetLeft(m_Arrow, Spt.X);
            Canvas.SetTop(m_Arrow, Spt.Y);

            cvs.Children.Add(m_Arrow);
        }

        public override void DrawDown(Canvas cvs, Point Spt)
        {
            base.DrawDown(cvs, Spt);
            DrawArrow(cvs, Spt);
        }

        public override void AddAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null)
            {
                Adorner ad = new RectangleAdorner(this.m_Arrow);
                adLayer.Add(ad);
            }
        }

        public override void RemoveAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null && m_Arrow != null)
            {
                Adorner[] ad = adLayer.GetAdorners(m_Arrow);
                if (ad != null)
                    adLayer.Remove(ad[0]);
            }
        }

        public override void DrawMove(Canvas cvs, Point ept)
        {
            base.DrawMove(cvs, ept);
            if (m_Arrow == null) return;

            var x = Math.Min(ept.X, m_Spt.X);
            var y = Math.Min(ept.Y, m_Spt.Y);

            var w = Math.Max(ept.X, m_Spt.X) - x;
            var h = Math.Max(ept.Y, m_Spt.Y) - y;

            m_Arrow.Width = w;
            m_Arrow.Height = h;

            Canvas.SetLeft(m_Arrow, x);
            Canvas.SetTop(m_Arrow, y);

            Point a1, a2, a3, a4, a5, a6, a7;
            if (m_Spt.X < ept.X)
            {
                a1 = new Point(0, h / 4);
                a2 = new Point(0, h * 3 / 4);
                a3 = new Point(w * 7 / 10, h * 3 / 4);
                a4 = new Point(w * 7 / 10, h);
                a5 = new Point(w, h / 2);
                a6 = new Point(w * 7 / 10, 0);
                a7 = new Point(w * 7 / 10, h / 4);
            }
            else
            {
                a1 = new Point(0, h / 2);
                a2 = new Point(w * 3 / 10, h);
                a3 = new Point(w * 3 / 10, h * 3 / 4);
                a4 = new Point(w, h * 3 / 4);
                a5 = new Point(w, h / 4);
                a6 = new Point(w * 3 / 10, h / 4);
                a7 = new Point(w * 3 / 10, 0);
            }

            PointCollection Arr = new PointCollection();
            Arr.Add(a1);
            Arr.Add(a2);
            Arr.Add(a3);
            Arr.Add(a4);
            Arr.Add(a5);
            Arr.Add(a6);
            Arr.Add(a7);

            m_Arrow.Points = Arr;
            m_Arrow.Stretch = Stretch.Fill;
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Arrow);
        }

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Arrow);
        }

        public override void ChangeColor(SolidColorBrush color1, LinearGradientBrush color2)
        {
            base.ChangeColor(color1, color2);
            m_Arrow.Stroke = color1;
            m_Arrow.Fill = color2;
        }

        public override void ChangeDash(DoubleCollection dash) 
        {
            base.ChangeDash(dash);
            m_Arrow.StrokeDashArray = dash;
        }

        public override void ChangeThickness(int thick)
        {
            base.ChangeThickness(thick);
            m_Arrow.StrokeThickness = thick;
        }
    }
}
