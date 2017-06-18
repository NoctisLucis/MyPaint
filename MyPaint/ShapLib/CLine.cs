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
    class CLine : CShape
    {
        private Line m_Line;

        public CLine() { }

        private void DrawLine(Point Spt, Point Ept)
        {
            m_Line = new Line();
            m_Line.X1 = Spt.X;
            m_Line.Y1 = Spt.Y;
            m_Line.X2 = Ept.X;
            m_Line.Y2 = Ept.Y;
            m_Line.Stroke = m_Stroke;
            m_Line.Fill = m_Fill;
            m_Line.StrokeThickness = m_ThinkNess;
            m_Line.StrokeDashArray = m_Dash;
            //if (isDash == true)
            //    m_Line.StrokeDashArray = new DoubleCollection(new double[] { 6, 8 });
        }
        public override void DrawDown(Canvas cvs, Point spt)
        {
            base.DrawDown(cvs, spt);
            if (cvs.CaptureMouse())
            {
                DrawLine(spt, spt);
                cvs.Children.Add(m_Line);
           }
        }
        public override void AddAdorner(AdornerLayer s)
        {
            if (s != null && m_Line != null)
            {
                Adorner ad = new LineAdorner(this.m_Line);
                s.Add(ad);
            }
        }

        public override void RemoveAdorner(AdornerLayer s)
        {
            if (s != null && m_Line != null)
            {
                Adorner[] ad = s.GetAdorners(this.m_Line);
                if (ad != null)
                    s.Remove(ad[0]);
            }
        }

        public override void DrawMove(Canvas cvs, Point ept)
        {
           base.DrawMove(cvs, ept);
           if (cvs.IsMouseCaptured)
           {
              if (m_Line == null)
                  return;

              m_Line.X2 = ept.X;
              m_Line.Y2 = ept.Y;
           }
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Line);
        }
        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Line);
        }

        public override void ChangeColor(SolidColorBrush color1, LinearGradientBrush color2)
        {
            base.ChangeColor(color1, color2);
            m_Line.Stroke = color1;
            m_Line.Fill = color2;
        }

        public override void ChangeDash(DoubleCollection dash)
        {
            base.ChangeDash(dash);
            m_Line.StrokeDashArray = dash;
        }

        public override void ChangeThickness(int thick)
        {
            base.ChangeThickness(thick);
            m_Line.StrokeThickness = thick;
        }
    }
}
