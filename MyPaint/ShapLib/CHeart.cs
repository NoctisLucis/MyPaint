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
    class CHeart : CShape
    {
        private Path m_Heart;

        public CHeart() { }

        private void DrawHeart(Canvas cvs, Point Spt)
        {
            m_Heart = new Path();
            m_Heart.Stroke = m_Stroke;
            m_Heart.Fill = m_Fill;
            m_Heart.StrokeThickness = m_ThinkNess;
            m_Heart.StrokeDashArray = m_Dash;

            m_Spt = Spt;
            Canvas.SetLeft(m_Heart, Spt.X);
            Canvas.SetTop(m_Heart, Spt.Y);

            cvs.Children.Add(m_Heart);
        }

        public override void DrawDown(Canvas cvs, Point Spt)
        {
            base.DrawDown(cvs, Spt);
            DrawHeart(cvs, Spt);
        }

        public override void AddAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null)
            {
                Adorner ad = new RectangleAdorner(this.m_Heart);
                adLayer.Add(ad);
            }
        }

        public override void RemoveAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null && m_Heart != null)
            {
                Adorner[] ad = adLayer.GetAdorners(m_Heart);
                if (ad != null)
                    adLayer.Remove(ad[0]);
            }
        }

        public override void DrawMove(Canvas cvs, Point ept)
        {
            base.DrawMove(cvs, ept);
            if (m_Heart == null) return;

            var x = Math.Min(ept.X, m_Spt.X);
            var y = Math.Min(ept.Y, m_Spt.Y);

            var w = Math.Max(ept.X, m_Spt.X) - x;
            var h = Math.Max(ept.Y, m_Spt.Y) - y;

            m_Heart.Width = w;
            m_Heart.Height = h;

            Canvas.SetLeft(m_Heart, x);
            Canvas.SetTop(m_Heart, y);

            if (m_Spt.Y < ept.Y)
            m_Heart.Data = Geometry.Parse("M 40,0 A 20,20 0 0 0 0,40 C 10,50 40,70 40,70 C 40,70 60,60 80,40 A 20,20 0 0 0 40,0 Z");
            else
            m_Heart.Data = Geometry.Parse("M 0,40 A 20,20 0 0 0 40,0 C 50,10 0,-40 0,-40 C 0,-40 -20,-20 -40,0 A 20,20 0 0 0 0,40 Z");
            m_Heart.Stretch = Stretch.Fill;
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Heart);
        }

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Heart);
        }

        public override void ChangeColor(SolidColorBrush color1, LinearGradientBrush color2)
        {
            base.ChangeColor(color1, color2);
            m_Heart.Stroke = color1;
            m_Heart.Fill = color2;
        }

        public override void ChangeDash(DoubleCollection dash)
        {
            base.ChangeDash(dash);
            m_Heart.StrokeDashArray = dash;
        }

        public override void ChangeThickness(int thick)
        {
            base.ChangeThickness(thick);
            m_Heart.StrokeThickness = thick;
        }

    }
}
