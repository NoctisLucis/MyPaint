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
    class CEllipse : CShape
    {
        private Ellipse m_Ellipse;

        public CEllipse() { }

        private void DrawEllipse(Canvas cvs, Point Spt)
        {
            m_Ellipse = new Ellipse();
            m_Ellipse.Stroke = m_Stroke;
            m_Ellipse.Fill = m_Fill;
            m_Ellipse.StrokeThickness = m_ThinkNess;
            m_Ellipse.StrokeDashArray = m_Dash;
            m_Spt = Spt;
            Canvas.SetLeft(m_Ellipse, Spt.X);
            Canvas.SetTop(m_Ellipse, Spt.Y);

            cvs.Children.Add(m_Ellipse);
        }

        public override void AddAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null)
            {
                Adorner ad = new RectangleAdorner(this.m_Ellipse);
                adLayer.Add(ad);
            }
        }

        public override void RemoveAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null && m_Ellipse != null)
            {
                Adorner[] ad = adLayer.GetAdorners(m_Ellipse);
                if(ad != null)
                    adLayer.Remove(ad[0]);
            }
        }

        public override void DrawDown(Canvas cvs, Point Spt)
        {
            base.DrawDown(cvs, Spt);
            DrawEllipse(cvs, Spt);
        }

        public override void DrawMove(Canvas cvs, Point ept)
        {
            base.DrawMove(cvs,ept);
            if (m_Ellipse == null) return;

            var x = Math.Min(ept.X, m_Spt.X);
            var y = Math.Min(ept.Y, m_Spt.Y);

            var w = Math.Max(ept.X, m_Spt.X) - x;
            var h = Math.Max(ept.Y, m_Spt.Y) - y;

            m_Ellipse.Width = w;
            m_Ellipse.Height = h;

            Canvas.SetLeft(m_Ellipse, x);
            Canvas.SetTop(m_Ellipse, y);
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Ellipse);
        }

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Ellipse);
        }

        public override void ChangeColor(SolidColorBrush color1, LinearGradientBrush color2)
        {
            base.ChangeColor(color1, color2);
            m_Ellipse.Stroke = color1;
            m_Ellipse.Fill = color2;
        }

        public override void ChangeDash(DoubleCollection dash)
        {
            base.ChangeDash(dash);
            m_Ellipse.StrokeDashArray = dash;
        }

        public override void ChangeThickness(int thick)
        {
            base.ChangeThickness(thick);
            m_Ellipse.StrokeThickness = thick;
        }

    }
}
