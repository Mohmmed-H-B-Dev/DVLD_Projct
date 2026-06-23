using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
namespace DVLD_DataAccess
{
    public  class clsDataAccessSitting
    {

 
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["MyConnectionDB"].ConnectionString;

    }
}
