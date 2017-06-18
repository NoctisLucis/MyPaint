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
    class CCircle : CShape
    {
        private Ellipse m_Circle;

        public CCircle() { }

        private void DrawCircle(Canvas cvs, Point Spt)
        {
            m_Circle = new Ellipse();
            m_Circle.Stroke = m_Stroke;
            m_Circle.Fill = m_Fill;
            m_Circle.StrokeThickness = m_ThinkNess;
            m_Circle.StrokeDashArray = m_Dash;
            m_Spt = Spt;
            Canvas.SetLeft(m_Circle, Spt.X);
            Canvas.SetTop(m_Circle, Spt.Y);

            cvs.Children.Add(m_Circle);
        }

        public override void DrawDown(Canvas cvs, Point Spt)
        {
            base.DrawDown(cvs, Spt);
            DrawCircle(cvs, Spt);
        }

        public override void AddAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null)
            {
                Adorner ad = new RectangleAdorner(this.m_Circle);
                adLayer.Add(ad);
            }
        }

        public override void RemoveAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null && m_Circle != null)
            {
                Adorner[] ad = adLayer.GetAdorners(m_Circle);
                if (ad != null)
                    adLayer.Remove(ad[0]);
            }
        }

        public override void DrawMove(Canvas cvs, Point ept)
        {
            base.DrawMove(cvs,ept);
            if (m_Circle == null) return;

            if (Math.Abs(m_Spt.Y - ept.Y) < Math.Abs(m_Spt.X - ept.X))
                m_Circle.Height = m_Circle.Width = Math.Abs(m_Spt.Y - ept.Y);
            else
                m_Circle.Height = m_Circle.Width = Math.Abs(m_Spt.X - ept.X);
            if ((m_Spt.X < ept.X) && (m_Spt.Y < ept.Y))
            {
                Canvas.SetLeft(m_Circle, m_Spt.X);
                Canvas.SetTop(m_Circle, m_Spt.Y);
            }
            if ((m_Spt.X < ept.X) && (m_Spt.Y > ept.Y))
            {
                Canvas.SetLeft(m_Circle, m_Spt.X);
                Canvas.SetTop(m_Circle, ept.Y);
            }
            if ((m_Spt.X > ept.X) && (m_Spt.Y > ept.Y))
            {
                if (Math.Abs(m_Spt.Y - ept.Y) < Math.Abs(m_Spt.X - ept.X))
                {
                    Canvas.SetLeft(m_Circle, m_Spt.X - m_Circle.Width);
                    Canvas.SetTop(m_Circle, ept.Y);
                }
                else
                {
                    Canvas.SetLeft(m_Circle, ept.X);
                    Canvas.SetTop(m_Circle, m_Spt.Y - m_Circle.Width);
                }
            }
            if ((m_Spt.X > ept.X) && (m_Spt.Y < ept.Y))
            {
                if (Math.Abs(m_Spt.Y - ept.Y) < Math.Abs(m_Spt.X - ept.X))
                {
                    Canvas.SetLeft(m_Circle, m_Spt.X - m_Circle.Width);
                    Canvas.SetTop(m_Circle, m_Spt.Y);
                }
                else
                {
                    Canvas.SetLeft(m_Circle, ept.X);
                    Canvas.SetTop(m_Circle, m_Spt.Y);
                }
            }
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Circle);
        }

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Circle);
        }

        public override void ChangeColor(SolidColorBrush color1, LinearGradientBrush color2)
        {
            base.ChangeColor(color1, color2);
            m_Circle.Stroke = color1;
            m_Circle.Fill = color2;
        }

        public override void ChangeDash(DoubleCollection dash)
        {
            base.ChangeDash(dash);
            m_Circle.StrokeDashArray = dash;
        }

        public override void ChangeThickness(int thick)
        {
            base.ChangeThickness(thick);
            m_Circle.StrokeThickness = thick;
        }
    }
}
