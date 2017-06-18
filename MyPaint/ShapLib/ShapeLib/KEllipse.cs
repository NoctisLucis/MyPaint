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
    class CEllipse : CShape
    {
        private Ellipse m_Ellipse;
        private Ellipse m_Copy;

        public CEllipse() { }

        private void DrawEllipse(Canvas cvs, Point Spt, bool isDash)
        {
            m_Ellipse = new Ellipse();
            m_Ellipse.Stroke = m_Stroke;
            m_Ellipse.Fill = m_Fill;
            m_Ellipse.StrokeThickness = m_ThinkNess;
            if (isDash == true)
                m_Ellipse.StrokeDashArray = new DoubleCollection(new double[] { 6, 8 });
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

        public override void DrawDown(Canvas cvs, Point Spt, bool isDash)
        {
            base.DrawDown(cvs, Spt,isDash);
            DrawEllipse(cvs, Spt,isDash);
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
        public override void RotateLeft(Canvas canvas)
        {
            m_Copy = m_Ellipse;
            if (m_Copy.Width > m_Copy.Height)
            {
                Canvas.SetLeft(m_Copy, Canvas.GetLeft(m_Ellipse) + m_Ellipse.Width / 4);
                Canvas.SetTop(m_Copy, Canvas.GetTop(m_Ellipse) - m_Ellipse.Width / 4);
            }
            else
            {
                Canvas.SetLeft(m_Copy, Canvas.GetLeft(m_Ellipse) - m_Ellipse.Height / 4);
                Canvas.SetTop(m_Copy, Canvas.GetTop(m_Ellipse) + m_Ellipse.Height / 4);
            }
            double temp = m_Copy.Width;
            m_Copy.Width = m_Copy.Height;
            m_Copy.Height = temp;
            Remove(canvas);
            canvas.Children.Add(m_Copy);
            m_Ellipse = m_Copy;
            m_Copy = null;
        }

        public override void RotateRight(Canvas canvas)
        {
            RotateLeft(canvas);
        }

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Ellipse);
        }
    }
}
