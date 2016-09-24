using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.DB
{
    interface IDataBase
    {
         List<CAccountDetails> getAccountDetails();
    }
}
