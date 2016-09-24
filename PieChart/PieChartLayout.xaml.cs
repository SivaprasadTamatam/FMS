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
    public partial class PieChartLayout : UserControl
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
                       DependencyProperty.RegisterAttached("PlottedProperty", typeof(String), typeof(PieChartLayout),
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
                       DependencyProperty.RegisterAttached("ColorSelectorProperty", typeof(IBrushSelector), typeof(PieChartLayout),
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

        public PieChartLayout()
        {
            InitializeComponent();

            rtbExpensesInfo.Document.Blocks.Clear();
            rtbExpensesInfo.Document.Blocks.Add(new Paragraph(new Run("                 In All Categories\n                  In All Accounts")));

            rtbIncomeInfo.Document.Blocks.Clear();
            rtbIncomeInfo.Document.Blocks.Add(new Paragraph(new Run("                 In All Categories\n                  In All Accounts")));

            
        }


        internal void UpdateNetBalance(string p)
        {
            tbxBalance.Text = "\u20B9" + " " + p;
        }

        internal void UpdateNetOutFlow(string p)
        {
            tbxOutflow.Text = "\u20B9"+" " +p;
        }

        internal void UpdateNetInFlow(string p)
        {
            tbxInflow.Text = "\u20B9"+" " +p;
        }
        System.Collections.ObjectModel.ObservableCollection<AssetClass> classes;
        public System.Collections.ObjectModel.ObservableCollection<AssetClass> Classes 
        {
            get
            {
                return classes;
            }
            set
            {
                if( value != null)
                {
                    classes = value;
                    this.DataContext = classes;
                }
            }
        }
        
    }
}
