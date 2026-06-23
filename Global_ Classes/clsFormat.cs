using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_With_MY_teatcher.Global__Classes
{
   
    public   class clsFormat
    {
        public static string ToShort(DateTime dt)
        //Date to Short 
        {//Date to Short 
            return dt.ToString("dd/MMM/yyyy");
        }
    }
}
