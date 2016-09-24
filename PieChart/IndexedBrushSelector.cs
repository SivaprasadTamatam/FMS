using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using FMS.PieChart;
namespace FMS.PieChart
{
    public class IndexedBrushSelector : DependencyObject, IBrushSelector
    {
        /// <summary>
        /// An array of brushes.
        /// </summary>
        public Brush[] Brushes
        {
            get { return (Brush[])GetValue(BrushesProperty); }
            set { SetValue(BrushesProperty, value); }
        }

        public static readonly DependencyProperty BrushesProperty =
                       DependencyProperty.Register("BrushesProperty", typeof(Brush[]), typeof(IndexedBrushSelector), new UIPropertyMetadata(null));


        public Brush SelectBrush(object item, int index)
        {
            if (Brushes == null || Brushes.Length == 0)
            {
                return System.Windows.Media.Brushes.Black;
            }
            return Brushes[index % Brushes.Length];
        }
    }
}
