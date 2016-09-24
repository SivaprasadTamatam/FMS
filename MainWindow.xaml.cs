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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using System.Data.SQLite;
using FMS.PieChart.Shapes;
using FMS.DB;

namespace FMS
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<CAccountDetails> lAccountDetails = new List<CAccountDetails>();
        
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const string INCOME_STR = "Income    :";
        public const string EXPENSES_STR = "Expenses :";
        public const string BALANCE_STR = "Balance   :";
        public const string NETWORTH_STR = "Net Worth : ";

        public const int HT_CAPTION = 0x2;
        public MainWindow()
        {
            InitializeComponent();
            
            //dpDatePicker.SelectedDate = DateTime.Now.Date;
            UpdateDetails();
            //spendingTrackerCtrl2.image2.Source = new BitmapImage(new Uri("F:\\FMS\\FMS\\FMS\\FMS\\SpentImages\\Shopping.png"));
            //spendingTrackerCtrl3.image2.Source = new BitmapImage(new Uri("F:\\FMS\\FMS\\FMS\\FMS\\SpentImages\\Food.png"));
            // create our test dataset and bind it
            
            this.WindowState = WindowState.Maximized;


            dpEndDate.Text = DateTime.Today.ToString();
            dpStartDate.Text = DateTime.Today.AddDays(-30).ToString();

            for (int i = 0; i > -30; i--)
            {
                tlAllTransactions.LoadAllTransaction(DateTime.Today.AddDays(i).ToShortDateString());
                tlexpTransactionList.LoadAllExpendTransactions(DateTime.Today.AddDays(i).ToShortDateString());
            }

            AssetClass.ConstructGraphData((DataBase.Instance.GetCategoryList()));
            UpdateBudgetSettings((DataBase.Instance.GetCategoryList()));
            //classes = new ObservableCollection<AssetClass>(AssetClass.ConstructGraphData((DataBase.Instance.GetCategoryList())));
            piePlotter.pPOutFlowGraph.DrawClasses = new ObservableCollection<AssetClass>(AssetClass.ExpensesData);
            pieExp.pPOutFlowGraph.DrawClasses = new ObservableCollection<AssetClass>(AssetClass.ExpensesData);
            pieExp.UpdateExpCategory();
            piePlotter.pPInFlow.DrawClasses = new ObservableCollection<AssetClass>(AssetClass.IncomeData);   
             
        }

        public PieChart.PieChartLayout GetChartLayout()
        {
            return piePlotter;
        }
        public void UpdateDetails()
        {
            tvAccountDetails.Items.Clear();
            DataBase DB = DataBase.Instance;
            double netBalance = 0;

            foreach (CAccountDetails ac in DB.getAccountDetails())
            {
                TreeViewItem it = null;
                it = FMSGetTreeView(ac.sAccountName,ac.sAccountImage);
                it.Items.Add(FMSGetTreeView(INCOME_STR, ac.dIncome));
                it.Items.Add(FMSGetTreeView(EXPENSES_STR, ac.dExpenses));
                it.Items.Add(FMSGetTreeView(BALANCE_STR, ac.dBalance));
                tvAccountDetails.Items.Add(it);
                netBalance = netBalance + ac.dBalance;
            }
            foreach (Budget b in DB.getBudgetSettings())
            {
                switch (b.sCategory)
                {
                    case "Vehicle":
                        {
                            sptVehicle.SetTotalAllocation(b.dBudget);
                            break;
                        }
                    case "Donations":
                        {
                            sptDonations.SetTotalAllocation(b.dBudget);
                            break;
                        }
                    case "Family unit":
                        {
                            sptFamilyunit.SetTotalAllocation(b.dBudget);
                            break;
                        }
                    case "Offerings":
                        {
                            sptOfferings.SetTotalAllocation(b.dBudget);
                            break;
                        }
                    case "Learning":
                        {
                            sptLearning.SetTotalAllocation(b.dBudget);
                            break;
                        }
                    case "Food":
                        {
                            sptFood.SetTotalAllocation(b.dBudget);
                            break;
                        }
                    case "Home belongings":
                        {
                            sptHomebelongings.SetTotalAllocation(b.dBudget);
                            break;
                        }
                    case "Medical":
                        {
                            sptMedical.SetTotalAllocation(b.dBudget);
                            break;
                        }
                    default:
                        break;
                }
            }
            UpdateMainPage();
        }

       
       
        private TreeViewItem FMSGetTreeView(string p, string imagepath)
        {
            TreeViewItem item = new TreeViewItem();

            item.IsExpanded = false;

            // create stack panel
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            string uripath = "pack://siteoforigin:,,,/" + imagepath;
            // create Image
            Image image = new Image();
            image.Source = new BitmapImage
                (new Uri(uripath));

            // Label
            Label lbl = new Label();
            lbl.Content = p;



            // Add into stack
            stack.Children.Add(image);
            stack.Children.Add(lbl);


            // assign stack to header
            item.Header = stack;

            return item;
        }

        [DllImportAttribute("user32.dll")]
	    public static extern int SendMessage(IntPtr hWnd, int Msg,
                int wParam, int lParam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public void move_window(object sender, MouseButtonEventArgs e)
        {
            ReleaseCapture();
            SendMessage(new WindowInteropHelper(this).Handle,
	                WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
        private void EXIT(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MINIMIZE(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MAX_RESTORE(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == WindowState.Normal) this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void Activate_Title_Icons(object sender, MouseEventArgs e)
        {
            Close_btn.Fill = (ImageBrush)Main.Resources["Close_act"];
            Min_btn.Fill = (ImageBrush)Main.Resources["Min_act"];
            Max_btn.Fill = (ImageBrush)Main.Resources["Max_act"];
        }

        private void Deactivate_Title_Icons(object sender, MouseEventArgs e)
        {
            Close_btn.Fill = (ImageBrush)Main.Resources["Close_inact"];
            Min_btn.Fill = (ImageBrush)Main.Resources["Min_inact"];
            Max_btn.Fill = (ImageBrush)Main.Resources["Max_inact"];
        }

        private void Close_pressing(object sender, MouseButtonEventArgs e)
        {
            Close_btn.Fill = (ImageBrush)Main.Resources["Close_pr"];
        }

        private void Min_pressing(object sender, MouseButtonEventArgs e)
        {
            Min_btn.Fill = (ImageBrush)Main.Resources["Min_pr"];
        }

        private void Max_pressing(object sender, MouseButtonEventArgs e)
        {
            Max_btn.Fill = (ImageBrush)Main.Resources["Max_pr"];
        }
        private TreeViewItem FMSGetTreeView(string text, double sbalance)
        {
            
            TreeViewItem subItem = new TreeViewItem();
            StackPanel substack = new StackPanel();
            substack.Orientation = Orientation.Horizontal;

            // create Image
            //Image subImage = new Image();
            //string uripath = "pack://application:,,,/UI_RES/" + "INR.bmp";
            //subImage.Source = new BitmapImage
            //    (new Uri(uripath));

            // Label
            Label subIncome = new Label();
            subIncome.Content = "\u20B9" + " " + text + Convert.ToString(sbalance);

           
            Label lbBalance = new Label();
            lbBalance.Content = sbalance;
           // substack.Children.Add(subImage);
            substack.Children.Add(subIncome);
            //substack.Children.Add(lbBalance);

            subItem.Header = substack;
            return subItem;
        }
        private void OnColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            GridViewColumn column = ((GridViewColumnHeader)e.OriginalSource).Column;
           // piePlotter.PlottedProperty = column.Header.ToString();
        }

        //private void AddNewItem(object sender, RoutedEventArgs e)
        //{
        //    AssetClass asset = new AssetClass() { Class = "new class" };
        //    classes.Add(asset);
        //}

        private void OnAddTransactionClick(object sender, RoutedEventArgs e)
        {

            TransactionWindow dlg = new TransactionWindow(tlAllTransactions, tlexpTransactionList);
            dlg.Owner = (MainWindow)System.Windows.Application.Current.MainWindow;
            dlg.ShowDialog();
        }

        public void UpdateAccountDetails(CAccountDetails ac)
        {
            foreach (TreeViewItem lv in tvAccountDetails.Items)
            {
                StackPanel stack = (StackPanel)lv.Header;
                foreach (object obj in stack.Children)
                {
                    if (obj is Label)
                    {
                        if (ac.sAccountName == ((Label)(obj)).Content.ToString())
                        {
                            foreach (TreeViewItem it in lv.Items)
                            {
                                 StackPanel substack = (StackPanel)it.Header;
                                 foreach (object subobj in substack.Children)
                                 {
                                     if (subobj is Label)
                                     {
                                         if (((Label)(subobj)).Content.ToString().Contains(INCOME_STR))
                                         {
                                             ((Label)(subobj)).Content = "\u20B9" + " " + INCOME_STR + Convert.ToString(ac.dIncome);
                                         }
                                         if (((Label)(subobj)).Content.ToString().Contains(EXPENSES_STR))
                                         {
                                             ((Label)(subobj)).Content = "\u20B9" + " " + EXPENSES_STR + Convert.ToString(ac.dExpenses);
                                         }
                                         if (((Label)(subobj)).Content.ToString().Contains(BALANCE_STR))
                                         {
                                             ((Label)(subobj)).Content = "\u20B9" + " " + BALANCE_STR + Convert.ToString(ac.dBalance);
                                         }
                                     }
                                 }
                            }
                        }
                    }
                }
            }
        }

        internal void UpdateMainPage()
        {
            DataBase DB = DataBase.Instance;
            double netBalance = 0;
            double netOutflow = 0;
            double netInflow = 0;

            foreach (CAccountDetails ac in DB.getAccountDetails())
            {                
                netBalance = netBalance + ac.dBalance;
                netOutflow = netOutflow + ac.dExpenses;
                netInflow = netInflow + ac.dIncome;
            }
            
            tblkNetWorth.Text = NETWORTH_STR + "\u20B9"+" "+Convert.ToString(netBalance);
            piePlotter.UpdateNetBalance(Convert.ToString(netBalance));
            piePlotter.UpdateNetOutFlow(Convert.ToString(netOutflow));
            piePlotter.UpdateNetInFlow(Convert.ToString(netInflow));
            AssetClass.ConstructGraphData((DataBase.Instance.GetCategoryList()));
            //classes = new ObservableCollection<AssetClass>(AssetClass.ConstructGraphData((DataBase.Instance.GetCategoryList())));
            UpdateBudgetSettings((DataBase.Instance.GetCategoryList()));
            
            piePlotter.pPOutFlowGraph.DrawClasses = new ObservableCollection<AssetClass>(AssetClass.ExpensesData);
            pieExp.pPOutFlowGraph.DrawClasses = new ObservableCollection<AssetClass>(AssetClass.ExpensesData);
            pieExp.UpdateExpCategory();
            piePlotter.pPInFlow.DrawClasses = new ObservableCollection<AssetClass>(AssetClass.IncomeData);           
            
        }

        private void UpdateBudgetSettings(List<Categories> list)
        {
            foreach (Categories c in list)
            {
                switch(c.sCategory)
                {
                    case "Vehicle":
                    {
                        sptVehicle.SetCategoryImage(c.sCategory);
                        sptVehicle.SetSpentAmount(c.dTotal);
                        break;
                    }
                    case "Donations":
                    {
                        sptDonations.SetCategoryImage(c.sCategory);
                        sptDonations.SetSpentAmount(c.dTotal);
                        break;
                    }
                    case "Family unit":
                    {
                        sptFamilyunit.SetCategoryImage(c.sCategory);
                        sptFamilyunit.SetSpentAmount(c.dTotal);
                        break;
                    }
                    case "Offerings":
                    {
                        sptOfferings.SetCategoryImage(c.sCategory);
                        sptOfferings.SetSpentAmount(c.dTotal);
                        break;
                    }
                    case "Learning":
                    {
                        sptLearning.SetCategoryImage(c.sCategory);
                        sptLearning.SetSpentAmount(c.dTotal);
                        break;
                    }
                    case "Food":
                    {
                        sptFood.SetCategoryImage(c.sCategory);
                        sptFood.SetSpentAmount(c.dTotal);
                        break;
                    }
                    case "Home belongings":
                    {
                        sptHomebelongings.SetCategoryImage(c.sCategory);
                        sptHomebelongings.SetSpentAmount(c.dTotal);
                        break;
                    }
                    case "Medical":
                    {
                        sptMedical.SetCategoryImage(c.sCategory);
                        sptMedical.SetSpentAmount(c.dTotal);
                        break;
                    }
                    default:
                    break;
                }
            }
        }
    }
}
