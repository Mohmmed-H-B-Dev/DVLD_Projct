using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public  class clsCountries
    {
        static SqlConnection conn = new SqlConnection(clsDataAccessSitting.ConnectionString);


        public static bool GetCountryByCountryID(int CountryID,ref string CountryName)
        {
            bool isFound=false;

            string query= "SELECT * FROM Countries WHERE CountryID = @CountryID";

            SqlCommand cmd =new SqlCommand(query,conn);
            cmd.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    CountryName=(string)rdr["CountryName"];
                    isFound = true;
                }
                rdr.Close();
            }catch (Exception ex)
            {
                isFound = false;
            }
            finally { conn.Close(); }   

            return isFound;
        }
        public static bool GetCountryByCountryName(string CountryName, ref int CountryID)
        {
            bool isFound = false;

            string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    CountryID=(int)rdr["CountryID"];
                    isFound = true;
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally { conn.Close(); }

            return isFound;
        }

        public static DataTable GetAllCountries()
        {
            string query = "SELECT * FROM Countries ;";

            SqlCommand cmd=new SqlCommand(query, conn);
            DataTable dt=new DataTable();
            try
            {
                conn.Open();    

                SqlDataReader rdr=cmd.ExecuteReader();

                if(rdr.HasRows)
                {
                    dt.Load(rdr);
                }

            }catch(Exception ex) { }finally { conn.Close(); }

            return dt;

        }
    }
}
