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
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
namespace FMS
{
    public class InvestingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string date;
        string symbol;

        private void NotifyPropertyChanged( String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Date 
        { 
            get
            { 
                return date; 
            } 
            set
            {
                if (value != date)
                {
                    date = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Symbol
        {
            get
            {
                return symbol;
            }
            set
            {
                if (value != symbol)
                    symbol = value;
            }
        }

        public int NoOfShares { get; set; }

        public string Comments { get; set; }

        public double ShareValue { get; set; }

        public double Total { get; set; }
       
    }

    /// <summary>
    /// Interaction logic for InvestingTransaction.xaml
    /// </summary>
    public partial class InvestingTransaction : UserControl
    {
        public InvestingTransaction()
        {
            InitializeComponent();          

        }
        public void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            User  item =(User) TransactionInformation.SelectedItem;
            if (item != null)
            {

                TransactionWindow dlg = new TransactionWindow();
                dlg.Owner = (MainWindow)System.Windows.Application.Current.MainWindow;
                dlg.UpdateColumn(item);
                dlg.ShowDialog();
                item.Date = dlg.mItem.Date;

                ((User)(TransactionInformation.SelectedItem)).Date = item.Date;
            }
  
        }

        internal void AddNewTransaction(User user)
        {
            //string dDate = user.Date;


            string sDirectory = user.Date; // dDate.Year.ToString() + dDate.Month.ToString() + dDate.Day.ToString();
            sDirectory = sDirectory.Replace('/', '_');

            string sFilePath = sDirectory + "\\" + sDirectory + ".db";
            List<User> lsUser = new List<User>();
            if (!Directory.Exists(sDirectory ))
            {
                Directory.CreateDirectory(sDirectory);
            }
            if (File.Exists(sFilePath))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<User>));
                TextReader reader = new StreamReader(sFilePath);
                lsUser = (List<User>)deserializer.Deserialize(reader);
                reader.Close();
            }
            using (TextWriter writer = new StreamWriter(sFilePath, false))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                lsUser.Add(user);
                serializer.Serialize(writer, lsUser);
                writer.Close();
            }
           
            TransactionInformation.Items.Add(user);
            DB.DataBase db = DB.DataBase.Instance;
            if (CategoryType.EXPENSES == db.GetCategoryType(user.Category))
            {
                db.UpdateCategoryTotalValue(user.Category, user.Payment);
            }
            else
            {
                db.UpdateCategoryTotalValue(user.Category, user.Deposit);
            }

        }

        public void LoadAllTransaction(string sDate)
        {
            string sDirectory = sDate;
            string sFilePath = sDirectory + "\\" + sDate + ".db";
            if (Directory.Exists(sDirectory))
            {
                if (File.Exists(sFilePath))
                {
                    using (TextReader reader = new StreamReader(sFilePath))
                    {
                        List<User> lsUser = new List<User>();
                        XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                        lsUser = (List<User>)serializer.Deserialize(reader);
                        DB.DataBase db = DB.DataBase.Instance;
                        foreach (User u in lsUser)
                        {
                            TransactionInformation.Items.Add(u);
                            if (CategoryType.EXPENSES == db.GetCategoryType(u.Category))
                            {
                                db.UpdateCategoryTotalValue(u.Category, u.Payment);
                            }
                            else
                            {
                                db.UpdateCategoryTotalValue(u.Category, u.Deposit);
                            }
                        }
                        reader.Close();
                    }
                }
            }          
            
        }

        public void LoadAllExpendTransactions(string sDate)
        {
            string sDirectory = sDate;
            string sFilePath = sDirectory + "\\" + sDate + ".db";
            if (Directory.Exists(sDirectory))
            {
                if (File.Exists(sFilePath))
                {
                    using (TextReader reader = new StreamReader(sFilePath))
                    {
                        List<User> lsUser = new List<User>();
                        XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                        lsUser = (List<User>)serializer.Deserialize(reader);
                        DB.DataBase db = DB.DataBase.Instance;
                        foreach (User u in lsUser)
                        {                            
                            if (CategoryType.EXPENSES == db.GetCategoryType(u.Category))
                            {
                               // db.UpdateCategoryTotalValue(u.Category, u.Payment);
                                TransactionInformation.Items.Add(u);
                            }
                            
                        }
                        reader.Close();
                    }
                }
            }          
            
        }
    }
}
