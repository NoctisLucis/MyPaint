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
    class CRectangle : CShape
    {
        private Rectangle m_Rectangle;
        public CRectangle() { }

        private void DrawRectangle(Canvas cvs, Point Spt)
        {
            m_Rectangle = new Rectangle();
            m_Rectangle.Stroke = m_Stroke;
            m_Rectangle.Fill = m_Fill;
            m_Rectangle.StrokeThickness = m_ThinkNess;
            m_Rectangle.StrokeDashArray = m_Dash;
            m_Spt = Spt;
            Canvas.SetLeft(m_Rectangle, Spt.X);
            Canvas.SetTop(m_Rectangle, Spt.Y);

            cvs.Children.Add(m_Rectangle);
        }

        public override void DrawDown(Canvas cvs, Point Spt)
        {
            base.DrawDown(cvs, Spt);
            DrawRectangle(cvs, Spt); 
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

        public override void AddElement(List<UIElement> list)
        {
            list.Add(m_Rectangle);
        }

      //  public override void RemoveElement(List<UIElement> list)
      //  {
      //      list.RemoveAt(list.Count - 1);
      //  }

        public override void ChangeColor(SolidColorBrush color1, LinearGradientBrush color2)
        {
            base.ChangeColor(color1, color2);
            m_Rectangle.Stroke = color1;
            m_Rectangle.Fill = color2;
        }

        public override void ChangeDash(DoubleCollection dash)
        {
            base.ChangeDash(dash);
            m_Rectangle.StrokeDashArray = dash;
        }

        public override void ChangeThickness(int thick)
        {
            base.ChangeThickness(thick);
            m_Rectangle.StrokeThickness = thick;
        }
    }
}
