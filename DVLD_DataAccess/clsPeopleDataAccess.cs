using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsPeopleDataAccess
    {
        // 1. تعريف نص الاتصال كمتغير عام وثابت على مستوى الكلاس
        private static readonly string strConnection = clsDataAccessSitting.ConnectionString;

        public static bool GetPersonByPersonId(int PersonID, ref string FirstName, ref string SecondName,
          ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime DateOfBirth,
           ref byte Gendor, ref string Address, ref string Phone, ref string Email,
           ref int NationalityCountryID, ref string ImagePath)
        {
            bool IsFound = false;
            string query = "SELECT * FROM People WHERE PersonID = @PersonID;";

            // 2. بناء كائن الاتصال داخل الدالة باستخدام using
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;

                                FirstName = (string)reader["FirstName"];
                                SecondName = (string)reader["SecondName"];

                                // 3. اختصار التعامل مع القيم الفارغة
                                ThirdName = (reader["ThirdName"] != DBNull.Value) ? (string)reader["ThirdName"] : "";

                                NationalNo = (string)reader["NationalNo"];
                                LastName = (string)reader["LastName"];
                                DateOfBirth = (DateTime)reader["DateOfBirth"];
                                Gendor = (byte)reader["Gendor"];
                                Address = (string)reader["Address"];
                                Phone = (string)reader["Phone"];

                                Email = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
                                NationalityCountryID = (int)reader["NationalityCountryID"];
                                ImagePath = (reader["ImagePath"] != DBNull.Value) ? (string)reader["ImagePath"] : "";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // 4. كتابة اسم الكلاس والدالة بدقة في سجل الأخطاء
                        clsUntil.HandelEventViewerExceptions("clsPeopleDataAccess _ GetPersonByPersonId", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        IsFound = false;
                    }
                }
            }
            return IsFound;
        }

        public static bool GetPersonByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
        ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
         ref byte Gendor, ref string Address, ref string Phone, ref string Email,
         ref int NationalityCountryID, ref string ImagePath)
        {
            bool IsFound = false;
            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo;";

            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;

                                PersonID = (int)reader["PersonID"];
                                FirstName = (string)reader["FirstName"];
                                SecondName = (string)reader["SecondName"];
                                ThirdName = (reader["ThirdName"] != DBNull.Value) ? (string)reader["ThirdName"] : "";
                                NationalNo = (string)reader["NationalNo"];
                                LastName = (string)reader["LastName"];
                                DateOfBirth = (DateTime)reader["DateOfBirth"];
                                Gendor = (byte)reader["Gendor"];
                                Address = (string)reader["Address"];
                                Phone = (string)reader["Phone"];
                                Email = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
                                NationalityCountryID = (int)reader["NationalityCountryID"];
                                ImagePath = (reader["ImagePath"] != DBNull.Value) ? (string)reader["ImagePath"] : "";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsPeopleDataAccess _ GetPersonByNationalNo", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        IsFound = false;
                    }
                }
            }
            return IsFound;
        }

        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName,
           string ThirdName, string LastName, DateTime DateOfBirth, byte Gendor, string Address,
           string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int PersonIdReturn = -1;
            string query = @"INSERT INTO [dbo].[People]
                                ([NationalNo], [FirstName], [SecondName], [ThirdName], [LastName], 
                                 [DateOfBirth], [Gendor], [Address], [Phone], [Email], 
                                 [NationalityCountryID], [ImagePath])  
                             VALUES
                                (@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, 
                                 @DateOfBirth, @Gendor, @Address, @Phone, @Email, 
                                 @NationalityCountryID, @ImagePath); 
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@SecondName", SecondName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gendor", Gendor);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Phone", Phone);
                    cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

                    // معالجة النصوص التي قد تكون فارغة
                    if (string.IsNullOrEmpty(ThirdName))
                        cmd.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ThirdName", ThirdName);

                    if (string.IsNullOrEmpty(Email))
                        cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Email", Email);

                    if (string.IsNullOrEmpty(ImagePath))
                        cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ImagePath", ImagePath);

                    try
                    {
                        connection.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int PId))
                        {
                            PersonIdReturn = PId;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsPeopleDataAccess _ AddNewPerson", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }
            return PersonIdReturn;
        }

        public static bool UpdatePerson(int PersonID, string FirstName, string SecondName,
              string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
              short Gendor, string Address, string Phone, string Email,
               int NationalityCountryID, string ImagePath)
        {
            int rowsAffected = 0;
            string query = @"UPDATE People  
                             SET FirstName = @FirstName,
                                 SecondName = @SecondName,
                                 ThirdName = @ThirdName,
                                 LastName = @LastName, 
                                 NationalNo = @NationalNo,
                                 DateOfBirth = @DateOfBirth,
                                 Gendor = @Gendor,
                                 Address = @Address,  
                                 Phone = @Phone,
                                 Email = @Email, 
                                 NationalityCountryID = @NationalityCountryID,
                                 ImagePath = @ImagePath
                             WHERE PersonID = @PersonID";

            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@FirstName", FirstName);
                    cmd.Parameters.AddWithValue("@SecondName", SecondName);
                    cmd.Parameters.AddWithValue("@LastName", LastName);
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gendor", Gendor);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@Phone", Phone);
                    cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

                    if (string.IsNullOrEmpty(ThirdName))
                        cmd.Parameters.AddWithValue("@ThirdName", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ThirdName", ThirdName);

                    if (string.IsNullOrEmpty(Email))
                        cmd.Parameters.AddWithValue("@Email", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@Email", Email);

                    if (string.IsNullOrEmpty(ImagePath))
                        cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@ImagePath", ImagePath);

                    try
                    {
                        connection.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsPeopleDataAccess _ UpdatePerson", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, 
                                    People.ThirdName, People.LastName, People.DateOfBirth,
                                    CASE 
                                        WHEN People.Gendor = 0 THEN 'Male'
                                        ELSE 'Female' 
                                    END as GendorCaption, 
                                    People.Address, People.Phone, People.Email, 
                                    People.NationalityCountryID, Countries.CountryName, People.ImagePath
                             FROM People 
                             INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID;";

            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                dt.Load(rdr);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsPeopleDataAccess _ GetAllPeople", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }
            return dt;
        }

        public static bool DeletePerson(int PersonId)
        {
            int rowsAffected = 0;
            string query = "DELETE FROM People WHERE PersonID = @PersonID;";

            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PersonId", PersonId);
                    try
                    {
                        connection.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsPeopleDataAccess _ DeletePerson", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static bool IsPersonExist(int PersonId)
        {
            int rowsAffected = 0;
            string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID;";

            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PersonId", PersonId);
                    try
                    {
                        connection.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int num))
                        {
                            rowsAffected = num;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsPeopleDataAccess _ IsPersonExist (ByID)", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static bool IsPersonExist(string NationalNo)
        {
            int rowsAffected = 0;
            string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo;";

            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
                    try
                    {
                        connection.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int num))
                        {
                            rowsAffected = num;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsPeopleDataAccess _ IsPersonExist (ByNationalNo)", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }
            return (rowsAffected > 0);
        }
    }
}