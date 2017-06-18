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
    class CSquare : CShape
    {
        private Rectangle m_Square;
        private Rectangle m_Copy;

        public CSquare() { }

        private void DrawSquare(Canvas cvs, Point Spt, bool isDash)
        {
            m_Square = new Rectangle();
            m_Square.Stroke = m_Stroke;
            m_Square.Fill = m_Fill;
            m_Square.StrokeThickness = m_ThinkNess;
            if (isDash == true)
                m_Square.StrokeDashArray = new DoubleCollection(new double[] { 6, 8 });
            m_Spt = Spt;
            Canvas.SetLeft(m_Square, Spt.X);
            Canvas.SetTop(m_Square, Spt.Y);

            cvs.Children.Add(m_Square);
        }

        public override void DrawDown(Canvas cvs, Point Spt, bool isDash)
        {
            base.DrawDown(cvs, Spt,isDash);
            DrawSquare(cvs, Spt,isDash);
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

            var x = Math.Min(ept.X, m_Spt.X);
            var y = Math.Min(ept.Y, m_Spt.Y);

            var w = Math.Max(ept.X, m_Spt.X) - x;
            var h = Math.Max(ept.Y, m_Spt.Y) - y;

            m_Square.Width = w;
            m_Square.Height = w;

            Canvas.SetLeft(m_Square, x);
            Canvas.SetTop(m_Square, y);
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Square);
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
            list.Add(m_Square);
        }
    }
}
