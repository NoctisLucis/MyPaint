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
    class CStar : CShape
    {
        private Polygon m_Star;

        public CStar() { }

        private void DrawStar(Canvas cvs, Point Spt)
        {
            m_Star = new Polygon();
            m_Star.Stroke = m_Stroke;
            m_Star.Fill = m_Fill;
            m_Star.StrokeThickness = m_ThinkNess;
            m_Star.StrokeDashArray = m_Dash;

            m_Spt = Spt;
            Canvas.SetLeft(m_Star, Spt.X);
            Canvas.SetTop(m_Star, Spt.Y);

            cvs.Children.Add(m_Star);
        }

        public override void DrawDown(Canvas cvs, Point Spt)
        {
            base.DrawDown(cvs, Spt);
            DrawStar(cvs, Spt);
        }

        public override void AddAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null)
            {
                Adorner ad = new RectangleAdorner(this.m_Star);
                adLayer.Add(ad);
            }
        }

        public override void RemoveAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null && m_Star != null)
            {
                Adorner[] ad = adLayer.GetAdorners(m_Star);
                if (ad != null)
                    adLayer.Remove(ad[0]);
            }
        }

        public override void DrawMove(Canvas cvs, Point ept)
        {
            base.DrawMove(cvs, ept);
            if (m_Star == null) return;

            var x = Math.Min(ept.X, m_Spt.X);
            var y = Math.Min(ept.Y, m_Spt.Y);

            var w = Math.Max(ept.X, m_Spt.X) - x;
            var h = Math.Max(ept.Y, m_Spt.Y) - y;

            m_Star.Width = w;
            m_Star.Height = h;

            Canvas.SetLeft(m_Star, x);
            Canvas.SetTop(m_Star, y);

            Point a1, a2, a3, a4, a5, a6, a7, a8, a9, a10;
            if (m_Spt.Y < ept.Y)
            {
                a1 = new Point(w / 2, 0);
                a2 = new Point(w * 1.8 / 5, h * 2 / 5);
                a3 = new Point(0, h * 2 / 5);
                a4 = new Point(w * 3 / 10, h * 3 / 5);
                a5 = new Point(w / 5, h);
                a6 = new Point(w / 2, h * 7.5 / 10);
                a7 = new Point(w * 4 / 5, h);
                a8 = new Point(w * 7 / 10, h * 3 / 5);
                a9 = new Point(w, h * 2 / 5);
                a10 = new Point(w * 3.2 / 5, h * 2 / 5);
            }
            else
            {
                a1 = new Point(w / 2, 0);
                a2 = new Point(w * 1.8 / 5, -h * 2 / 5);
                a3 = new Point(0, -h * 2 / 5);
                a4 = new Point(w * 3 / 10, -h * 3 / 5);
                a5 = new Point(w / 5, -h);
                a6 = new Point(w / 2, -h * 7.5 / 10);
                a7 = new Point(w * 4 / 5, -h);
                a8 = new Point(w * 7 / 10, -h * 3 / 5);
                a9 = new Point(w, -h * 2 / 5);
                a10 = new Point(w * 3.2 / 5, -h * 2 / 5);
            }

            PointCollection Star = new PointCollection();
            Star.Add(a1);
            Star.Add(a2);
            Star.Add(a3);
            Star.Add(a4);
            Star.Add(a5);
            Star.Add(a6);
            Star.Add(a7);
            Star.Add(a8);
            Star.Add(a9);
            Star.Add(a10);

            m_Star.Points = Star;
            m_Star.Stretch = Stretch.Fill;
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Star);
        }

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Star);
        }

        public override void ChangeColor(SolidColorBrush color1, LinearGradientBrush color2)
        {
            base.ChangeColor(color1, color2);
            m_Star.Stroke = color1;
            m_Star.Fill = color2;
        }

        public override void ChangeDash(DoubleCollection dash)
        {
            base.ChangeDash(dash);
            m_Star.StrokeDashArray = dash;
        }

        public override void ChangeThickness(int thick)
        {
            base.ChangeThickness(thick);
            m_Star.StrokeThickness = (double)thick;
        }
    }
}
