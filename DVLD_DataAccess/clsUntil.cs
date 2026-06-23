using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public  class clsUntil
    {
       static long Code = 8932;
        //static public string Encoding(string Text)
        //{
        //    string st = "";
        //    foreach (char c in Text)
        //    {
        //        st+= Convert.ToChar(((int) c  +Code));


        //    }

        //    return st;
        //}
        public static void HandelEventViewerExceptions(string SourceNameClassLibrary,string eMessage ,EventLogEntryType type)
        {
            try
            {
                if (!EventLog.SourceExists("DVLD_" + SourceNameClassLibrary))
                    EventLog.CreateEventSource("DVLD_" + SourceNameClassLibrary,null);

                EventLog.WriteEntry("DVLD_" + SourceNameClassLibrary,"An except problem is : "+ eMessage, type);

            }catch (Exception e)
            {
                EventLog.WriteEntry("DVLD_ Library Until " , "There a problem in code event log exceptions..", type);

            }
        }
        //static public string Decoding(string Text)
        //{
        //    string st = "";
        //    foreach (char c in Text)
        //    {
        //        st+= Convert.ToChar(((int)c-Code));
            
        //    }
        //    return st;
        //}
    }
}
