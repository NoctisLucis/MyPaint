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
    class CSquare : CShape
    {
        private Rectangle m_Square;

        public CSquare() { }

        private void DrawSquare(Canvas cvs, Point Spt)
        {
            m_Square = new Rectangle();
            m_Square.Stroke = m_Stroke;
            m_Square.Fill = m_Fill;
            m_Square.StrokeThickness = m_ThinkNess;
            m_Square.StrokeDashArray = m_Dash;
            m_Spt = Spt;
            Canvas.SetLeft(m_Square, Spt.X);
            Canvas.SetTop(m_Square, Spt.Y);

            cvs.Children.Add(m_Square);
        }

        public override void DrawDown(Canvas cvs, Point Spt)
        {
            base.DrawDown(cvs, Spt);
            DrawSquare(cvs, Spt);
        }

        public override void AddAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null)
            {
                Adorner ad = new RectangleAdorner(this.m_Square);
                adLayer.Add(ad);
            }
        }

        public override void RemoveAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null && m_Square != null)
            {
                Adorner[] ad = adLayer.GetAdorners(m_Square);
                if (ad != null)
                    adLayer.Remove(ad[0]);
            }
        }

        public override void DrawMove(Canvas cvs, Point ept)
        {
            base.DrawMove(cvs,ept);
            if (m_Square == null) return;


            if (Math.Abs(m_Spt.Y - ept.Y) < Math.Abs(m_Spt.X - ept.X))
                m_Square.Height = m_Square.Width = Math.Abs(m_Spt.Y - ept.Y);
            else
                m_Square.Height = m_Square.Width = Math.Abs(m_Spt.X - ept.X);
            if ((m_Spt.X < ept.X) && (m_Spt.Y < ept.Y))
            {
                Canvas.SetLeft(m_Square, m_Spt.X);
                Canvas.SetTop(m_Square, m_Spt.Y);
            }
            if ((m_Spt.X < ept.X) && (m_Spt.Y > ept.Y))
            {
                Canvas.SetLeft(m_Square, m_Spt.X);
                Canvas.SetTop(m_Square, ept.Y);
            }
            if ((m_Spt.X > ept.X) && (m_Spt.Y > ept.Y))
            {
                if (Math.Abs(m_Spt.Y - ept.Y) < Math.Abs(m_Spt.X - ept.X))
                {
                    Canvas.SetLeft(m_Square, m_Spt.X - m_Square.Width);
                    Canvas.SetTop(m_Square, ept.Y);
                }
                else
                {
                    Canvas.SetLeft(m_Square, ept.X);
                    Canvas.SetTop(m_Square, m_Spt.Y - m_Square.Width);
                }
            }
            if ((m_Spt.X > ept.X) && (m_Spt.Y < ept.Y))
            {
                if (Math.Abs(m_Spt.Y - ept.Y) < Math.Abs(m_Spt.X - ept.X))
                {
                    Canvas.SetLeft(m_Square, m_Spt.X - m_Square.Width);
                    Canvas.SetTop(m_Square, m_Spt.Y);
                }
                else
                {
                    Canvas.SetLeft(m_Square, ept.X);
                    Canvas.SetTop(m_Square, m_Spt.Y);
                }
            }
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Square);
        }

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Square);
        }
        public override void ChangeColor(SolidColorBrush color1, LinearGradientBrush color2)
        {
            base.ChangeColor(color1, color2);
            m_Square.Stroke = color1;
            m_Square.Fill = color2;
        }

        public override void ChangeDash(DoubleCollection dash)
        {
            base.ChangeDash(dash);
            m_Square.StrokeDashArray = dash;
        }

        public override void ChangeThickness(int thick)
        {
            base.ChangeThickness(thick);
            m_Square.StrokeThickness = thick;
        }
    }
}
