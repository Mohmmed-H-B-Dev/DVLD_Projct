using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsApplicationTypeData
    {
      public static SqlConnection conn = new SqlConnection(clsDataAccessSitting.ConnectionString);
        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID,
           ref string ApplicationTypeTitle, ref float ApplicationFees)
        {
            bool IsFound = false;   
            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    IsFound=true;
                    ApplicationTypeTitle =(string)rdr["ApplicationTypeTitle"];
                    ApplicationFees=Convert.ToSingle(rdr["ApplicationFees"]);
                }
                rdr.Close();

            }catch(Exception ex)
            {
                IsFound=false;
            }
            finally
            {
                conn.Close();
            }

            return IsFound;
        }

        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM ApplicationTypes order by ApplicationTypeTitle";

            SqlCommand cmd = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    dt.Load(rdr);
                }

            }catch(Exception ex) { return null; }
            finally { conn.Close(); }

            return dt;
        }
        public static int AddNewApplicationType(string Title,float Fees)
        {
            int ApplicationTypeID = -1;
            string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle,ApplicationFees)
                            Values (@Title,@Fees)
                            
                            SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("Title", Title);
            cmd.Parameters.AddWithValue("Fees", Fees);

            try
            {
                conn.Open();
                object result = cmd.ExecuteScalar();
                if(result!=null && int.TryParse(result.ToString(),out int InsertedId))
                {
                    ApplicationTypeID=InsertedId;
                }

            }catch(Exception ex) { }
            finally { conn.Close();  }
            return ApplicationTypeID;
        }
        public static bool UpdateApplicationType(int ApplicationTypeID, string Title, float Fees)
        {

            int rowsAffected = 0;
          

            string query = @"Update  ApplicationTypes  
                            set ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                                where ApplicationTypeID = @ApplicationTypeID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            cmd.Parameters.AddWithValue("@Title", Title);
            cmd.Parameters.AddWithValue("@Fees", Fees);

            try
            {
                conn.Open();
                rowsAffected =cmd.ExecuteNonQuery();

            }catch(Exception ex)
            {
                return false;
            }
            finally { conn.Close(); }

            return (rowsAffected>0);

        }


    }
}
