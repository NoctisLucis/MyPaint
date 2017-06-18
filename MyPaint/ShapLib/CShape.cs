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

namespace ShapesLib
{
    class CShape
    {
        public CShape() { }
        protected Point m_Spt;
        public SolidColorBrush m_Stroke;
        public LinearGradientBrush m_Fill;
        //public SolidColorBrush m_Fill;
        public int m_ThinkNess;
        public DoubleCollection m_Dash;

        public virtual void DrawDown(Canvas cvs, Point Spt) { }
        public virtual void AddAdorner(AdornerLayer adLayer) { }
        public virtual void RemoveAdorner(AdornerLayer adLayer) { }
        public virtual void DrawMove(Canvas cvs, Point Ept) { }
        public virtual void DrawUp() { }
        public virtual double getHeight() { return 0; }
        public virtual double getWidth() { return 0; }
        public virtual void Remove(Canvas canvas) { }
        public virtual void AddElement(List<UIElement> list) { }
        public virtual void RemoveElement(List<UIElement> list) { }
        public virtual void ChangeColor(SolidColorBrush color1, LinearGradientBrush color2) { }
        public virtual void ChangeDash(DoubleCollection dash) { }
        public virtual void ChangeThickness(int thick) { }
    }
}
