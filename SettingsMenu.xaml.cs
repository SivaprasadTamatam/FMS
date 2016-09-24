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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace FMS
{
    /// <summary>
    /// Interaction logic for SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenu : UserControl
    {
        public SettingsMenu()
        {
            InitializeComponent();
        }


        private void FMSBudgetSettings_Click(object sender, RoutedEventArgs e)
        {
            BudgetSettings budget = new BudgetSettings();
            budget.Owner = (MainWindow)System.Windows.Application.Current.MainWindow;
            budget.ShowDialog();
        }
       
    }
}
