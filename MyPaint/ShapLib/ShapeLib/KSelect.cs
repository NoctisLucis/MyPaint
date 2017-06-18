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
    class CSelect : CShape
    {
        private Rectangle m_Rectangle;

        public CSelect() { }

        private void DrawRectangle(Canvas cvs, Point Spt, bool isDash)
        {
            m_Rectangle = new Rectangle();
            m_Rectangle.Stroke = Brushes.Black;
            m_Rectangle.StrokeThickness = 1;
            m_Rectangle.StrokeDashArray = new DoubleCollection(new double[] { 1, 2 });
            m_Spt = Spt;
            Canvas.SetLeft(m_Rectangle, Spt.X);
            Canvas.SetTop(m_Rectangle, Spt.Y);

            cvs.Children.Add(m_Rectangle);
        }

        public override void DrawDown(Canvas cvs, Point Spt, bool isDash)
        {
            base.DrawDown(cvs, Spt,isDash);
            DrawRectangle(cvs, Spt,isDash);
        }

        public override void AddAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null)
            {
                Adorner ad = new RectangleAdorner(this.m_Rectangle);
                adLayer.Add(ad);
            }
        }

        public override void RemoveAdorner(AdornerLayer adLayer)
        {
            if (adLayer != null && m_Rectangle != null)
            {
                Adorner[] ad = adLayer.GetAdorners(m_Rectangle);
                if (ad != null)
                    adLayer.Remove(ad[0]);
            }
        }

        public override void DrawMove(Canvas cvs, Point ept)
        {
            base.DrawMove(cvs,ept);
            if (m_Rectangle == null) return;

            var x = Math.Min(ept.X, m_Spt.X);
            var y = Math.Min(ept.Y, m_Spt.Y);

            var w = Math.Max(ept.X, m_Spt.X) - x;
            var h = Math.Max(ept.Y, m_Spt.Y) - y;

            m_Rectangle.Width = w;
            m_Rectangle.Height = h;

            Canvas.SetLeft(m_Rectangle, x);
            Canvas.SetTop(m_Rectangle, y);
        }

        public override double getHeight()
        {
            return m_Rectangle.Height;
        }

        public override double getWidth()
        {
            return m_Rectangle.Width;
        }

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Rectangle);
        }
    }
}
