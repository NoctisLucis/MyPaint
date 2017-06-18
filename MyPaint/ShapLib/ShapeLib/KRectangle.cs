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
    class CRectangle : CShape
    {
        private Rectangle m_Rectangle;
        private Rectangle m_Copy;
        public CRectangle() { }

        private void DrawRectangle(Canvas cvs, Point Spt, bool isDash)
        {
            m_Rectangle = new Rectangle();
            m_Rectangle.Stroke = m_Stroke;
            m_Rectangle.Fill = m_Fill;
            m_Rectangle.StrokeThickness = m_ThinkNess;
            if (isDash == true)
                m_Rectangle.StrokeDashArray = new DoubleCollection(new double[] { 6, 8 });
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

        public override void Remove(Canvas canvas)
        {
            canvas.Children.Remove(m_Rectangle);
        }

        public override void RotateLeft(Canvas canvas)
        {
            m_Copy = m_Rectangle;
            if (m_Copy.Width > m_Copy.Height)
            {
                Canvas.SetLeft(m_Copy, Canvas.GetLeft(m_Rectangle) + m_Rectangle.Width / 4);
                Canvas.SetTop(m_Copy, Canvas.GetTop(m_Rectangle) - m_Rectangle.Width / 4);
            }
            else
            {
                Canvas.SetLeft(m_Copy, Canvas.GetLeft(m_Rectangle) - m_Rectangle.Height / 4);
                Canvas.SetTop(m_Copy, Canvas.GetTop(m_Rectangle) + m_Rectangle.Height / 4);
            }
            double temp = m_Copy.Width;
            m_Copy.Width = m_Copy.Height;
            m_Copy.Height = temp;
            Remove(canvas);
            canvas.Children.Add(m_Copy);
            m_Rectangle = m_Copy;
            m_Copy = null;
        }

        public override void RotateRight(Canvas canvas)
        {
            RotateLeft(canvas);
        }

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Rectangle);
        }

      //  public override void RemoveElement(List<UIElement> list)
      //  {
      //      list.RemoveAt(list.Count - 1);
      //  }
    }
}
