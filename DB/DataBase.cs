using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

namespace FMS.DB
{
    public sealed class DataBase : IDataBase
    {
       
        public List<CAccountDetails> lAccountDetails = new List<CAccountDetails>();
        public static List<Categories> lCategories = new List<Categories>();
        public static List<Budget> lBudget = new List<Budget>();

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly DataBase instance = new DataBase();
        }
        public static DataBase Instance
        {
            get
            {
                return Nested.instance;;
            }
        }
   
        private DataBase()
        {
            DeSerializeAccountDetails();
            if (lCategories.Count <= 0)
            {
                lCategories.Add(new Categories(CategoryType.EXPENSES, "Vehicle", 0));
                lCategories.Add(new Categories(CategoryType.EXPENSES, "Donations", 0));
                lCategories.Add(new Categories(CategoryType.EXPENSES, "Family unit", 0));
                lCategories.Add(new Categories(CategoryType.EXPENSES, "Offerings", 0));
                lCategories.Add(new Categories(CategoryType.EXPENSES, "Learning", 0));
                lCategories.Add(new Categories(CategoryType.EXPENSES, "Food", 0));
                lCategories.Add(new Categories(CategoryType.EXPENSES, "Home belongings", 0));
                lCategories.Add(new Categories(CategoryType.EXPENSES, "Medical", 0));
                lCategories.Add(new Categories(CategoryType.EXPENSES, "Others", 0));

                lCategories.Add(new Categories(CategoryType.INCOME, "Salary", 0));
                lCategories.Add(new Categories(CategoryType.INCOME, "Rents", 0));
                lCategories.Add(new Categories(CategoryType.INCOME, "Earning Interest", 0));
            }
            DeserializeBudgetSettings();
            if (lBudget.Count <= 0)
            {
                lBudget.Add(new Budget("Vehicle", 0));
                lBudget.Add(new Budget("Donations", 0));
                lBudget.Add(new Budget("Family unit", 0));
                lBudget.Add(new Budget("Offerings", 0));
                lBudget.Add(new Budget("Learning", 0));
                lBudget.Add(new Budget("Food", 0));
                lBudget.Add(new Budget("Home belongings", 0));
                lBudget.Add(new Budget("Medical", 0));
            }
            
        }

        private void DeserializeBudgetSettings()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Budget>));

            if (File.Exists(@"BudgetSettings.db"))
            {
                TextReader reader = new StreamReader(@"BudgetSettings.db");
                object obj = deserializer.Deserialize(reader);
                lBudget = (List<Budget>)obj;
                reader.Close();
            }
        }

        internal void SerializeBudgetSettings(List<Budget> budDetails)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Budget>));
            using (TextWriter writer = new StreamWriter(@"BudgetSettings.db"))
            {
                serializer.Serialize(writer, budDetails);
            }
        }
        #region IDataBase Members

        public List<CAccountDetails> getAccountDetails()
        {
            return lAccountDetails;
        }

        public List<Categories> getCategories()
        {
            return lCategories;
        }
        #endregion


        public void DeSerializeAccountDetails()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<CAccountDetails>));

            if (File.Exists(@"AccountDetails.db"))
            {
                TextReader reader = new StreamReader(@"AccountDetails.db");
                object obj = deserializer.Deserialize(reader);
                lAccountDetails = (List<CAccountDetails>)obj;
                reader.Close();
            }
        }       

        internal void SerializeAccountDetails(List<CAccountDetails> accountDetails)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<CAccountDetails>));
            using (TextWriter writer = new StreamWriter(@"AccountDetails.db"))
            {
                serializer.Serialize(writer, accountDetails);
            }
        }


        public void UpdateAccountDetails(CAccountDetails account)
        {
            foreach (CAccountDetails ac in lAccountDetails)
            {
                if (ac.sAccountName == account.sAccountName)
                {
                    ac.dBalance = account.dBalance;
                    ac.dIncome = account.dIncome;
                    ac.dExpenses = account.dExpenses;
                    ac.sCountry = account.sCountry;
                    break;
                }
            }
            SerializeAccountDetails(lAccountDetails);
            DeSerializeAccountDetails();
        }

        public CAccountDetails GetAccount(string acName)
        {
            foreach (CAccountDetails ac in lAccountDetails)
            {
                if (ac.sAccountName == acName) return ac;
            }
            return null;
        }

        internal CategoryType GetCategoryType(string s)
        {
            CategoryType ct = CategoryType.EXPENSES;
            foreach (Categories c in lCategories)
            {
                if (c.sCategory == s)
                {
                    ct = c.eType;
                    return ct;
                }
            }
            return ct;
        }

        internal double GetCategoryValue(string s)
        {
            double  ct = 0;
            foreach (Categories c in lCategories)
            {
                if (c.sCategory == s)
                {
                    ct = c.dTotal;
                    return ct;
                }
            }
            return ct;
        }

        internal void UpdateCategoryTotalValue(string s, double d)
        {
            foreach (Categories c in lCategories)
            {
                if (c.sCategory == s)
                {
                    c.dTotal  = c.dTotal + d;                  
                }
            }
        }

        internal List<Categories> GetCategoryList()
        {
            return lCategories;
        }

        internal void UpdateBudgetSettings(string p, double dbud)
        {
            bool isNotExits = true;
            foreach (Budget c in lBudget)
            {
                if (c.sCategory == p)
                {
                    c.dBudget = dbud;
                    isNotExits = false;
                    break;
                }
            }
            if (isNotExits)
            {
                lBudget.Add(new Budget(p,dbud));
            }
            SerializeBudgetSettings(lBudget);
        }

        internal List<Budget> getBudgetSettings()
        {
            return lBudget;
        }
    }
}
