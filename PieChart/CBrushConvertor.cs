using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.Globalization;
using FMS.PieChart.Base;

namespace FMS.PieChart
{
    [ValueConversion(typeof(object), typeof(Brush))]
    public class CBrushConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType,
           object parameter, CultureInfo culture)
        {
            // find the item 
            FrameworkElement element = (FrameworkElement)value;
            object item = element.Tag;

            // find the item container
            DependencyObject container = (DependencyObject)BaseUtils.FindElementOfTypeUp(element, typeof(ListBoxItem));

            // locate the items control which it belongs to
            ItemsControl owner = ItemsControl.ItemsControlFromItemContainer(container);

            // locate the legend
            Legend legend = (Legend)BaseUtils.FindElementOfTypeUp(owner, typeof(Legend));

            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(owner.DataContext);

            // locate this item (which is bound to the tag of this element) within the collection
            int index = collectionView.IndexOf(item);

            if (legend.ColorSelector != null)
                return legend.ColorSelector.SelectBrush(item, index);
            else
                return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
