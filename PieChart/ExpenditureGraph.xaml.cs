using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FMS.PieChart
{
    public partial class ExpenditureGraph : UserControl
    {
        #region dependency properties

        /// <summary>
        /// The property of the bound object that will be plotted (CLR wrapper)
        /// </summary>
        public String PlottedProperty
        {
            get { return GetPlottedProperty(this); }
            set { SetPlottedProperty(this, value); }
        }

        // PlottedProperty dependency property
        public static readonly DependencyProperty PlottedPropertyProperty =
                       DependencyProperty.RegisterAttached("PlottedProperty", typeof(String), typeof(ExpenditureGraph),
                       new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.Inherits));

        // PlottedProperty attached property accessors
        public static void SetPlottedProperty(UIElement element, String value)
        {
            element.SetValue(PlottedPropertyProperty, value);
        }
        public static String GetPlottedProperty(UIElement element)
        {
            return (String)element.GetValue(PlottedPropertyProperty);
        }

        /// <summary>
        /// A class which selects a color based on the item being rendered.
        /// </summary>
        public IBrushSelector ColorSelector
        {
            get { return GetColorSelector(this); }
            set { SetColorSelector(this, value); }
        }

        // ColorSelector dependency property
        public static readonly DependencyProperty ColorSelectorProperty =
                       DependencyProperty.RegisterAttached("ColorSelectorProperty", typeof(IBrushSelector), typeof(ExpenditureGraph),
                       new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        // ColorSelector attached property accessors
        public static void SetColorSelector(UIElement element, IBrushSelector value)
        {
            element.SetValue(ColorSelectorProperty, value);
        }
        public static IBrushSelector GetColorSelector(UIElement element)
        {
            return (IBrushSelector)element.GetValue(ColorSelectorProperty);
        }


        #endregion

        public ExpenditureGraph()
        {
            InitializeComponent();            
        }
        public void UpdateExpCategory()
        {

            foreach (AssetClass a in pPOutFlowGraph.DrawClasses)
            {
                if (a.Class == tbxTextVehicle.Text.ToString())
                {
                    tbxValueVehicle.Text = "\u20B9"+" " + Convert.ToString(a.Data);
                }
                if (a.Class == tbxTextDonations.Text.ToString())
                {
                    tbxValueDonations.Text = "\u20B9" + " " + Convert.ToString(a.Data);
                }
                if (a.Class == tbxTextFamilyunit.Text.ToString())
                {
                    tbxValueFamilyunit.Text = "\u20B9" + " " + Convert.ToString(a.Data);
                }
            }
        }
    }
}
