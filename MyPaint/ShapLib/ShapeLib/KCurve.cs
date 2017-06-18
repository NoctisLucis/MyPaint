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

namespace MyPaint1312624
{
    class CCurve : CShape
    {
        private Ellipse m_Curve;

        public CCurve() { }

        private void DrawCurve(Canvas cvs, Point Spt, bool isDash)
        {
            m_Curve = new Ellipse();
            m_Curve.Stroke = m_Stroke;
            m_Curve.Fill = m_Fill;
            m_Curve.StrokeThickness = m_ThinkNess;
            if (isDash == true)
                m_Curve.StrokeDashArray = new DoubleCollection(new double[] { 6, 8 });
            m_Spt = Spt;
            Canvas.SetLeft(m_Curve, Spt.X);
            Canvas.SetTop(m_Curve, Spt.Y);

            cvs.Children.Add(m_Curve);
        }

        public override void DrawDown(Canvas cvs, Point Spt, bool isDash)
        {
            base.DrawDown(cvs, Spt,isDash);
            DrawCurve(cvs, Spt,isDash);
        }

        public override void AddAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null)
            {
                Adorner ad = new RectangleAdorner(this.m_Curve);
                adLayer.Add(ad);
            }
        }

        public override void RemoveAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null && m_Curve != null)
            {
                Adorner[] ad = adLayer.GetAdorners(m_Curve);
                if (ad != null)
                    adLayer.Remove(ad[0]);
            }
        }

        public override void DrawMove(Canvas cvs, Point ept)
        {
            base.DrawMove(cvs,ept);
            if (m_Curve == null) return;

            var x = Math.Min(ept.X, m_Spt.X);
            var y = Math.Min(ept.Y, m_Spt.Y);

            var w = Math.Max(ept.X, m_Spt.X) - x;
            var h = Math.Max(ept.Y, m_Spt.Y) - y;

            m_Curve.Width = w;
            m_Curve.Height = w;

            Canvas.SetLeft(m_Curve, x);
            Canvas.SetTop(m_Curve, y);
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Curve);
        }

        public override void RotateLeft(Canvas canvas)
        {
            return;
        }

        public override void RotateRight(Canvas canvas)
        {
            return;
        }

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Curve);
        }
    }
}
