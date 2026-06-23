using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public  class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public clsCountry() 
        {
            CountryID = 0;
            CountryName="";
        }
        public clsCountry(int CountryId,string CountryName)
        {
            this.CountryID = CountryId;
            this.CountryName=CountryName;
        }

        public static clsCountry Find(int CountryID)
        {
            string CountryName = "";

            if (clsCountries.GetCountryByCountryID(CountryID, ref CountryName))
            {

                return new clsCountry(CountryID, CountryName);

            }
            else

                return null;


        }

        public static clsCountry Find(string CountryName)
        {
            int CountryID = -1;

            if (clsCountries.GetCountryByCountryName(CountryName, ref CountryID))
            {

                return new clsCountry(CountryID, CountryName);

            }
            else

                return null;


        }

        public static DataTable GetAllCountries()
        {
            return clsCountries.GetAllCountries();
        }
    }

}

