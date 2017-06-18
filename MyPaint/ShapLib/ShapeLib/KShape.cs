using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyPaint1312624
{
    class CShape
    {
        public CShape() { }
        protected Point m_Spt;
        public SolidColorBrush m_Stroke;
        public SolidColorBrush m_Fill;
        public int m_ThinkNess;

        public virtual void DrawDown(Canvas cvs, Point Spt, bool isDash)
        {
        }
        public virtual void AddAdorner(AdornerLayer adLayer)
        {
        }
        public virtual void RemoveAdorner(AdornerLayer adLayer) { }
        public virtual void DrawMove(Canvas cvs, Point Ept) { }
        public virtual void DrawUp() { }
        public virtual double getHeight() { return 0; }
        public virtual double getWidth() { return 0; }
        public virtual void Remove(Canvas canvas) { }
        public virtual void RotateLeft(Canvas canvas) { }
        public virtual void RotateRight(Canvas canvas) { }
        public virtual void AddElement(List<UIElement> list) { }
        public virtual void RemoveElement(List<UIElement> list) { }
    }
}
